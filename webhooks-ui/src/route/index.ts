import { createRouter, createWebHashHistory, RouteRecordRaw } from "vue-router";
import Welcome from "../components/Welcome.vue";
import GroupsRoute from "./routing-modules/group-routing";
import WorksRoute from "./routing-modules/work-routing";

const routes: RouteRecordRaw[] = [
    {
        path: "/",
        component: Welcome,
        name: "welcome",
    },
    GroupsRoute,
    WorksRoute,
];

const routerHistory = createWebHashHistory();

const router = createRouter({
    routes: routes,
    history: routerHistory,
});

export default router;
