import { createApp } from 'https://unpkg.com/petite-vue?module';
import { ajaxRequest } from "/js/utils.js";

const addGroupModalConfig = {
    name: "",
    description: "",
    create()
    {
        const self = this;
        const handler = "CreateGroup";
        const onSuccess = function ()
        {
            location.reload()
        };
        let formData = new FormData();
        formData.append("Name", this.name);
        formData.append("Description", this.description);
        ajaxRequest(handler, formData, onSuccess)
    }
}

const addGroupModal = createApp(addGroupModalConfig).mount("#addGroupModal");

export default { addGroupModal };