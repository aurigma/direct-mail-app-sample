<script setup lang="ts">
  import type { LineItemUpdateModel } from '@/core/direct-mail-api-client';
  import ModalList from '@/core/modal/list'; 
  import { E_LOADING_STATUS } from "@/core/loading.enums";

  const emit = defineEmits(['loaded']);

  const noViewOption = 'Category';

  const storeLoading = useLoadingStore();
  storeLoading.setStatus(E_LOADING_STATUS.start);

  const storeModal = useModalStore();
  const modalOptions = storeModal.getOptions;

  const lineItemsStore = useLineItemsStore();
  const integratedProductsStore = useIntegratedProductsStore();

  const productStore = useProductsStore();
  productStore.fetchProductById({ id: modalOptions.id });
  const product = computed(() => productStore.getProduct);

  integratedProductsStore.resetTemplateInfo();
  await integratedProductsStore.fetchIntegratedProductTemplateDetails(modalOptions);

  const templateData = computed(() => integratedProductsStore.getTemplateInfo);

  emit('loaded');

  async function chooseTemplate() {
    const lineItem = lineItemsStore.getLineItem;
    if (modalOptions.templateId === lineItem?.templateId) {
      CloseModal();
      return navigateTo('/create-project/recipients?lineItemId='+lineItemsStore.getLineItem?.id);
    }
    const payload: {id: string, body: LineItemUpdateModel} = {
      id: (lineItem?.id as string),
      body: {
        campaignId: lineItem?.campaignId,
        quantity: lineItem?.quantity,
        productId: lineItem?.productId,
        productVariantId: modalOptions.productVariantId,
        templateId: modalOptions.templateId,
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
        CloseModal();
        navigateTo('/create-project/recipients?lineItemId='+lineItemsStore.getLineItem?.id);
      } catch (err) {
        console.error(err);
      }
    }
  };
</script>

<template>
  <BaseModalBox :has-header="true">
    <div class="modal">

      <div class="modal__slider">
        <PreviewGallery
          v-if="templateData?.previewUrls"
          :image-list="templateData?.previewUrls"
        />
      </div>
      <div class="modal__info">
        <div>
          <div class="modal__header">
            <h2>{{ templateData?.templateName }}</h2>
            <p class="price">Folded cards ${{ product?.price }}</p>
          </div>
          <div v-if="templateData?.customFields" class="modal__description">
            <p>{{ templateData?.customFields?.description }}</p>
          </div>
          <div class="modal__options">
            <template v-for="option in templateData?.options">
              <p
                v-if="option.title !== noViewOption"
                :key="`${option.title}-option`"
              >
                <b>{{ option.title }}:</b> {{ option.value }}
              </p>
            </template>
          </div>
        </div>
        <base-button
          tag="button"
          native-type="button"
          style-type="primary"
          :click="chooseTemplate"
          class="modal__button"
        >
          Continue
        </base-button>
      </div>    

    </div>
  </BaseModalBox>
</template>

<style scoped lang="scss">
.modal {
  min-height: 500px;
  width: 1020px;

  color: black;

  display: grid;
  gap: 60px;
  grid-template-columns: 1.5fr 1.3fr;

  padding: 10px 50px 50px 50px;
  &__slider {
    min-width: 480px;
    display: flex;
    flex-direction: column;
    justify-content: space-between;
  }
  &__info {
    display: flex;
    flex-direction: column;
    justify-content: space-between;
  }
  &__button {
    width: 100%;
    margin-top: 20px;
  }
  &__header {
    h2 {
      color: #282828;
      text-align: left;
      font-family: 'Inter';
      font-size: 24px;
      font-style: normal;
      line-height: 32px;
      text-wrap: wrap;
      word-break: break-all;
      font-weight: 600;
    }

    .price {
      color: #666666;
      text-align: left;
      font-family: 'Inter';
      font-size: 18px;
      font-style: normal;
      font-weight: 400;
      line-height: 28px;
      padding: 20px 0;
    }
  }
  &__description {
    color: #282828;
    font-family: 'Inter';
    font-style: normal;
    font-weight: 400;
    font-size: 16px;
    line-height: 24px;
  }
  &__options {
    color: #282828;
    font-family: 'Inter';
    font-size: 16px;
    font-style: normal;
    font-weight: 400;
    line-height: 24px;
    margin-top: 10px;
  }
}
</style>