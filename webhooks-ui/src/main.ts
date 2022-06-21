import { createApp } from "vue";
import App from "./App.vue";
import "bootstrap/dist/css/bootstrap.min.css";
import "bootstrap/dist/js/bootstrap.bundle.min.js";
import "./assets/site.css";
import "./assets/bs-extension.css";
import "./assets/webhooks-ui.css";
import router from "./route/index";
import { globalInstallExternalConfigComponents } from "./views/ExternalConfigs/ExternalConfigInstall";

const app = createApp(App);

globalInstallExternalConfigComponents(app);

app.use(router);
app.mount("#app");
