import { inject, InjectionKey, provide } from "vue";

export class GlobalSpinnerProxy {
    constructor() {}
    onShow?: (message?: string) => void;
    onHide?: () => void;

    show(message?: string) {
        if (this.onShow != null) {
            this.onShow(message);
        }
    }

    close() {
        if (this.onHide != null) {
            this.onHide();
        }
    }
}

const __key = "inject-global-spinner-key";
export const InjectGlobalSpinnerProxyKey: InjectionKey<GlobalSpinnerProxy> =
    Symbol(__key);

/**获取全局Loading */
export function useGlobalSpinner(): GlobalSpinnerProxy {
    return inject(InjectGlobalSpinnerProxyKey)!;
}

/**提供全局Loading, 返回全局Loading代理 */
export function provideGlobalSpinner(): GlobalSpinnerProxy {
    const proxy = new GlobalSpinnerProxy();

    provide(InjectGlobalSpinnerProxyKey, proxy);

    return proxy;
}
