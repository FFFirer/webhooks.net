<script setup lang="ts">
import { ShellExecutedResultLine } from "@/shared/webapi/client";
import { computed, PropType, Ref, ref } from "vue";
const props = defineProps({
    line: {
        type: Object as PropType<ShellExecutedResultLine>,
        default: {},
    },
});

const showSwitch: Ref<boolean> = ref(false);

const scriptStackTraceLines = computed(() => {
    return props.line?.stackTrace?.split("\r\n") ?? [];
});
const scriptStackTraceLineClasses = (line: string) => {
    return {
        "script-stack-trace-line": true,
    };
};

const exceptionLines = computed(() => {
    return props.line?.exception?.split("\r\n") ?? [];
});
const exceptionLineClasses = (line: string) => {
    return {
        "script-exception-line-at": line.indexOf(" ") == 0,
        "script-exception-line": line.indexOf(" ") > 0,
    };
};

const switchShowStatus = () => {
    showSwitch.value = !showSwitch.value;
};
</script>
<template>
    <div>
        <button
            type="button"
            class="btn badge bg-info mb-1"
            @click="switchShowStatus"
        >
            查看异常详情
        </button>
        <div v-show="showSwitch" class="script-stack-trace text-border mb-1">
            <div class="badge bg-info">异常调用堆栈</div>
            <p
                v-for="(stackTraceLine, index) in scriptStackTraceLines"
                :key="index"
                :class="scriptStackTraceLineClasses(stackTraceLine)"
            >
                {{ stackTraceLine }}
            </p>
        </div>

        <div v-show="showSwitch" class="script-exception text-danger">
            <div class="badge bg-danger">异常详细信息</div>
            <p
                v-for="(exceptionLine, index) in exceptionLines"
                :key="index"
                :class="exceptionLineClasses(exceptionLine)"
            >
                {{ exceptionLine }}
            </p>
        </div>
    </div>
</template>
<style>
.result-line .script-stack-trace .script-stack-trace-line {
    text-indent: 30px;
}

.result-line .script-exception {
    border: 1px solid black;
    border-radius: 5px;
    padding: 5px;
    overflow: auto;
}

.result-line .script-exception .script-exception-line-at {
    text-indent: 50px;
}

.result-line .script-exception .script-exception-line {
    text-indent: 30px;
}

.text-border {
    border: 1px solid black;
    border-radius: 5px;
    padding: 5px;
}
</style>
