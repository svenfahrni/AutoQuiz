import { createRouter, createWebHistory } from 'vue-router'
import Overview from '@/views/Overview.vue'
import Create from '@/views/Create.vue'
import Learn from '@/views/Learn.vue'

const routes = [
  {
    path: '/',
    name: 'Overview',
    component: Overview
  },
  {
    path: '/create',
    name: 'Create',
    component: Create
  },
  {
    path: '/learn',
    name: 'Learn',
    component: Learn,
  }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

export default router 