<script setup lang="ts">
import { Modal } from "bootstrap";
import { onMounted, PropType, Ref, ref } from "vue";
import { GlobalMessageProxy } from "./GlobalMessageProxy";

const props = defineProps({
    proxy: {
        type: Object as PropType<GlobalMessageProxy>,
        default: null,
    },
});

const messageRef: Ref<HTMLDivElement | undefined> = ref();
const message = ref("");
let messageModal: Modal;

const showMessage = (msg: string) => {
    message.value = msg;
    messageModal?.show();
};

onMounted(() => {
    if (messageRef?.value != undefined) {
        messageModal = new Modal(messageRef.value);
    }

    // 初始化，设置事件响应
    if (props.proxy != null) {
        props.proxy.onShow = showMessage;
    }
});
</script>

<template>
    <div
        ref="messageRef"
        class="modal fade"
        tabindex="-1"
        aria-labelledby="messageModalLabel"
        aria-hidden="true"
    >
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="messageModalLabel">消息</h5>
                    <button
                        type="button"
                        class="btn-close"
                        data-bs-dismiss="modal"
                        aria-label="Close"
                    ></button>
                </div>
                <div class="modal-body">
                    {{ message }}
                </div>
                <div class="modal-footer">
                    <button
                        type="button"
                        class="btn btn-secondary"
                        data-bs-dismiss="modal"
                    >
                        确认
                    </button>
                </div>
            </div>
        </div>
    </div>
</template>
