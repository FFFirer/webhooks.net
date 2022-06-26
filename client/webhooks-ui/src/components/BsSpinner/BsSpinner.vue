<script setup lang="ts">
import { computed } from "vue";

const props = defineProps({
    show: {
        type: Boolean,
        default: false,
    },
    size: {
        type: String,
        default: "",
    },
});

const baseClass = "spinner-border";

const classList = computed(() => {
    if (props.size == "") {
        return [baseClass];
    } else {
        return [baseClass, `${baseClass}-${props.size}`];
    }
});
</script>

<template>
    <div v-show="show" :class="classList" role="status"></div>
</template>
