/* eslint-disable @typescript-eslint/no-explicit-any */
import { E_LOADING_STATUS } from "~/core/loading.enums";
import type {
  LineItemCreationModel,
  LineItemDto,
  LineItemUpdateModel,
} from "~/core/direct-mail-api-client";

export const useLineItemsStore = defineStore("line-items", {
  state: () => ({
    lineItem: null as LineItemDto | null,
  }),

  getters: {
    getLineItem: (state): LineItemDto | null => state.lineItem,
  },

  actions: {
    async createLineItem(payload: any) {
      const loading = useLoadingStore();
      loading.setStatus(E_LOADING_STATUS.start);
      try {
        const body: LineItemCreationModel = {
          campaignId: payload.campaignId,
          quantity: payload.quantity,
          productId: payload.productId,
        };
        this.lineItem = (await apiClient().LineItems.createLineItem(
          body,
        )) as LineItemDto;
        loading.setStatus(E_LOADING_STATUS.finish);
        return this.lineItem?.id;
      } catch (err) {
        console.error(err);
        loading.setStatus(E_LOADING_STATUS.finish);
      }
    },

    async fetchLineItemById(payload: { id: string }) {
      const loading = useLoadingStore();
      loading.setStatus(E_LOADING_STATUS.start);
      try {
        this.lineItem = (await apiClient().LineItems.getLineItem(
          payload.id,
        )) as LineItemDto;
        loading.setStatus(E_LOADING_STATUS.finish);
        return this.lineItem;
      } catch (err) {
        console.error(err);
        loading.setStatus(E_LOADING_STATUS.finish);
      }
    },

    async updateLineItem(payload: { id: string; body: LineItemUpdateModel }) {
      const loading = useLoadingStore();
      loading.setStatus(E_LOADING_STATUS.start);
      try {
        const res = (await apiClient().LineItems.updateLineItem(
          payload.id,
          payload.body,
        )) as LineItemDto;
        this.lineItem = Object.assign({ id: this.lineItem?.id }, res);
        loading.setStatus(E_LOADING_STATUS.finish);
        return this.lineItem;
      } catch (err) {
        console.error(err);
        loading.setStatus(E_LOADING_STATUS.finish);
      }
    },
  },
});
