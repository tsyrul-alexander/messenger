<template>
    <div v-for="message of messages" :key="message.id">
      <div>{{ message.authorName }}</div>
      <div>{{ message.body }}</div>
    </div>
    <div>
        <InputText v-model="message"/>
        <Button :disabled="!message" @click="send">Send</Button>
    </div>
  </template>
  

<script lang="ts">
import { defineComponent } from 'vue'
import type { Message } from '@/model'
import axios from 'axios'

export default defineComponent({
    props: {
        id: null
    },
    data() {
        return {
            messages: [] as Message[],
            connection: null as WebSocket | null,
            message: null as string | null
        }
    },
    mounted() {
        this.loadData()
        this.listen()
    },
    methods: {
        async loadData() {
            let response = await axios.get("/api/message", {
                params: {
                    roomId: this.id
                }
            });
            this.messages = response.data?.results || [];
        },
        async listen() {
            this.connection = new WebSocket(`ws://${window.location.host}/listen?roomId=${this.id}`)
            this.connection.onmessage = this.onMessage;
            this.connection.onopen = this.onOpen;
            this.connection.onclose = this.onClose;
        },
        async send() {
            let response = await axios.post("/api/message", {
                params: {
                    message: this.message
                }
            });
        },
        onMessage(event: MessageEvent<any>) {
            console.log(event)
        },
        onOpen(event: Event) {
            console.log(event)
        },
        onClose(event: CloseEvent) {
            console.log(event);
            this.$router.push({
                name: "home"
            })
        },
    }
})
</script>