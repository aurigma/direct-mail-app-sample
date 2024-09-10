<!-- eslint-disable @typescript-eslint/no-explicit-any -->
<script setup lang="ts">
import { JobStatus, type JobDto } from '~/core/direct-mail-api-client';

definePageMeta({
  middleware: ['update-design-navigate'],
});

const lineItemsStore = useLineItemsStore();
const integratedProductsStore = useIntegratedProductsStore();
const approvalStore = useApprovalStore();
const jobStore = useJobStore();

const jobs = computed(() => jobStore.getJobs);
const downloadLinks = computed(() => jobStore.getDownloadLinks);
const lineItem = computed(() => (lineItemsStore.getLineItem) ? lineItemsStore.getLineItem : { id: '' });
const productDetails = computed(() => integratedProductsStore.getTemplateInfo);
const designInfo = computed(() => approvalStore.getDesignInfo);
const preview = computed(() => approvalStore.getPreview);

const jobHistory = ref({
  columns: [
    {
      key: 'id',
      label: 'Job ID'
    },
    {
      key: 'status',
      label: 'Status'
    },
    {
      key: 'download',
    },
    {
      key: 'restart',
    },
  ],
  tableData: [] as JobDto[],
});
const isActiveButtons: any = {
  [JobStatus.Completed]: { download: true, restart: false },
  [JobStatus.Failed || 'Discarded']: { download: false, restart: true },
  [JobStatus.InProgress]: { download: false, restart: false },
  [JobStatus.Pending || 'New']: { download: false, restart: false },
}

await integratedProductsStore.fetchIntegratedProductTemplateDetails({
  id: lineItem.value.productId as string, 
  templateId: lineItem.value.templateId as string, 
  productVariantId: lineItem.value.productVariantId as number,
});

watch(designInfo, async (_) => {
  if (designInfo.value) {
    const surfaceCount = designInfo.value?.surfaceCount as number;
    await approvalStore.fetchDesignPreview({ lineItemId: lineItem.value?.id as string, surfaceIdx: createArray(0, surfaceCount) });
  }
}, {deep: true});

jobStore.fetchJobs({ lineItemId: lineItem.value.id as string });
approvalStore.fetchDesignInfo({ lineItemId: lineItem.value.id as string });

watch(jobs, (jobsValue) => {
  if (jobsValue?.length) jobHistory.value.tableData = jobsValue;
}, {deep: true});

const recreateCCJob = async () => {
  jobHistory.value.tableData.push({
    id: '...',
    status: JobStatus.Pending,
  });
  await approvalStore.finishPersonalization({ id: lineItem.value.id as string })
    .finally(() => {
      jobStore.fetchJobs({ lineItemId: lineItem.value.id as string });
    });
};
const jobDownload = async (jobId: string) => {
  await jobStore.fetchDownloadResult({ jobId: jobId });
  downloadLinks.value?.forEach(async (link) => {
    await navigateTo(link, {
      external: true,
      open: {
        target: "_blank",
      },
    });
  });
};
const jobRestart = async (jobId: string) => {
  await jobStore.restartJob({ lineItemId: lineItem.value.id as string, jobId: jobId });
}
</script>

<template>
  <div class="page page__wrapper update-design">
    <div class="update-design__gallery">
      <PreviewGallery
        v-if="preview?.length"
        :image-list="preview.map(item => item.img)"
        height="auto"
      />
      <div
        v-else  
        class="update-design__gallery-skeleton"
      >
        <div class="update-design__gallery-skeleton_box">
          <BaseSkeleton skeleton-type="box"/>
        </div>
        <div class="update-design__gallery-skeleton_mini">
          <BaseSkeleton skeleton-type="mini-card"/>
          <BaseSkeleton skeleton-type="mini-card"/>
          <BaseSkeleton skeleton-type="mini-card"/>
        </div>
      </div>
    </div>

    <div class="update-design__info">
      <h1 class="info__title">Current Design</h1>

      <h3 class="info__template-name">{{ productDetails?.templateName }}</h3>
      <div class="info__description">
        <p>{{ productDetails?.customFields?.description }}</p>
      </div>

      <div class="info__buttons">
        <base-button
          tag="button"
          type="button"
          native-type="button"
          style-type="primary"
          :click="() => navigateTo(`/update-design/${lineItem.id}/customize`)"
        >
          Update design
        </base-button>
        <base-button
          tag="button"
          type="button"
          native-type="button"
          style-type="tretiary"
          :click="recreateCCJob"
        >
          Recreate cc job
        </base-button>
      </div>
    </div>
    
    <div class="update-design__wrap-table">
      <h2>CC Jobs history</h2>
      <UTable
        :empty-state="{ icon: 'i-heroicons-circle-stack-20-solid', label: 'No items.' }"
        :rows="jobHistory.tableData"
        :columns="jobHistory.columns"
        class="specific w-full"
        :ui="{ td: { base: 'modification-for-project' } }"
      >
        <template #status-data="{ row }">
          <base-badge :status="row.status">{{ row.status }}</base-badge>
        </template>
        <template #download-data="{ row }">
          <button
            class="button-base button-base__ghost update-design__table-button"
            :disabled="!isActiveButtons[row.status]?.download"
            @click="jobDownload(row.id)"
          >
            <Icon
              class="icon button-base__icon-left"
              name="fa:download"
            />
            Download
          </button>
        </template>
        <template #restart-data="{ row }">
          <base-button
            tag="button"
            type="button"
            native-type="button"
            style-type="tretiary"
            class="update-design__table-button"
            :click="() => jobRestart(row.id)"
            :disabled="!isActiveButtons[row.status]?.restart"
          >
            Restart
          </base-button>
        </template>
      </UTable>
    </div>
    
  </div>
</template>

<style scoped lang="scss">
@include button-base;
.update-design {
  display: grid;
  gap: 40px;

  &__gallery {
    grid-area: gallery;
  }

  &__info {
    grid-area: info;
    display: flex;
    flex-direction: column;
    align-items: center;
  }

  &__wrap-table {
    grid-area: wrap-table;
    > h2 {
      text-align: left;
      font-family: 'Inter';
      font-size: 32px;
      font-style: normal;
      font-weight: 700;
      line-height: 40px;
      text-transform: capitalize;
      margin-bottom: 20px;
      margin-top: 10px;
    }
  }

  &__gallery-skeleton {
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
    
    &_box {
      width: 100%;
      height: 300px;
    }
    &_mini {
      display: flex;
      align-items: center;
      justify-content: center;
      margin-top: 10px;
      gap: 5px;
    }
  }

  &__table-button {
    height: 44px;
  }

  grid-template-columns: 1fr 1fr;
  grid-template-areas:
    "gallery info"
    "wrap-table wrap-table";
}
.info {
  &__description {
    text-wrap: wrap;
    overflow-wrap: break-word;
  }
  &__template-name {
    padding: 20px 0;
    color: #808080;
    text-align: center;
    font-family: 'Inter';
    font-size: 18px;
    font-style: normal;
    font-weight: 400;
    line-height: 28px;
  }
  &__buttons {
    display: flex;
    flex-direction: column;
    width: 100%;
    gap: 10px;
    padding-top: 30px;
  }
}
</style>