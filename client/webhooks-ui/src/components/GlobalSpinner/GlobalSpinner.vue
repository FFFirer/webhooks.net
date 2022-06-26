<script setup lang="ts">
import { computed } from "@vue/reactivity";
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

const defaultMessage = "Loading";
const spinnerMessage: Ref<string> = ref("Loading");

const isShowing: Ref<boolean> = ref(false);

const show = (message?: string) => {
    spinnerMessage.value = message ?? defaultMessage;
    isShowing.value = true;
};

const hide = () => {
    isShowing.value = false;
};

const classList = computed(() => {
    return {
        modal: true,
        fade: true,
        show: isShowing.value,
    };
});

onMounted(() => {
    if (props.proxy != null) {
        props.proxy.onShow = show;
        props.proxy.onHide = hide;
    }
});
</script>

<template>
    <Teleport to="body">
        <div class="mask" v-show="isShowing">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-body">
                        <div class="d-flex align-items-center">
                            <strong>{{ spinnerMessage }}</strong>
                            <div
                                class="spinner-border ms-auto"
                                role="status"
                                aria-hidden="true"
                            ></div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </Teleport>
</template>

<style scoped>
.mask {
    position: fixed;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    z-index: 1200;
    background-color: rgba(0, 0, 0, 0.5);
}
</style>
