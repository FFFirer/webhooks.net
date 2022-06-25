<script setup lang="ts">
import { defineAsyncComponent, onMounted, ref, Ref } from "vue";
import BsTab from "@/components/BsTabs/BsTab.vue";
import BsTabItem from "@/components/BsTabs/BsTabItem.vue";
import BsSpinner from "@/components/BsSpinner/BsSpinner.vue";
import { useGlobalMessage } from "@/components/GlobalMessage/GlobalMessageProxy";
import {
    GiteeConfigClientProxy,
    WorkClientProxy,
    WorkRunnerClientProxy,
} from "@/shared/client-proxy";
import {
    BuildScript,
    BuildScriptDto,
    GiteeWebHookAuthentication,
    GiteeWebHookConfigDto,
    SaveGiteeWebHookConfigInput,
    WorkDto,
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

    const code = (script.value?.scripts ?? [])?.join("\n");

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
    dto.scripts = value?.split("\n") ?? [];
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

const externalConfigTypes: Array<{ label: string; value: string }> = [
    {
        label: "无",
        value: "",
    },
    {
        label: "Git",
        value: "git",
    },
    {
        label: "Gitee",
        value: "gitee",
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

onMounted(async () => {
    await loadDetail();
});
</script>
<template>
    <div class="row mb-3">
        <div class="col-12">
            <button
                type="button"
                class="btn btn-outline-primary"
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
                                <label for="displayName" class="form-label">
                                    名称
                                </label>
                                <input
                                    name="displayName"
                                    type="text"
                                    class="form-control"
                                    v-model="work.displayName"
                                />
                            </div>
                            <div class="mb-3">
                                <label
                                    for="externalConfigType"
                                    class="form-label"
                                >
                                    扩展配置
                                </label>
                                <select
                                    name="externalConfigType"
                                    id="externalConfigType"
                                    class="form-select"
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
                                    保存
                                </button>
                                <button
                                    @click="run"
                                    class="btn btn-outline-primary"
                                    :disabled="running"
                                >
                                    <span>
                                        运行
                                        <Icon v-show="!running">
                                            <Play16Regular />
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
                    ></ExternalConfig>
                </bs-tab-item>
                <bs-tab-item :id="ScriptsTab" label="脚本设置">
                    <div class="row">
                        <div class="col-12 mb-2">
                            <div class="code-editor" ref="codeEditorRef"></div>
                        </div>
                        <div class="col-12 mb-2">
                            <button
                                type="button"
                                class="btn btn-primary"
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
                            <table class="table">
                                <thead>
                                    <tr>
                                        <th>开始时间</th>
                                        <th>耗时</th>
                                        <th>结果</th>
                                    </tr>
                                </thead>
                                <tbody></tbody>
                            </table>
                        </div>
                        <div class="col-12 empty-table text-muted" v-if="true">
                            没有数据
                        </div>
                    </div>
                </bs-tab-item>
                <!-- <bs-tab-item id="test" label="测试websocket">
                    <div class="row">
                        <div class="col-12">
                            <button
                                type="button"
                                class="btn btn-primary"
                                @click="connectLspServer"
                            >
                                测试
                            </button>
                        </div>
                    </div>
                </bs-tab-item> -->
            </bs-tab>
        </div>
    </div>
</template>
<style scoped>
.code-editor {
    height: 500px;
    border: 1px solid #eee;
}
</style>
