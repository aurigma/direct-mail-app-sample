import { useLoadingStore } from "@/stores/loading.store";
import { E_LOADING_STATUS } from "~/core/loading.enums";

export default defineNuxtPlugin((nuxtApp) => {
  const store = useLoadingStore();
  store.setStatus(E_LOADING_STATUS.start);
  nuxtApp.hook("page:finish", () => {
    store.setStatus(E_LOADING_STATUS.finish);
  });
});
