import { RouteRecordRaw } from "vue-router";
import Works from "../../views/Work/Works.vue";
import WorkList from "../../views/Work/WorkList.vue";

const WorksRoute: RouteRecordRaw = {
    name: "Work",
    path: "/work",
    component: Works,
    children: [
        {
            path: "",
            component: WorkList,
            name: "WrokList",
        },
        {
            path: "detail/:id",
            component: () => import("../../views/Work/WorkDetail.vue"),
            name: "WorkDetail",
            props: (route) => ({
                id: route.params.id as string,
            }),
        },
    ],
};

export default WorksRoute;
