<script setup lang="ts">
const projectStore = useProjectStore();
projectStore.fetchProjectsData();

const promotionTable = computed(() => {
  const data = (projectStore.getProjectsTableData === null) ? [] : projectStore.getProjectsTableData;

  return {
    columns: [
      {
        key: 'name',
        label: 'Name'
      },
      {
        key: 'status',
        label: 'Status'
      },
      {
        key: 'edit',
      }
    ],
    data,
  };
});
</script>

<template>
  <div class="page page__wrapper projects">
    <div class="projects__title">
      <h1>Projects</h1>
      <base-button
        tag="button"
        type="button"
        native-type="button"
        style-type="primary"
        :click="() => navigateTo('/create-project/')"
      >
        New Project
      </base-button>
    </div>
    
    <div class="projects__table">
      <UTable
        :empty-state="{ icon: 'i-heroicons-circle-stack-20-solid', label: 'No items.' }"
        :rows="promotionTable.data"
        :columns="promotionTable.columns"
        class="specific w-full"
        :ui="{ td: { base: 'modification-for-project' } }"
      >
        <template #status-data="{ row }">
          <base-badge :status="row.status">{{ row.status }}</base-badge>
        </template>
        <template #edit-data="{ row }">
          <base-button
            tag="button"
            type="button"
            native-type="button"
            style-type="tretiary"
            :click="() => navigateTo(`/update-design/${row.lineItemId}/`)"
          >
            Edit
          </base-button>
        </template>
      </UTable>
    </div>

  </div>
</template>

<style scoped lang="scss">
.projects {
  &__title {
    display: flex;
    align-items: center;
    justify-content: space-between;
    padding: 0 0 24px 0;

    button {
      width: 320px;
    }
  }
} 
</style>