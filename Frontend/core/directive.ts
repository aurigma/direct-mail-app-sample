/* eslint-disable @typescript-eslint/no-explicit-any */
import { directiveHooks } from "@vueuse/shared";
import type { ObjectDirective } from "vue-demi";
import { onClickOutside } from "@vueuse/core";
import type {
  OnClickOutsideHandler,
  OnClickOutsideOptions,
} from "@vueuse/core";

export const vOnClickOutside: ObjectDirective<
  HTMLElement,
  OnClickOutsideHandler | [(evt: any) => void, OnClickOutsideOptions]
> = {
  [directiveHooks.mounted](el: any, binding: any) {
    const capture = !binding.modifiers.bubble;
    if (typeof binding.value === "function") {
      (el as any).__onClickOutside_stop = onClickOutside(el, binding.value, {
        capture,
      });
    } else {
      const [handler, options] = binding.value;
      (el as any).__onClickOutside_stop = onClickOutside(
        el,
        handler,
        Object.assign({ capture }, options),
      );
    }
  },
  [directiveHooks.unmounted](el: any) {
    (el as any).__onClickOutside_stop();
  },
};

export { vOnClickOutside as VOnClickOutside };
