import { inject, InjectionKey, provide } from "vue";

export class GlobalMessageProxy {
    constructor() {}
    onShow?: (message: string, autoClise?: number) => void;
    onNotice?: (message: string, autoClose?: number) => void;
    show(message: string, autoClose?: number) {
        if (this.onShow != null) {
            this.onShow(message, autoClose);
        }
    }
    notice(message: string, closeAfter?: number) {
        if (this.onNotice != null) {
            this.onNotice(message, closeAfter);
        }
    }
}

const __key = "inject-global-message-key";
/**全局消息组件注入Key */
export const InjectGlobalMessageProxyKey: InjectionKey<GlobalMessageProxy> =
    Symbol(__key);

/**获取全局消息组件 */
export function useGlobalMessage(): GlobalMessageProxy {
    return inject(InjectGlobalMessageProxyKey)!;
}

/**提供全局消息提示，返回全局消息提示代理 */
export function provideGlobalMessage(): GlobalMessageProxy {
    const proxy = new GlobalMessageProxy();

    provide(InjectGlobalMessageProxyKey, proxy);

    return proxy;
}
