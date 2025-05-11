import { createApp } from 'vue'
import App from './App.vue'
import { createRouter, createWebHistory } from 'vue-router'
import Appointments from './components/Appointment/Appointments.vue'
import Users from './components/User/Users.vue'

const router = createRouter({
  history: createWebHistory(),
  routes: [
    { path: '/users', component: Users },
    { path: '/appointments', component: Appointments },
  ],
})
const app = createApp(App)

app.use(router)

app.mount('#app')
