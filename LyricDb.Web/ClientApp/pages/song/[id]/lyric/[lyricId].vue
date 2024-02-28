<script setup lang="ts">

import type {ALRCFile, ALRCLine, UserInfoResponse} from "~/utils/LyricDbApi";
import type {UserInfo} from "~/models/user";
import {mapUserInfoResponseToUserInfo} from "~/utils/mappers";

class SongInfoDisplay {
  id: string | undefined;
  name: string | undefined;
  artists: string | undefined;
  album: string | undefined;
  cover: string | undefined;
  submitter: UserInfoResponse | undefined;
  createTime: string | undefined;
  lyrics: string[] | undefined;
  currentLyric: string | undefined;
  bindPlatforms: { platform: string; id: string; }[] | undefined;
}

class LyricInfoDisplay {
  id: string | undefined;
  content: string[] = [];
  submitter: UserInfo | undefined;
  createTime: string | undefined;
}

let lyrics = reactive({
  submitter: {
    id: '',
    name: '未知用户',
    avatar: '',
  } as UserInfoResponse,
} as LyricInfoDisplay)
let id = useRoute().params.id as string
let song = reactive({} as SongInfoDisplay)
let loading = ref(true)

function getLyric() {
  useApi().lyric.getLyric(useRoute().params.lyricId as string).then((res) => {
    lyrics.submitter = {
      id: res.data.submitter?.id!,
      name: res.data.submitter?.userName ?? "未知用户",
      avatar: `https://cravatar.cn/avatar/${res.data.submitter?.avatar}` ,
      role: res.data.submitter?.role!,
    }
    lyrics.createTime = res.data.createTime!
    lyrics.id = res.data.id!
    useApi().lyric.getLyricContent(lyrics.id).then((res)=>{
      let lyricContent = res.data as ALRCFile
      lyrics.content = []
      lyricContent?.l?.map((line: ALRCLine) => {
        lyrics.content.push(line.tx!)
      })
    })

  }).catch((err) => {
    if (err.response.status === 404) {
      lyrics.content = ['暂无歌词']
      return
    }
    lyrics.content = ['获取歌词失败']
  })
}

useApi().song.getSong(id).then((res) => {
  song.id = res.data.id!
  song.name = res.data.name!
  song.artists = res.data.artists!
  song.album = res.data.album!
  song.cover = res.data.cover!
  song.submitter = res.data.submitter!
  song.createTime = res.data.createTime!
  song.lyrics = res.data.lyrics!
  song.currentLyric = res.data.currentLyric!
  song.bindPlatforms = res.data.binds!.map((bind) => {
    return {
      platform: bind.slice(0, 3),
      id: bind.slice(5),
    }
  })
  getLyric()
}).catch((err) => {
  console.log(err)
}).finally(() => {
  loading.value = false
})


</script>

<template>
  <v-card class="mx-auto pa-12 pb-8">
    <v-row>
      <v-col cols="12" md="4">
        <v-img
            :src="song.cover"
            class="align-end text-white"
        />
      </v-col>

      <v-col cols="12" md="8">
        <div>
          <div class="text-h5 font-weight-bold">{{ song.name }}</div>
          <div class="text-h6">By: {{ song.artists }}</div>
          <div>From: {{ song.album }}</div>
          <v-divider class="my-4"/>
          <div>提交时间: {{ new Date(lyrics.createTime).toLocaleString() }}</div>
          <div>
            <NuxtLink :to="`/song/${song.id}/lyrics`">歌词历史: {{ song.lyrics?.length ?? 0 }} 个</NuxtLink>
          </div>
          <v-divider class="my-4"/>
          <div style="display: flex; flex-direction: row; align-self: center;"><span
              style="align-self: center">歌词提交: </span>
            <UserAvatar :is-known="true" :user-info="lyrics.submitter" class="ml-8"
                        style="align-self: start"/>
          </div>
        </div>
      </v-col>
    </v-row>
    <v-divider class="my-8"/>
    <div>平台:</div>
    <div class="flex-row" style="display: flex">
      <MusicPlatform v-for="plt in song.bindPlatforms" :id="plt.id" :platform="plt.platform" />
    </div>
    <v-divider class="my-8"/>
    <div v-if="lyrics.id">
      <div v-for="line in lyrics.content">
        {{ line }}
      </div>
    </div>
    <div v-else>
      暂无歌词
    </div>
    <NuxtLink :to="`/song/${song.id}/lyric/submit?id=${lyrics.id}`">
      <v-btn class="my-8 w-100" color="primary" text="编辑歌词"/>
    </NuxtLink>

  </v-card>
</template>

<style scoped>

</style>