<script setup lang="ts">
useApi().user.logout()
.then(() => {
  updateUserInfo()
  useRouter().push('/user/login')
})
</script>

<template>

</template>

<style scoped>

</style>