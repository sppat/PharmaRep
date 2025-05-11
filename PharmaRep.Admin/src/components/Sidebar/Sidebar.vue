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
        <li class="nav-list-item"><user-icon /></li>
        <li class="nav-list-item"><appointment-icon /></li>
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
</style>
