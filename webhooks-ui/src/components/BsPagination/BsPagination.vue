<script setup lang="ts">
import { computed } from "@vue/reactivity";
import { onMounted, Ref, ref, watch, watchEffect } from "vue";
import {
    PageChangedEventName,
    PageChangedEventType,
} from "./BsPaginationEmits";
import BsPaginationProps from "./BsPaginationProps";

const props = defineProps(BsPaginationProps);
const emits = defineEmits<{
    (event: PageChangedEventType, page: number): void;
}>();

const _pageCount = ref(1);

interface IPageItem {
    page: number;
    class: { [key: string]: boolean };
}

/**跳转 */
const goto = (page: number) => {
    emits(PageChangedEventName, page);
};

/**计算总页数 */
const calcPageCount = () => {
    console.log("before calc page count", props.total, props.pageSize);
    _pageCount.value = Math.ceil(props.total / props.pageSize);
    if (_pageCount.value <= 0) {
        _pageCount.value = 1;
    }
    console.log("calc page count", _pageCount.value);
};

/**跳到前一页 */
const gotoPrevious = () => {
    if (props.page > 1) {
        emits(PageChangedEventName, props.page - 1);
    }
};

/**跳到后一页 */
const gotoNext = () => {
    if (props.page < _pageCount.value) {
        emits(PageChangedEventName, props.page + 1);
    }
};

const pages = computed(() => {
    const arr: Array<IPageItem> = [];

    for (let index = 0; index < _pageCount.value; index++) {
        const item: IPageItem = {
            page: index + 1,
            class: {
                "page-item": true,
                active: props.page == index + 1,
            },
        };
        arr.push(item);
    }

    return arr;
});

/**计算后退按钮样式 */
const previousItemClass = computed(() => {
    return {
        "page-item": true,
        disabled: props.page <= 1,
    };
});

/**计算前进按钮样式 */
const nextItemClass = computed(() => {
    return {
        "page-item": true,
        disabled: props.page >= _pageCount.value,
    };
});

/**判断页码是否为当前页 */
const isCurrentPage = (page: number) => {
    return page === props.page;
};

watch(
    () => props.total,
    (newValue, oldValue) => {
        calcPageCount();
    }
);

onMounted(() => {
    calcPageCount();
});
</script>
<template>
    <ul class="pagination">
        <li :class="previousItemClass">
            <a href="javascript:;" class="page-link" @click="gotoPrevious()">
                <slot name="previous"> 上一页 </slot>
            </a>
        </li>
        <li v-for="(page, index) in pages" :key="index" :class="page.class">
            <a
                v-if="isCurrentPage(page.page)"
                href="javascript:;"
                class="page-link"
            >
                {{ page.page }}
            </a>

            <a
                v-else
                href="javascript:;"
                class="page-link"
                @click="goto(page.page)"
            >
                {{ page.page }}
            </a>
        </li>
        <li :class="nextItemClass">
            <a href="javascript:;" class="page-link" @click="gotoNext()">
                <slot name="next"> 下一页 </slot>
            </a>
        </li>
    </ul>
</template>
