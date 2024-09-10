/* eslint-disable @typescript-eslint/no-explicit-any */
/* eslint-disable @typescript-eslint/ban-ts-comment */
import { E_LOADING_STATUS } from "~/core/loading.enums";
import type {
  IntegratedProductDto,
  IntegratedProductOptionDto,
  IntegratedProductTemplateDto,
  IntegratedProductTemplateDetailsDto,
  IntegrationProductOptionRequestModel,
} from "~/core/direct-mail-api-client";

interface IIntegratedStoreState {
  templateInfo: IntegratedProductTemplateDetailsDto | null;
  templates: IntegratedProductTemplateDto[] | null;
  integratedProductList: IntegratedProductDto[] | null;
  optionsList: IntegratedProductOptionDto[] | null;
}

interface ITemplate {
  id: string;
  img: string;
  title: string;
  productVariantId: number;
}

interface IIntegratedProduct {
  id: string;
  img: string;
  title: string;
}

export const useIntegratedProductsStore = defineStore("integrated-products", {
  state: (): IIntegratedStoreState => ({
    templateInfo: null,
    templates: null,
    integratedProductList: null,
    optionsList: null,
  }),

  getters: {
    getTemplateInfo: (state) => state.templateInfo,
    getTemplates: (state): null | ITemplate[] => {
      if (state.templates === null) {
        return null;
      }

      return state.templates?.map((item): ITemplate => {
        return {
          id: item.templateId ? item.templateId : "",
          img: item.previewUrl ? item.previewUrl : "",
          title: item.templateName ? item.templateName : "",
          productVariantId: Number(item.productVariantId),
        };
      });
    },
    getIntegratedProducts: (state): null | IIntegratedProduct[] => {
      if (state.integratedProductList === null) {
        return null;
      }

      return state.integratedProductList?.map((item): IIntegratedProduct => {
        return {
          id: item.id ? item.id : "",
          img: item.previewUrl ? item.previewUrl : "",
          title: item.title ? item.title : "",
        };
      });
    },
    getProductOptionsList: (state): IntegratedProductOptionDto[] | null =>
      state.optionsList,
  },

  actions: {
    async fetchIntegratedProducts(payload: {
      categoryId?: string | undefined;
    }) {
      const loading = useLoadingStore();
      this.integratedProductList = null;
      loading.setStatus(E_LOADING_STATUS.start);
      try {
        const integratedProductList =
          (await apiClient().IntegratedProduct.getIntegratedProducts(
            payload?.categoryId,
          )) as IntegratedProductDto[];
        this.setIfChanged("integratedProductList", integratedProductList);
        loading.setStatus(E_LOADING_STATUS.finish);
      } catch (err) {
        console.error(err);
        loading.setStatus(E_LOADING_STATUS.finish);
      }
    },

    async fetchIntegratedProductsTemplateByProductId(payload: {
      id: string;
      body: { templateTitle?: string | undefined; options?: any[] };
    }) {
      const loading = useLoadingStore();
      loading.setStatus(E_LOADING_STATUS.start);
      try {
        const body: IntegrationProductOptionRequestModel = {
          templateTitle: payload?.body.templateTitle,
          options: payload?.body.options?.map((option) => {
            return {
              optionId: option.optionId,
              valueIds: option.valueIds,
            };
          }),
        };
        const templates =
          (await apiClient().IntegratedProduct.getIntegratedProductTemplates(
            payload.id,
            body,
          )) as IntegratedProductTemplateDto[];
        this.setIfChanged("templates", templates);
        loading.setStatus(E_LOADING_STATUS.finish);
      } catch (err) {
        console.error(err);
        loading.setStatus(E_LOADING_STATUS.finish);
      }
    },

    async fetchOptions(payload: { id: string }) {
      const loading = useLoadingStore();
      loading.setStatus(E_LOADING_STATUS.start);
      try {
        const optionsList =
          (await apiClient().IntegratedProduct.getIntegratedProductOptions(
            payload?.id,
          )) as IntegratedProductOptionDto[];
        this.setIfChanged("optionsList", optionsList);
        loading.setStatus(E_LOADING_STATUS.finish);
      } catch (err) {
        console.error(err);
        loading.setStatus(E_LOADING_STATUS.finish);
      }
    },

    async clearOptionList() {
      this.optionsList = null;
    },

    async fetchIntegratedProductTemplateDetails(payload: {
      id: string;
      templateId: string;
      productVariantId: number;
    }) {
      const loading = useLoadingStore();
      loading.setStatus(E_LOADING_STATUS.start);
      try {
        this.templateInfo =
          (await apiClient().IntegratedProduct.getIntegratedProductTemplateDetails(
            payload.id,
            payload.templateId,
            payload.productVariantId,
          )) as IntegratedProductTemplateDetailsDto;
        loading.setStatus(E_LOADING_STATUS.finish);
      } catch (err) {
        console.error(err);
        loading.setStatus(E_LOADING_STATUS.finish);
      }
    },

    resetTemplateInfo() {
      this.templateInfo = null;
    },

    setIfChanged<IntegratedStoreStateKey extends keyof IIntegratedStoreState>(
      stateName: IntegratedStoreStateKey,
      data: IIntegratedStoreState[IntegratedStoreStateKey],
    ) {
      if (this[stateName] !== data) {
        //@ts-ignore
        this[stateName] = data;
      }
    },

    resetTemplates() {
      this.templates = null;
    },

    async updateProductResources(payload: { id: string }) {
      const loading = useLoadingStore();
      loading.setStatus(E_LOADING_STATUS.start);
      try {
        await apiClient().IntegratedProduct.updateIntegratedProductResources(
          payload.id,
        );
        loading.setStatus(E_LOADING_STATUS.finish);
      } catch (err) {
        console.error(err);
        loading.setStatus(E_LOADING_STATUS.finish);
      }
    },
  },
});
