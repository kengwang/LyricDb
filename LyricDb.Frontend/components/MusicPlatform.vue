<script lang="ts" setup>
defineProps<{
  platform: string
  id: string
}>()

class PlatformInfo {
  name: string | undefined
  color: string | undefined
  icon: string | undefined
  url: string | undefined
}

const platformInfo : {[key:string]:PlatformInfo} =
    {
      "ncm": {
        name: "网易云音乐",
        color: "#c20c0c",
        icon: "http://p3.music.126.net/tBTNafgjNnTL1KlZMt7lVA==/18885211718935735.jpg",
        url: "https://music.163.com/#/song?id={id}"
      } as PlatformInfo,
      "qmc": {
        name: "QQ音乐",
        color: "#FFD700",
        icon: "https://y.qq.com/favicon.ico",
        url: "https://y.qq.com/n/yqq/song/{id}.html"
      } as PlatformInfo,
      "spo": {
        name: "Spotify",
        color: "#1DB954",
        icon: "https://open.spotifycdn.com/cdn/images/favicon.0f31d2ea.ico",
        url: "https://open.spotify.com/track/{id}"
      } as PlatformInfo,
      "apl": {
        name: "Apple Music",
        color: "#ff0059",
        icon: "https://music.apple.com/assets/favicon/favicon-180.png",
        url: "https://music.apple.com/cn/song/{id}"
      } as PlatformInfo,
      "isr": {
        name: "ISRC",
        color: "#000000",
        icon: "",
        url: ""
      } as PlatformInfo
    }

function goto(url: string) {
  window.open(url)
}

function getPlatformInfo(platform: string) : PlatformInfo {
  if (!platformInfo.hasOwnProperty(platform)) {
    return {
      name: platform,
      color: "#000000",
      icon: "",
      url: ""
    }
  }
  return platformInfo[platform]
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