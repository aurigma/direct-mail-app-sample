<script setup lang="ts">
  interface IProps {
    imageList: string[],
    height?: string,
  }
  defineProps<IProps>();
  
  const currentSlide = ref<number>(1);

  const slideTo = (to: number) => {
    currentSlide.value = to;
  };
</script>

<template>
  <Carousel
    v-model="currentSlide"
    class="gallery"
    :class="[{ 'carousel__height-auto' : height === 'auto' }]"
    :items-to-show="1"
    :wrap-around="true"
  >
    <Slide v-for="(image, slide) in imageList" :key="slide">
      <div class="carousel__item">
        <img :src="image" alt="image-slide">
      </div>
    </Slide>
  </Carousel>

  <Carousel
    v-if="imageList.length > 1"
    ref="carousel"
    v-model="currentSlide"
    :items-to-show="5"
    :wrap-around="false"
    class="slider-line specific"
  >
    <Slide
      v-for="(image, slide) in imageList"
      :key="slide"
    >
      <div
        class="carousel__item carousel__navigate"
        :class="{ 'carousel__active': currentSlide === slide }"
        @click="slideTo(slide)"
      >
        <img :src="image" alt="image-slide">
      </div>
    </Slide>
  </Carousel>
</template>

<style scoped lang="scss">
.carousel__slide {
  border-radius: 10px;
  overflow: hidden;
}
.carousel {
  &:nth-child(1) {
    height: 100%;
    width: 100%;
    display: flex;
    align-items: center;
  }
  &__height-auto {
    height: auto !important;
  }
  &__item {
    margin: 0 4px;
    border-radius: 10px;
    overflow: hidden;
    background: #EFF0F0;
  }
  &__navigate {
    border-radius: 10px;
    overflow: hidden;
    margin-top: 20px;
    position: relative;
    border: 1px solid #E0E0E0;
    height: calc(100% - 20px);
    display: flex;
    > img {
      margin: auto;
    }
  }
  &__navigate::before {
    border-radius: 10px;
    content: '';
    width: 100%;
    height: 100%;
    position: absolute;
    left: 0;
    z-index: 100000;
  }
  &__active::before {
    border-radius: 10px;
    border: 2px solid #0072E0;
  }
}
</style>