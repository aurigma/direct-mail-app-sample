<script setup lang="ts">
import ModalList from '@/core/modal/list'; 

definePageMeta({
  middleware: ['update-design-navigate'],
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
      navigateTo(`/update-design/${lineItemsStore.getLineItem?.id}/approval`); 
    } else {
      ShowModal({
        key: ModalList.validateVDPAlert,
        options: {
          missingListVariableNames: resultValidateDesign.missingListVariableNames,
          missingDesignVariableNames: resultValidateDesign.missingDesignVariableNames,
          error: resultValidateDesign.error,
          to: `/update-design/${lineItemsStore.getLineItem?.id}/approval`,
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
.page__wrapper {
  margin-top: 0;
}
.customize {
  &__title {
    text-align: center;
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