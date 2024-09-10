/* eslint-disable @typescript-eslint/no-explicit-any */
import { E_LOADING_STATUS } from "~/core/loading.enums";
import type {
  IntegrationDetailsDto,
  TokenDto,
  TokenRequestModel,
  DesignValidationResultDto,
  EditorVariableInfoDto,
  DesignDto,
} from "@/core/direct-mail-api-client";

export const useEditorInfoStore = defineStore("editor-info", {
  state: () => ({
    proofs: null as Array<Array<string>> | null,
    details: null as IntegrationDetailsDto | null,
    privateDesign: null as any | null,
    token: null as TokenDto | null,
    variableList: null as EditorVariableInfoDto[] | null,
  }),

  getters: {
    getProofs: (state) => {
      if (state.proofs)
        return state.proofs.map((item, order) => {
          return {
            id: order,
            img: item[0].toString(),
          };
        });
      else return null;
    },
    getProductPersonalizationWorkflow: (state) => {
      if (state.details?.productPersonalizationWorkflow)
        return JSON.parse(state.details?.productPersonalizationWorkflow);
    },
    getDesignEditorUrl: (state) => state.details?.designEditorUrl,
    // getPrivateDesignId: (state) => state.privateDesign?.stateId,
    getToken: (state) => state.token,
    getVariableList: (state) => state.variableList,
  },

  actions: {
    setProofs(payload: Array<Array<string>> | null) {
      this.proofs = payload;
    },

    async fetchIntegrationDetails(payload: { productId: string }) {
      const loading = useLoadingStore();
      loading.setStatus(E_LOADING_STATUS.start);
      try {
        this.details = (await apiClient().EditorInfo.getIntegrationDetails(
          payload.productId,
        )) as IntegrationDetailsDto;
        loading.setStatus(E_LOADING_STATUS.finish);
      } catch (err) {
        console.error(err);
        loading.setStatus(E_LOADING_STATUS.finish);
      }
    },

    async fetchTokenDE(payload: { userId: string }) {
      const loading = useLoadingStore();
      loading.setStatus(E_LOADING_STATUS.start);
      try {
        const body = {
          userId: payload.userId,
        } as TokenRequestModel;
        this.token = (await apiClient().EditorInfo.getEditorToken(
          body,
        )) as TokenDto;
        loading.setStatus(E_LOADING_STATUS.finish);
      } catch (err) {
        console.error(err);
        loading.setStatus(E_LOADING_STATUS.finish);
      }
    },

    async validateDesign(payload: { lineItemId: string }) {
      const loading = useLoadingStore();
      loading.setStatus(E_LOADING_STATUS.start);
      try {
        const resultValidateDesign: DesignValidationResultDto | any =
          await apiClient().EditorInfo.validateDesign(payload.lineItemId);
        resultValidateDesign.error = false;
        loading.setStatus(E_LOADING_STATUS.finish);
        return resultValidateDesign;
      } catch (err: any) {
        loading.setStatus(E_LOADING_STATUS.finish);
        return {
          error: `error: ${err?.propertyValue}`,
          isValid: false,
        };
      }
    },

    async fetchAvailableVariables(payload: { lineItemId: string }) {
      const loading = useLoadingStore();
      loading.setStatus(E_LOADING_STATUS.start);
      try {
        this.variableList = (await apiClient().EditorInfo.getAvailableVariables(
          payload.lineItemId,
        )) as EditorVariableInfoDto[];
        loading.setStatus(E_LOADING_STATUS.finish);
      } catch (err: any) {
        loading.setStatus(E_LOADING_STATUS.finish);
        return {
          missingVariableNames: [`error: ${err?.propertyValue}`],
          isValid: false,
        } as DesignValidationResultDto;
      }
    },

    async fetchDesignId(payload: { lineItemId: string; userId: string }) {
      return (await apiClient().EditorInfo.createEditorDesign(
        payload.lineItemId,
        payload.userId,
      )) as DesignDto;
    },
  },
});
