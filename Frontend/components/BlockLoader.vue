<script setup lang="ts">
import { computed, type ComputedRef } from 'vue';

interface IProps {
  loaderColor?: string,
}

const props = defineProps<IProps>();

const backgroundColor: ComputedRef = computed(() => (props.loaderColor) ? props.loaderColor : '#0090ff');
</script>

<template>
  <div class="cc-loader">
    <div>
      <svg
        xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink"
        width="42px"
        height="42px"
        viewBox="0 0 100 100" 
        preserveAspectRatio="xMidYMid"
        class="cc-loader__animation-circle"
      >
        <circle
          :stroke="backgroundColor"
          stroke-width="10"
          cx="50"
          cy="50"
          fill="none"
          r="42"
          stroke-dasharray="197.92033717615698 67.97344572538566"
        />
      </svg>
    </div>
  </div>
</template>

<style scoped lang="scss">
.cc-loader {
  background-color: #ffffff;

  &__animation-circle {
    animation-name: loader-circle;
    animation-duration: 1.5s;
    animation-iteration-count: infinite;
    animation-timing-function: linear;
  }

  @keyframes loader-circle {
    from{ -webkit-transform: rotate(-360deg); }
    to{ -webkit-transform: rotate(360deg); }
  }
}
</style>