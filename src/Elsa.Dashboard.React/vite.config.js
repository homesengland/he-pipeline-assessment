import { defineConfig } from 'vite'
import { resolve } from 'path'
import { fileURLToPath, URL } from 'node:url'
import react from '@vitejs/plugin-react'

// https://vitejs.dev/config/
export default defineConfig({
  server: {
    port: 5000
  },
  plugins: [react()],
  build: {
    outDir: "./wwwroot/Scripts/React",
    emptyOutDir: true,
    rollupOptions: {
      output: {
        entryFileNames: 'index.react.js',
      }
    }
  },
  resolve: {
    alias: {
      '@': fileURLToPath(new URL('./src-react', import.meta.url))
    }
  }
})
