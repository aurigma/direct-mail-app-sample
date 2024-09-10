<script setup lang="ts">
interface IRecipient {
  value: string,
  label: string,
}

interface IProps {
  options: IRecipient[],
}

const props = defineProps<IProps>();
const emit = defineEmits(['select']);

const changedSelected: Ref<number> = ref(0);
const firstOption = props.options[changedSelected.value].value;
const selectedRecipientId: Ref<IRecipient['value']> = ref(firstOption);

function selectRecipient(recipientValue: string, isInitValue?: boolean) {
  if (selectedRecipientId.value != recipientValue || isInitValue) {
    selectedRecipientId.value = recipientValue;
    ExecutionDelay.add('select-recipient', () => {
      emit('select', selectedRecipientId.value);
    }, 500);
  }
};

function prevOrNextRecipient (side: ('prev' | 'next')) {
  let currentOrder = 0; 
  props.options.forEach((item, order) => {
    if (item.value === selectedRecipientId.value) currentOrder = order;
  });

  if (side === 'next') {
    const nextOrder = currentOrder + 1;
    if (nextOrder <= props.options.length - 1) changedSelected.value = nextOrder;
    else changedSelected.value = 0;
  }
  
  if (side === 'prev') {
    const prevOrder = currentOrder - 1;
    if (prevOrder > -1) changedSelected.value = prevOrder;
    else changedSelected.value = props.options.length - 1;
  }
}
</script>

<template>
  <div
    class="select-recipient"
  >
    <base-button
      tag="button"
      type="button"
      native-type="button"
      style-type="icon"
      icon-name="ep:arrow-left"
      class="buttons"
      :click="() => prevOrNextRecipient('prev')"
    />
    <BaseDropdown
      name="select-recipient"
      :options="options"
      class="select-recipient__dropdown"
      :initial-value-id="changedSelected"
      direction="up"
      :no-icon="true"
      @update:dropdown-value="selectRecipient"
      @update:init-value="(recipientId: string) => selectRecipient(recipientId, true)"
    />
    <base-button
      tag="button"
      type="button"
      native-type="button"
      style-type="icon"
      icon-name="ep:arrow-right"
      class="buttons"
      :click="() => prevOrNextRecipient('next')"
    />
  </div>
</template>

<style scoped lang="scss">
.select-recipient {
  display: flex;
  justify-content: center;
  align-items: center;
  gap: 5px;
  &__dropdown {
    width: 240px;
  }
  .buttons {
    color: #808080;
    &:focus {
      box-shadow: unset;
    }
  }
}
</style>