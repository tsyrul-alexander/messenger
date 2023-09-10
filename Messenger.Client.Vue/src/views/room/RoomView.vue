<template>
    <Splitter style="height: 300px" class="mb-5">
        <SplitterPanel class="flex align-items-center justify-content-center">
            <Fieldset legend="Users" class="min-w-full">
                <div class="room-users flex-2">
                    <div v-for="user of users" :key="user.id" class="room-user">
                        {{ user.name }}
                        <Divider />
                    </div>
                </div>
            </Fieldset>
        </SplitterPanel>
        <SplitterPanel class="flex align-items-center justify-content-center">
            <div class="min-w-full">
                <ScrollPanel style="width: 100%; height: 200px">
                    <div v-for="message of messages" :key="message.id" class="room-message">
                        {{ message.authorName }}: {{ message.body }}
                    </div>
                </ScrollPanel>
                <Divider />
                <div>
                    <InputText v-model="message" placeholder="Enter text"/>
                    <Button :disabled="!message" @click="send">Send</Button>
                </div>
            </div>
        </SplitterPanel>
        
    </Splitter>
  </template>
  

<script lang="ts">
import { defineComponent } from 'vue'
import { MessageType, type Message, type User } from '@/model'
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
        this.listen()
    },
    methods: {
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
            if (!this.message) {
                return;
            }
            const messages: {[key: string]: string} = {}
            for (let i = 0; i < this.users.length; i++) {
                const user = this.users[i];
                messages[user.id] = await this.encryptMessage(user, this.message as string)
            }
            const messageJson = JSON.stringify({
                messages: messages
            });
            console.log(messageJson);
            this.connection?.send(messageJson);
            this.message = null;
        },
        encryptMessage(user: User, message: string) : Promise<string> {
            return this.$secureService.encrypt(user.publicKey, message);
        },
        decryptMessage(message: string) : Promise<string> {
            return this.$secureService.decrypt(message);
        },
        onMessage(event: MessageEvent<any>) {
            console.log(event)
            const message = JSON.parse(event.data);
            if (message.type === MessageType.User) {
                this.onNewUserMessage(message)
            } else if (message.type === MessageType.Message) {
                this.onNewMessage(message)
            }
            return false;
        },
        onNewUserMessage(message: any) {
            this.loadUsers();
        },
        async onNewMessage(message: any) {
            const decryptBody = await this.decryptMessage(message.body)
            this.messages.push({
                id: message.id,
                createdAt: message.createdAt,
                body: decryptBody,
                authorName: message.authorName
            });
        },
        onOpen(event: Event) {
            console.log(event)
            this.loadUsers()
        },
        onClose(event: CloseEvent) {
            console.log(event)
            this.$router.push({
                name: "home"
            })
        },
    }
})
</script>