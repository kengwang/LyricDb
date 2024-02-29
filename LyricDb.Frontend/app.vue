<template>
  <v-app>
    <NuxtLayout>
      <NuxtPage/>
    </NuxtLayout>
  </v-app>
</template>
<script lang="ts" setup>
//挂载钩子
onMounted(() => {
  getLocal()
  updateUserInfo()
  window.onbeforeunload = () => {
    setLocal()
  }
})
</script>

<style>
a {
  color: inherit;
  text-decoration: none;
}

a:hover {
  text-decoration: none;
}

.v-btn{
  text-transform: none !important;
}
</style>