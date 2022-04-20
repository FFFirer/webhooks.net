import { createApp } from "vue";
import App from "./App.vue";
import "bootstrap/dist/css/bootstrap.min.css";
import "bootstrap/dist/js/bootstrap.bundle.min.js";
import "./assets/site.css";
import "./assets/webhooks-ui.css";
import router from "./route/index";

const app = createApp(App);
app.use(router);
app.mount("#app");
