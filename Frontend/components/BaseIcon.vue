<script setup lang="ts">
interface IProps {
  name: string,
};
defineProps<IProps>();

const loading = ref(true);

const nuxtApp = useNuxtApp();
nuxtApp.hook("page:finish", () => {
  loading.value = false;
});
</script>

<template>
  <Icon
    v-if="!loading"
    :name="name"
  />
</template>