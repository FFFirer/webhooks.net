import { App } from "vue";

import GitExternalConfig from "./GitConfig.vue";
import GiteeExternalConfig from "./GiteeConfig.vue";
import NotFoundExternalConfig from "./NotFoundExternalConfig.vue";
import EmptyExternalConfig from "./EmptyExternalConfig.vue";

export function globalInstallExternalConfigComponents(app: App) {
    app.component("NotFoundExternalConfig", NotFoundExternalConfig);
    app.component("ExternalGitConfig", GitExternalConfig);
    app.component("ExternalGiteeConfig", GiteeExternalConfig);
    app.component("EmptyExternalConfig", EmptyExternalConfig);
}
