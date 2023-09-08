<template>
    <div>
        <RoomCreateView></RoomCreateView>
        <div v-for="room of rooms" :key="room.id">
        <div>
            <router-link :to="`/room/${room.id}`">
                {{ room.name }}
            </router-link>
            </div>
        </div>
    </div>
</template>
  

<script lang="ts">
import { defineComponent } from 'vue'
import RoomCreateView from '@/views/room/RoomCreateView.vue'
import type { Room } from '@/model'
import axios from 'axios'

export default defineComponent({
    name: "RoomsView",
    data() {
        return {
            rooms: [] as Room[]
        }
    },
    components: {
        RoomCreateView
    },
    mounted() {
        this.loadData()
    },
    methods: {
        async loadData() {
            let response = await axios.get("/api/room");
            this.rooms = response.data?.results || [];
        }
    }
})
</script>