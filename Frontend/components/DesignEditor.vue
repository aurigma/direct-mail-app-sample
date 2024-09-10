<!-- eslint-disable @typescript-eslint/ban-ts-comment -->
<!-- eslint-disable @typescript-eslint/no-explicit-any -->
<script setup lang="ts">
import { E_LOADING_STATUS } from  '@/core/loading.enums';
import type { LineItemUpdateModel, EditorVariableInfoDto } from '@/core/direct-mail-api-client';

interface IProps {
  isDesignChanged?: boolean,
};

const props = defineProps<IProps>();
const emit = defineEmits(['ready', 'editor-loaded']);

const route = useRoute();

const loading = useLoadingStore();
const editorInfoStore = useEditorInfoStore();
const lineItemsStore = useLineItemsStore();

const token = computed(() => editorInfoStore.getToken?.tokenId);  
const proofs = computed(() => editorInfoStore.getProofs);
const lineItem = computed(() => lineItemsStore.getLineItem);
const productPersonalizationWorkflow = computed(() => editorInfoStore.getProductPersonalizationWorkflow);

const editorFrame = ref<HTMLElement>();

let editor: any;
const selectedSurfaceId: Ref<string> = ref('');
let config: any = {};
const isFirstLoadProofs: Ref<boolean> = ref(true);
const savingInProgress: Ref<boolean> = ref(false);
const surfaceNames: Ref<string[]> = ref([]);

const newButtonData = {
  name: 'Variable',
  icon: 'cc-icon-variables',
}

await editorInfoStore.fetchAvailableVariables({ lineItemId: lineItem.value?.id as string });

const listVariableButtons: Ref<EditorVariableInfoDto[] | null> = computed(() => editorInfoStore.getVariableList);

const saveDesign = async () => {
  try {
    savingInProgress.value = true;
    await editor.saveProduct(lineItem.value?.designId);
    const proofImages = await editor.getProofImages({maxHeight: 120, maxWidth: 120, pregeneratePreviewImages: true});
    editorInfoStore.setProofs(proofImages.proofImageUrls);
    savingInProgress.value = false;
  } catch (err) {
    console.error(err);
  }
};

watch(() => props.isDesignChanged, async (value) => {
  if (value) {
    await saveDesign();
    emit('ready');
  }
}, {deep: true});

watch(proofs, (proofsValue) => {
  if (!!proofsValue && isFirstLoadProofs.value) {
    selectedSurfaceId.value = proofsValue[0].id.toString();
    isFirstLoadProofs.value = false;
  }
}, { deep: true });

async function switchSide(sideNumber: number) {
  const product = await editor.getProduct();
  const side = product.surfaces[sideNumber];
  product.switchTo(side);
};

async function getDesignId(payload: { lineItemId: string, userId: string }) {
  const response = await editorInfoStore.fetchDesignId(payload);
  return response.id;
}

onMounted(async () => {
  if (!editorFrame.value) {
    throw createError({
      statusCode: 422,
      statusMessage: 'Unprocessable Content',
      fatal: true,
    })
  }

  const userId = lineItem.value?.campaignId as string;

  const designId = await getDesignId({ lineItemId: String(lineItem.value?.id), userId: userId });

  const lineItemPayload = {
    id: String(route.query.lineItemId),
    body: {
      campaignId: lineItem.value?.campaignId,
      quantity: lineItem.value?.quantity,
      productId: lineItem.value?.productId,
      templateId: lineItem.value?.templateId,
      productVariantId: lineItem.value?.productVariantId,
      designId: designId,
    } as LineItemUpdateModel,
  };

  await lineItemsStore.updateLineItem(lineItemPayload);

  await editorInfoStore.fetchTokenDE({ userId: lineItem.value?.campaignId as string });
  config = {
    userId: userId,
    tokenId: token.value,
  };
  
  config = Object.assign(config, productPersonalizationWorkflow.value);
  
  const typeAction: { [index: string]: string } = {
    CustomImage: 'CustomPlaceholder',
  };

  function dropIfExistsInConfig() {
    const hasButton = config.widgets.Toolbox.buttons.find((btn: any) => btn.translationKey === newButtonData.name);

    if (hasButton) {
      const buttonIdx = config.widgets.Toolbox.buttons.findIndex((btn: any) => btn.translationKey === newButtonData.name);
      config.widgets.Toolbox.buttons.splice(buttonIdx, 1);
    }
  }
  
  dropIfExistsInConfig();

  if (listVariableButtons.value) {
    let variableButtons = listVariableButtons.value.map((btn) => {
      const buttonType = btn.type?.toString();
      const title = btn?.name;

      if (!buttonType || !title) return undefined;

      const schema: any = {
        translationKey: `${title}`,
        translationKeyTitle: `Add ${title}`,
        action: (Object.keys(typeAction).includes(buttonType)) ? typeAction[buttonType] : btn.type,
        placeholder: true,
        itemConfig: {
          name: `${title}`,
          text: `{{${title}}}`,
          isVariable: true,
          location: {
            originX: 'center',
            originY: 'center',
            x: '50%',
            y: '62%',
          },
          alignment: 'center',
          color: '#000',
        }
      };
      if (typeAction[buttonType] === 'CustomPlaceholder') {
        schema.itemConfig.allowedSubfolder = 'ui';
        schema.itemConfig.allowedTabs = [ 'Public' ];
        schema.itemConfig.contentImageUrl = 'public:DirectMail/placeholder.png';
        schema.itemConfig.isStubContent = true;
        schema.itemConfig.fixedStubContentSize = false;
        schema.itemConfig.isCoverMode = true;
        schema.itemConfig.contentResizeMode = 'fit';
        schema.itemConfig.placeholderPermissions = {
          allowEditContent: false,
          showSelectButton: false,
          showHandleButton: true,
        };
        schema.itemConfig.contentPermissions = {
          imagePermissions: {
            allowEditImage: false,
          }
        };
      }
      if (buttonType === 'Text') {
        schema.itemConfig.width = (title.length + 4) * 10;
        schema.itemConfig.height = 25;
        schema.itemConfig.overflowStrategy = 'fitToWidth';
        schema.itemConfig.font = {
          size: 90,
        };
      }
      if (buttonType === 'CustomBarcode') {
        schema.itemConfig.barcodeContent = {
          barcodeFormat: btn.barcodeFormat,
          barcodeSubType: btn.barcodeSubType,
        };
        schema.itemConfig.barcodePermissions = {
          allowChangeBarcodeContent: false,
        };
      }
      return schema;
    });
    
    variableButtons = variableButtons.filter((btn) => !!btn);

    config.widgets.Toolbox.buttons.push({
      translationKey: newButtonData.name,
      translationKeyTitle: `Add ${newButtonData.name}`,
      iconClass: newButtonData.icon,
      buttons: variableButtons,
    });
  }
  
  loading.setStatus(E_LOADING_STATUS.start);
  // @ts-ignore comment
  await CustomersCanvas.IframeApi.loadEditor(editorFrame.value, lineItem.value.designId, config).then((e) => {
    editor = e;
    
    // @ts-ignore comment
    editor.subscribe(CustomersCanvas.IframeApi.PostMessage.Events.ItemChanged, (event) => {
      if (!savingInProgress.value) saveDesign();
      event.preventDefault();
    });
    // @ts-ignore comment
    editor.subscribe(CustomersCanvas.IframeApi.PostMessage.Events.ItemAdded, (event) => {
      if (!savingInProgress.value) saveDesign();
      event.preventDefault();
    });
    // @ts-ignore comment
    editor.subscribe(CustomersCanvas.IframeApi.PostMessage.Events.ItemRemoved, (event) => {
      if (!savingInProgress.value) saveDesign();
      event.preventDefault();
    });

    editor.getProduct().then((product: any) => {
      surfaceNames.value = product.surfaces.map((surface: any) => surface.name);
    });
  });

  const proof = await editor.getProofImages();
  if (proof.proofImageUrls) editorInfoStore.setProofs(proof.proofImageUrls);
  loading.setStatus(E_LOADING_STATUS.finish);

  emit('editor-loaded');
});
</script>

<template>
  <div class="editor">
    <div class="editor__wrap-buttons">
      <template v-if="!!proofs">
        <BaseCard
          v-for="(card, order) in proofs"
          :key="`${card.id}-proofs`"
          :active-id="{id: selectedSurfaceId}"
          :id-card="{id: card.id.toString()}"
          :img="card.img"
          card-type="mini"
          @select-card="(payload) => selectedSurfaceId = payload.id"
          @click="switchSide(card.id)"
        >
          <template #title>
            <label class="card-label">{{ surfaceNames[order] }}</label>
          </template>
        </BaseCard>
      </template>
      <template v-else>
        <BaseSkeleton
          v-for="i in [0, 1, 2]"
          :key="`${i}-mini-card-skeleton`"
          skeleton-type="mini-card"
          title
        />
      </template>
    </div>
    <main class="main">
      <div class="main__section main__section-iframe">
        <div class="main__iframe">
          <iframe 
            id="editorFrame"
            ref="editorFrame"
            title="editorFrame"
          />
        </div>
      </div>
    </main>
  </div>
</template>

<style scoped lang="scss">
.editor {
  width: 100%;
  min-height: 800px;
  height: 100%;
  display: grid;
  grid-template-columns: 120px 1fr;
  gap: 20px;

  &__wrap-buttons {
    display: flex;
    flex-direction: column;
    flex-wrap: wrap;
    gap: 15px;
    max-width: 120px;
  }
}

.main {
  &__iframe {
    height: calc(100% - 8rem);
    width: 100%;

    iframe {
      border: none;
      height: 100%;
      width: 0;
      min-width: 100%;
      position: relative;
    }
  }

  #editorFrame {
    min-height: 800px;
  }
}

.card-label {
  font-family: 'Inter';
  font-size: 14px;
  font-weight: 400;
  line-height: 20px;
  letter-spacing: 0em;
  text-align: center;
  color: #808080;
}
</style>