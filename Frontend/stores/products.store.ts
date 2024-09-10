import { E_LOADING_STATUS } from "~/core/loading.enums";
import type { ProductDto } from "~/core/direct-mail-api-client";

export const useProductsStore = defineStore("products", {
  state: () => ({
    product: null as ProductDto | null,
  }),

  getters: {
    getProduct: (state) => state.product,
  },

  actions: {
    async fetchProductById(payload: { id: string }) {
      const loading = useLoadingStore();
      loading.setStatus(E_LOADING_STATUS.start);
      try {
        this.product = (await apiClient().Product.getProduct(
          payload?.id,
        )) as ProductDto;
        loading.setStatus(E_LOADING_STATUS.finish);
      } catch (err) {
        console.error(err);
        loading.setStatus(E_LOADING_STATUS.finish);
      }
    },
  },
});
