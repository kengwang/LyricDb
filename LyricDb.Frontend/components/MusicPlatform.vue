<script lang="ts" setup>
defineProps<{
  platform: string
  id: string
}>()

const platformInfo =
    {
      "ncm": {
        name: "网易云音乐",
        color: "#c20c0c",
        icon: "http://p3.music.126.net/tBTNafgjNnTL1KlZMt7lVA==/18885211718935735.jpg",
        url: "https://music.163.com/#/song?id={id}"
      },
      "qmc": {
        name: "QQ音乐",
        color: "#FFD700",
        icon: "https://y.qq.com/favicon.ico",
        url: "https://y.qq.com/n/yqq/song/{id}.html"
      },
      "spo": {
        name: "Spotify",
        color: "#1DB954",
        icon: "https://open.spotifycdn.com/cdn/images/favicon.0f31d2ea.ico",
        url: "https://open.spotify.com/track/{id}"
      },
      "apl": {
        name: "Apple Music",
        color: "#ff0059",
        icon: "https://music.apple.com/assets/favicon/favicon-180.png",
        url: "https://music.apple.com/cn/song/{id}"
      }
    }
function goto(url: string) {
  window.open(url)
}
</script>

<template>
  <ClientOnly>
    <v-btn class="align-self-center mx-4" @click="goto(platformInfo[platform].url.replace('{id}', id))">
      <template v-slot:prepend>
        <v-avatar :image="platformInfo[platform].icon" class="pa-1"/>
      </template>
      {{ platformInfo[platform].name }}
    </v-btn>
  </ClientOnly>
</template>

<style scoped>

</style>