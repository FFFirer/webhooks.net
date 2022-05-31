<script setup lang="ts">
import { onMounted, ref, Ref } from "vue";
import BsTab from "../../components/BsTabs/BsTab.vue";
import BsTabItem from "../../components/BsTabs/BsTabItem.vue";
import { useGlobalMessage } from "../../components/GlobalMessage/GlobalMessageProxy";
import {
    GiteeConfigClientProxy,
    WorkClientProxy,
} from "../../shared/client-proxy";
import {
    BuildScript,
    GiteeWebHookAuthentication,
    GiteeWebHookConfigDto,
    SaveGiteeWebHookConfigInput,
    WorkDetailDto,
    WorkDto,
} from "../../shared/webapi/client";
import WorkDetailViewProps from "./WorkDetailProps";
import Clipboard from "clipboard";

const props = defineProps(WorkDetailViewProps);

const globalMessage = useGlobalMessage();

const BasicTabId = "basic";
const SetupTabId = "setup";
const LogTabId = "log";
const ScriptsId = "scripts";

const copyBtnRef: Ref<HTMLButtonElement | undefined> = ref();

const handleTabActived = (id: string) => {
    console.log("active", id);
};

const workClient = new WorkClientProxy();
const giteeConfigClient = new GiteeConfigClientProxy();

const work: Ref<WorkDto> = ref(new WorkDto());
const config: Ref<GiteeWebHookConfigDto> = ref(new GiteeWebHookConfigDto());
const scripts: Ref<Array<BuildScript>> = ref([]);

const loadDetail = async () => {
    const result = await workClient.detail(props.id);
    work.value = result.work ?? new WorkDto();
    config.value = result.config ?? new GiteeWebHookConfigDto();
    scripts.value = result.scripts ?? [];
};

const saveWork = async () => {
    await workClient
        .save(work.value)
        .then(() => {
            globalMessage.notice("保存成功！");
        })
        .catch((e) => {});
};

const saveGiteeConfig = async () => {
    const input = new SaveGiteeWebHookConfigInput();
    input.init(config.value);
    await giteeConfigClient
        .save(input)
        .then(() => {
            globalMessage.notice("保存成功！");
        })
        .catch((e) => {});
};

const initCopy = () => {
    if (copyBtnRef.value) {
        const clipboard = new Clipboard(copyBtnRef.value, {
            text: function () {
                return config.value.webHookUrl ?? "";
            },
            action: function () {
                return "copy";
            },
            container: this,
        });

        clipboard.on("success", () => {
            globalMessage.notice("复制成功");
        });
    }
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

onMounted(async () => {
    await loadDetail();
    initCopy();
});
</script>
<template>
    <div class="row mb-3">
        <div class="col-12">
            <button type="button" class="btn btn-outline-primary">
                返回列表
            </button>
        </div>
    </div>
    <div class="row">
        <div class="col-12">
            <bs-tab @tab-actived="handleTabActived">
                <bs-tab-item :id="BasicTabId" label="基础信息">
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
                                <button
                                    class="btn btn-primary"
                                    @click="saveWork()"
                                >
                                    保存
                                </button>
                            </div>
                        </div>
                    </div>
                </bs-tab-item>
                <bs-tab-item :id="SetupTabId" label="WebHook">
                    <div class="row">
                        <div class="col-12">
                            <div class="mb-3">
                                <label for="webHookUrl" class="form-label">
                                    WebHook地址
                                    <button
                                        class="btn btn-sm btn-outline-primary"
                                        ref="copyBtnRef"
                                    >
                                        copy
                                    </button>
                                </label>
                                <input
                                    type="text"
                                    name="webHookUrl"
                                    class="form-control"
                                    v-model="config.webHookUrl"
                                />
                            </div>
                            <div class="mb-3">
                                <label for="authentication">
                                    密码/签名密钥
                                </label>
                                <select
                                    name="authentication"
                                    id="authentication"
                                    class="form-select"
                                >
                                    <option
                                        v-for="a in giteeAuthentications"
                                        :value="a.value"
                                    >
                                        {{ a.label }}
                                    </option>
                                </select>
                            </div>

                            <div class="mb-3">
                                <input
                                    type="text"
                                    class="form-control"
                                    name="secret"
                                    v-model="config.authenticationKey.value"
                                />
                            </div>

                            <div class="mb-3">
                                <label for="event">触发事件</label>

                                <div
                                    class="form-check"
                                    v-for="(evt, index) in giteeWebHookEvents"
                                    :key="index"
                                >
                                    <input
                                        type="checkbox"
                                        class="form-check-input"
                                        name="event"
                                        v-model="config.events"
                                        :value="evt.value"
                                    />
                                    <label class="form-check-label">
                                        {{ evt.label }}
                                    </label>
                                </div>
                            </div>

                            <div class="mb-3">
                                <button
                                    class="btn btn-primary"
                                    @click="saveGiteeConfig()"
                                >
                                    保存
                                </button>
                            </div>
                        </div>
                    </div>
                </bs-tab-item>
                <bs-tab-item :id="ScriptsId" label="脚本设置"> </bs-tab-item>
                <bs-tab-item :id="LogTabId" label="执行日志">
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
            </bs-tab>
        </div>
    </div>
</template>
