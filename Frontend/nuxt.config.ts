/* eslint-disable @typescript-eslint/no-require-imports */
/* eslint-disable @typescript-eslint/ban-ts-comment */
export default defineNuxtConfig({
  devtools: { enabled: false },
  ssr: false,

  app: {
    head: {
      title: "Customer`s Canvas",
      meta: [
        { charset: "utf-8" },
        { name: "viewport", content: "width=device-width, initial-scale=1" },
        { name: "description", content: "Nuxt 3" },
      ],
      link: [
        {
          rel: "stylesheet",
          href: "https://fonts.googleapis.com/css2?family=Inter:ital,wght@0,100;0,300;0,400;0,500;0,700;0,900;1,100;1,300;1,400;1,500;1,700;1,900&display=swap",
        },
      ],
    },
  },

  components: [
    {
      path: "~/components",
      pathPrefix: false,
    },
  ],

  css: ["@/assets/scss/main.scss"],

  modules: [
    "@pinia/nuxt",
    "@vee-validate/nuxt",
    "@nuxt/ui",
    "vue3-carousel-nuxt",
    "@nuxt/eslint",
  ],

  // @ts-ignore
  ui: {
    global: true,
    icons: ["mdi", "simple-icons"],
  },

  vite: {
    css: {
      preprocessorOptions: {
        scss: {
          sourceMap: true,
          implementation: require("sass"),
          additionalData: `
            @import "@/assets/scss/_vars.scss";
            @import "@/assets/scss/mixins/_page.scss";
            @import "@/assets/scss/mixins/_input.scss";
            @import "@/assets/scss/mixins/_button.scss";
            @import "@/assets/scss/mixins/_badge.scss";
            @import "@/assets/scss/mixins/_error.scss";
            @import "@/assets/scss/mixins/_radio.scss";
          `,
        },
      },
    },
  },

  runtimeConfig: {
    public: {
      apiBase: process.env.BASE_URL || "https://api.example.com/",
    },
  },

  pinia: {
    storesDirs: ["./stores/**"],
  },

  compatibilityDate: "2024-09-10",
});
