<script setup lang="ts">
interface IProps {
  modalKey: string,
  modalOptions: unknown,
}
const props = defineProps<IProps>();

const openModal = () => {
  ShowModal({
    key: props.modalKey,
    options: props.modalOptions,
  });
}
</script>

<template>
  <base-button
    v-if="!!modalKey"
    class="details-template-button"
    tag="button"
    native-type="button"
    style-type="secondary"
    :click="openModal"
  >
    View Details
  </base-button>
</template>

<style scoped lang="scss">
.details-template-button {
  width: calc(100% - 24px);
}
</style>