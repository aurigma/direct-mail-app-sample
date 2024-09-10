<script setup lang="ts">
import ModalList from '@/core/modal/list'; 

definePageMeta({
  middleware: ['create-project-navigate'],
});

const lineItemsStore = useLineItemsStore();

const lineItem = computed(() => lineItemsStore.getLineItem);

const hasUrlEditor: Ref<boolean> = ref(false);
const isDesignChanged: Ref<boolean> = ref(false);
const readyForApprove: Ref<boolean> = ref(false);
const editorIsNotLoaded: Ref<boolean> = ref(true);

const editorInfoStore = useEditorInfoStore();
const designEditorUrl = computed(() => (editorInfoStore.getDesignEditorUrl) ? editorInfoStore.getDesignEditorUrl : '');

editorInfoStore.setProofs(null);

const getIntegrationsDetails = async (productId: string) => {
  Promise.all([await editorInfoStore.fetchIntegrationDetails({ productId: productId })]).then(() => {
    useHead({
      script: [
        {
          src: `${designEditorUrl.value}/Resources/Generated/IframeApi.js`,
          id:"CcIframeApiScript",
          async: true,
          defer: true
        },
      ],
    });
    hasUrlEditor.value = true;
  });
};

if (lineItem.value?.productId) await getIntegrationsDetails(lineItem.value?.productId);

watch(lineItem, async () => {
  if (lineItem.value?.productId) await getIntegrationsDetails(lineItem.value?.productId);
});

const toApproval = async () => {
  isDesignChanged.value = true;
};

watch(readyForApprove, async (value) => {
  if (value) {
    const resultValidateDesign = await editorInfoStore.validateDesign({ lineItemId: lineItem.value?.id as string });
    if (resultValidateDesign.isValid) {
      navigateTo('/create-project/approval?lineItemId='+lineItemsStore.getLineItem?.id); 
    } else {
      ShowModal({
        key: ModalList.validateVDPAlert,
        options: {
          missingListVariableNames: resultValidateDesign.missingListVariableNames,
          missingDesignVariableNames: resultValidateDesign.missingDesignVariableNames,
          error: resultValidateDesign.error,
          to: '/create-project/approval?lineItemId='+lineItemsStore.getLineItem?.id,
        },
      });
      isDesignChanged.value = false;
      readyForApprove.value = false;
    }
  }
});
</script>

<template>
  <div class="page page__wrapper">
    <h1 class="customize__title">Customize Your Template</h1>
    <div class="customize__top">
      <p>Your edits were automatically saved.</p>
      <BaseButton
        tag="button"
        native-type="button"
        style-type="primary"
        class="customize__button-next"
        :click="toApproval"
        :disabled="isDesignChanged || editorIsNotLoaded"
      >
        Next
      </BaseButton>
    </div>
    <ClientOnly>
      <template #fallback>
        <div/> 
      </template>
      <DesignEditor
        v-if="hasUrlEditor"
        :is-design-changed="isDesignChanged"
        @ready="() => readyForApprove = true"
        @editor-loaded="() => editorIsNotLoaded = false"
      />
    </ClientOnly>
  </div>
</template>

<style scoped lang="scss">
.customize {
  &__title {
    display: flex;
  }
  &__top {
    padding: 30px 0 16px 0;
    display: flex;
    justify-content: space-between;
    align-items: center;
    p {
      font-family: 'Inter';
      font-size: 18px;
      font-weight: 400;
      line-height: 26px;
      letter-spacing: 0em;
      text-align: left;
    }
  }
  &__button-next {
    width: 200px;
    height: 44px;
  }
}
</style>