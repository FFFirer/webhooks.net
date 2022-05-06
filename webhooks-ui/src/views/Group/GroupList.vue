<script setup lang="ts">
import { Modal } from "bootstrap";
import { onMounted, Ref, ref } from "vue";
import { GroupClientProxy } from "../../shared/client-proxy";
import {
    ApiException,
    GroupDto,
    PageGroupInput,
    RemoveGroupInput,
} from "../../shared/webapi/client";
import { useGlobalMessage } from "../../components/GlobalMessage/GlobalMessageProxy";
import BsModal from "../../components/BsModal/BsModal.vue";
import BsModalHelper from "../../components/BsModal/BsModalHelper";
import BsPagination from "../../components/BsPagination/BsPagination.vue";
import { useRouter } from "vue-router";

// 模态框
const modalRef: Ref<HTMLDivElement | undefined> = ref();
const messageRef: Ref<HTMLDivElement | undefined> = ref();
const router = useRouter();

const modalTitle = ref("");

const groupClient = new GroupClientProxy();

const groups: Ref<GroupDto[]> = ref([]);

const editGroupModalTarget = "editGroupModal";
let editGroupModal: Modal;

const globalMsg = useGlobalMessage();

let edittingGroup: Ref<GroupDto> = ref(new GroupDto());

// 页码
const currentPage: Ref<number> = ref(1);
// 页大小
const pageSize: Ref<number> = ref(5);
// 总数
const total: Ref<number> = ref(2);

const list = async () => {
    await query(1);
};

const query = async (page: number) => {
    try {
        const queryInput: PageGroupInput = new PageGroupInput({
            page: page,
            pageSize: pageSize.value,
        });

        const result = await groupClient.query(queryInput);

        groups.value = result.rows;
        currentPage.value = page;
        total.value = result.total;
    } catch (error) {
        if (ApiException.isApiException(error)) {
            console.log((error as ApiException).message);

            globalMsg?.show((error as ApiException).message);
        }
    }
};

const cancelEdit = () => {
    editGroupModal.hide();
};

/**编辑窗口关闭时触发事件 */
const onEditGroupModalHidden = () => {
    edittingGroup.value = new GroupDto();
};

/**保存 */
const saveGroup = async () => {
    const input = edittingGroup.value;

    await groupClient.save(input);
    await query(currentPage.value);
    editGroupModal.hide();
};

/**创建 */
const createGroup = () => {
    modalTitle.value = "创建分组";

    editGroupModal.show();
};

/**编辑 */
const editGroup = (group: GroupDto) => {
    modalTitle.value = "编辑分组";
    edittingGroup.value = group;
    editGroupModal.show();
};

/**删除 */
const removeGroup = async (group: GroupDto) => {
    const input = new RemoveGroupInput({
        id: group.id,
    });
    await groupClient.remove(input);
    await query(currentPage.value);
};

const handlePageChanged = async (page: number) => {
    await query(page);
};

onMounted(async () => {
    editGroupModal = BsModalHelper.useModal(editGroupModalTarget);

    await query(currentPage.value);
});
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
            <button type="button" class="btn btn btn-primary" @click="list()">
                搜索
            </button>
        </div>
        <div class="col-12">
            <table class="table">
                <thead>
                    <tr>
                        <th class="w-50px">#</th>
                        <th class="w-200px">操作</th>
                        <th>分组名称</th>
                        <th>描述</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="(group, index) in groups" :key="index">
                        <td>
                            {{ index + 1 }}
                        </td>
                        <td>
                            <div class="btn-group">
                                <button
                                    type="button"
                                    class="btn btn-sm btn-secondary"
                                >
                                    详情
                                </button>
                                <button
                                    type="button"
                                    class="btn btn-sm btn-primary"
                                    @click="editGroup(group)"
                                >
                                    编辑
                                </button>
                                <button
                                    type="button"
                                    class="btn btn-sm btn-danger"
                                    @click="removeGroup(group)"
                                >
                                    删除
                                </button>
                            </div>
                        </td>
                        <td>
                            {{ group.name }}
                        </td>
                        <td>
                            {{ group.description }}
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div class="col-12 empty-table text-muted" v-if="groups.length == 0">
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

    <bs-modal
        :bs-target="editGroupModalTarget"
        @hidden="onEditGroupModalHidden"
    >
        <template #header>
            <h5 id="modalTitle" class="modal-title">
                {{ modalTitle }}
            </h5>
        </template>
        <div class="row">
            <div class="col-12">
                <div class="form-group">
                    <label for="name" class="form-label">分组名称</label>
                    <input
                        type="text"
                        class="form-control"
                        placeholder="请输入分组名称..."
                        v-model="edittingGroup.name"
                    />
                </div>
                <div class="form-group">
                    <label for="description" class="form-label">描述</label>
                    <textarea
                        placeholder="请输入分组描述..."
                        class="form-control"
                        name="description"
                        id="description"
                        rows="10"
                        v-model="edittingGroup.description"
                    ></textarea>
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
            <button type="button" class="btn btn-primary" @click="saveGroup()">
                保存
            </button>
        </template>
    </bs-modal>
</template>

<style scoped></style>
