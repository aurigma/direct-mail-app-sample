<script setup lang="ts">
interface IProps {
  label?: string,
  required?: boolean,
  iconLabel?: string,
}
defineProps<IProps>()
</script>

<template>
  <label v-if="label" class="label">
    <span class="label__required">
      {{ required ? '*' : '' }}
    </span>
    {{ label }}
    <BaseIcon
      v-if="!!iconLabel"
      :name="(iconLabel as string)"
      class="label__icon"
    />
  </label>
</template>

<style lang="scss">
.label {
  display: flex;
  align-items: center;

  font-size: 16px;
  font-weight: 600;
  line-height: 24px;
  color: #282828;

  margin-bottom: 10px;
  
  &__required {
    color: #E81F76;
    padding-right: 5px;
  }
  &__icon {
    color: #3FBEAC;
    margin-left: 5px;
    width: 24px;
  }
}
</style>