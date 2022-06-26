<script setup lang="ts">
import {
    WorkExecutionLog,
    ShellExecutedResultLine,
    ResultLineLevel,
} from "@/shared/webapi/client";
import { computed, defineProps, PropType } from "vue";
const props = defineProps({
    line: {
        type: Object as PropType<ShellExecutedResultLine>,
        default: {},
    },
});

const lineClasses = computed(() => {
    return {
        "result-line": true,
    };
});

const errorLine = computed(() => {
    return props.line.level == ResultLineLevel.Error;
});

const exceptionLines = computed(() => {
    return props.line?.exception?.split("\r\n") ?? [];
});

const exceptionLineClasses = (line: string) => {
    return {
        "script-exception-line-at": line.indexOf(" ") == 0,
        "script-exception-line": line.indexOf(" ") > 0,
    };
};
</script>

<template>
    <div :class="lineClasses">
        <p>{{ line.message }}</p>
        <div v-if="errorLine" class="script-stack-trace text-border mb-1">
            <div class="badge bg-info">出错位置</div>
            {{ line.stackTrace }}
        </div>

        <div v-if="errorLine" class="script-exception text-danger">
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

<style scoped>
.line-info {
    background-color: lightgreen;
}

.line-error {
    /* color: red; */
    background-color: red;
    color: white;
}
.result-line {
    margin: 0;
    padding: 0;
}
.result-line p {
    margin: 0;
    padding: 0;
}

.result-line .script-stack-trace {
    text-indent: 4;
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

.result-line .text-border {
    border: 1px solid black;
    border-radius: 5px;
    padding: 5px;
}
</style>
