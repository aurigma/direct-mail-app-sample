<script setup lang="ts">
import { IntegratedProductOptionType } from '~/core/direct-mail-api-client';

interface IChoiceData {
  data: number | 'all',
  order: number,
}

interface IChoice extends IChoiceData {
  type: 'choice' | 'chips',
  optionId: number,
  optionName: string,
  optionType: string,
};

interface IOption {
  optionId: number,
  optionName: string,
  optionType: string,
  valueIds: number[],
};

interface IProps {
  productId: string,
  templateTitleUpdate: string,
};

const props = defineProps<IProps>();

const emit = defineEmits(['selected']);

const integratedProductsStore = useIntegratedProductsStore();

const templateTitle = ref('');

watch(() => props.templateTitleUpdate, (value) => {
  templateTitle.value = value;
}, { deep: true, immediate: true });

integratedProductsStore.fetchOptions({ id: props.productId });
const productOptionsList = computed(() => integratedProductsStore.getProductOptionsList);

const selectedOptions: Ref<IOption[]> = ref([]);

function optionValueUpdate(
  payload: IChoice,
  dataOption: IOption,
  valueField: keyof IChoiceData,
  clear?: string
) {
  const optionId = selectedOptions.value.indexOf(dataOption);
  const valueIds = selectedOptions.value[optionId].valueIds as (number | 'all')[];
  const dataField = payload[valueField];
  const hasDataFromField = valueIds.includes(dataField);
  const hasClearMode = !!clear;

  if (!hasDataFromField && !hasClearMode) {
    valueIds.push(dataField);
  } else {
    const optionValueId = valueIds.indexOf(payload[valueField]);
    valueIds.splice(optionValueId, 1);
  }
}

// [payload.type] BaseOneChoice = 'choice';
// [payload.type] BaseChips = 'chips';
function selectOptions(payload: IChoice) {
  let option = selectedOptions.value.find((dataOption) => dataOption.optionId === payload.optionId);

  if (option) {
    if (payload.type === 'chips') optionValueUpdate(payload, option, 'order');
    if (payload.type === 'choice') {
      optionValueUpdate(payload, option, 'data', 'clear');
      if (payload.data !== 'all') optionValueUpdate(payload, option, 'data');
    }
  } else {
    selectedOptions.value.push({
      optionId: payload.optionId,
      optionName: payload.optionName,
      optionType: payload.type,
      valueIds: [],
    });
    option = selectedOptions.value.find((dataOption) => dataOption.optionId === payload.optionId);
    if (option) {
      if (payload.type === 'chips') optionValueUpdate(payload, option, 'order');
      if (payload.type === 'choice') optionValueUpdate(payload, option, 'data');
    }
  }

  if (option) {
    const selectedOptionId = selectedOptions.value.indexOf(option);
    const selectedOption = selectedOptions.value[selectedOptionId];
    const hasNoValueSelectedOption = selectedOption?.valueIds[0] === undefined;

    if (hasNoValueSelectedOption) {
      selectedOptions.value.splice(selectedOptionId, 1);
    }
  }
  
  ExecutionDelay.add('choose-option', () => {
    integratedProductsStore.fetchIntegratedProductsTemplateByProductId({
      id: props.productId,
      body: {
        templateTitle: templateTitle.value,
        options: selectedOptions.value
      }
    });
    emit('selected', selectedOptions.value);
  }, 700);
};

// One chips (envelope) option should always be enabled
const disabledChipsId = computed(() => {
  const chips = selectedOptions.value.find(searchLonelyChips);
  const chipsIds = chips?.valueIds[0];
  return chipsIds;
});

function searchLonelyChips(option: IOption) {
  return option.optionType === 'chips' && option.valueIds.length === 1;
}

</script>

<template>
  <SideBar class="choose-template-side-bar">
    <template v-if="!!productOptionsList?.length">
      <SideBarItem
        v-for="(option, order) in productOptionsList"
        :key="`choose-template-side-bar-option-${order}`"
      >
        <template #title>{{ option.title }}</template>
        <template #options>
          <template v-if="!!option.values && option.optionType === IntegratedProductOptionType.Radio">
            <!-- TODO: option.optionType === IntegratedProductOptionType.Radio - from BackOffice -->
            <BaseOneChoice
              :choices="option.values.map((item: any) => {
                return {
                  title: item.title,
                  value: item.id,
                }
              })"
              :order-option="order"
              :option-id="(option.id as number)"
              :option-name="(option.title as string)"
              @selected="selectOptions"
            />
          </template>
          <template v-else>
            <!-- TODO: option.optionType === IntegratedProductOptionType.Chips or other option type - hardcode -->
            <div
              v-if="!!option.values"
              class="choose-template-side-bar__wrap-chips"
            >
              <BaseChips
                v-for="(item, pos) in option.values.map((item: any) => {
                  return {
                    label: item.title,
                    order: item.id,
                    name: item.title,
                    value: false,
                    optionId: option.id,
                    optionName: option.title,
                  }
                })"
                :key="`${item.name}-${item.label}-choose-template-chips`"
                :disabled-order="disabledChipsId"
                :is-active="pos === 0"
                :label="item.label"
                :name="item.name"
                :order="item.order"
                :option-id="(item.optionId as number)"
                :option-name="(item.optionName as string)"
                :value="(pos === 0) ? true : item.value"
                @update:value="selectOptions"
              />
            </div>
          </template>
        </template>
      </SideBarItem>
    </template>

    <BaseSkeleton
      v-else
      skeleton-type="side-bar"
    />
  </SideBar>
</template>

<style scoped lang="scss">
.choose-template-side-bar {

  &__wrap-chips {
    display: flex;
    gap: 10px;
    flex-wrap: wrap;
  }
}
</style>