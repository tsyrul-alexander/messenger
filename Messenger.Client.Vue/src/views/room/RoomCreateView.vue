<template>
    <Button @click="isVisible = !isVisible">
        Create room
    </Button>
    <Dialog v-model:visible="isVisible" modal>
        <div class="flex flex-column gap-2">
            <label for="roomname">Room Name</label>
            <InputText id="roomname" v-model="roomName" />
        </div>
        <template #footer>
            <Button label="No" icon="pi pi-times" @click="isVisible = false" text />
            <Button label="Yes" icon="pi pi-check" @click="create" autofocus />
        </template>
    </Dialog>
</template>
  

<script lang="ts">
import { defineComponent } from 'vue'
import axios from 'axios'

export default defineComponent({
    name: "RoomsView",
    data() {
        return {
            isVisible: false,
            roomName: ""
        }
    },
    methods: {
        async create() {
            this.isVisible = false
            let response = await axios.post("/api/room", {
                name: this.roomName
            });
            let roomId = response.data.id;
            this.$router.push({
                name: "room",
                params: {
                    id: roomId
                }
            });
            
        }
    }
})
</script>