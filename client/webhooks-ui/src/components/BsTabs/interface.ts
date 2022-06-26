import { InjectionKey, Ref } from "vue";

const __key = "bs-tab";

export const BsTabInjectionKey: InjectionKey<BsTabInjection> = Symbol(__key);

export interface BsTabInjection {
    active: Ref<string>;
}
