import { E_LOADING_STATUS } from "@/core/loading.enums";

export const useLoadingStore = defineStore("loading", {
  state: () => ({
    status: E_LOADING_STATUS.finish as E_LOADING_STATUS,
  }),

  getters: {
    getStatus: (state) => !!state.status,
  },

  actions: {
    setStatus(payload: E_LOADING_STATUS) {
      this.$patch({ status: payload });
      if (payload === E_LOADING_STATUS.start)
        this.$patch({ status: E_LOADING_STATUS.pending });
    },
  },
});
