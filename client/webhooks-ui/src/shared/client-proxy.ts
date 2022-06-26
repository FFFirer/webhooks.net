import {
    ApiException,
    GitConfigClient,
    GiteeConfigClient,
    GroupClient,
    GroupDto,
    SettingClient,
    WorkClient,
    WorkExecutionLogClient,
    WorkRunnerClient,
} from "./webapi/client";

const API_URL = import.meta.env.VITE_API_URL;

class FetchProxyWithBusy {
    constructor() {}

    fetch(url: RequestInfo, init?: RequestInit): Promise<Response> {
        // init!.mode = "no-cors";
        return window.fetch(url, init).then((response) => {
            return response;
        });
    }
}

const fetchProxy = {
    fetch(url: RequestInfo, init?: RequestInit): Promise<Response> {
        // init!.mode = "no-cors";
        return window.fetch(url, init).then((response) => {
            return response;
        });
    },
};

export class GroupClientProxy extends GroupClient {
    constructor() {
        super(API_URL, fetchProxy);
    }
}

export class WorkClientProxy extends WorkClient {
    constructor() {
        super(API_URL, fetchProxy);
    }
}

export class GiteeConfigClientProxy extends GiteeConfigClient {
    constructor() {
        super(API_URL, fetchProxy);
    }
}

export class GitConfigClientProxy extends GitConfigClient {
    constructor() {
        super(API_URL, fetchProxy);
    }
}

export class SettingClientProxy extends SettingClient {
    constructor() {
        super(API_URL, fetchProxy);
    }
}

export class WorkRunnerClientProxy extends WorkRunnerClient {
    constructor() {
        super(API_URL, fetchProxy);
    }
}

export class WorkExecutionLogClientProxy extends WorkExecutionLogClient {
    constructor() {
        super(API_URL, fetchProxy);
    }
}

type ClientType =
    | GroupClient
    | WorkClient
    | GiteeConfigClient
    | GitConfigClient
    | SettingClient
    | WorkRunnerClient
    | WorkExecutionLogClient;
