<script lang="ts" setup>
let userInfo = useUserInfo();
let drawer = ref(false);

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
    <v-app-bar-nav-icon variant="text" @click.stop="drawer = !drawer"></v-app-bar-nav-icon>
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
  <v-navigation-drawer
      v-model="drawer"
      temporary
  >
    <v-list>
      <NuxtLink to="/song">
        <v-list-item prepend-icon="mdi-music-box-multiple" title="歌曲列表"/>
      </NuxtLink>
      <NuxtLink to="/lyric">
        <v-list-item prepend-icon="mdi-text-box-multiple" title="歌词列表"/>
      </NuxtLink>
      <NuxtLink to="https://docs.lyricdb.kengwang.com.cn">
        <v-list-item prepend-icon="mdi-text-box" title="文档"/>
      </NuxtLink>
      <v-divider />
      <NuxtLink  v-if="userInfo.isLogin" to="/user">
        <v-list-item prepend-icon="mdi-account" title="个人中心"/>
      </NuxtLink>
      <NuxtLink v-if="userInfo.isLogin" to="/user/logout">
        <v-list-item prepend-icon="mdi-logout" title="登出"/>
      </NuxtLink>
      <NuxtLink v-else to="/user/login">
        <v-list-item prepend-icon="mdi-login" title="登录"/>
      </NuxtLink>
    </v-list>
  </v-navigation-drawer>
</template>

<style scoped>
.app-title {
  font-family: "IBM Plex Sans", -apple-system, BlinkMacSystemFont, "Helvetica Neue", "PingFang SC", "Microsoft YaHei", "Source Han Sans SC", "Noto Sans CJK SC", sans-serif;
  font-weight: 700;
}
</style>