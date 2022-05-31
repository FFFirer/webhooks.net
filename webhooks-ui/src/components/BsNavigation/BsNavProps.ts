import { PropType } from "vue";

type BsNavTypeType = undefined | "tabs" | "pills";
type BsNavAlignType = "horizontal" | "vertical";
const BsNavProps = {
    type: {
        type: String as PropType<BsNavTypeType>,
        default: undefined,
    },
    align: {
        type: String as PropType<BsNavAlignType>,
        default: "horizontal",
    },
};

export default BsNavProps;
