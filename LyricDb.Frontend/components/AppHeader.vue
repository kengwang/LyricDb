<script lang="ts" setup>
let userInfo = useUserInfo();

function onAvatarClick() {
  if (userInfo.value.isLogin) {
    useRouter().push('/user')
  } else {
    useRouter().push('/user/login')
  }
}
</script>

<template>
  <v-app-bar
      class="px-3">
    <NuxtLink to="/song">
      <v-app-bar-title class="app-title">
        LyricDB
      </v-app-bar-title>
    </NuxtLink>
    <v-spacer/>


    <v-spacer/>
    <v-btn>
      <UserAvatar :is-known="userInfo.isLogin" :user-info="userInfo" @click="onAvatarClick"/>
    </v-btn>
  </v-app-bar>
</template>

<style scoped>
.app-title {
  font-family: "IBM Plex Sans", -apple-system, BlinkMacSystemFont, "Helvetica Neue", "PingFang SC", "Microsoft YaHei", "Source Han Sans SC", "Noto Sans CJK SC", sans-serif;
  font-weight: 700;
}
</style>