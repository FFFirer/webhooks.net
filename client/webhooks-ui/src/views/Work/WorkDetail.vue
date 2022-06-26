<script setup lang="ts">
import { onMounted, ref, Ref } from "vue";
import BsTab from "@/components/BsTabs/BsTab.vue";
import BsTabItem from "@/components/BsTabs/BsTabItem.vue";
import BsSpinner from "@/components/BsSpinner/BsSpinner.vue";
import { useGlobalMessage } from "@/components/GlobalMessage/GlobalMessageProxy";
import {
    GiteeConfigClientProxy,
    WorkClientProxy,
    WorkRunnerClientProxy,
    WorkExecutionLogClientProxy,
} from "@/shared/client-proxy";
import {
    BuildScript,
    BuildScriptDto,
    GiteeWebHookAuthentication,
    GiteeWebHookConfigDto,
    SaveGiteeWebHookConfigInput,
    WorkDto,
    WorkExecutionLog,
    WorkExecutionLogSummary,
} from "@/shared/webapi/client";
import WorkDetailViewProps from "./WorkDetailProps";
import Clipboard from "clipboard";
import * as monaco from "monaco-editor";
// import "monaco-editor/esm/vs/basic-languages/powershell/powershell.contribution";

import { useGlobalSpinner } from "@/components/GlobalSpinner/GlobalSpinnerProxy";
import { BusyMonitor } from "@/components/Monitor/BusyMonitor";

import { useRouter } from "vue-router";
import ExternalConfig from "@/views/ExternalConfigs/ExternalConfig.vue";
import {
    ExternalConfigService,
    ExternalConfigType,
} from "../ExternalConfigs/ExternalConfigService";
import { computed } from "@vue/reactivity";

import { Play16Regular } from "@vicons/fluent";
import { Icon, IconConfigProvider } from "@vicons/utils";

import { alertException } from "@/shared/helpers/ExceptionHelper";
import { Modal } from "bootstrap";
import BsModalHelper from "@/components/BsModal/BsModalHelper";
import BsModal from "@/components/BsModal/BsModal.vue";
import WorkExecutionLogDetail from "./WorkExecutionLogDetail.vue";
import { externalConfigTypes } from "./WorkBasic";
import { formatStandardDateTime, formatElapsedTime } from "@/utils/datetime";
import {
    formatWorkExecutionStatus,
    formatWorkExecutionResult,
} from "@/utils/work";

const props = defineProps(WorkDetailViewProps);

const globalMessage = useGlobalMessage();
const globalSpinner = useGlobalSpinner();

const BasicTab = "basic-tab";
const SetupTab = "setup-tab";
const LogTab = "log-tab";
const ScriptsTab = "scripts-tab";
const ExternalConfigTab = "external-config-tab";

const copyBtnRef: Ref<HTMLButtonElement | undefined> = ref();
const codeEditorRef: Ref<HTMLDivElement | undefined> = ref();

const handleTabActived = (id: string) => {
    if (id == ScriptsTab) {
        if (!hasInitCodeEditor.value) {
            // 延时渲染
            setTimeout(() => {
                initCodeEditor();
            }, 100);
        }
    }
};

const workClient = new WorkClientProxy();
const giteeConfigClient = new GiteeConfigClientProxy();
const workRunner = new WorkRunnerClientProxy();

const work: Ref<WorkDto> = ref(new WorkDto());

const script: Ref<BuildScript> = ref(new BuildScript());
const hasInitCodeEditor: Ref<boolean> = ref(false);

const monitor = new BusyMonitor(globalSpinner, 100);

const externalConfigService = new ExternalConfigService();

const loadDetail = async () => {
    try {
        monitor.IfBusy("加载详情中");
        const result = await workClient.detail(props.id);
        work.value = result.work ?? new WorkDto();
        script.value = result.script ?? new BuildScript();
    } finally {
        monitor.NotBusyNow();
    }
};

/**保存工作项设置 */
const saveWork = async () => {
    try {
        monitor.IfBusy("正在保存工作项中...请稍后");
        await workClient.save(work.value);
        globalMessage.notice("保存成功！");
    } finally {
        monitor.NotBusyNow();
    }
};

let editor: monaco.editor.IStandaloneCodeEditor | null = null;
const needSave: Ref<boolean> = ref(false);

/**初始化脚本编辑器 */
const initCodeEditor = async () => {
    if (!codeEditorRef.value) {
        return;
    }

    if (hasInitCodeEditor.value) {
        return;
    }

    monaco.languages.register({
        id: "powershell",
        extensions: [".ps1", "psm1", "psd1"],
        aliases: ["pwsh"],
    });

    const code = (script.value?.script ?? [])?.join("\n");

    const codeModel = monaco.editor.createModel(code, "powershell");

    editor = monaco.editor.create(codeEditorRef.value, {
        language: "powershell",
        model: codeModel,
        lineNumbers: "on",
        roundedSelection: false,
        scrollBeyondLastLine: false,
        theme: "vs-dark",
        readOnly: false,
        automaticLayout: true,
    });

    hasInitCodeEditor.value = true;
};

/**保存脚本 */
const saveScripts = () => {
    if (!hasInitCodeEditor.value) {
        globalMessage.show("编辑器未初始化");
        return;
    }

    const value = editor!.getModel()?.getValue() ?? "";
    const dto = new BuildScriptDto();
    dto.init(script.value);
    dto.script = value?.replace("\r", "").split("\n") ?? [];
    dto.workId = work.value.id;

    workClient
        .saveScripts(dto)
        .then(() => {
            globalMessage.notice("保存成功！", 3);
        })
        .catch((e) => {
            globalMessage.show("保存出错");
        });
};

const giteeAuthentications: Array<{
    value: GiteeWebHookAuthentication;
    label: string;
}> = [
    {
        value: GiteeWebHookAuthentication.Srcret,
        label: "密码",
    },
    {
        value: GiteeWebHookAuthentication.SignatureKey,
        label: "签名密钥",
    },
];

const giteeWebHookEvents: Array<{ value: string; label: string }> = [
    {
        value: "Push Hook",
        label: "Push",
    },
    {
        value: "Tag Push Hook",
        label: "Tag Push",
    },
    {
        value: "Issue Hook",
        label: "Issue",
    },
    {
        value: "Merge Request Hook",
        label: "Pull Request",
    },
    {
        value: "Note Hook",
        label: "评论",
    },
];

const router = useRouter();
const backToWorkList = () => {
    router.push({
        name: "WorkList",
    });
};

const externalConfigLabel = computed(() => {
    console.log("work", work.value);
    return `${work.value.externalConfigType ?? ""}配置`;
});

const activedTab: Ref<string> = ref("");

const run = async () => {
    try {
        running.value = true;
        await workRunner.run(work.value.id);
    } catch (e) {
        alertException(e, (msg) => {
            globalMessage.show(msg);
        });
    } finally {
        running.value = false;
    }
};

const running: Ref<boolean> = ref(false);

const workExecutionLogs = new WorkExecutionLogClientProxy();
const logs: Ref<Array<WorkExecutionLogSummary>> = ref([]);
const logsQuerying: Ref<boolean> = ref(false);
const queryLogs = async () => {
    try {
        logsQuerying.value = true;
        logs.value = await workExecutionLogs.getSummaries(work.value.id);
    } finally {
        logsQuerying.value = false;
    }
};
const hasLogs = computed(() => {
    return logs.value.length >= 0;
});
const showLogDetailModalTarget = "showLogDetailModalTarget";
let showLogDetailModal: Modal;
const logDetail: Ref<WorkExecutionLog> = ref(new WorkExecutionLog());
const showDetail = async (logId: number) => {
    try {
        logsQuerying.value = true;
        logDetail.value = await workExecutionLogs.getDetail(
            work.value.id,
            logId
        );

        showLogDetailModal.show();
    } catch (e) {
        logDetail.value = new WorkExecutionLog();
    } finally {
        logsQuerying.value = false;
    }
};

onMounted(async () => {
    showLogDetailModal = BsModalHelper.useModal(showLogDetailModalTarget);
    await loadDetail();
});
</script>
<template>
    <div class="row mb-3">
        <div class="col-12">
            <button
                class="btn btn-outline-primary"
                type="button"
                @click="backToWorkList"
            >
                返回列表
            </button>
        </div>
    </div>
    <div class="row">
        <div class="col-12">
            <bs-tab @tab-actived="handleTabActived" :actived="activedTab">
                <bs-tab-item :id="BasicTab" label="基础信息">
                    <div class="row">
                        <div class="col-12">
                            <div class="mb-3">
                                <label class="form-label" for="displayName"
                                    >名称</label
                                ><input
                                    class="form-control"
                                    name="displayName"
                                    type="text"
                                    v-model="work.displayName"
                                />
                            </div>
                            <div class="mb-3">
                                <label
                                    class="form-label"
                                    for="externalConfigType"
                                >
                                    扩展配置
                                </label>
                                <select
                                    class="form-select"
                                    id="externalConfigType"
                                    name="externalConfigType"
                                    v-model="work.externalConfigType"
                                >
                                    <option
                                        v-for="t in externalConfigTypes"
                                        :value="t.value"
                                    >
                                        {{ t.label }}
                                    </option>
                                </select>
                            </div>
                            <div class="mb-3 btn-group">
                                <button
                                    class="btn btn-primary"
                                    @click="saveWork()"
                                >
                                    保存</button
                                ><button
                                    class="btn btn-outline-primary"
                                    @click="run"
                                    :disabled="running"
                                >
                                    <span>
                                        运行
                                        <Icon v-show="!running">
                                            <Play16Regular> </Play16Regular>
                                        </Icon>
                                    </span>
                                    <BsSpinner :show="running" size="sm">
                                    </BsSpinner>
                                </button>
                            </div>
                        </div>
                    </div>
                </bs-tab-item>
                <bs-tab-item :id="ExternalConfigTab" label="扩展配置">
                    <ExternalConfig
                        :external-type="work.externalConfigType"
                        :work-id="work.id"
                    >
                    </ExternalConfig>
                </bs-tab-item>
                <bs-tab-item :id="ScriptsTab" label="脚本设置">
                    <div class="row">
                        <div class="col-12 mb-2">
                            <div class="code-editor" ref="codeEditorRef"></div>
                        </div>
                        <div class="col-12 mb-2">
                            <button
                                class="btn btn-primary"
                                type="button"
                                @click="saveScripts"
                            >
                                保存
                            </button>
                        </div>
                    </div>
                </bs-tab-item>
                <bs-tab-item :id="LogTab" label="执行日志">
                    <div class="row">
                        <div class="col-12">
                            <button
                                class="btn btn-primary"
                                type="button"
                                @click="queryLogs"
                            >
                                查询
                            </button>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-12">
                            <table class="table">
                                <thead>
                                    <tr>
                                        <th style="width: 50px">序号</th>
                                        <th style="width: 100px">操作</th>
                                        <th>开始时间</th>
                                        <th>结束时间</th>
                                        <th>耗时</th>
                                        <th style="width: 100px">执行状态</th>
                                        <th style="width: 100px">执行结果</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr
                                        v-for="(log, index) in logs"
                                        :key="index"
                                    >
                                        <td>{{ index + 1 }}</td>
                                        <td>
                                            <button
                                                type="button"
                                                class="btn btn-sm btn-outline-primary"
                                                @click="showDetail(log.id)"
                                            >
                                                查看详情
                                            </button>
                                        </td>
                                        <td>
                                            {{
                                                formatStandardDateTime(
                                                    log.executeStartAt
                                                )
                                            }}
                                        </td>
                                        <td>
                                            {{
                                                formatStandardDateTime(
                                                    log.executeEndAt
                                                )
                                            }}
                                        </td>
                                        <td>
                                            {{
                                                formatElapsedTime(
                                                    log.elapsedTime
                                                )
                                            }}
                                        </td>
                                        <td>
                                            {{
                                                formatWorkExecutionStatus(
                                                    log.status
                                                )
                                            }}
                                        </td>
                                        <td
                                            :class="{
                                                'bg-danger': !log.success,
                                                'text-light': !log.success,
                                            }"
                                        >
                                            {{
                                                formatWorkExecutionResult(
                                                    log.success
                                                )
                                            }}
                                        </td>
                                    </tr>
                                    <tr v-if="logs.length == 0">
                                        <td class="text-center" colspan="7">
                                            没有数据
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                        <div
                            class="col-12 empty-table text-muted"
                            v-if="!hasLogs"
                        >
                            没有数据
                        </div>
                    </div>
                </bs-tab-item>
            </bs-tab>
        </div>
    </div>
    <BsModal
        :bs-target="showLogDetailModalTarget"
        size="xl"
        title="执行脚本及输出"
    >
        <WorkExecutionLogDetail :log="logDetail"></WorkExecutionLogDetail>
    </BsModal>
</template>
<style scoped>
.code-editor {
    height: 500px;
    border: 1px solid #eee;
}
</style>
