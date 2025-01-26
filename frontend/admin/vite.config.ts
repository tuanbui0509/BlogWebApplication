import { defineConfig } from "vite";
import react from "@vitejs/plugin-react";

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [react()],
  resolve: {
    alias: {
      "@": "/src", // Set up the alias for your `src` folder
    },
  },
  build: {
    outDir: "dist", // Specify the output directory for the build
  },
});
