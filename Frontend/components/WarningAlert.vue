<script setup lang="ts">
const alertStore = useAlertStore();

const alerts = computed(() => alertStore.getAlerts);
const isShow = computed(() => alertStore.getIsVisible);

let alertCloseTimer: NodeJS.Timeout;

watch(isShow, (value) => {
  if (value) {
    alertCloseTimer = setTimeout(() => {
      close();
    }, 5000);
  }
})

function close() {
  if (alertCloseTimer) clearTimeout(alertCloseTimer);
  CloseAlert();
}
</script>

<template>
  <transition name="move-left">
    <div
      v-if="isShow"
      class="alert"
      @click="close"
    >
      <div class="alert__wrap-icon">
        <img
          src="/img/icons/alert.svg"
          alt="alert-icon"
          class="alert__icon"
        >
      </div>
      <div class="alert__content">
        <div
          v-for="{ label, message } in alerts"
          :key="`${label}-alert`"
          class="alert__item"  
        >
          <label v-show="!!label">{{ label }}</label>
          <p v-show="!!message">{{ message }}</p>
        </div>
      </div>
    </div>
  </transition>
</template>

<style scoped lang="scss">
.alert {
  cursor: pointer;

  position: fixed;
  bottom: 10px;
  right: 10px;

  background-color: #0090FF;

  display: flex;
  justify-content: center;

  padding: 5px;

  border: 1px #0090FF50 solid;
  border-radius: 10px;

  gap: 5px;

  &__wrap-icon {
    max-width: 60px;
    max-height: 60px;
  }
  &__icon {
    width: 100%;
    height: 100%;
  }

  &__content {
    cursor: pointer;
    max-width: 40vw;
    gap: 5px;
    display: flex;
    flex-direction: column;
    justify-content: center;
  }

  &__item {
    cursor: pointer;
    border: 1px #ffffff50 dashed;
    padding: 5px;
    label {
      cursor: pointer;
      font-weight: 700;
    }
  }
}
</style>