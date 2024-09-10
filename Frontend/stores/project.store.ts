import type { ICampaignsTable } from "~/core/api.interfaces";
import { E_LOADING_STATUS } from "~/core/loading.enums";
import type {
  CampaignType,
  CampaignDto,
  CategoryDto,
  CampaignCreationModel,
  CampaignUpdateModel,
} from "~/core/direct-mail-api-client";

interface ICategory {
  title: string;
  value: string;
}

export const useProjectStore = defineStore("project", {
  state: () => ({
    projectList: null as CampaignDto[] | null,
    projectTypes: null as CampaignType[] | null,
    categoryList: null as CategoryDto[] | null,
    currentlyEditedProduct: null as CampaignDto | null,
  }),

  getters: {
    getCurrentlyEditedProduct: (state) => state.currentlyEditedProduct,

    getProjectsTableData: (state): null | ICampaignsTable[] => {
      if (state.projectList === null) return null;

      const projects = state.projectList.filter((project) => {
        if (!!project?.lineItems?.length === false) return false;
        return !!project?.lineItems[0].designId;
      });

      return projects.map((project) => {
        return {
          id: project.id,
          name: project.title,
          status: "Live",
          lineItemId: project.lineItems?.length
            ? project.lineItems[0].id
            : undefined,
        };
      }) as ICampaignsTable[];
    },

    getProductCategoryList: (state) =>
      state.categoryList
        ?.map((category) => {
          return {
            title: category.title,
            value: category.id,
          };
        })
        .concat([{ title: "All", value: "all" }])
        .reverse() as ICategory[],

    getProductTypes: (state) =>
      state.projectTypes?.map((type) => {
        return { label: type, value: type };
      }),
  },

  actions: {
    async fetchProjectsData(
      payload: { type: CampaignType } = { type: "Default" as CampaignType },
    ) {
      const companyStore = useCompanyStore();
      const loading = useLoadingStore();
      loading.setStatus(E_LOADING_STATUS.start);
      try {
        this.projectList = (await apiClient().Campaign.getCampaigns(
          payload.type,
          companyStore.getCompanyId,
        )) as CampaignDto[];
        loading.setStatus(E_LOADING_STATUS.finish);
      } catch (err) {
        console.error(err);
        loading.setStatus(E_LOADING_STATUS.finish);
      }
    },

    async fetchProjectTypes() {
      const loading = useLoadingStore();
      loading.setStatus(E_LOADING_STATUS.start);
      try {
        this.projectTypes =
          (await apiClient().Campaign.getCampaignTypes()) as CampaignType[];
        loading.setStatus(E_LOADING_STATUS.finish);
      } catch (err) {
        console.error(err);
        loading.setStatus(E_LOADING_STATUS.finish);
      }
    },

    async fetchProjectById(id: string) {
      try {
        return (await apiClient().Campaign.getCampaignById(
          id,
        )) as CampaignDto[];
      } catch (err) {
        return err;
      }
    },

    async fetchProductCategory() {
      const loading = useLoadingStore();

      loading.setStatus(E_LOADING_STATUS.start);
      try {
        this.categoryList =
          (await apiClient().Category.getCategories()) as CategoryDto[];
        loading.setStatus(E_LOADING_STATUS.finish);
      } catch (err) {
        console.error(err);
        loading.setStatus(E_LOADING_STATUS.finish);
      }
    },

    async createProject(payload: { title: string; type: CampaignType }) {
      const companyStore = useCompanyStore();

      const body = {
        title: payload.title as string,
        type: payload.type as CampaignType,
        companyId: companyStore.getCompanyId,
      } as CampaignCreationModel;

      try {
        this.currentlyEditedProduct =
          (await apiClient().Campaign.createCampaign(body)) as CampaignDto;
        return this.currentlyEditedProduct?.id;
      } catch (err) {
        console.error(err);
        return undefined;
      }
    },

    async updateProjectRecipients(payload: {
      recipientListIds: string[];
      projectId: string;
    }) {
      const loading = useLoadingStore();
      loading.setStatus(E_LOADING_STATUS.start);
      try {
        const id = payload.projectId;
        const project: CampaignDto = (await this.fetchProjectById(
          id,
        )) as CampaignDto;
        const body = {
          title: project?.title,
          type: project?.type,
          recipientListIds: payload.recipientListIds,
        } as CampaignUpdateModel;
        this.currentlyEditedProduct =
          (await apiClient().Campaign.updateCampaign(id, body)) as CampaignDto;
        loading.setStatus(E_LOADING_STATUS.finish);
      } catch (err) {
        console.error(err);
        loading.setStatus(E_LOADING_STATUS.finish);
      }
    },
  },
});
