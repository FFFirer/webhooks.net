import { PropType } from "vue";

type BsModalPositionType = "center" | "top" | "bottom" | "left" | "right";

const BsModalProps = {
    /**控件Id,注意触发时加上# */
    bsTarget: {
        type: String,
        default: null,
    },
    /**模态框出现的未知 */
    position: {
        type: String as PropType<BsModalPositionType>,
        default: "top",
    },
    /**静态背景 */
    staticBackdrop: {
        type: Boolean,
        default: false,
    },
    title: {
        type: String,
        default: "",
    },
    size: {
        type: String,
        default: "",
    },
};

export default BsModalProps;
