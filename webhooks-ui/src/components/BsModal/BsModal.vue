<script setup lang="ts">
import { onMounted, Ref, ref } from "vue";
import BsModalProps from "./BsModalProps";
import { BsModalEmits, formatBsModalNativeEventName } from "./BsModalEmits";
import { computed } from "@vue/reactivity";

const props = defineProps(BsModalProps);
const emit = defineEmits(BsModalEmits);
/**模态框HTML引用 */
const modalRef: Ref<HTMLDivElement | undefined> = ref();

const modalDialogClasses = computed(() => {
    return {
        "modal-dialog": true,
        "modal-dialog-centered": true,
        "modal-xl": props.size == "xl",
        "modal-sm": props.size == "sm",
        "modal-lg": props.size == "lg",
    };
});

onMounted(() => {
    if (modalRef.value) {
        // 绑定bootstrap modal原生事件
        BsModalEmits.forEach((eventName) => {
            const nativeEventName = formatBsModalNativeEventName(eventName);
            modalRef.value?.addEventListener(nativeEventName, (event) => {
                emit(eventName, event);
            });
        });

        // 设置data-bs-target
        if (props.bsTarget != null) {
            modalRef.value.id = props.bsTarget;
        }

        if (props.staticBackdrop) {
            modalRef.value.setAttribute("data-bs-backdrop", "static");
        }
    }
});
</script>

<template>
    <div
        ref="modalRef"
        class="modal fade"
        tabindex="-1"
        aria-labelledby="modalTitle"
        aria-hidden="true"
    >
        <div :class="modalDialogClasses">
            <div class="modal-content">
                <div class="modal-header">
                    <slot name="header">
                        <h5 id="modalTitle" class="modal-title">{{ title }}</h5>
                    </slot>
                    <button
                        type="button"
                        class="btn-close"
                        data-bs-dismiss="modal"
                        aria-label="Close"
                    ></button>
                </div>
                <div class="modal-body">
                    <slot></slot>
                </div>
                <div class="modal-footer">
                    <slot name="footer">
                        <button
                            type="button"
                            class="btn btn-secondary"
                            data-bs-dismiss="modal"
                        >
                            关闭
                        </button>
                    </slot>
                </div>
            </div>
        </div>
    </div>
</template>
