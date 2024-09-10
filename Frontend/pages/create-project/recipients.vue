<!-- eslint-disable @typescript-eslint/no-explicit-any -->
<!-- eslint-disable @typescript-eslint/no-dynamic-delete -->
<script setup lang="ts">
import ModalList from '@/core/modal/list';
import { startCase, camelCase } from 'lodash';

interface IRecipientsTableData {
  title: string;
  rows: {
    [index: string]: string;
  }[];
  columns: {
    key: string;
    label: string;
  }[];
}

definePageMeta({
  middleware: ['create-project-navigate'],
});

const route = useRoute();

const recipientListStore = useRecipientListStore();
const projectsStore = useProjectStore();
const lineItemsStore = useLineItemsStore();

await recipientListStore.fetchRecipientList();

const recipientLists = computed(() => recipientListStore.getRecipientLists);
const lineItem = computed(() => lineItemsStore.getLineItem)

const isDisabledButton: Ref<boolean> = ref(false);

async function updateProject() {
  try {
    if (!!recipientLists.value && !!lineItem.value?.campaignId) {
      isDisabledButton.value = true;
      const recipientListIds = recipientLists.value.map((list) => list.id) as string[];
      await projectsStore.updateProjectRecipients({
        recipientListIds: recipientListIds,
        projectId: lineItem.value?.campaignId,
      });
      try {
        await recipientListStore.submitRecipientList({
          body: {
            campaignId: lineItem.value?.campaignId,
            recipientListIds: recipientListIds,
          },
        });
      } catch (err) {
        console.error(err);
        return abortNavigation();
      }
    }
    navigateTo(`/create-project/customize?lineItemId=${route.query.lineItemId}`);
  } catch (err) {
    console.error(err);
    return abortNavigation();
  }
};

function openModalInfo(listId: string) {
  if (!!recipientLists.value === false) throw new Error('Recipients lists not found');

  const recipientList = recipientLists.value.find((list) => list.id === listId);

  if (!!recipientList?.recipients === false) throw new Error('Recipients list not found');

  const tableData = getTableData(recipientList);

  const { title, rows, columns } = removeColumn('id', tableData);

  ShowModal({
    key: ModalList.infoRecipients,
    options: {
      title,
      rows,
      columns,
    },
  });
}

function removeColumn(columnKey: string, payload: IRecipientsTableData): IRecipientsTableData {
  const columns = payload.columns.filter((column) => column.key !== columnKey);
  const rows = payload.rows.map((row) => {
    delete row[columnKey];
    return row;
  });
  const title = payload.title;
  return { title, rows, columns };
}

function getTableData(recipientList: any): IRecipientsTableData {
  const title = recipientList?.title;
  const rows = recipientList?.recipients;
  const columnList = Object.keys(recipientList?.recipients[0]);
  const columns = columnList.map((column: string) => {
    const label = startCase(camelCase(column));
    return {
      key: column,
      label,
    };
  });

  return {
    title: String(title),
    rows: rows as { [index: string]: string }[],
    columns: columns as {
      key: string,
      label: string,
    }[],
  };
}
</script>

<template>
  <div class="page page__wrapper">
    <h1 class="settings__title">Recipients</h1>
    <h2>Available lists</h2>
    <div
      v-if="!!recipientLists?.length"
      class="settings__wrap-lists"
      >
      <BaseList
        v-for="list in recipientLists" 
        :id="String(list.id)"
        :key="`${list.id}-recipients-list`"
        :title="String(list.title)"
        :description="list.recipients?.length + ' Recipients'"
        :options="list.recipients"
      >
        <BaseButton
          tag="button"
          native-type="button"
          style-type="tretiary"
          :click="() => openModalInfo(String(list.id))"
        >
          View
        </BaseButton>
      </BaseList>
    </div>
    <div class="settings__wrap-button">
      <BaseButton
        tag="button"
        native-type="button"
        style-type="primary"
        :click="updateProject"
        class="settings__button"
        :disabled="isDisabledButton"
      >
        Start design
      </BaseButton>
    </div>
  </div>
</template>

<style scoped lang="scss">
.settings {
  &__title {
    display: flex;
    margin-bottom: 40px;
  }
  &__wrap-lists {
    display: flex;
    flex-direction: column;
    gap: 20px;
  }
  &__wrap-button {
    width: 100%;
    display: flex;
    justify-content: center;
    margin-top: 40px;
  }
  &__button {
    width: 550px;
  }
}
</style>