<script setup lang="ts">

</script>

<template>
  <li class="side-bar-item">
    <h2>
      <slot name="title" />
    </h2>
    <slot name="options" />
  </li>
</template>

<style scoped lang="scss">
.side-bar-item {
  h2 {
    font-family: 'Inter';
    font-size: 24px;
    font-weight: 600;
    line-height: 32px;
    text-align: left;
    margin-bottom: 16px;
  }
}
</style>