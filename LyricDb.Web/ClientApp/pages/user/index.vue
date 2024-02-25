<script lang="ts" setup>
// get current user info
let userInfo = useUserInfo()
if (!userInfo.value.isLogin) {
  useRouter().push('/user/login')
} else {
  useRouter().push('/user/' + userInfo.value.id)
}
</script>

<template>
正在跳转...
</template>

<style scoped>

</style>