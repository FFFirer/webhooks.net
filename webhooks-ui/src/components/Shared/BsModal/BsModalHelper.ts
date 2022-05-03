import { Modal } from "bootstrap";

function useModal(target: string): Modal {
    const el = document.getElementById(target);
    if (!el) {
        throw "没有找到目标元素";
    }
    const modal = new Modal(el);
    return modal;
}

export default {
    useModal,
};
