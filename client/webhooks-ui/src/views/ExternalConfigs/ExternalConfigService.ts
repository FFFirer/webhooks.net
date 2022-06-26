import {
    GitConfigClientProxy,
    GiteeConfigClientProxy,
} from "@/shared/client-proxy";
import {
    GitConfigDto,
    GiteeWebHookConfigDto,
    WorkDto,
} from "@/shared/webapi/client";
import { type } from "os";

const giteeConfigService = new GiteeConfigClientProxy();
const gitConfigService = new GitConfigClientProxy();

export type ExternalConfigType = GiteeWebHookConfigDto | GitConfigDto;

export class ExternalConfigService {
    giteeConfigService: GiteeConfigClientProxy;
    gitConfigService: GitConfigClientProxy;

    constructor() {
        this.gitConfigService = new GitConfigClientProxy();
        this.giteeConfigService = new GiteeConfigClientProxy();
    }

    getExternalConfig(work: WorkDto): Promise<ExternalConfigType> {
        if (work.externalConfigType == "gitee") {
            return this.giteeConfigService.get(work.id);
        } else {
            return this.gitConfigService.get(work.id);
        }
    }
}
