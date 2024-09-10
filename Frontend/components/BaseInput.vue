<script setup lang="ts">
  interface IProps {
    name: string,
    required?: boolean,
    label?: string,
    placeholder?: string,
    disabled?: boolean,
    iconLabel?: string,
    leftIcon?: string,
  }
  const props = defineProps<IProps>();
  
  const emit = defineEmits(['pressed-enter']);

  const { value, errorMessage } = useField(() => props.name);

  function pressedEnter() {
    emit('pressed-enter', {
      fieldName: props.name,
    });
  }
</script>

<template>
  <div
    class="form-group"
    :class="{
      'has-danger': errorMessage,
      'has-success': !errorMessage,
      'has-label': label,
      'has-icon': leftIcon,
    }"
  >
    <BaseLabel
      :label="label"
      :required="required"
      :icon-label="iconLabel"
    />
    <div class="form-wrap">
      <BaseIcon
        v-if="!!leftIcon"
        :name="(leftIcon as string)"
        class="form-group__icon-left"
      />
      <slot>
        <input
          v-model="value"
          class="form-control"
          :placeholder="placeholder"
          :disabled="disabled"
          @keydown.enter="pressedEnter"
        >
      </slot>
    </div>
  </div>
</template>

<style scoped lang="scss">
@include form-field-base;
</style>