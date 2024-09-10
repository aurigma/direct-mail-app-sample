<script setup lang="ts"></script>

<template>
  <ul class="side-bar">
    <slot />
  </ul>
</template>

<style scoped lang="scss">
.side-bar {
  display: flex;
  flex-direction: column;

  gap: 20px;

  max-width: 280px;
  width: 100%;  
}
</style>