<script setup lang="ts">
interface IProps {
  name: string,
  order: number,
  label: string,
  optionId: number,
  optionName: string,
  isActive?: boolean,
  disabledOrder?: number | undefined,
}
const props = defineProps<IProps>();
const emit = defineEmits(['update:value']);

const newValue = ref<unknown>(false);

watch(newValue, (_) => {
  emit('update:value', {
    type: 'chips',
    name: props.name,
    order: props.order,
    optionId: props.optionId,
    optionName: props.optionName,
    value: newValue
  });
});

const disabled: Ref<boolean> = ref(false);
watch(() => props.disabledOrder, (order) => {
  if (props.order === order) {
    disabled.value = true;
  } else {
    disabled.value = false;
  }

  if (order === undefined) disabled.value = false;
}, {deep: true, immediate: true});

if (props.isActive) newValue.value = true;
</script>

<template>
<div class="base-chips">
  <input
    :id="`chips-checkbox-` + label + order"
    v-model="newValue"
    type="checkbox"
    :name="name"
    class="base-chips__input"
    :disabled="disabled"
  >
  <label
    :for="`chips-checkbox-` + label + order"
    class="base-chips__title"
  >
    {{ label }}
  </label>
</div>
</template>

<style scoped lang="scss">
.base-chips {
  &__input {
    position: absolute;
    visibility: hidden;
    left: -1000px;
  }

  &__title {
    display: flex;
    padding: 6px 12px;

    color: #666666;
    border: 1px solid #CCCCCC;
    align-items: center;
    gap: 8px;

    flex-shrink: 0;

    border-radius: 22px;

    transition: all .2s;

    cursor: pointer;
  }

  input:checked + label {
    background: #0090FF;
    border: 1px solid #0090FF;
    color: white;
  }
}
</style>