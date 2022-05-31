<script setup lang="ts">
import BsTab from "../../components/BsTabs/BsTab.vue";
import BsTabItem from "../../components/BsTabs/BsTabItem.vue";

const BasicTabId = "basic";
const SetupTabId = "setup";
const LogTabId = "log";

const handleTabActived = (id: string) => {
    console.log("active", id);
};
</script>
<template>
    <div class="row">
        <div class="col-12">
            <bs-tab @tab-actived="handleTabActived">
                <bs-tab-item :id="BasicTabId" label="基础信息">
                    <div class="row">
                        <div class="col-12"></div>
                    </div>
                </bs-tab-item>
                <bs-tab-item :id="SetupTabId" label="任务配置">
                    <div class="row">
                        <div class="col-12"></div>
                    </div>
                </bs-tab-item>
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

    <div class="row"></div>
</template>
