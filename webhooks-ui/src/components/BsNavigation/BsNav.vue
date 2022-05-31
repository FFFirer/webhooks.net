<template>
    <ul :class="navClasses">
        <slot></slot>
    </ul>
</template>
<script setup lang="ts">
import { computed } from "@vue/reactivity";
import { reactive } from "vue";
import BsNavProps from "./BsNavProps";
import BsNa

const props = defineProps(BsNavProps);
const navClasses = computed(() => {
    return {
        nav: true,
        "nav-tabs": true,
    };
});
</script>
