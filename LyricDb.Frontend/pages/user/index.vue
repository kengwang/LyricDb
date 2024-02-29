<script lang="ts" setup>
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