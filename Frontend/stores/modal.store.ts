/* eslint-disable @typescript-eslint/no-explicit-any */
import type { IModalOptions } from "@/core/modal/interfaces";

export const useModalStore = defineStore("modal", {
  state: () => ({
    isVisible: false as boolean,
    currentModalKey: "" as string,
    options: {
      title: "",
      description: "",
      isUnclosable: false,
      hasHeader: true,
    } as IModalOptions | any,
  }),

  getters: {
    getOptions: (state) => state.options,
    getIsVisible: (state) => !!state.isVisible,
    getCurrentModalKey: (state) => state.currentModalKey,
  },

  actions: {
    setIsVisible(payload: boolean) {
      this.isVisible = payload;
    },
    setCurrentModalKey(payload: string) {
      this.currentModalKey = payload;
    },
    setOptions(payload: IModalOptions | any) {
      this.options = payload;
    },
    resetOptions() {
      this.options = {
        title: "",
        description: "",
        isUnclosable: false,
        hasHeader: true,
      };
    },
    setUnclosable() {
      this.options = {
        ...this.options,
        isUnclosable: true,
      };
    },
    show(payload: { key: string; options?: IModalOptions | any }) {
      this.setIsVisible(true);
      this.setCurrentModalKey(payload.key);
      if (payload.options) this.setOptions(payload.options);
    },
    hide() {
      this.setIsVisible(false);
      this.setCurrentModalKey("");
      this.resetOptions();
    },
  },
});
