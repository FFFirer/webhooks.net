<script setup lang="ts">
import { Modal } from "bootstrap";
import { onMounted, Ref, ref } from "vue";
import { GroupClientProxy } from "../../shared/client-proxy";
import { GroupDto } from "../../shared/webapi/client";

// 模态框
const modalRef: Ref<HTMLDivElement | undefined> = ref();

const groupClient = new GroupClientProxy();

const groups: Ref<GroupDto[]> = ref([]);

// bootstrap modal 实例
let modal: bootstrap.Modal;

onMounted(() => {
    if (modalRef?.value != undefined) {
        modalRef.value.addEventListener("hidden.bs.modal", () => {
            // TODO: 清空表单
        });
        modal = new Modal(modalRef.value);
    }
});

const list = async () => {
    groups.value = await groupClient.list();
};

const modalTitle = ref("");

const createGroup = () => {
    modalTitle.value = "新增";
    modal.show();
};

const saveGroup = () => {
    // TODO: 调用保存
    modal.hide();
};
</script>
<template>
    <div class="row">
        <div class="col-12">
            <h1>所有分组</h1>
        </div>
        <div class="col-12 toolbar">
            <button
                type="button"
                class="btn btn-primary"
                @click="createGroup()"
            >
                添加
            </button>
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

    <div
        ref="modalRef"
        class="modal fade"
        tabindex="-1"
        aria-labelledby="exampleModalLabel"
        aria-hidden="true"
    >
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLabel">
                        {{ modalTitle }}
                    </h5>
                    <button
                        type="button"
                        class="btn-close"
                        data-bs-dismiss="modal"
                        aria-label="Close"
                    ></button>
                </div>
                <div class="modal-body">Hello!</div>
                <div class="modal-footer">
                    <button
                        type="button"
                        class="btn btn-secondary"
                        data-bs-dismiss="modal"
                    >
                        取消
                    </button>
                    <button
                        type="button"
                        class="btn btn-primary"
                        @click="saveGroup()"
                    >
                        保存
                    </button>
                </div>
            </div>
        </div>
    </div>
</template>

<style scoped>
.toolbar .btn {
    margin-right: 1rem;
}
</style>
