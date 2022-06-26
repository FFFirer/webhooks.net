<script setup lang="ts">
const props = defineProps();
</script>
<template>
    <div class="mask-layer">
        <slot></slot>
    </div>
</template>
<style scoped>
.mask-layer {
    position: relative;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background-color: rgba(0, 0, 0, 0.5);
}
</style>
