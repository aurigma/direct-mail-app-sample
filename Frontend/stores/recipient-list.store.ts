import { E_LOADING_STATUS } from "~/core/loading.enums";
import { useLoadingStore } from "./loading.store";
import type {
  RecipientListDto,
  RecipientListSubmitModel,
} from "~/core/direct-mail-api-client";

export const useRecipientListStore = defineStore("recipient-list", {
  state: () => ({
    recipientList: null as RecipientListDto[] | null,
  }),

  getters: {
    getRecipientLists: (state) => state.recipientList,
  },

  actions: {
    async fetchRecipientList() {
      const loading = useLoadingStore();
      loading.setStatus(E_LOADING_STATUS.start);
      try {
        const recipientList =
          (await apiClient().RecipientList.getRecipientLists()) as RecipientListDto[];
        this.recipientList = recipientList;
        loading.setStatus(E_LOADING_STATUS.finish);
      } catch (err) {
        console.error(err);
        loading.setStatus(E_LOADING_STATUS.finish);
      }
    },

    async submitRecipientList(payload: { body: RecipientListSubmitModel }) {
      const loading = useLoadingStore();
      loading.setStatus(E_LOADING_STATUS.start);
      try {
        await apiClient().RecipientList.submitRecipientLists(payload.body);
        loading.setStatus(E_LOADING_STATUS.finish);
      } catch (err) {
        console.error(err);
        loading.setStatus(E_LOADING_STATUS.finish);
      }
    },
  },
});
