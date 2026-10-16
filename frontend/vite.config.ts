import { defineConfig } from "vite";
import react from "@vitejs/plugin-react";
import manifest from "./src/mfe-manifest";
import { createMFEConfig } from "@ikco/mfe-core/vite";

export default defineConfig({
  plugins: [react(), createMFEConfig(manifest)],
  server: {
    port: 5005,
    strictPort: true,
    cors: true,
  },
  preview: {
    port: 5005,
    strictPort: true,
    cors: true,
  },
  build: {
    modulePreload: false,
    target: "esnext",
    minify: false,
    cssCodeSplit: false,
    assetsDir: "",
  },
});
