import { App } from "vue";

import GitExternalConfig from "./GitConfig.vue";
import GiteeExternalConfig from "./GiteeConfig.vue";
import NotFoundExternalConfig from "./NotFoundExternalConfig.vue";
import EmptyExternalConfig from "./EmptyExternalConfig.vue";

export function globalInstallExternalConfigComponents(app: App) {
    app.component("not-found-external-config", NotFoundExternalConfig);
    app.component("external-git-config", GitExternalConfig);
    app.component("external-gitee-config", GiteeExternalConfig);
    app.component("external-empty-config", EmptyExternalConfig);
}
