<script setup lang="ts">
  import { useModalStore } from '@/stores/modal.store';
  import type { IModalOptions } from '@/core/modal/interfaces';

  const props = defineProps<IModalOptions>();

  const storeModal = useModalStore();
  const options = storeModal.getOptions;

  const setHeaderStyle = computed( () => {
    let className = 'base-modal__header';

    const hasDescription = options.description || props.description;
    if (hasDescription) {
      className += ' base-modal__header_description';
    }

    return className;
  });
</script>

<template>
  <div
    class="base-modal__box"
  >
    <div
      v-if="hasHeader === true"
      :class="setHeaderStyle"
    >
      <div class="base-modal__title">
        <span>{{ options.title || title || '' }}</span>
      </div>
      <button
        v-if="!options.isUnclosable"
        class="button-base button-base__icon base-modal__close"
        @click="CloseModal"
      >
        <Icon
          name="uiw:close"
        />
      </button>
    </div>
    <div
      v-if="options.description || description"
      class="base-modal__description"
    >
      <span>{{ options.description || description }}</span>
    </div>
    <slot />
  </div>
</template>

<style scoped lang="scss">
@include button-base;

.base-modal {
  &__box {
    color: black;
  }
  &__header {
    width: 100%;
    display: flex;
    align-items: center;
    justify-content: space-between;
  }
  &__close {
    color: #808080;
  } 
}
</style>