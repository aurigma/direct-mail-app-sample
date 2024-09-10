<script setup lang="ts">
  interface IProps {
    tag: string,
    nativeType: string,
    styleType?: 'primary' | 'ghost' | 'tretiary' | 'primary-blue' | 'icon' | 'secondary',
    iconName?: string,
    type?: string,
    loading?: boolean,
    disabled?: boolean,
    link?: boolean,
    click?(): unknown,
  }
  const props = defineProps<IProps>();
</script>

<template>
  <component
    :is="tag"
    :type="tag === 'button' ? nativeType : ''"
    :disabled="disabled || loading"
    class="button-base"
    :class="[
      { [`button-base__${type}`]: type },
      { 'button-base__link': link },
      { disabled: disabled && tag !== 'button' },
      `button-base__${styleType}`
    ]"
    @click="click"
  >
    <BaseIcon
      v-if="!!props.iconName"
      :name="(props.iconName as string)"
      :class="{'button-base__icon-left': props.styleType !== 'icon'}"
    />
    <slot />
  </component>
</template>

<style scoped lang="scss">
@include button-base;
</style>