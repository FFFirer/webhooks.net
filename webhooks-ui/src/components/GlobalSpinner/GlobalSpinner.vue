<script setup lang="ts">
import BsModal from "@/components/BsModal/BsModal.vue";
import { Modal } from "bootstrap";
import { onMounted, PropType, ref, Ref } from "vue";
import BsModalHelper from "../BsModal/BsModalHelper";
import { GlobalSpinnerProxy } from "./GlobalSpinnerProxy";

const props = defineProps({
    proxy: {
        type: Object as PropType<GlobalSpinnerProxy>,
        default: null,
    },
});

const globalSpinnerTarget = "global-spinner-modal";
let globalSpinnerModal: Modal;

const defaultMessage = "Loading";
const spinnerMessage: Ref<string> = ref("Loading");

const isShowing: Ref<boolean> = ref(false);

const show = (message?: string) => {
    spinnerMessage.value = message ?? defaultMessage;
    isShowing.value = true;

    if (isShowing.value) {
        globalSpinnerModal.show();
    }
};

const hide = () => {
    isShowing.value = false;
    globalSpinnerModal.hide();
};

onMounted(() => {
    globalSpinnerModal = BsModalHelper.useModal(globalSpinnerTarget);

    if (props.proxy != null) {
        props.proxy.onShow = show;
        props.proxy.onHide = hide;
    }
});
</script>

<template>
    <BsModal :bs-target="globalSpinnerTarget" :title="spinnerMessage">
        <div class="d-flex align-items-center">
            <div
                class="spinner-border ms-auto"
                role="status"
                aria-hidden="true"
            ></div>
        </div>
        <template #footer>
            <button
                type="button"
                class="btn btn-secondary"
                data-bs-dismiss="modal"
                @click="hide()"
            >
                取消
            </button>
        </template>
    </BsModal>
</template>
