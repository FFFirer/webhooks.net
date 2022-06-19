import { defineConfig, loadEnv } from "vite";
import vue from "@vitejs/plugin-vue";
import * as path from "path";
import inject from "@rollup/plugin-inject";

// https://vitejs.dev/config/
export default ({ mode }) => {
    return defineConfig({
        plugins: [vue()],
        server: {
            https: true,
            port: 13425,
            host: "127.0.0.1",
        },
        resolve: {
            alias: [
                {
                    find: "@",
                    replacement: path.resolve(__dirname, "src"),
                },
                {
                    find: "vscode",
                    replacement: path.resolve(
                        __dirname,
                        "./node_modules/monaco-languageclient/lib/vscode-compatibility"
                    ),
                },
            ],
        },
        optimizeDeps: {
            include: ["monaco-languageclient"],
        },
        build: {
            commonjsOptions: {
                include: [/monaco-languageclient/, /node_modules/],
            },
        },
    });
};
