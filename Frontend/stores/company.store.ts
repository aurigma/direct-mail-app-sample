import { E_LOADING_STATUS } from "~/core/loading.enums";

interface ICompany {
  id: string;
  name: string;
}

export const useCompanyStore = defineStore("company", {
  state: () => ({
    company: {
      id: "",
      name: "",
    } as ICompany,
  }),

  getters: {
    getCompanyId: (state) => state.company.id,
  },

  actions: {
    async fetchCompany() {
      const loading = useLoadingStore();
      loading.setStatus(E_LOADING_STATUS.start);
      try {
        const allCompany =
          (await apiClient().Company.getCompanies()) as ICompany[];
        this.company = allCompany[0];
        loading.setStatus(E_LOADING_STATUS.finish);
      } catch (err) {
        console.error(err);
        loading.setStatus(E_LOADING_STATUS.finish);
      }
    },
  },
});
