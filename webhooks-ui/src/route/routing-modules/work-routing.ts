import { RouteRecordRaw } from "vue-router";
import Works from "../../components/Work/Works.vue";
import WorkList from "../../components/Work/WorkList.vue";

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
            component: () => import("../../components/Work/WorkDetail.vue"),
            name: "WorkDetail",
            props: (route) => ({
                id: route.params.id as string,
            }),
        },
    ],
};

export default WorksRoute;
