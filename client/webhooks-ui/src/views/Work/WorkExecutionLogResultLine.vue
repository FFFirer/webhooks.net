<script setup lang="ts">
import {
    WorkExecutionLog,
    ShellExecutedResultLine,
    ResultLineLevel,
} from "@/shared/webapi/client";
import AnsiUp from "ansi_up";
import { computed, defineProps, PropType } from "vue";
import WorkExecutionLogResultLineExceptionContainer from "./WorkExecutionLogResultLineExceptionContainer.vue";

const ansiUp = new AnsiUp();

const props = defineProps({
    line: {
        type: Object as PropType<ShellExecutedResultLine>,
        default: {},
    },
    showWarningDetail: {
        type: Boolean,
        default: false,
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

const convertAnsiToHtml = (content: string) => {
    return ansiUp.ansi_to_html(content);
};
</script>

<template>
    <div :class="lineClasses">
        <p class="xrow" v-html="convertAnsiToHtml(line.message)"></p>
        <WorkExecutionLogResultLineExceptionContainer
            v-if="errorLine && showWarningDetail"
            :line="line"
        >
        </WorkExecutionLogResultLineExceptionContainer>
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
</style>
