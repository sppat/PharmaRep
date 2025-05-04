import { createApp } from 'vue'
import App from './App.vue'
import User from './components/User.vue'

const app = createApp(App)

app.component("user", User);

app.mount('#app');
