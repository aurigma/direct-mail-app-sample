<script setup lang="ts">
interface IProps {
  title: string,
  id: string,
  description?: string;
  options?: {
    id?: string | null,
    fullName?: string | null,
  }[] | null,
  hasToggle?: boolean,
}
const props = defineProps<IProps>();

const isShow = ref(false);

const toggleShowing = () => {
  if (props.hasToggle) isShow.value = !isShow.value;
}
</script>

<template>
  <div class="base-list">
    <div class="base-list__head" @click="toggleShowing">
      <p class="base-list__info-wrap">
        <span class="base-list__title">{{ title }}</span>
        &nbsp;<span class="base-list__description">{{ description }}</span>
      </p>
      <slot />
    </div>
    <transition name="move-down">
      <ul v-show="isShow && !!options?.length" class="base-list__body">
        <hr >
        <li
          v-for="(item, itemIndex) in options"
          :key="`${itemIndex}-option-list`"
          class="base-list__item"
        >
          {{ item.fullName }}
        </li>
      </ul>
    </transition>
  </div>
</template>

<style scoped lang="scss">
.base-list {
  width: 100%;
  border-radius: 10px;
  background: #EFF0F0;

  &__head {
    display: flex;
    align-items: center;
    align-self: stretch;
    justify-content: space-between;

    width: 100%;

    padding: 20px;
    gap: 20px;

    color: #808080;
  }
  
  &__info-wrap {
    padding: 0;
    margin: 0;
  }

  &__title {
    color: #304050;
  }

  &__body {
    display: flex;
    flex-direction: column;
    gap: 5px;
    padding: 0 20px 20px 20px;
  }
}
</style>