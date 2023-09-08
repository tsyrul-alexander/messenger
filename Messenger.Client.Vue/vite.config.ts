import { fileURLToPath, URL } from 'node:url'

import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'

// https://vitejs.dev/config/
export default defineConfig({
  server: {
    proxy: {
      "/api": {
        target: 'http://127.0.0.1:5214',
        changeOrigin: true,
        secure: false
      },
      "/listen": {
        target: 'ws://127.0.0.1:5214',
        changeOrigin: true,
        secure: false,      
        ws: true,
      }
    }
  },
  plugins: [
    vue(),
  ],
  resolve: {
    alias: {
      '@': fileURLToPath(new URL('./src', import.meta.url))
    }
  },
})
