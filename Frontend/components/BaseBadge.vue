<script setup lang="ts">
  interface IProps {
    status: string,
  }
  const props = defineProps<IProps>();

  const getStyleStatus = computed(() => props.status.toLowerCase().split(' ').join('-'));
</script>

<template>
  <div
    class="base-badge"
    :class="`base-badge__${getStyleStatus}`"
  >
    <span class="base-badge__title">
      <slot />
    </span>
  </div>
</template>

<style scoped lang="scss">
@include base-badge;
</style>