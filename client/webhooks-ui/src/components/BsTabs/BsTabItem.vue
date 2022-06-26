<script lang="ts" setup>
import { computed, inject } from "vue";
import BsTabItemProps from "./BsTabItemProps";
import { BsTabInjectionKey } from "./interface";

const props = defineProps(BsTabItemProps);

const injection = inject(BsTabInjectionKey);

const visiable = computed(() => {
    return injection?.active.value == props.id;
});
</script>
<template>
    <div v-show="visiable">
        <slot></slot>
    </div>
</template>
