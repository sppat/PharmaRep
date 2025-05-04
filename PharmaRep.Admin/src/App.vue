<script>
export default {
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
  <section>
    <h2>Users</h2>
    <ul>
      <li v-for="user in users" :key="user.id">
        <user :user="user"></user>
      </li>
    </ul>
  </section>
</template>

<style scoped>
* {
  box-sizing: border-box;
  margin: 0;
  padding: 0;
}
</style>
