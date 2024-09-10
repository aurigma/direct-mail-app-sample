/* eslint-disable @typescript-eslint/no-explicit-any */
import type { IModalOptions } from "@/core/modal/interfaces";

export const ShowModal = (payload: {
  key: string;
  options?: IModalOptions | any;
}) => {
  const modalStore = useModalStore();
  modalStore.show(payload);
};

export const CloseModal = () => {
  const modalStore = useModalStore();
  modalStore.hide();
};
