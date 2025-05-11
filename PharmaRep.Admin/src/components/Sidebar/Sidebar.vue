<script>
import AppointmentIcon from '@/icons/AppointmentIcon.vue'
import UserIcon from '@/icons/UserIcon.vue'

export default {
  components: { UserIcon, AppointmentIcon },
}
</script>

<template>
  <aside class="main-side-bar">
    <nav>
      <ul class="nav-list">
        <li class="nav-list-item">
          <router-link to="/users"><user-icon /></router-link>
        </li>
        <li class="nav-list-item">
          <router-link to="/appointments"><appointment-icon /></router-link>
        </li>
      </ul>
    </nav>
  </aside>
</template>

<style scoped>
nav,
.nav-list {
  height: 100%;
  width: 100%;
}

.nav-list {
  display: flex;
  flex-direction: column;
  justify-content: flex-start;
  align-items: center;
  padding: 0.5rem;
}

.nav-list-item {
  list-style-type: none;
  display: flex;
  justify-content: center;
  align-items: center;
  padding: 0.4rem;
}

a {
  text-decoration: none;
  color: #fff;
}

a:hover,
a.router-link-active {
  color: #948979;
}
</style>
