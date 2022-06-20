<script setup lang="ts">
import { Modal } from "bootstrap";
import { onMounted, Ref, ref } from "vue";
import { useRouter } from "vue-router";
import { WorkClientProxy } from "../../shared/client-proxy";
import {
    ApiException,
    PagingInput,
    RemoveWorkInput,
    WorkDto,
} from "../../shared/webapi/client";
import BsModal from "../../components/BsModal/BsModal.vue";
import BsModalHelper from "../../components/BsModal/BsModalHelper";
import BsPagination from "../../components/BsPagination/BsPagination.vue";
import { useGlobalMessage } from "../../components/GlobalMessage/GlobalMessageProxy";
import {
    GlobalSpinnerProxy,
    useGlobalSpinner,
} from "../../components/GlobalSpinner/GlobalSpinnerProxy";
import { BusyMonitor } from "../../components/Monitor/BusyMonitor";
import BsSpinner from "../../components/BsSpinner/BsSpinner.vue";

const modalTitle = ref("");
const workClient = new WorkClientProxy();
const router = useRouter();

const works: Ref<Array<WorkDto>> = ref([]);
const editWorkModalTarget = "editWorkModal";

let editWorkModal: Modal;

const globalMsg = useGlobalMessage();
const globalSpinner = useGlobalSpinner();

let editingWork: Ref<WorkDto> = ref(new WorkDto());

// 分页信息
const currentPage: Ref<number> = ref(1);
const pageSize: Ref<number> = ref(20);
const total: Ref<number> = ref(0);

const isQuerying: Ref<boolean> = ref(false);
/**
 * 查询
 * @param page 页码
 */
const query = async (page: number = 1) => {
    try {
        const input: PagingInput = new PagingInput({
            page: page,
            pageSize: pageSize.value,
        });
        isQuerying.value = true;
        globalSpinner.show("正在查询中...请稍后");
        var result = await workClient.query(input);
        works.value = result.rows;
        currentPage.value = page;
        total.value = result.total;
    } catch (error) {
        if ((error as any)["isApiException"]) {
            console.log((error as ApiException).message);

            globalMsg?.show((error as ApiException).message);
        } else {
            globalMsg?.show("未连接到服务器");
        }
    } finally {
        isQuerying.value = false;
        globalSpinner.close();
    }
};

const cancelEdit = () => {
    editingWork.value = new WorkDto();
    editWorkModal.hide();
};

const onEditWorkModalHidden = () => {
    editingWork.value = new WorkDto();
};

const isSaving: Ref<boolean> = ref(false);

/**保存 */
const save = async () => {
    const input = editingWork.value;
    try {
        isSaving.value = true;
        await workClient.save(input);
        await query(currentPage.value);
    } finally {
        isSaving.value = false;
    }

    editWorkModal.hide();
};

/**创建 */
const create = () => {
    modalTitle.value = "创建工作项";

    editWorkModal.show();
};

/**编辑 */
const edit = (work: WorkDto) => {
    modalTitle.value = "编辑工作项";

    editingWork.value = work;

    editWorkModal.show();
};

/**移除 */
const remove = async (work: WorkDto) => {
    const input = new RemoveWorkInput({
        id: work.id,
    });

    await workClient.remove(input);
    await query(currentPage.value);
};

const handlePageChanged = async (page: number) => {
    await query(page);
};

/**查看详细信息 */
const showDetail = (id: string) => {
    router.push({
        name: "WorkDetail",
        params: {
            id: id,
        },
    });
};

onMounted(async () => {
    editWorkModal = BsModalHelper.useModal(editWorkModalTarget);

    await query(currentPage.value);
});
</script>
<template>
    <div class="row">
        <div class="col-12">
            <h1>所有工作项</h1>
        </div>
        <div class="col-12 toolbar">
            <button type="button" class="btn btn-primary" @click="create()">
                添加
            </button>
            <button
                type="button"
                class="btn btn-primary"
                @click="query()"
                :disabled="isQuerying"
            >
                搜索
                <BsSpinner :show="isQuerying" size="sm"></BsSpinner>
            </button>
        </div>
        <div class="col-12">
            <table class="table">
                <thead>
                    <tr>
                        <th class="w-50px">#</th>
                        <th class="w-200px">操作</th>
                        <th>名称</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="(work, index) in works" :key="index">
                        <td>
                            {{ index + 1 }}
                        </td>
                        <td>
                            <div class="btn-group">
                                <button
                                    type="button"
                                    class="btn btn-sm btn-secondary"
                                    @click="showDetail(work.id)"
                                >
                                    详情
                                </button>
                                <button
                                    type="button"
                                    class="btn btn-sm btn-primary"
                                    @click="edit(work)"
                                >
                                    编辑
                                </button>
                                <button
                                    type="button"
                                    class="btn btn-sm btn-danger"
                                    @click="remove(work)"
                                >
                                    删除
                                </button>
                            </div>
                        </td>
                        <td>
                            {{ work.displayName }}
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div class="col-12 empty-table text-muted" v-if="works.length == 0">
            没有数据
        </div>
        <div class="col-12">
            <bs-pagination
                :page="currentPage"
                :total="total"
                :page-size="pageSize"
                @page-changed="handlePageChanged"
            >
            </bs-pagination>
        </div>
    </div>

    <BsModal :bs-target="editWorkModalTarget" @hidden="onEditWorkModalHidden">
        <template #header>
            <h5 id="modalTitle" class="modal-title">
                {{ modalTitle }}
            </h5>
        </template>
        <div class="row">
            <div class="col">
                <div class="form-group">
                    <label for="name" class="form-label">名称</label>
                    <input
                        type="text"
                        class="form-control"
                        placeholder="请输入..."
                        v-model="editingWork.displayName"
                    />
                </div>
            </div>
        </div>
        <template #footer>
            <button
                type="button"
                class="btn btn-secondary"
                @click="cancelEdit()"
            >
                关闭
            </button>
            <button
                type="button"
                class="btn btn-primary"
                @click="save()"
                :disabled="isSaving"
                size="sm"
            >
                保存
                <BsSpinner :show="isSaving" size="sm"></BsSpinner>
            </button>
        </template>
    </BsModal>
</template>
