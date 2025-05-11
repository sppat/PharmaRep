<script>
export default {
  props: {
    user: Object,
  },
  data() {
    return {}
  },
}
</script>

<template>
  <p>Name: {{ user.name }}</p>
  <p>Age: {{ user.age }}</p>
</template>
