<script>
export default {
  props: {
    user: Object
  },
  data() {
    return {}
  },
}
</script>

<template>
  <li>
    <p>Name: {{ user.name }}</p>
    <p>Age: {{ user.age }}</p>
  </li>
</template>
