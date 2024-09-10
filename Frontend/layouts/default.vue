<script setup lang="ts">
const companyStore = useCompanyStore();

const companyIdIsNotDefine: boolean = !!companyStore.getCompanyId === false;

if (companyIdIsNotDefine) {
  companyStore.fetchCompany();
}
</script>

<template>
<div
  class="default-layout"
>
  <Header />

  <div class="wrapper base-workflow">
    <Workflow />
  </div>

  <DevLoading />
  
  <div class="wrapper">
    <slot />
  </div>
  
  <WarningAlert />
  
  <BaseModalContainer />
</div>
</template>

<style scoped lang="scss">
.default-layout {
  display: flex;
  flex-direction: column;
  align-items: center;
  min-height: 100vh;
  background: $pages-background;
}
.base-workflow {
  margin: 20px 0;
}
</style>