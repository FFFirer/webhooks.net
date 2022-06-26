<script setup lang="ts">
import {
    onActivated,
    onBeforeMount,
    onBeforeUnmount,
    onBeforeUpdate,
    onDeactivated,
    onErrorCaptured,
    onMounted,
    onRenderTracked,
    onRenderTriggered,
    onUnmounted,
    onUpdated,
    ref,
    Ref,
} from "vue";
import { GiteeConfigClientProxy } from "@/shared/client-proxy";
import {
    GiteeWebHookAuthentication,
    GiteeWebHookConfigDto,
    SaveGiteeWebHookConfigInput,
} from "@/shared/webapi/client";
import { useGlobalMessage } from "@/components/GlobalMessage/GlobalMessageProxy";
import { useGlobalSpinner } from "@/components/GlobalSpinner/GlobalSpinnerProxy";
import { BusyMonitor } from "@/components/Monitor/BusyMonitor";
import ClipboardJS from "clipboard";

const props = defineProps({
    workId: {
        type: String,
        default: "",
    },
});

const config: Ref<GiteeWebHookConfigDto> = ref(new GiteeWebHookConfigDto());

/**gitee 身份验证 */
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

/**gitee webhook 事件 */
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

const globalMessage = useGlobalMessage();
const globalSpinner = useGlobalSpinner();

const monitor = new BusyMonitor(globalSpinner);

const giteeConfigClient = new GiteeConfigClientProxy();

/**加载 */
const load = async () => {
    config.value = await giteeConfigClient.get(props.workId);
};

/**保存配置 */
const saveGiteeConfig = async () => {
    const input = new SaveGiteeWebHookConfigInput();

    input.init(config.value);
    monitor.IfBusy("正在保存中");
    await giteeConfigClient
        .save(input)
        .then(() => {
            globalMessage.notice("保存成功！");
        })
        .catch((e) => {})
        .finally(() => {
            monitor.NotBusyNow();
        });
};

const copyBtnRef = ref();
const initCopy = () => {
    if (!copyBtnRef.value) {
        return;
    }

    const clipboard = new ClipboardJS(copyBtnRef.value, {
        text: () => {
            return config.value.webHookUrl ?? "";
        },
        action: () => {
            return "copy";
        },
        container: this,
    });

    clipboard.on("success", () => {
        globalMessage.notice("复制成功");
    });
};

onMounted(async () => {
    console.log("gitee mounted", props.workId);
    await load();
    initCopy();
});

onBeforeMount(() => console.log("gitee before mount", props.workId));
onBeforeUpdate(() => console.log("gitee before update", props.workId));
onUpdated(() => console.log("gitee update", props.workId));
onBeforeUnmount(() => console.log("gitee before unmount", props.workId));
onUnmounted(() => console.log("gitee unmounted", props.workId));
onErrorCaptured(() => console.log("gitee error captured", props.workId));
onRenderTracked(() => console.log("gitee render tracked", props.workId));
onRenderTriggered(() => console.log("gitee render triggered", props.workId));
onActivated(() => console.log("gitee activated", props.workId));
onDeactivated(() => console.log("gitee deactivated", props.workId));
</script>
<template>
    <div class="row" id="gitee-external-config">
        <div class="col-12 mb-3">
            <label for="webHookUrl" class="form-label">
                WebHook地址
                <button class="btn btn-sm btn-outline-primary" ref="copyBtnRef">
                    复制
                </button>
            </label>
            <input
                type="text"
                name="webHookUrl"
                class="form-control"
                v-model="config.webHookUrl"
            />
        </div>
        <div class="col-12 mb-3">
            <label for="authentication"> 密码/签名密钥 </label>
            <select
                name="authentication"
                id="authentication"
                class="form-select"
                v-model="config.authentication"
                placeholder="请选择一个授权方式"
            >
                <option v-for="a in giteeAuthentications" :value="a.value">
                    {{ a.label }}
                </option>
            </select>
        </div>
        <div class="col-12 mb-3">
            <input
                type="text"
                class="form-control"
                name="secret"
                v-model="config.authenticationKey.value"
            />
        </div>
        <div class="col-12 mb-3">
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
        <div class="col-12 mb-3">
            <button class="btn btn-primary" @click="saveGiteeConfig()">
                保存
            </button>
        </div>
    </div>
</template>
