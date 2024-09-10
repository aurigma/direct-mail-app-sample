<script setup lang="ts">
const storeModal = useModalStore();
const modalOptions = storeModal.getOptions;

const tableRecipients = computed(() => {
  return {
    rows: modalOptions.rows,
    columns: modalOptions.columns,
  };
});
</script>

<template>
  <BaseModalBox :has-header="false">
    <div class="modal">
      <div class="recipients-custom-header">
        <h4 class="recipients-custom-header__title">{{ modalOptions?.title }}</h4>
        <button
          class="button-base button-base__icon base-modal__close"
          @click="CloseModal"
        >
          <Icon
            name="uiw:close"
          />
        </button>
      </div>
      <div class="recipients-table">
        <UTable
          :empty-state="{ icon: 'i-heroicons-circle-stack-20-solid', label: 'No items.' }"
          :rows="tableRecipients.rows"
          :columns="tableRecipients.columns"
          class="recipients-table__table specific w-full"
          :ui="{ td: { base: 'modification-for-project' } }"
        >
          <template #qrCodeUrl-data="{ row }">
            <a
              target="_blank"
              :href="row.qrCodeUrl"
              class="recipients-table__links"
            >
              Check
            </a>
          </template>
        </UTable>
      </div>
    </div>
  </BaseModalBox>
</template>

<style scoped lang="scss">
@include button-base;

.modal {
  max-width: 90vw;
  max-height: 90vh;
}

.recipients-custom-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 16px 18px 16px 30px;

  &__title {
    font-size: 24px;
    font-weight: 600;
    line-height: 32px;
    text-align: left;
  }
}

.recipients-table {
  padding: 0 30px 30px 30px;

  &__table {
    max-height: 80vh;
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
  }

  &__links {
    color: #0090FF;
    font-size: 16px;
    font-weight: 700;
    line-height: 24px;
  }
}
</style>