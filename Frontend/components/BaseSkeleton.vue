<script setup lang="ts">
interface IProps {
  skeletonType: 'card' | 'side-bar' | 'mini-card' | 'box',
  title?: boolean,
}
defineProps<IProps>();
</script>

<template>
  <div
    v-if="skeletonType === 'box'"
    class="skeleton-box"
  >
    <div class="skeleton-animations skeleton-box__box"/>
  </div>

  <div
    v-if="skeletonType === 'card'"
    class="skeleton-card"
    :class="[{ 'skeleton-card_with-title': title }]"
  >
    <div class="skeleton-animations skeleton-card__image"/>
    <div v-if="title" class="skeleton-animations skeleton-card__title"/>
  </div>

  <div
    v-if="skeletonType === 'mini-card'"
    class="skeleton-mini-card"
    :class="[{ 'skeleton-card_with-title': title }]"
  >
    <div class="skeleton-animations skeleton-mini-card__image"/>
    <div v-if="title" class="skeleton-animations skeleton-mini-card__title"/>
  </div>
  
  <div
    v-if="skeletonType === 'side-bar'"
    class="skeleton-side-bar"
  >
    <div class="skeleton-animations skeleton-side-bar__title"/>
    <div class="skeleton-animations skeleton-side-bar__option"/>
    <div class="skeleton-animations skeleton-side-bar__option"/>
    <div class="skeleton-animations skeleton-side-bar__option"/>

    <div class="skeleton-animations skeleton-side-bar__title"/>
    <div class="skeleton-animations skeleton-side-bar__option"/>
    <div class="skeleton-animations skeleton-side-bar__option"/>

    <div class="skeleton-animations skeleton-side-bar__title"/>
    <div class="skeleton-animations skeleton-side-bar__option"/>
  </div>
</template>

<style scoped lang="scss">
.skeleton-animations {
  width: 100%;
  height: 100%;

  border-radius: 4px;

  background: linear-gradient(-25deg, #e4e4e4, #e2e2e2, #ffffff, #e4e4e4, #ffffff);
  background-size: 400% 400%;
  animation: gradient 3s ease infinite;
}

.skeleton-card {
  display: flex;
  flex-direction: column;
  justify-content: space-between;
  
  width: 220px;
  height: 220px;

  &_with-title {
    height: 252px;
  }

  &__image {
    height: 220px;
  }
  &__title {
    height: 24px;
    margin-top: 5px;
  }
}

.skeleton-mini-card {
  display: flex;
  flex-direction: column;
  justify-content: space-between;
  
  width: 120px;
  height: 120px;

  &_with-title {
    height: 154px;
  }

  &__image {
    height: 120px;
  }
  &__title {
    height: 17px;
    margin-top: 5px;
  }
}

.skeleton-box {
  display: flex;
  width: 100%;
  height: 100%;

  &__box {
    width: 100%;
    height: 100%;
  }
}

.skeleton-side-bar {
  display: flex;
  flex-direction: column;
  width: 100%;

  &__title {
    height: 44px;
    margin-bottom: 16px;
  }
  &__option {
    width: 100%;
    height: 32px;
    &:nth-child(even) {
      width: 70%;
    }
    &:nth-child(-n + 1) {
      width: 55%;
    }
    &:not(:last-child) {
      margin-bottom: 20px;
    }
  }
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