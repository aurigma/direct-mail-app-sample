<script setup lang="ts">
definePageMeta({
  middleware: ['create-project-navigate'],
});

const lineItemsStore = useLineItemsStore();
const approvalStore = useApprovalStore();

const lineItem = computed(() => lineItemsStore.getLineItem);

const disabledApproveButton: Ref<boolean> = ref(false);

const approve = async () => {
  if (lineItem.value) {
    disabledApproveButton.value = true;
    try {
      await approvalStore.finishPersonalization({ id: lineItem.value.id as string });
      navigateTo('/');
    } catch (err) {
      console.error(err);
    }
    disabledApproveButton.value = false;
  }
};
</script>

<template>
  <div class="page page__wrapper">
    <h1 class="approval__title">Preview Your Design</h1>
    <p class="approval__description">
      We strongly recommend you carefully review all the elements of your design and make sure everything looks exactly as you envisioned. Check the colors, sizes, fonts, and positioning of objects within the design.
    </p>
    
    <ApprovalViewer />

    <div class="approval-info">
      <p class="approval-info__description">
        By clicking Approve, you are agreeing to accept the final product regardless of image quality.
      </p>
      <BaseButton
        tag="button"
        native-type="button"
        style-type="primary"
        class="approval-info__button"
        :click="approve"
        :disabled="disabledApproveButton"
      >
        Approve
      </BaseButton>
    </div>
  </div>
</template>

<style scoped lang="scss">
.approval {
  &__title {
    display: flex;
  }
  &__description {
    text-align: left;
    padding: 15px 0 30px 0;
    font-family: 'Inter';
    font-size: 18px;
    font-weight: 400;
    line-height: 26px;
    letter-spacing: 0em;
    color: #808080;
  }
}
.approval-info {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-top: 20px;
  
  &__button {
    width: 400px;
    height: 44px;
  }
  &__description {
    max-width: 768px;
    padding: 0;
    font-family: "Inter";
    font-size: 16px;
    font-weight: 400;
    line-height: 24px;
    letter-spacing: 0em;
    text-align: left;
    color: #304050;
  }
}
</style>