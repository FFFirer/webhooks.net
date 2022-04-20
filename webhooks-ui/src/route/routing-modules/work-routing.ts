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
    ],
};

export default WorksRoute;
