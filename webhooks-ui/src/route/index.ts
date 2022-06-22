import { createRouter, createWebHashHistory, RouteRecordRaw } from "vue-router";
import Welcome from "../views/Welcome.vue";
import GroupsRoute from "./routing-modules/group-routing";
import SettingsRoute from "./routing-modules/setting-routing";
import WorksRoute from "./routing-modules/work-routing";

const routes: RouteRecordRaw[] = [
    {
        path: "/",
        component: Welcome,
        name: "welcome",
    },
    GroupsRoute,
    WorksRoute,
    SettingsRoute,
];

const routerHistory = createWebHashHistory();

const router = createRouter({
    routes: routes,
    history: routerHistory,
});

export default router;
