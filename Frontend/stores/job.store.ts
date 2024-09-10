import { E_LOADING_STATUS } from "~/core/loading.enums";
import { useLoadingStore } from "./loading.store";
import type {
  JobDto,
  JobProcessingResultDto,
} from "~/core/direct-mail-api-client";

export const useJobStore = defineStore("job", {
  state: () => ({
    jobs: null as JobDto[] | null,
    download: null as JobProcessingResultDto[] | null,
  }),

  getters: {
    getJobs: (state) => state.jobs,
    getDownloadLinks: (state) => state.download?.map((item) => item.url),
  },

  actions: {
    async fetchJobs(payload: { lineItemId: string }) {
      const loading = useLoadingStore();
      loading.setStatus(E_LOADING_STATUS.start);
      try {
        this.jobs = (await apiClient().Job.getJobs(
          payload.lineItemId,
        )) as JobDto[];
        loading.setStatus(E_LOADING_STATUS.finish);
      } catch (err) {
        console.error(err);
        loading.setStatus(E_LOADING_STATUS.finish);
      }
    },

    async restartJob(payload: { lineItemId: string; jobId: string }) {
      const loading = useLoadingStore();
      loading.setStatus(E_LOADING_STATUS.start);
      try {
        await apiClient().ApiClient.restart(payload.jobId);
        this.fetchJobs(payload);
        loading.setStatus(E_LOADING_STATUS.finish);
      } catch (err) {
        console.error(err);
        loading.setStatus(E_LOADING_STATUS.finish);
      }
    },

    async fetchDownloadResult(payload: { jobId: string }) {
      const loading = useLoadingStore();
      loading.setStatus(E_LOADING_STATUS.start);
      try {
        this.download = (await apiClient().ApiClient.downloadResults(
          payload.jobId,
        )) as JobProcessingResultDto[];
        loading.setStatus(E_LOADING_STATUS.finish);
      } catch (err) {
        console.error(err);
        loading.setStatus(E_LOADING_STATUS.finish);
      }
    },
  },
});
