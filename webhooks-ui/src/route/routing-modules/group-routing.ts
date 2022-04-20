import { RouteRecordRaw } from "vue-router";

import Groups from "../../components/Group/Groups.vue";
import GroupList from "../../components/Group/GroupList.vue";
import GroupDetail from "../../components/Group/GroupDetail.vue";

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
