<script setup lang="ts">
import { useGlobalMessage } from "@/components/GlobalMessage/GlobalMessageProxy";
import { useGlobalSpinner } from "@/components/GlobalSpinner/GlobalSpinnerProxy";
import { BusyMonitor } from "@/components/Monitor/BusyMonitor";
import { SettingClientProxy } from "@/shared/client-proxy";
import { BasicSetting } from "@/shared/webapi/client";
import { onMounted, ref, Ref } from "vue";
import BsSpinner from "@/components/BsSpinner/BsSpinner.vue";

const settings = new SettingClientProxy();
const globalMessage = useGlobalMessage();
const globalSpinner = useGlobalSpinner();
const monitor = new BusyMonitor(globalSpinner, 100);

const basicSeting: Ref<BasicSetting> = ref(new BasicSetting());

const load = async () => {
    try {
        monitor.IfBusy("正在加载中...请稍候");
        basicSeting.value = await settings.getBasicSetting();
    } finally {
        monitor.NotBusyNow();
    }
};

const save = async () => {
    try {
        monitor.IfBusy("正在保存中...请稍候");
        saving.value = true;
        await settings.saveBasicSetting(basicSeting.value);
        globalMessage.notice("保存成功");
    } finally {
        monitor.NotBusyNow();
        saving.value = false;
    }
};

const saving: Ref<boolean> = ref(false);

onMounted(async () => {
    await load();
});
</script>
<template>
    <div class="row">
        <div class="col-12 mb-3">
            <label for="baseWorkDirectory" class="form-label"
                >基础工作目录</label
            >
            <input
                type="text"
                class="form-control"
                v-model="basicSeting.baseWorkDirectory"
            />
        </div>

        <div class="col-12 mb-3">
            <button class="btn btn-primary" @click="save" :disabled="saving">
                保存
                <BsSpinner :show="saving" size="sm"></BsSpinner>
            </button>
        </div>
    </div>
</template>
