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
        "line-info": props.line.level == ResultLineLevel.Info,
        "line-error": props.line.level == ResultLineLevel.Error,
    };
});

const errorLine = computed(() => {
    return props.line.level == ResultLineLevel.Error;
});
</script>

<template>
    <div :class="lineClasses">
        <p>{{ line.message }}</p>
        <p v-if="errorLine">
            {{ line.stackTrace }}
        </p>
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
</style>
