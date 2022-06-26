import { RouteRecordRaw } from "vue-router";

const SettingsRoute: RouteRecordRaw = {
    path: "/setting",
    component: () => import("@/views/Setting/Settings.vue"),
    name: "Settings",
    children: [
        {
            path: "",
            component: () => import("@/views/Setting/Basic.vue"),
            name: "basic-setting",
        },
    ],
};

export default SettingsRoute;
