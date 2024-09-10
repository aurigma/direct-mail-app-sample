<script setup lang="ts">
  import { useModalStore } from '@/stores/modal.store';
  import { vOnClickOutside } from '@/core/directive';
  import ModalList from '@/core/modal/list'; 

  const storeModal = useModalStore();
  const options = storeModal.getOptions;

  const isLoadedModal = ref(false);

  const currentModal = computed(() => storeModal.currentModalKey);
  const isVisible = computed(() => storeModal.getIsVisible);
  const isLoading = computed(() => {
    if (currentModal.value === ModalList.productTemplate) {
      return isLoadedModal.value;
    } else {
      return false;
    }
  });

  watch(currentModal, (value) => {
    initLoadingValue(value);
  });

  let isClosable: boolean = false;
  let timer: NodeJS.Timeout | null;
  const timerDelay: number = 200;
  watch(isVisible, (value) => {
    if (value) {
      if (timer) {
        clearTimeout(timer);
        timer = null;
      }
      timer = setTimeout(() => {
        isClosable = true;
      }, timerDelay);
    } else {
      isClosable = false;
    }
  })

  function backgroundClick() {
    if (!options.isUnclosable && isClosable) {
      CloseModal()
    }
  };

  function modalLoad() {
    isLoadedModal.value = false;
  }

  function initLoadingValue(current: string) {
    if (current === ModalList.productTemplate) {
      isLoadedModal.value = true;
    } 
  }
</script>

<template>
  <transition name="fade">
    <div
      v-if="isVisible"
      class="modal-background"
    >
      <div
        v-on-click-outside.bubble="backgroundClick"
        class="base-modal"
      >
        <DefaultModal v-if="currentModal === ModalList.default" />
        <ProductTemplateModal
          v-if="currentModal === ModalList.productTemplate"
          @loaded="modalLoad"
        />
        <ChooseTemplateAlertModal v-if="currentModal === ModalList.chooseTemplateAlert" />
        <ValidateVDPAlertModal v-if="currentModal === ModalList.validateVDPAlert" />
        <InfoRecipientsModal v-if="currentModal === ModalList.infoRecipients" />
        <ApprovalImagePreviewModal v-if="currentModal === ModalList.approvalImagePreview" />

        <BlockLoader v-if="isLoading" />
      </div>
    </div>
  </transition>
</template>

<style scoped lang="scss">
.base-modal {
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;

  border-radius: 16px;
  background: #FFF;
  box-shadow: 0px 9px 46px 0px rgba(0, 0, 0, 0.12), 0px 24px 38px 0px rgba(0, 0, 0, 0.14);
  color: black;
  min-height: 200px;
  min-width: 200px;
  padding: 10px;
}
.modal-background {
  display: flex;
  justify-content: center;
  align-items: center;

  background-color: rgba(0, 0, 0, 0.40);

  width: 100vw;
  height: 100vh;

  position: fixed;
  z-index: calc(10000000);
}
</style>