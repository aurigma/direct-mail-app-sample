<script setup lang="ts">
import type { RecipientDto, RecipientListDto } from '~/core/direct-mail-api-client';
import ModalList from '@/core/modal/list.js';

const approvalStore = useApprovalStore();
const lineItemsStore = useLineItemsStore();
const recipientListStore = useRecipientListStore();

const lineItem = computed(() => lineItemsStore.getLineItem);
const designInfo = computed(() => approvalStore.getDesignInfo);
const proofs = computed(() => approvalStore.getPreview);
const recipientLists = computed(() => recipientListStore.getRecipientLists);
const proofZip = computed(() => approvalStore.getProofZip);

const loadingDownload: Ref<boolean> = ref(false);
const recipientList: Ref<RecipientDto[] | null> = ref(null);
const image: Ref<string | null> = ref(null);
const selectedSurfaceId: Ref<string> = ref('');
const selectedRecipientId: Ref<string> = ref('');

await recipientListStore.fetchRecipientList();

if (lineItem.value) {
  await approvalStore.fetchDesignInfo({ lineItemId: lineItem.value.id as string });
}

if (!!recipientLists.value?.length && !!recipientLists.value) {
  recipientList.value = getFilteredRandomRecipients(recipientLists.value, 10);
}

watch(selectedRecipientId, async (_) => {
  const surfaceCount = designInfo.value?.surfaceCount as number;
  const payload = {
    recipientId: selectedRecipientId.value,
    lineItemId: lineItem.value?.id as string,
    surfaceIdx: createArray(0, surfaceCount),
  };
  await approvalStore.fetchDesignPreview(payload);
}, {deep: true});

watch(proofs, (_) => {
  if (proofs.value?.length) {
    selectedSurfaceId.value = proofs.value[0].id.toString();
    switchSide(proofs.value[0].id);
  }
}, {deep: true});

function openPreviewModal() {
  ShowModal({
    key: ModalList.approvalImagePreview,
    options: {
      image: image.value,
    },
  });
}

function switchSide (side: string | number) {
  const proof = proofs.value.find((proof) => proof.id === side);
  image.value = proof?.img;
};

function getFilteredRandomRecipients(RecipientDataLists: RecipientListDto[], countRecipients: number) {
  const filteredRecipientDataLists = RecipientDataLists.filter((list) => !!list.recipients);
  const filteredRecipientLists = filteredRecipientDataLists.map((list) => list.recipients);
  const recipients: RecipientDto[] = filteredRecipientLists.flat() as RecipientDto[];
  const randomRecipients = getListWithRandomItems<RecipientDto>(recipients, countRecipients);
  return randomRecipients as RecipientDto[];
}

function selectRecipient(recipientId: string) {
  image.value = null;
  selectedRecipientId.value = recipientId;
}

async function downloadProofs () {
  loadingDownload.value = true;
  await approvalStore.fetchProofZip({ lineItemId: lineItem.value?.id as string });
  await navigateTo(proofZip.value, {
    external: true,
    open: {
      target: "_blank",
    },
  });
  loadingDownload.value = false;
};
</script>

<template>
  <div class="download">
    <p class="download__description">
      Please download and review them before continuing.
    </p>
    <div class="download__wrap-button">
      <BaseButton
        tag="button"
        native-type="button"
        style-type="ghost"
        icon-name="fa:download"
        :disabled="loadingDownload"
        :click="downloadProofs"
      >
        Download proofs
      </BaseButton>
      <BaseIcon
        v-show="loadingDownload"
        class="loadingDownload"
        name="tdesign:loading"
      />
    </div>
  </div>
  <div class="content">
    <div class="content__wrap-buttons">
      <template v-if="!!proofs.length">
        <BaseCard
          v-for="card in proofs"
          :key="`${card.id}-card`"
          card-type="mini"
          :active-id="{id: selectedSurfaceId}"
          :id-card="{id: card.id.toString()}"
          :img="card.img"
          @select-card="(payload) => selectedSurfaceId = payload.id"
          @click="switchSide(card.id)"
        />
      </template>
      <BaseSkeleton
        v-for="i in [0, 1, 2]"
        v-else
        :key="`skeleton--mini-card--${i}`"
        skeleton-type="mini-card"
      />
    </div>
    <div class="content__wrap-image">
      <div v-show="image">
        <img
          v-if="image"
          class="content__image"
          :src="image"
          alt="image"
          @click="openPreviewModal"
        >
        <div class="select-recipient">
          <SelectRecipient
            v-if="recipientList?.length"
            :options="recipientList.map((recipient): { value: string, label: string } => {
              return {
                value: String(recipient.id),
                label: String(recipient.fullName),
              }
            })"
            @select="selectRecipient"
          />
        </div>
      </div>
      <BaseSkeleton v-if="!!image === false" skeleton-type="box"/>
    </div>
    
  </div>
</template>

<style lang="scss">
.content {
  width: 100%;
  min-height: 600px;
  height: 100%;
  display: grid;
  grid-template-columns: 120px 1fr;
  gap: 20px;

  padding: 10px 0;

  &__wrap-buttons {
    display: flex;
    flex-direction: column;
    gap: 12px;
  }
  &__wrap-image {
    display: flex;
    justify-content: center;
    align-items: center;
    flex-direction: column;
  }
  &__image {
    cursor: zoom-in;
  }
}
.download {
  display: flex;
  align-items: center;

  &__description {
    font-family: 'Inter';
    font-size: 16px;
    font-weight: 400;
    line-height: 24px;
    letter-spacing: 0em;
    color: #282828;
  }
  &__wrap-button {
    position: relative;
    margin-left: 15px;
    .button-base__ghost {
      color: #666666;
    }
  }
}
.approval-info {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-top: 20px;
  gap: 20px;
  
  &__button {
    width: 400px;
    height: 44px;
  }
  &__description {
    max-width: 768px;
    padding: 0;
    font-family: 'Inter';
    font-size: 16px;
    font-weight: 400;
    line-height: 24px;
    letter-spacing: 0em;
    text-align: left;
    color: #304050;
  }
}

.loadingDownload {
  position: absolute;
  width: 38px;
  height: 38px;
  color: #3FBEAC;
  animation: circle 3s linear infinite;
  right: -38px;
  top: 3px;
}
@keyframes circle {0% {transform: rotate(0deg);} 100% {transform: rotate(360deg);}}
</style>