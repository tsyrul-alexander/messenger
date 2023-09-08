import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomeView
    },
    {
      path: '/rooms',
      name: 'rooms',
      component: () => import('../views/room/RoomsView.vue')
    },
    {
      path: '/room/:id',
      name: 'room',
      component: () => import('../views/room/RoomView.vue'),
      props: true
    }
  ]
})

export default router
