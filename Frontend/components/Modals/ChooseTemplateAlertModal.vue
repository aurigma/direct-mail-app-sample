<script setup lang="ts">
const storeModal = useModalStore();
const lineItemsStore = useLineItemsStore();

const modalOptions = storeModal.getOptions;

const updateLineItemDesignId = async () => {
  try {
    await lineItemsStore.updateLineItem(modalOptions.payloadForUpdateLineItem);
  } catch (err) {
    console.error(err);
    return abortNavigation();
  }
  navigateTo('/create-project/recipients?lineItemId='+lineItemsStore.getLineItem?.id);
  CloseModal();
};
</script>

<template>
  <BaseModalBox :has-header="false">
    <div class="modal">
      <p class="modal__message">{{ modalOptions?.message }}</p>
      <div class="modal__wrap-buttons">
        <base-button
          tag="button"
          native-type="button"
          style-type="tretiary"
          :click="CloseModal"
          class="modal__button"
        >
          Close
        </base-button>
        <base-button
          tag="button"
          native-type="button"
          style-type="primary-blue"
          :click="updateLineItemDesignId"
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
  display: flex;
  flex-direction: column;
  justify-content: space-between;
  padding: 20px;
  color: black;
  width: 480px;
  height: 180px;

  &__message {
    color: #000000;
    font-size: 20px;
    font-weight: 400;
    line-height: 20px;
    margin-top: 10px;
  }
  &__wrap-buttons {
    width: 100%;
    display: flex;
    justify-content: flex-end;
    gap: 20px;
  }
  &__button {
    width: 103px;
    height: 36px;
    margin-top: 20px;
    font-size: 16px;
    font-weight: 400;
    line-height: 20px;
    text-align: left;
  }
}
</style>