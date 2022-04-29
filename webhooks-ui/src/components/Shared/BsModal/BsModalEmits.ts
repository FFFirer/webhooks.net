const showBsModalEventName = "show";
const shownBsModalEventName = "shown";
const hideBsModalEventName = "hide";
const hiddenBsModalEventName = "hidden";
const hidePreventedEventName = "hidePrevented";

const BsModalEmits: Array<string> = [
    showBsModalEventName,
    shownBsModalEventName,
    hideBsModalEventName,
    hiddenBsModalEventName,
    hidePreventedEventName,
];

const formatBsModalNativeEventName = (event: string) => {
    return `${event}.bs.modal`;
};

export {
    showBsModalEventName,
    shownBsModalEventName,
    hideBsModalEventName,
    hiddenBsModalEventName,
    hidePreventedEventName,
    BsModalEmits,
    formatBsModalNativeEventName,
};
