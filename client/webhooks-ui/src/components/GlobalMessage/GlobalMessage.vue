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

const showHeader: Ref<boolean> = ref(true);
const showFooter: Ref<boolean> = ref(true);
const showClose: Ref<boolean> = ref(true);

/**
 *
 * @param msg 消息
 * @param autoClose 自动隐藏秒数，默认不自动隐藏，单位秒
 */
const showMessage = (msg: string, autoClose?: number) => {
    message.value = msg;

    showHeader.value = true;
    showFooter.value = true;
    showClose.value = true;

    messageModal?.show();

    if (autoClose && autoClose > 0) {
        setTimeout(() => {
            messageModal?.hide();
        }, autoClose * 1000);
    }
};

/**
 * 弹出通知
 * @param msg 通知
 * @param autoClose 自动关闭时间，默认3秒，最少1秒
 */
const notice = (msg: string, autoClose?: number) => {
    message.value = msg;

    autoClose = (autoClose ?? 0) >= 1 ? autoClose : 1;

    showHeader.value = false;
    showClose.value = false;
    showFooter.value = false;

    messageModal?.show();
    console.log("show");
    if (autoClose && autoClose > 0) {
        setTimeout(() => {
            messageModal?.hide();
            console.log("hide");
        }, autoClose * 1000);
    }
};

onMounted(() => {
    if (messageRef?.value != undefined) {
        messageModal = new Modal(messageRef.value);
    }

    // 初始化，设置事件响应
    if (props.proxy != null) {
        props.proxy.onShow = showMessage;
        props.proxy.onNotice = notice;
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
                <div class="modal-header" v-show="showHeader">
                    <h5 class="modal-title" id="messageModalLabel">消息</h5>
                    <button
                        v-show="showClose"
                        type="button"
                        class="btn-close"
                        data-bs-dismiss="modal"
                        aria-label="Close"
                    ></button>
                </div>
                <div class="modal-body">
                    {{ message }}
                </div>
                <div class="modal-footer" v-show="showFooter">
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
