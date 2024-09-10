<script setup lang="ts">
import { useLoadingStore } from "@/stores/loading.store";

const store = useLoadingStore();

const loading = ref(true);

const nuxtApp = useNuxtApp();
nuxtApp.hook("page:finish", () => {
  loading.value = false;
});
</script>

<template>
  <div
    v-if="!loading && store.getStatus"
    class="loading"
  />
</template>

<style scoped lang="scss">
.loading {
  display: flex;
  position: fixed;
  min-width: 100%;
  height: 5px;
  align-items: center;
  justify-content: center;
  z-index: calc(10000000);

  background: linear-gradient(-25deg, #60a3fa, #23528f, #1379ff, #60a3fa, #a6cdff);
  background-size: 400% 400%;
  animation: gradient 3s ease infinite;
}

@keyframes gradient {
	0% {
		background-position: 0% 50%;
	}
	50% {
		background-position: 100% 50%;
	}
	100% {
		background-position: 0% 50%;
	}
}
</style>