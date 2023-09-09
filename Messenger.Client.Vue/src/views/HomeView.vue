<template>
   <InputText v-model="userName"/>
   <Button @click="create">Connect</Button>
</template>

<script lang="ts">
import { defineComponent } from 'vue'
import axios from 'axios'

export default defineComponent({
    data() {
        return {
            userName: null as string | null,
            connection: null as WebSocket | null,
        }
    },
    methods: {
      async create() {
        await axios.post("/api/user", {
          name: this.userName,
          publicKey: this.$secureService.getPublicKey()
        });
        this.$router.push({
          name: "rooms"
        })
      }
    }
})
</script>

