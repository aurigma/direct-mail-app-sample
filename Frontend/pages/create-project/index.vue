<script setup lang="ts">
definePageMeta({
  middleware: ['create-project-navigate'],
});

const projectsStore = useProjectStore();
const integratedProductsStore = useIntegratedProductsStore();

const errorMessage: Ref<string> = ref('');
const selectCardId: Ref<string> = ref('');

projectsStore.fetchProductCategory();
const productCategoryList = computed(() => projectsStore.getProductCategoryList);

integratedProductsStore.fetchIntegratedProducts({ categoryId: undefined });

const products = computed(() => integratedProductsStore.getIntegratedProducts);

watch(selectCardId, (_) => {
  errorMessage.value = '';
});

function selectCategory(payload: { selected: string, data: string }) {
  integratedProductsStore.fetchIntegratedProducts({ categoryId: payload.data });
};
function updateErrorProduct(err: string) {
  errorMessage.value = err;
};
function selectCard(payload: { id: string }) {
  if (selectCardId.value !== payload.id) {
    selectCardId.value = payload.id;
  }
}
function preSelectFirstProduct(newProductsValue: { id: string }[], oldProductsValue: { id: string }[] | null) {
  if (oldProductsValue === null && !!newProductsValue) {
    const firstProduct = newProductsValue[0];
    selectCard(firstProduct);
  }
}

watch(products, (newProductsValue, oldProductsValue) => {
  preSelectFirstProduct(
    newProductsValue as { id: string }[],
    oldProductsValue
  );
});
</script>

<template>
  <div class="page page__wrapper">
    <h1 class="setup-project__title">Create Project</h1>
    <div class="setup-project__wrap-form">
      <ProjectSetupForm
        :product-id="selectCardId"
        @error:product="updateErrorProduct"
      />
    </div>
    <div class="setup-project__wrap-content">
      
      <SideBar class="setup-project__side-bar">
        <SideBarItem v-if="!!productCategoryList?.length">
          <template #title>Select Your Product</template>
          <template #options>
            <BaseOneChoice
              :choices="productCategoryList"
              :order-option="0"
              @selected="selectCategory"
            />
          </template>
        </SideBarItem>
        <BaseSkeleton
          v-else
          skeleton-type="side-bar"
        />
      </SideBar>

      <div class="setup-project__content">

        <template v-if="!!products">
          <BaseCard
            v-for="(product, productOrder) in products"
            :key="`product-${productOrder}-${product.id}-${product.title}`"
            :active-id="{id: selectCardId}"
            :id-card="{id: product.id}"
            :img="product.img"
            @select-card="selectCard"
          >
            <template #title>
              {{ product.title }}
            </template>
          </BaseCard>
        </template>

        <template v-else>
          <BaseSkeleton
            v-for="i in [0, 1, 2, 4]"
            :key="`${i}-skeleton-create-project`"
            skeleton-type="card"
            title
          />  
        </template>
        
      </div>
    </div>
  </div>
</template>

<style scoped lang="scss">
.setup-project {
  &__title {
    display: flex;
  }
  &__wrap-content {
    display: flex;
    justify-content: space-between;
  }
  &__content {
    width: 100%;
    display: flex;
    justify-content: flex-start;
    flex-wrap: wrap;
    gap: 20px;
    padding-left: 40px;
  }
}
</style>