<template>
    <div v-for="user of users" :key="user.id">
        {{ user.name }}
        {{ user.publicKey }}
    </div>
    <DataView :value="messages" dataKey="id">
        <template #list="slotProps">
            <div class="col-12">
                <div class="flex flex-column align-items-center sm:align-items-start">
                    <div>{{ slotProps.data.createdAt }}</div>
                    <div v-if="slotProps.data.type == 1">system</div>
                    <div v-if="slotProps.data.type == 2">{{ slotProps.data.authorName }}</div>
                    <div>{{ slotProps.data.body }}</div>
                </div>
            </div>
        </template>
    </DataView>
    <div>
        <InputText v-model="message"/>
        <Button :disabled="!message" @click="send">Send</Button>
    </div>
  </template>
  

<script lang="ts">
import { defineComponent } from 'vue'
import type { Message, User } from '@/model'
import axios from 'axios'

export default defineComponent({
    props: {
        id: null
    },
    data() {
        return {
            messages: [] as Message[],
            users: [] as User[],
            connection: null as WebSocket | null,
            message: null as string | null
        }
    },
    mounted() {
        this.loadMessages()
        this.listen()
    },
    methods: {
        async loadMessages() {
            let response = await axios.get("/api/message", {
                params: {
                    roomId: this.id
                }
            });
            this.messages = response.data?.results || [];
        },
        async loadUsers() {
            let response = await axios.get("/api/user", {
                params: {
                    roomId: this.id
                }
            });
            this.users = response.data?.results || [];
        },
        async listen() {
            this.connection = new WebSocket(`ws://${window.location.host}/listen?roomId=${this.id}`)
            this.connection.onmessage = this.onMessage;
            this.connection.onopen = this.onOpen;
            this.connection.onclose = this.onClose;
        },
        async send() {
            this.connection?.send(JSON.stringify({
                message: this.message
            }));
        },
        onMessage(event: MessageEvent<any>) {
            console.log(event)
            this.loadMessages()
            return false;
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