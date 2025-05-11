<script>
import Sidebar from './components/Sidebar.vue'
export default {
  components: { Sidebar },
  data() {
    return {
      users: [
        { id: 1, name: 'John Doe', age: 30 },
        { id: 2, name: 'Jane Smith', age: 25 },
        { id: 3, name: 'Alice Johnson', age: 28 },
      ],
    }
  },
}
</script>

<template>
  <div class="main-container">
    <sidebar></sidebar>
    <main class="main-content">main</main>
  </div>
</template>

<style  >
* {
  box-sizing: border-box;
  margin: 0;
  padding: 0;
}

body {
  background-color: #393E46;
  color: #fff;
}

.main-container {
  width: 100vw;
  height: 100vh;
  display: grid;
  grid-template-columns: 60px 1fr;
}

.main-side-bar {
  background-color: #000;
}
</style>
