<script setup lang="ts">
const storeModal = useModalStore();
const modalOptions = storeModal.getOptions;

const image = computed(() => {
  return modalOptions.image;
});
</script>

<template>
  <BaseModalBox :has-header="false">
    <div class="modal">
      <img
        :src="image"
        alt="approval-preview-image"
        class="preview-image"
      >
    </div>
  </BaseModalBox>
</template>

<style scoped lang="scss">
@include button-base;

.modal {
  max-width: 95vw;
  max-height: 95vh;
}
.preview-image {
  width: 100%;
  height: 100%;
}
</style>