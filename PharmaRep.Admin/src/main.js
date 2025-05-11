import { createApp } from 'vue'
import App from './App.vue'
import User from './components/User.vue'
import UserIcon from './icons/UserIcon.vue'
import AppointmentIcon from './icons/AppointmentIcon.vue'
import { createRouter, createWebHistory } from 'vue-router'
import Appointments from './components/Appointments.vue'
import Users from './components/Users.vue'

const router = createRouter({
  history: createWebHistory(),
  routes: [
    { path: '/users', component: Users },
    { path: '/appointments', component: Appointments },
  ],
})
const app = createApp(App)

app.use(router)

app.component('user-icon', UserIcon)
app.component('appointment-icon', AppointmentIcon)


app.component('users', Users)
app.component('user', User)

app.component('appointments', Appointments)

app.mount('#app')
