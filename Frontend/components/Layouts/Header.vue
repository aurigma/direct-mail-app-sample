<script setup lang="ts">
useSeoMeta({
  ogImage: [
    { url: '/img/icons/projects.svg' },
    { url: '/img/Logo.svg' },
  ],
});
</script> 

<template>
  <header class="header">
    <div class="wrapper">
      <div class="header__wrap">
      <div class="header__logo">
        <img
          src="/img/Logo.svg"
          alt="Logo"
        >
      </div>

      <div>
        <NuxtLink class="link-projects" to="/">
          <img
            src="/img/icons/projects.svg"
            alt="projects-icon"
          >
          Projects
        </NuxtLink>
      </div>
    </div>
    </div>
  </header>
</template>

<style scoped lang="scss">
.header {
  display: flex;
  width: 100%;
  height: 100px;
  align-items: center;
  justify-content: center;
  background: $layout-header-background;
  color: white;

  &__wrap {
    width: 100%;
    display: flex;
    align-items: center;
  }
  &__logo {
    padding-right: 69px;
  }
}

.link-projects {
  display: flex;
  justify-content: center;
  align-items: center;
  gap: 8px;
  color: #282828;
  font-family: 'Inter';
  font-size: 16px;
  font-style: normal;
  font-weight: 400;
  line-height: 28px;
  letter-spacing: 0.15px;
}
</style>