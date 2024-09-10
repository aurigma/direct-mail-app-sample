import type { IAlert } from "@/core/alert.interfaces";

export const useAlertStore = defineStore("alert", {
  state: () => ({
    isVisible: false as boolean,
    alerts: [] as IAlert[],
  }),

  getters: {
    getAlerts: (state) => state.alerts,
    getIsVisible: (state) => !!state.isVisible,
  },

  actions: {
    setIsVisible(payload: boolean) {
      this.isVisible = payload;
    },
    setAlerts(alerts: IAlert[]) {
      this.alerts = alerts;
    },
    resetOptions() {
      this.alerts = [];
    },
    show(alerts: IAlert[]) {
      this.setIsVisible(true);
      if (alerts) this.setAlerts(alerts);
    },
    hide() {
      this.setIsVisible(false);
      this.resetOptions();
    },
  },
});
