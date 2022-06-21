<script setup lang="ts">
import { computed } from "@vue/reactivity";
import GitExternalConfig from "@/GitConfig.vue";
import GiteeExternalConfig from "./GiteeConfig.vue";
import NotFoundExternalConfig from "./NotFoundExternalConfig.vue";
import EmptyExternalConfig from "./EmptyExternalConfig.vue";

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

const externalConfigTypeDict: { [key: string]: boolean } = {
    ExternalGitConfig: true,
    ExternalGiteeConfig: true,
};

const currentComponent = computed(() => {
    if (props.externalType == "") {
        return "EmptyExternalConfig";
    } else if (externalConfigTypeDict[props.externalType]) {
        return props.externalType;
    } else {
        return "NotFoundExternalConfig";
    }
});
</script>
<template>
    <!-- <component :is="currentComponent" :workId="workId"></component> -->
    <h1>test</h1>
</template>
