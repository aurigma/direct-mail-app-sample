import type { IAlert } from "@/core/alert.interfaces";

export const ShowAlert = (alerts: IAlert[]) => {
  const alertStore = useAlertStore();
  alertStore.show(alerts);
};

export const CloseAlert = () => {
  const alertStore = useAlertStore();
  alertStore.hide();
};
