import { inject, InjectionKey, provide } from "vue";

export class GlobalMessageProxy {
    constructor() {}
    onShow?: (message: string) => void;
    show(message: string) {
        if (this.onShow != null) {
            this.onShow(message);
        }
    }
}

const __key = "inject-global-message-key";
export const InjectGlobalMessageProxyKey: InjectionKey<GlobalMessageProxy> =
    Symbol(__key);

export function useGlobalMessage(): GlobalMessageProxy {
    return inject(InjectGlobalMessageProxyKey)!;
}

/**提供全局消息提示，返回全局消息提示代理 */
export function provideGlobalMessage(): GlobalMessageProxy {
    const proxy = new GlobalMessageProxy();

    provide(InjectGlobalMessageProxyKey, proxy);

    return proxy;
}
