<script setup lang="ts">
import { useGlobalMessage } from "@/components/GlobalMessage/GlobalMessageProxy";
import { GitConfigClientProxy } from "@/shared/client-proxy";
import { GitConfigDto, SaveGitConfigInput } from "@/shared/webapi/client";
import ClipboardJS from "clipboard";
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
    Ref,
    ref,
} from "vue";

const props = defineProps({
    workId: {
        type: String,
        default: "",
    },
});
const globalMessage = useGlobalMessage();
const gitConfigClient = new GitConfigClientProxy();
const config: Ref<GitConfigDto> = ref(new GitConfigDto());

const load = async () => {
    config.value =
        (await gitConfigClient.get(props.workId)) ?? new GitConfigDto();
};
const save = async () => {
    const input = new SaveGitConfigInput();
    input.toSave = config.value;
    try {
        await gitConfigClient.save(input);
        globalMessage.notice("保存成功");
    } finally {
    }
};

const copyBtnRef = ref();
const initCopy = () => {
    if (!copyBtnRef.value) {
        return;
    }

    const clipboard = new ClipboardJS(copyBtnRef.value, {
        text: () => {
            return config.value.repositoryAddress ?? "";
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
    console.log("git mounted", props.workId);
    await load();
    initCopy();
});

onBeforeMount(() => console.log("git before mount", props.workId));
onBeforeUpdate(() => console.log("git before update", props.workId));
onUpdated(() => console.log("git update", props.workId));
onBeforeUnmount(() => console.log("git before unmount", props.workId));
onUnmounted(() => console.log("git unmounted", props.workId));
onErrorCaptured(() => console.log("git error captured", props.workId));
onRenderTracked(() => console.log("git render tracked", props.workId));
onRenderTriggered(() => console.log("git render triggered", props.workId));
onActivated(() => console.log("git activated", props.workId));
onDeactivated(() => console.log("git deactivated", props.workId));
</script>
<template>
    <div class="row">
        <div class="col-12 mb-3">
            <label for="addressType" class="form-label"> 仓库地址类型 </label>
            <select
                name="addressType"
                class="form-control"
                v-model="config.addressType"
            >
                <option value="https">HTTPS</option>
                <option value="ssh">SSH</option>
            </select>
        </div>
        <div class="col-12 mb-3">
            <label for="repositoryAddress" class="form-label">
                仓库地址
                <button class="btn btn-sm btn-outline-primary" ref="copyBtnRef">
                    复制
                </button>
            </label>
            <input
                type="text"
                name="repositoryAddress"
                class="form-control"
                v-model="config.repositoryAddress"
            />
        </div>
        <div class="col-12 mb-3">
            <label for="branch" class="form-label">分支</label>
            <input
                type="text"
                name="branch"
                class="form-control"
                v-model="config.branch"
            />
        </div>
        <div class="col-12 mb-3">
            <label for="tag" class="form-label">标签</label>
            <input
                type="text"
                name="tag"
                class="form-control"
                v-model="config.tag"
            />
        </div>
        <div class="col-12 mb-3">
            <label for="username" class="form-label"> 用户名 </label>
            <input
                type="text"
                name="username"
                class="form-control"
                v-model="config.userName"
            />
        </div>
        <div class="col-12 mb-3">
            <label for="email" class="form-label">邮件</label>
            <input
                type="text"
                name="email"
                class="form-control"
                v-model="config.email"
            />
        </div>
        <div class="col-12 mb-3">
            <label for="password" class="form-label">密码</label>
            <input
                type="password"
                name="password"
                class="form-control"
                v-model="config.password"
            />
        </div>
        <div class="col-12 mb-3">
            <button class="btn btn-primary" @click="save">保存</button>
        </div>
    </div>
</template>
