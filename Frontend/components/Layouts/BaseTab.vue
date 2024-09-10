<script setup lang="ts">
interface IProps {
  status: IWorkflowTab['status'],
  to: IWorkflowTab['to'],
}
const props = defineProps<IProps>();
</script>

<template>
  <div
    class="base-tab"
    :class="[
      { 'base-tab__default': props.status === E_TAB_STATUS.default },
      { 'base-tab__active': props.status === E_TAB_STATUS.active },
      { 'base-tab__check': props.status === E_TAB_STATUS.check },
    ]"
  >
    <button
      class="base-tab__title"
      :disabled="props.status !== E_TAB_STATUS.check"
      @click="navigateTo(props.to)"
    >
      <slot />
    </button>
  </div>
</template>

<style scoped lang="scss">
.base-tab {
  display: flex;
  
  background: $tab-bg-default;
  color: $tab-text-default;
  border-radius: 8px;

  &__title {
    display: flex;
    width: 100%;
    align-items: center;
    padding: 8px 16px;

    font-family: "Inter";
    font-size: 16px;
    font-weight: 600;
    line-height: 24px;
    letter-spacing: 0.15px;
  }

  &__active {
    background: $tab-bg-active;
    color: $tab-text-active;
    border-color: $tab-border-active;
  }

  &__check {
    background: $tab-bg-check;
    color: $tab-text-check;
    border-color: $tab-border-check;
  }
}
</style>