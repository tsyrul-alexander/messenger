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
        await this.test()
        const publicKey = this.$secureService.getPublicKey();
        await axios.post("/api/user", {
          name: this.userName,
          publicKey: publicKey
        });
        this.$router.push({
          name: "rooms"
        })
      },
      async test() {
        await this.$secureService.create();
        const publicKey = this.$secureService.getPublicKey() as string;
        const text = "azazaz 453";
        const encrypt = await this.$secureService.encrypt(publicKey, text)
        console.log("encrypt", encrypt)
        const decript = await this.$secureService.decrypt(encrypt);
        console.log("decript", decript)
      }
    }
})
</script>

