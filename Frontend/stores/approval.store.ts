/* eslint-disable @typescript-eslint/no-explicit-any */
import { E_LOADING_STATUS } from "~/core/loading.enums";
import type {
  ProofRequestModel,
  DesignInfoDto,
  ProofsZipRequestModel,
  ProofsZipRequestConfigModel,
  PreviewRequestModel,
} from "~/core/direct-mail-api-client";

export const useApprovalStore = defineStore("approval", {
  state: () => ({
    designInfo: null as DesignInfoDto | null,
    proofZip: null as string | null,
    proofs: [] as any[],
    preview: [] as any[],
  }),

  getters: {
    getProofs: (state) => state.proofs,
    getPreview: (state) => state.preview,
    getDesignInfo: (state) => state.designInfo,
    getProofZip: (state) => state.proofZip,
  },

  actions: {
    async fetchDesignInfo(payload: { lineItemId: string }) {
      const loading = useLoadingStore();

      loading.setStatus(E_LOADING_STATUS.start);
      try {
        this.designInfo = (await apiClient().Preview.getDesignInfo(
          payload.lineItemId,
        )) as DesignInfoDto;
        loading.setStatus(E_LOADING_STATUS.finish);
      } catch (err) {
        console.error(err);
        loading.setStatus(E_LOADING_STATUS.finish);
      }
    },

    async fetchDesignProof(
      payload: {
        recipientId: string;
        lineItemId: string;
        surfaceIdx: number[];
      },
      width: number = 1200,
      height: number = 800,
    ) {
      const loading = useLoadingStore();

      loading.setStatus(E_LOADING_STATUS.start);
      try {
        this.proofs = [];
        const schemas: any[] = [];
        Promise.all([
          ...payload.surfaceIdx.map(async (surface) => {
            const body: ProofRequestModel = {
              recipientId: payload.recipientId,
              lineItemId: payload.lineItemId,
              config: {
                width: width,
                height: height,
                surfaceIndex: surface,
              },
            };
            return await apiClient()
              .Preview.renderDesignProof(body)
              .then((res) => URL.createObjectURL(res.data))
              .then((proof) => {
                const schema = {
                  id: surface,
                  img: proof,
                };
                schemas.push(schema);
              });
          }),
        ]).then((_) => {
          this.proofs = schemas.sort((a, b) => (b.id > a.id ? -1 : 1));
        });
        loading.setStatus(E_LOADING_STATUS.finish);
      } catch (err) {
        console.error(err);
        loading.setStatus(E_LOADING_STATUS.finish);
      }
    },

    async fetchDesignPreview(
      payload: {
        recipientId?: string;
        lineItemId: string;
        surfaceIdx: number[];
      },
      width: number = 1200,
      height: number = 800,
    ) {
      const loading = useLoadingStore();

      loading.setStatus(E_LOADING_STATUS.start);
      try {
        this.preview = [];
        const schemas: { id: number; img: string }[] = [];
        Promise.all([
          ...payload.surfaceIdx.map(async (surface) => {
            const body: PreviewRequestModel = {
              recipientId: payload.recipientId,
              lineItemId: payload.lineItemId,
              config: {
                width: width,
                height: height,
                surfaceIndex: surface,
              },
            };
            return await apiClient()
              .Preview.renderDesignPreview(body)
              .then((res) => URL.createObjectURL(res.data))
              .then((proof) => {
                const schema = {
                  id: surface,
                  img: proof,
                };
                schemas.push(schema);
              });
          }),
        ]).then((_) => {
          this.preview = schemas.sort((a, b) => (b.id > a.id ? -1 : 1));
        });
        loading.setStatus(E_LOADING_STATUS.finish);
      } catch (err) {
        console.error(err);
        loading.setStatus(E_LOADING_STATUS.finish);
      }
    },

    async finishPersonalization(payload: { id: string }) {
      const loading = useLoadingStore();

      loading.setStatus(E_LOADING_STATUS.start);
      try {
        await apiClient().LineItems.finishPersonalizationLineItem(payload.id);
        loading.setStatus(E_LOADING_STATUS.finish);
      } catch (err) {
        console.error(err);
        loading.setStatus(E_LOADING_STATUS.finish);
      }
    },

    async fetchProofZip(
      payload: { lineItemId: string },
      width: number = 1200,
      height: number = 800,
    ) {
      const loading = useLoadingStore();

      loading.setStatus(E_LOADING_STATUS.start);
      try {
        const body: ProofsZipRequestModel = {
          lineItemId: payload.lineItemId,
          config: {
            width: width,
            height: height,
          } as ProofsZipRequestConfigModel,
        };
        this.proofZip = (await apiClient()
          .Preview.downloadProofsArchive(body)
          .then((res) => URL.createObjectURL(res.data))) as string;
        loading.setStatus(E_LOADING_STATUS.finish);
      } catch (err) {
        console.error(err);
        loading.setStatus(E_LOADING_STATUS.finish);
      }
    },
  },
});
