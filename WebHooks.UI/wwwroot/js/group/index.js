import { createApp } from 'https://unpkg.com/petite-vue?module';
import { getUrl } from "/js/utils.js";

const addGroupModalConfig = {
    name: "",
    description: "",
    create()
    {
        const self = this;
        const handler = "CreateGroup";
        const url = getUrl(handler);
        let anti = document.querySelector("input[name=__RequestVerificationToken]");
        let formData = new FormData();
        formData.append("Name", this.name);
        formData.append("Description", this.description);
        $.ajax({
            type: "POST",
            url: url,
            headers: {
                "RequestVerificationToken": anti.value
            },
            data: formData,
            processData: false,
            contentType: false,
            success: function ()
            {
                self.name = "";
                self.description = "";
                location.reload();
            }
        })
    },
    delete(id)
    {
        const handler = "deleteGroup";
        const self = this;

    }
}

const addGroupModal = createApp(addGroupModalConfig).mount("#addGroupModal");

export { addGroupModal };