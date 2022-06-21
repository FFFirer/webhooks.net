<script setup lang="ts">
import { computed, Ref, ref } from "@vue/reactivity";

const props = defineProps({
    externalType: {
        type: String,
        default: "",
    },
    workId: {
        type: String,
        default: "",
    },
});

const currentComponent = computed(() => {
    if (props.externalType == "") {
        return "external-empty-config";
    } else {
        return `external-${props.externalType}-config`;
    }
});
</script>
<template>
    <component :is="currentComponent" :work-id="props.workId"></component>
</template>
