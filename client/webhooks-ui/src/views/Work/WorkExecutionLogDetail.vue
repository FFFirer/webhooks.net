<script setup lang="ts">
import { PropType, computed } from "vue";
import {
    ResultLineLevel,
    ShellExecutedResultLine,
    WorkExecutionLog,
} from "@/shared/webapi/client";
import BsTab from "@/components/BsTabs/BsTab.vue";
import BsTabItem from "@/components/BsTabs/BsTabItem.vue";
import WorkExecutionLogResultLine from "./WorkExecutionLogResultLine.vue";
import { formatStandardDateTime, formatElapsedTime } from "@/utils/datetime";
import {
    formatWorkExecutionStatus,
    formatWorkExecutionResult,
} from "@/utils/work";

const basicInfoTabId = "basicInfoTab";
const resultsTabId = "resultsTab";
const scriptTabId = "scriptTabId";

const props = defineProps({
    log: {
        type: Object as PropType<WorkExecutionLog>,
        default: null,
    },
});

const rowClasses = (
    line: ShellExecutedResultLine
): { [key: string]: boolean } => {
    return {
        "table-warning": line.level == ResultLineLevel.Error,
    };
};

const exceptionLines = computed(() => {
    return props.log.exception?.split("\r\n") ?? ["无异常"];
});
</script>
<template>
    <div class="row">
        <div class="col-12">
            <BsTab>
                <BsTabItem :id="basicInfoTabId" label="基础信息">
                    <div class="row">
                        <div for="executeStartAt" class="col-2">
                            开始执行时间
                        </div>
                        <div class="col-10">
                            {{ formatStandardDateTime(log.executeStartAt) }}
                        </div>
                    </div>
                    <div class="row">
                        <label for="executeEndAt" class="col-2">
                            结束执行时间
                        </label>
                        <div class="col-10">
                            {{ formatStandardDateTime(log.executeEndAt) }}
                        </div>
                    </div>
                    <div class="row">
                        <label for="elapsedTime" class="col-2"> 耗时 </label>
                        <div class="col-10">
                            {{ formatElapsedTime(log.elapsedTime) }}
                        </div>
                    </div>
                    <div class="row">
                        <label for="status" class="col-2"> 状态 </label>
                        <div class="col-10">
                            {{ formatWorkExecutionStatus(log.status) }}
                        </div>
                    </div>
                    <div class="row">
                        <label for="success" class="col-2"> 结果 </label>
                        <div
                            class="col-10"
                            :class="{
                                'bg-danger': !log.success,
                                'text-light': !log.success,
                            }"
                        >
                            {{
                                formatWorkExecutionResult(
                                    log.status,
                                    log.success
                                )
                            }}
                        </div>
                    </div>
                    <div class="row">
                        <label for="success" class="col-2"> 异常 </label>
                        <div class="col-10">
                            <!-- {{ log.exception }} -->
                            <p
                                v-for="(exceptionLine, index) in exceptionLines"
                                :key="index"
                                class="exception-line"
                            >
                                {{ exceptionLine }}
                            </p>
                        </div>
                    </div>
                </BsTabItem>
                <BsTabItem :id="resultsTabId" label="脚本执行输出">
                    <table class="table table-bordered">
                        <tbody>
                            <tr
                                v-for="(lineResult, index) in log?.results"
                                :key="index"
                                :class="rowClasses(lineResult)"
                            >
                                <td style="width: 100px">行 {{ index + 1 }}</td>
                                <td>
                                    <WorkExecutionLogResultLine
                                        :line="lineResult"
                                    ></WorkExecutionLogResultLine>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </BsTabItem>
                <BsTabItem :id="scriptTabId" label="执行脚本">
                    <table class="table table-striped table-bordered">
                        <tbody>
                            <tr v-for="(lineScript, index) in log.script">
                                <td
                                    style="
                                        border-right: 1px solid balck;
                                        width: 100px;
                                    "
                                >
                                    行 {{ index + 1 }}
                                </td>
                                <td>
                                    {{ lineScript }}
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </BsTabItem>
            </BsTab>
        </div>
    </div>
</template>
<style scoped>
.exception-line {
    margin: 0;
    padding: 0;
    text-indent: 30px;
}

.exception-line:first-child {
    text-indent: 0;
}
</style>
