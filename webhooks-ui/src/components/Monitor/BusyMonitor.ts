import { GlobalSpinnerProxy } from "../GlobalSpinner/GlobalSpinnerProxy";

export class BusyMonitor {
    constructor(proxy: GlobalSpinnerProxy, base?: number) {
        this.spinner = proxy;
        this.baseTimeout = base ?? 500;
    }

    timeout?: NodeJS.Timeout;
    spinner: GlobalSpinnerProxy;
    baseTimeout: number;

    IfBusy = (message?: string, time?: number) => {
        this.timeout = setTimeout(() => {
            console.log("now is busy");
            this.spinner.show(message);
        }, time ?? 500);
    };

    NotBusyNow = () => {
        console.log("not busy now");
        clearTimeout(this.timeout);
        this.spinner.close();
    };
}
