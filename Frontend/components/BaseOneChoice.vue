<script setup lang="ts">
interface IProps {
  choices?: { 
    title: string,
    value: string,
  } [] | null,
  optionId?: number,
  optionName?: string,
  orderOption: number,
}
const props = defineProps<IProps>();

const emit = defineEmits(['selected']);

const activeRef: Ref<string> = ref('');

watch(activeRef, (value: string) => {
  const selectedChoiceData = (activeRef.value === 'all') ? undefined : activeRef.value;
  emit('selected', { 
    type: 'choice', 
    optionId: props.optionId, 
    optionName: props.optionName, 
    selected: value,
    data: selectedChoiceData,
  });
});

onMounted(() => {
  if (props.choices) {
    const firstChoice = props.choices[0].value;
    activeRef.value = firstChoice;
  }
});
</script>

<template>
  <div v-if="choices" class="choices">
    <div
      v-for="(choice, order) in choices"
      :key="`${choice.title}-one-choice`"
      class="base-radio"
    >
      <input
        :id="`base-radio-${choice.title}-${order}-${orderOption}`"
        v-model="activeRef"
        type="radio"
        class="base-radio__input"
        :name="optionName"
        :value="choice.value"
      >
      <label
        :for="`base-radio-${choice.title}-${order}-${orderOption}`"
        class="base-radio__title"
      >
        {{ choice.title }}
      </label>
    </div>
  </div>
</template>

<style scoped lang="scss">
@include base-radio;
</style>