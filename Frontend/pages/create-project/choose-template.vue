<script setup lang="ts">
import type { LineItemUpdateModel } from '~/core/direct-mail-api-client';
import ModalList from '@/core/modal/list'; 

interface IOption {
  optionId: number,
  optionName: string,
  optionType: string,
  valueIds: unknown[]
};

definePageMeta({
  middleware: ['create-project-navigate'],
});

const lineItemsStore = useLineItemsStore();
const integratedProductsStore = useIntegratedProductsStore();

const route = useRoute();

const productId = computed(() => String(lineItemsStore.getLineItem?.productId));
const templates = computed(() => integratedProductsStore.getTemplates);

const templateTitle: Ref<string> = ref('');
const selectedOptions: Ref<IOption[] | undefined> = ref(undefined);
const selectTemplatesId: Ref<string> = ref('');
const selectProductVariantId: Ref<number> = ref(0);

function inputSearch(event: Event) {
  const searchInputTimerDelay: number = 300;
  const title = (event.target as HTMLInputElement).value;
  
  templateTitle.value = title;

  ExecutionDelay.add('search', () => {
    integratedProductsStore.fetchIntegratedProductsTemplateByProductId({
      id: productId.value,
      body: {
        templateTitle: title,
        options: selectedOptions.value,
      }
    });
  }, searchInputTimerDelay);
};

function selectOptions(options: IOption[]) {
  selectedOptions.value = options;
}

function selectCard(payload: {id: string, productVariantId: number}) {
  if (selectTemplatesId.value === payload.id && selectProductVariantId.value === payload.productVariantId) return;
  
  selectTemplatesId.value = payload.id;
  selectProductVariantId.value = payload.productVariantId;
};

async function chooseTemplate() {
  if (selectTemplatesId.value) { 
    const lineItem = lineItemsStore.getLineItem;
    if (selectTemplatesId.value === lineItem?.templateId) {
      return navigateTo('/create-project/recipients?lineItemId='+lineItemsStore.getLineItem?.id);
    }
    const payload: { id: string, body: LineItemUpdateModel } = {
      id: String(route.query.lineItemId),
      body: {
        campaignId: lineItem?.campaignId,
        quantity: lineItem?.quantity,
        productId: lineItem?.productId,
        productVariantId: selectProductVariantId.value,
        templateId: selectTemplatesId.value,
        designId: null,
      } as LineItemUpdateModel,
    };
    if (lineItem?.designId) {
      ShowModal({
        key: ModalList.chooseTemplateAlert,
        options: {
          message: 'Your previous design changes will be erased!',
          payloadForUpdateLineItem: payload,
        },
      });
    } else {
      try {
        await lineItemsStore.updateLineItem(payload);
        navigateTo('/create-project/recipients?lineItemId='+lineItemsStore.getLineItem?.id);
      } catch (err) {
        console.error(err);
      }
    }
  }
};

function preSelectFirstTemplate(
  newTemplatesValue: { id: string, productVariantId: number }[],
  oldTemplatesValue: { id: string, productVariantId: number }[] | null
) {
  if (oldTemplatesValue === null && !!newTemplatesValue) {
    const firstTemplate = newTemplatesValue[0];
    selectCard(firstTemplate);
  }
}

watch(templates, (newTemplatesValue, oldTemplatesValue) => {
  preSelectFirstTemplate(
    newTemplatesValue as { id: string, productVariantId: number }[],
    oldTemplatesValue
  );
});
</script>

<template>
  <div class="page page__wrapper choose-template">
    <h1 class="choose-template__title">Choose A Template</h1>
    <form class="choose-template__wrap-search">
      <BaseInput
        placeholder="Search"
        name="search"
        left-icon="tabler:search"
        @input="inputSearch"
      />
      <base-button
        tag="button"
        type="button"
        native-type="button"
        style-type="primary"
        :click="chooseTemplate"
        :disabled="!!selectTemplatesId === false"
      >
        Continue
      </base-button>
    </form>
    <div class="choose-template__wrap-content">
      <ChooseTemplateOptions
        class="choose-template__side-bar"
        :product-id="(productId as string)"
        :template-title-update="templateTitle"
        @selected="selectOptions"
      />
      
      <div class="choose-template__content">
        <template v-if="!!templates">
          <BaseCard
            v-for="(card, cardOrder) in templates"
            :key="`template-${cardOrder}-${card.id}-${card.productVariantId}`"
            :active-id="{
              id: selectTemplatesId,
              productVariantId: selectProductVariantId,
            }"
            :id-card="{
              id: card.id,
              productVariantId: card.productVariantId,
            }"
            :img="card.img"
            @select-card="selectCard"
          >
            <template #details>
              <DetailsTemplateButton
                :modal-key="ModalList.productTemplate"
                :modal-options="{
                  id: productId,
                  templateId: card.id,
                  productVariantId: card.productVariantId,
                }"
              />
            </template>
            <template #title>
              {{ card.title }}
            </template>
          </BaseCard>
        </template>
        
        <template v-else>
          <BaseSkeleton
            v-for="i in [0, 1, 2, 4]"
            :key="`${i}-skeleton-choose-template`"
            skeleton-type="card"
            title
          />
        </template>
      </div>
    </div>
  </div>
</template>

<style scoped lang="scss">
.choose-template {
  &__title {
    display: flex;
  }
  &__wrap-search {
    display: grid;
    grid-template-columns: 4fr 1fr;
    gap: 20px;
    margin: 40px 0;
    
    > * {
      height: 44px;
    }
  }
  &__wrap-content {
    display: flex;
    justify-content: space-between;
  }
  &__content {
    padding-bottom: 50px;
    width: 100%;
    display: flex;
    justify-content: flex-start;
    flex-wrap: wrap;
    gap: 20px;
    padding-left: 40px;
  }
  &__side-bar {
    max-width: 250px;
    width: 100%;
  }
}
</style>