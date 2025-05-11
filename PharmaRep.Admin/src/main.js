import { createApp } from 'vue'
import App from './App.vue'
import User from './components/User.vue'
import Sidebar from './components/Sidebar.vue'
import UserIcon from './icons/UserIcon.vue'
import AppointmentIcon from './icons/AppointmentIcon.vue'

const app = createApp(App)

app.component("user-icon", UserIcon);
app.component("appointment-icon", AppointmentIcon);
app.component("sidebar", Sidebar);
app.component("user", User);

app.mount('#app');
