import { defineConfig, loadEnv } from "vite";
import vue from "@vitejs/plugin-vue";

// https://vitejs.dev/config/
export default ({ mode }) => {
    return defineConfig({
        plugins: [vue()],
        server: {
            https: true,
            port: 13425,
            host: "127.0.0.1",
        },
    });
};
