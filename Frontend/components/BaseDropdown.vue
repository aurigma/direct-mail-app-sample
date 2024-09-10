<script setup lang="ts">
import type { OnClickOutsideHandler } from '@vueuse/core';
import { vOnClickOutside } from '@/core/directive';

interface IProps {
  name: string,
  placeholder?: string,
  noIcon?: boolean,
  options?: {
    value: string,
    label: string,
  }[],
  error?: string,
  initialValueId?: number,
  direction?: ('up' | 'down'), 
}
const props = defineProps<IProps>();

const isOpen = ref(false);
const dropdownHandler: OnClickOutsideHandler = () => { isOpen.value = false };

const emit = defineEmits(['update:dropdown-value', 'update:init-value']);

const inputValue = ref<string>('');
const label = ref<string>(props.placeholder || '');

const hasIcon = computed(() => (props.noIcon) ? false : true);

const updateDropdownData = (updatedValue: string, updatedLabel: string) => {
  inputValue.value = updatedValue;
  label.value = updatedLabel;
  isOpen.value = false;
  emit('update:dropdown-value', updatedValue);
};

const setValueFromInitialValueId = (id: number) => {
  if (!!id.toString() && !!props.options) {
    inputValue.value = props.options[id].value;
    label.value = props.options[id].label;
    isOpen.value = false;
    emit('update:init-value', props.options[id].value);
  }
};

if (props?.initialValueId?.toString()) {
  setValueFromInitialValueId(props.initialValueId);
}

watch(() => props.initialValueId, (id) => {
  setValueFromInitialValueId(id as number);
}, { deep: true });

function toggleDropdown() {
  isOpen.value = !isOpen.value;
}
</script>

<template>
  <div
    class="dropdown"
  >
    
    <button
      class="dropdown-toggle no-caret"
      :class="[
        { 'dropdown-toggle--up': noIcon },
        { 'dropdown-toggle--error': !!error },
      ]"
      type="button"
      @click.stop="toggleDropdown"
    >
      <span
        class="dropdown-toggle__label"
        :class="[
          { 'dropdown-toggle__label_placeholder': !inputValue },
        ]"
      >
        {{ label }}
      </span>
      <img
        v-if="hasIcon"
        class="dropdown-toggle__icon"
        :class="[
          {'dropdown-toggle__icon_rotated': isOpen}
        ]"
        src="/img/icons/dropdown_down.svg"
        alt="dropdown-arrow-down"
      >
    </button>
    
    <input
      v-model="inputValue"
      type="text"
      class="hide"
      :name="name"
    >
    
    <transition name="move-down">
      <ul
        v-show="isOpen"
        v-on-click-outside.bubble="dropdownHandler"
        class="dropdown-inner"
        :class="[
          { 'dropdown-inner--up': direction === 'up' }
        ]"
      >
        <li
          v-for="(item, itemOrder) in options"
          :key="`${item.label}-options-dropdown`"
        >
          <button
            :key="`dropdown-${name}-${itemOrder}`"
            class="dropdown-item__button"
            @click.stop="updateDropdownData(item.value, item.label)"
          >
            {{ item.label }}
          </button>
        </li>
      </ul>
    </transition>
  </div>
</template>

<style scoped lang="scss">
.dropdown {
  position: relative;
  cursor: pointer;
  user-select: none;

  .dropdown-toggle {
    display: flex;
    justify-content: space-between;
    align-items: center;

    width: 100%;
    padding: 9px 12px 9px 16px;

    border-radius: 4px;
    border: 1px solid #AEB3B7;
    background: #FFF;

    overflow: hidden;
    text-overflow: ellipsis;
    

    &__label {
      font-family: 'Inter';
      color: #282828;
      font-size: 16px;
      font-weight: 400;
      line-height: 24px;

      &_placeholder {
        color: #ADADAD;
      }
    }

    &--up {
      background: #E9EDF5;
      padding: 2px, 4px, 2px, 4px;
      border: none;
      justify-content: center;
      height: 30px;
      letter-spacing: 0em;
    }

    &--error {
      border-color: #F81616;
    }
    
    &__icon {
      padding: 0 0 0 4px;
      transform: rotateX(0deg);
      transition: transform .4s;
      &_rotated {
        transform: rotateX(180deg);
      }
    }
  }

  .dropdown-inner {
    display: flex;
    flex-direction: column;
    color: black;
    padding: 8px 0;
    box-shadow: 0px 5px 14px 0px rgba(0, 0, 0, 0.12), 0px 9px 10px 0px rgba(0, 0, 0, 0.14), 0px 5px 5px 0px rgba(0, 0, 0, 0.20);
    max-height: 390px;
    overflow-y: auto;
    position: absolute;
    z-index: 100;
    background: white;
    width: 100%;
    border-radius: 5px;
    margin-top: 5px;

    scroll-padding: 0;

    &::-webkit-scrollbar-track {
      background-color: white;
      border-radius: 10px;
    }

    &::-webkit-scrollbar {
      width: 20px;
      background-color: #F5F5F5;
    }

    &::-webkit-scrollbar-thumb {
      border-radius: 10px;
      background-color: #ADADAD;
      border: 6px solid white;
    }

    &--up {
      color: #304050;
      top: 0;
      margin-top: 0;
      transform: translateY(calc(-100% - 5px));
      box-shadow: 0px 2px 6px 0px #00000029;
      padding: 5px 0;
      .dropdown-item__button {
        padding: 4px 0px;
        justify-content: center;
        &:hover {
          background: #E9EDF5;
        }
      }
    }
  }

  .dropdown-item {
    &__button {
      display: flex;
      width: 100%;
      padding: 8px 16px;
      font-family: 'Inter';
      font-size: 16px;
      font-weight: 400;
      line-height: 24px;
    }
  }
}

.error {
  color: red;
}
.hide {
  visibility: hidden;
  position: absolute;
  left: -1000px;
}
</style>