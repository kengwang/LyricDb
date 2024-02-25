<script lang="ts" setup>
import type {SongInfoDisplay} from "~/models/song";
let song = ref({
  id: '',
  name: '',
  artists: '',
  album: '',
  cover: "https://via.placeholder.com/256x256.png?text=No+Cover"
} as SongInfoDisplay)
useApi().song.getSong(useRoute().params.id as string).then((res) => {
  song.value.id = res.data.id!
  lyricInfo.value.songId = song.value.id
  song.value.name = res.data.name!
  song.value.artists = res.data.artists!
  song.value.album = res.data.album!
  song.value.cover = res.data.cover ?? "https://via.placeholder.com/256x256.png?text=No+Cover"
  alertInfo.value.loading = false
}).catch((err) => {
  console.log(err)
})

function modeToLanguage(mode: string) {
  switch (mode) {
    case 'alrc':
      return 'json'
    case 'ttml':
      return 'xml'
    default:
      return 'plaintext'
  }
}

let lyricInfo = ref({
  songId: '',
  lyrics: '',
  type: 'alrc'
})

let alertInfo = ref({
  loading: false,
  message: '欢迎您参与共建！请确保歌词信息无误后再提交',
  type: 'info'
})

function submit() {
  alertInfo.value.loading = true
  useApi().lyric.postLyricType(lyricInfo.value.type,{
    songId: song.value.id,
    content: lyricInfo.value.lyrics
  }).then(() => {
    alertInfo.value.type = 'success'
    alertInfo.value.message = '提交成功！'
  }).finally(() => {
    alertInfo.value.loading = false
  })
}
</script>

<template>
  <v-card :loading="alertInfo.loading" class="mx-auto pa-12 pb-8">
    <v-card-title>
      <h2>提交歌词</h2>
    </v-card-title>
    <v-divider class="my-4"/>
    <div>您将要给以下歌曲提交歌词：</div>
    <SongInfo :song="song"/>

    <v-form>
      <v-select
          v-model="lyricInfo.type"
          :items="['alrc', 'ttml']"
          label="歌词类型"/>

      <LazyMonacoEditor
          v-model="lyricInfo.lyrics"
          :lang="modeToLanguage(lyricInfo.type)"
          style="height: 400px; width: 100%;"/>
      <v-btn
          class="w-100"
          color="primary"
          text="提交"
          @click="submit"
      />
      <v-divider class="my-4"/>
      <v-alert
          :type="alertInfo.type"
          dismissible
      >
        {{ alertInfo.message }}
      </v-alert>
    </v-form>
  </v-card>
</template>

<style scoped>

</style>