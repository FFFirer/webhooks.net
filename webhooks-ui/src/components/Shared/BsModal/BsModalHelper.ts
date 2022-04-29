import { Modal } from "bootstrap";

function useModal(target: string): Modal {
    const el = document.getElementById(target);
    if (!el) {
        return null;
    }
    const modal = new Modal(el);
    return modal;
}

export default {
    useModal,
};
