<!-- eslint-disable vue/require-default-prop -->
<script setup lang="ts">
import type { NuxtError } from '#app'

defineProps({
  error: Object as () => NuxtError
})
</script>

<template>
  <div class="error">
    <h1>{{ error?.statusCode }}</h1>
    <p>{{ error?.statusMessage }}</p>
    <NuxtLink to="/">Go home</NuxtLink>
  </div>
</template>

<style scoped lang="scss">
.error {
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;

  min-height: 100vh;

  > * {
    margin: 10px;
    font-size: 30px;
    color: white;
  }

  a {
    color: #fff01e !important;
    border: #fff01e 2px dashed !important;
    border-radius: 10px;
    padding: 10px 20px;
  }
} 
</style>