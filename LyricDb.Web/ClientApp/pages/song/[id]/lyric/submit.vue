<script lang="ts" setup>
import type {SongInfoDisplay} from "~/models/song";
import type {AxiosResponse} from "axios";

let song = reactive({
  id: '',
  name: '',
  artists: '',
  album: '',
  cover: "https://via.placeholder.com/256x256.png?text=No+Cover"
} as SongInfoDisplay)
useApi().song.getSong(useRoute().params.id as string).then((res) => {
  song.id = res.data.id!
  lyricInfo.songId = song.id
  song.name = res.data.name!
  song.artists = res.data.artists!
  song.album = res.data.album!
  song.cover = res.data.cover ?? "https://via.placeholder.com/256x256.png?text=No+Cover"
  alertInfo.loading = false
}).catch((err) => {
  console.log(err)
})

if (useRoute().query.id) {
  useApi().lyric.getLyric(useRoute().query.id as string).then((res) => {
    lyricInfo.status = res.data.status ?? 0
    lyricInfo.author = res.data.author ?? ''
    lyricInfo.translator = res.data.translator ?? ''
    lyricInfo.transliterator = res.data.transliterator ?? ''
    lyricInfo.timeline = res.data.timeline ?? ''
    lyricInfo.proofreader = res.data.proofreader ?? ''
    useApi().lyric.getLyricContent(useRoute().query.id as string).then((res) => {
      lyricInfo.lyrics = JSON.stringify(res.data)
    }).catch((err) => {
      console.log(err)
    })
  }).catch((err) => {
    console.log(err)
  })
}

let lyricStatusItems = reactive([
  {text: '未审核', value: 0},
  {text: '已审核', value: 1},
  {text: '已拒绝', value: 2},
])

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

let lyricInfo = reactive({
  songId: '',
  lyrics: '',
  status: 0,
  author: '',
  translator: '',
  transliterator: '',
  timeline: '',
  proofreader: '',
  type: 'alrc',
})

let alertInfo = reactive({
  loading: false,
  message: '欢迎您参与共建！请确保歌词信息无误后再提交',
  type: 'info'
})

function submit() {
  alertInfo.loading = true
  let data = {
    id: useRoute().query.id as string,
    songId: song.id,
    content: lyricInfo.lyrics,
    author: lyricInfo.author,
    translator: lyricInfo.translator,
    transliterator: lyricInfo.transliterator,
    timeline: lyricInfo.timeline,
    proofreader: lyricInfo.proofreader,
    status: lyricInfo.status
  }
  let promise: Promise<AxiosResponse<void>>
  if (useRoute().query.id) {
    data.status = lyricInfo.status
    promise = useApi().lyric.putLyricType(lyricInfo.type,data)
  } else {
    promise = useApi().lyric.postLyricType(lyricInfo.type,data)
  }
  promise.then(() => {
    alertInfo.type = 'success'
    alertInfo.message = '提交成功！'
  }).catch(()=>{
    alertInfo.type = 'error'
    alertInfo.message = '设置失败！'
  }).finally(() => {
    alertInfo.loading = false
  })
}

function setMainLyric() {
  alertInfo.loading = true
  useApi().song.setSongLyric(lyricInfo.songId, {lyricId: useRoute().query.id as string}).then(() => {
    alertInfo.type = 'success'
    alertInfo.message = '设置成功！'
  }).catch(()=>{
    alertInfo.type = 'error'
    alertInfo.message = '设置失败！'
  }).finally(() => {
    alertInfo.loading = false

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
      <v-divider class="my-4"/>
      <div class="my-2">歌词信息：</div>
      <v-text-field
          v-model="lyricInfo.author"
          label="作者"/>
      <v-text-field
          v-model="lyricInfo.translator"
          label="翻译"/>
      <v-text-field
          v-model="lyricInfo.transliterator"
          label="音译者"/>
      <v-text-field
          v-model="lyricInfo.timeline"
          label="时轴"/>
      <v-text-field
          v-model="lyricInfo.proofreader"
          label="校对"/>
      <v-select
          :items="lyricStatusItems"
          item-title="text"
          item-value="value"
          v-model="lyricInfo.status"
          label="歌词状态"/>
      <v-btn class="w-100 my-4" color="primary" v-if="useUserInfo().value.role >= 2" text="设为主歌词" @click="setMainLyric"/>
      <v-divider class="my-4"/>
      <v-btn
          class="w-100 my-8"
          color="primary"
          text="提交"
          @click="submit"
      />
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