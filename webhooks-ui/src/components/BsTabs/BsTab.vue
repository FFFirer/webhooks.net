<script setup lang="ts">
import {
    computed,
    ComputedRef,
    onMounted,
    provide,
    ref,
    Ref,
    useSlots,
} from "vue";
import { flatten } from "../../utils/flatten";

import {
    BsTabItemActivedEmitName,
    BsTabItemActivedEventType,
} from "./BsTabEmits";

import { BsTabInjectionKey } from "./interface";

const props = defineProps();
const emits = defineEmits<{
    (event: BsTabItemActivedEventType, id: string): void;
}>();

const slots = useSlots();

const active: Ref<string> = ref("");

const tabs: Ref<
    Array<{
        id: string;
        label: string;
        classes: ComputedRef<{ [key: string]: boolean }>;
    }>
> = ref([]);

const changeTab = (id: string) => {
    active.value = id;
    emits(BsTabItemActivedEmitName, id);
};

onMounted(() => {
    tabs.value = slots.default
        ? flatten(slots.default()).map((b) => {
              return {
                  id: (b.props as any).id,
                  label: (b.props as any).label,
                  classes: computed(() => {
                      return {
                          "nav-link": true,
                          active: active.value == (b.props as any).id,
                      };
                  }),
              };
          })
        : [];

    const initTab = tabs.value.length > 0 ? tabs.value[0].id : "";
    changeTab(initTab);
});

provide(BsTabInjectionKey, {
    active: active,
});
</script>

<template>
    <div>
        <ul class="nav nav-tabs mb-3">
            <li class="nav-item" v-for="(t, i) in tabs">
                <a
                    href="javascript:;"
                    :class="t.classes"
                    @click="changeTab(t.id)"
                >
                    {{ t.label }}
                </a>
            </li>
        </ul>
        <slot></slot>
    </div>
</template>
