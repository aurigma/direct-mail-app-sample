<script setup lang="ts">
interface IProps {
  img: string | null,
  activeId: {
    id: string,
    productVariantId?: number,
  },
  idCard: {
    id: string,
    productVariantId?: number,
  },
  cardType?: 'default' | 'mini',
}
const props = defineProps<IProps>();
const emit = defineEmits(['select-card']);

const isActive = computed(() => {
  if (props.activeId?.productVariantId) {
    return props.activeId.productVariantId === props.idCard.productVariantId && props.activeId.id === props.idCard.id;
  } else {
    return props.activeId.id === props.idCard.id;
  }
});

function selectCard() {
  const payload = {
    id: props.idCard.id,
    productVariantId: props.idCard.productVariantId,
  };

  emit('select-card', payload);
}
</script>

<template>
  <div
    class="base-card"
    :class="[{ 'active': isActive }]"
  >
    <button
      class="base-card__button"
      :class="[
        { 'base-card__button_default': cardType === 'default' || !!cardType === false },
        { 'base-card__button_mini': cardType === 'mini' },
      ]"
      @click="selectCard"
    >
      <img
        v-if="!!img"
        :src="img"
        :alt="`base-card-${img}`"
      >
      <div class="base-card__details">
        <slot name="details" />
      </div>
    </button>
    <span class="base-card__title">
      <slot name="title" />
    </span>
  </div>
</template>

<style scoped lang="scss">
.base-card {
  &__button {
    display: flex;
    justify-content: center;
    align-items: center;
    width: 100%;
    height: 100%;
    border: 1px solid #E0E0E0;
    background: #EFF0F0;
    border-radius: 4px;
    position: relative;
    overflow: hidden;

    &_default {
      width: 220px;
      height: 220px;
    }
    &_mini {
      width: 120px;
      height: 120px;
    }
  }
  &__title {
    margin-top: 8px;
    color: #304050;
    text-align: center;
    font-family: 'Inter';
    font-size: 18px;
    font-style: normal;
    font-weight: 400;
    line-height: 26px;
    text-align: center;
    display: block;
    max-width: 220px;
    overflow: hidden;
    white-space: nowrap;
    text-overflow: ellipsis;
  }
  &__details {
    opacity: 0;
    position: absolute;
    bottom: 12px;
    display: flex;
    justify-content: center;
    width: calc(100%);
    transition: opacity .3s;
  }

  img {
    width: calc(100% - 8px);
    max-width: 210px;
    max-height: 210px;
  }
}
.base-card__button:hover .base-card__details {
  opacity: 1;
}
.active .base-card {
  &__button {
    outline: 2px solid #0090FF;
  }
}
</style>