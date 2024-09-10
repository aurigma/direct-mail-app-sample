<script setup lang="ts">
const router = useRouter();
const route = useRoute();

const lineItemsStore = useLineItemsStore();
const lineItem = computed(() => lineItemsStore.getLineItem);

const progressBar = ref({
  countTabs: 0,
  currentIndex: 0,
});

const progressReference: Ref<HTMLElement | null> = ref(null);
const workflowName: Ref<TWorkflowName | null> = ref(null);

const workflows = computed<{ [key in TWorkflowName]: IWorkflowTab[] }>(() => workflowService.getWorkflows(router, lineItem.value?.id));
const tabList = computed<IWorkflowTab[] | undefined>(() => getCurrentWorkflowWithFilledStatuses());

function getCurrentWorkflowWithFilledStatuses() {
  const { currentWorkflow, currentWorkflowName } = workflowService.getCurrentWorkflow(workflows.value, route.name as TRoute['name']);
  if (!!currentWorkflow && !!currentWorkflowName) {
    workflowName.value = currentWorkflowName;
    const activeTabIndex = workflowService.getTabIndex(currentWorkflow, route.name as TRoute['name']);
    const currentWorkflowWithFilledStatuses = getCurrentWorkflowWithStatuses(currentWorkflow, activeTabIndex);
    setProgressBarValues();
    return currentWorkflowWithFilledStatuses;
  }
}

function getCurrentWorkflowWithStatuses(currentWorkflow: IWorkflowTab[], activeTabIndex: number | null) {
  if (!!activeTabIndex || activeTabIndex === 0) {
    const currentWorkflowWithStatuses = workflowService.changeActiveTab(currentWorkflow, activeTabIndex);
    return currentWorkflowWithStatuses;
  }
}

function setProgressBarValues() {
  progressBar.value.currentIndex = workflowService.activeTabIndex;
  progressBar.value.countTabs = workflowService.currentWorkflowTabsCount;
  if (progressReference.value) {
    const tabOrder = progressBar.value.currentIndex + 1;
    const percentile = tabOrder * (100 / progressBar.value.countTabs);
    progressReference.value.style.width = `${percentile}%`;
  }
}
</script>

<template>
  <div
    v-if="!!tabList"
    class="workflow"  
  >
    <div
      class="workflow__tab-list"
      :class="[
        { 'workflow__tab-list_create-project': workflowName === 'createProject' },
        { 'workflow__tab-list_update-design': workflowName === 'updateDesign' },
      ]"  
    >
      <BaseTab
        v-for="tab in tabList"
        :key="`${tab.sortIndex}-${tab.label}`"
        :status="tab.status"
        :to="tab.to"
      >
        {{ tab.label }}
      </BaseTab>
    </div>
    <div class="workflow__progress-bar-wrap">
      <div class="workflow__progress-bar">
        <div
          ref="progressReference"
          class="progress"
        />
      </div>
    </div>
  </div>
</template>

<style scoped lang="scss">
.workflow {
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;

  &__tab-list {
    display: grid;
    width: 100%;
    &_create-project {
      grid-template-columns: repeat(5, 1fr);
    }
    &_update-design {
      grid-template-columns: repeat(2, 1fr);
    }
  }

  &__progress-bar-wrap {
    display: flex;
    width: 100%;
  }

  &__progress-bar {
    width: 100%;
    height: 8px;
    background-color: #E0E0E0;
    border-radius: 4px;
  }

  .progress {
    height: 8px;
    border-radius: 4px;
    background-color: #0090FF;
    transition: all .2s ease-in-out;
  }
}
</style>