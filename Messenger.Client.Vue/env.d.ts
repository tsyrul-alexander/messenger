/// <reference types="vite/client" />
import Vue from 'vue'
import { SecureService } from '@/services/secure'

declare module 'vue' {
  interface ComponentCustomProperties {
    $secureService: SecureService
  }
}

export {}