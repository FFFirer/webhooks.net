<script setup lang="ts">
import { PropType } from "vue";
import { WorkExecutionLog } from "@/shared/webapi/client";
import WorkExecutionLogResultLine from "./WorkExecutionLogResultLine.vue";
const props = defineProps({
    log: {
        type: Object as PropType<WorkExecutionLog>,
        default: null,
    },
});
</script>
<template>
    <div class="row">
        <div class="col-12">
            <p v-for="(lineScript, index) in log.script">
                {{ lineScript }}
            </p>
        </div>
        <hr />
        <div class="col-12">
            <WorkExecutionLogResultLine
                v-for="(lineResult, index) in log?.results"
                :line="lineResult"
            ></WorkExecutionLogResultLine>
        </div>
    </div>
</template>
