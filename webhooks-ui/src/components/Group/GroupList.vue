<script setup lang="ts">
import { Ref, ref } from "vue";
import { GroupClientProxy } from "../../shared/client-proxy";
import { GroupDto } from "../../shared/webapi/client";

const groupClient = new GroupClientProxy();

const groups: Ref<GroupDto[]> = ref([]);

const list = async () => {
    groups.value = await groupClient.list();
};
</script>
<template>
    <div class="row">
        <div class="col-12">
            <h1>所有分组</h1>
        </div>
        <div class="col-12 toolbar">
            <button type="button" class="btn btn-primary">添加</button>
            <button type="button" class="btn btn btn-light" @click="list()">
                搜索
            </button>
        </div>
        <div class="col-12">
            <table class="table">
                <thead>
                    <tr>
                        <th>#</th>
                        <th>分组名称</th>
                        <th>任务数量</th>
                        <th>创建时间</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="(group, index) in groups" :key="index">
                        <td>
                            {{ index + 1 }}
                        </td>
                        <td>
                            {{ group.name }}
                        </td>
                        <td>暂无</td>
                        <td>暂无</td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
</template>

<style scoped>
.toolbar .btn {
    margin-right: 1rem;
}
</style>
