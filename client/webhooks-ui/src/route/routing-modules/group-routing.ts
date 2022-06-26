import { RouteRecordRaw } from "vue-router";

import Groups from "../../views/Group/Groups.vue";
import GroupList from "../../views/Group/GroupList.vue";
import GroupDetail from "../../views/Group/GroupDetail.vue";

const GroupsRoute: RouteRecordRaw = {
    path: "/group",
    component: Groups,
    name: "Groups",
    children: [
        {
            path: "",
            component: GroupList,
            name: "GroupList",
        },
        {
            path: "detail",
            component: GroupDetail,
            name: "GroupDetail",
        },
    ],
};

export default GroupsRoute;
