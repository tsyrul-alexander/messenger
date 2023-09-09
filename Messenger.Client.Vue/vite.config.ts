import { fileURLToPath, URL } from 'node:url'
import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import NodeGlobalsPolyfillPlugin from '@esbuild-plugins/node-globals-polyfill'

// https://vitejs.dev/config/
export default defineConfig({
  optimizeDeps: {
    esbuildOptions: {
      define: {
        global: 'globalThis'
      },
      plugins: [
          NodeGlobalsPolyfillPlugin({
            process: true,
            buffer: true
          })
      ]
    }
  },
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
