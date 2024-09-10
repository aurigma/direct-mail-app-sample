<script setup lang="ts">
const storeModal = useModalStore();

const modalOptions = storeModal.getOptions;

const isError = computed(() => !!modalOptions.error);

const acceptAlert = async () => {
  navigateTo(modalOptions?.to);
  CloseModal();
};
</script>

<template>
  <BaseModalBox :has-header="false">
    <div class="modal">
      <p v-if="isError" class="modal__message">{{ modalOptions.error }}</p>
      <div class="modal__text-content">
        <template v-if="modalOptions?.missingListVariableNames.length && !isError">
          <p class="modal__message">The following variable fields are missing from the design but present in the list:</p>
          <ol class="modal__list">
            <li
              v-for="(field, orderField) in modalOptions.missingListVariableNames"
              :key="`missing-list-variable-names-${field}-${orderField}`"
            >
              {{ field }}
            </li>
          </ol>
        </template>
        
        <template v-if="modalOptions?.missingDesignVariableNames.length && !isError">
          <p class="modal__message">The following variable fields are missing from the list but present in the design:</p>
          <ol class="modal__list">
            <li
              v-for="(field, orderField) in modalOptions.missingDesignVariableNames"
              :key="`missing-design-variable-names-${field}-${orderField}`"
            >
              {{ field }}
            </li>
          </ol>
        </template>
      </div>
      
      <p v-if="!isError" class="modal__message">They may have been removed or mistyped. Are you sure that you want to proceed without them?</p>
      
      <div class="modal__wrap-buttons">
        <base-button
          tag="button"
          native-type="button"
          style-type="tretiary"
          :click="CloseModal"
          class="modal__button"
        >
          Close
        </base-button>
        <base-button
          tag="button"
          native-type="button"
          style-type="primary"
          :click="acceptAlert"
          class="modal__button"
        >
          Continue
        </base-button>
      </div>
    </div>
  </BaseModalBox>
</template>

<style scoped lang="scss">
.modal {
  display: flex;
  flex-direction: column;
  justify-content: space-between;
  padding: 10px;
  color: black;
  width: 480px;  

  &__text-content {
    max-height: 60vh;
    overflow-y: auto;

    &::-webkit-scrollbar-track {
      border-radius: 10px;
      background-color: #F5F5F5;
    }
    &::-webkit-scrollbar {
      width: 3px;
      background-color: #F5F5F5;
    }
    &::-webkit-scrollbar-thumb {
      border-radius: 10px;
      background-color: #88888865;
    }
  }
  &__message {
    color: #000000;
    font-size: 16px;
    font-weight: 400;
    line-height: 20px;
    margin: 10px 0;
  }
  &__wrap-buttons {
    width: 100%;
    display: flex;
    gap: 20px;
  }
  &__button {
    width: 100%;
    height: 44px;
    margin-top: 20px;
    
    font-family: 'Inter';
    font-size: 16px;
    font-weight: 600;
    line-height: 24px;
  }
  &__list {
    list-style-type: decimal;
    padding-left: 50px;
    li {
      font-family: 'Inter';
      font-size: 16px;
      font-weight: 400;
      line-height: 20px;
      text-align: left;
      padding: 2px 0;
    }
  }
}
</style>