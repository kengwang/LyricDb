<script lang="ts" setup>
import {KnownMusicPlatforms} from "~/utils/Humanizer";
let success = ref(false)
let songId = ''
let songInfo = reactive({
  name: '',
  singer: '',
  album: '',
  duration: 0,
  binds: [
    {
      platform: 'ncm',
      id: ''
    }
  ]
})

let alertInfo = reactive({
  loading: false,
  message: '请注意：请确保歌曲信息无误后再提交',
  type: 'info'
})

function gotoSong(){
  useRouter().push(`/song/${songId}`)
}

function submit() {
  alertInfo.loading = true
  let data = {
    name: songInfo.name,
    artists: songInfo.singer,
    album: songInfo.album,
    duration: songInfo.duration,
    binds: [] as Array<string>
  };
  data.binds = songInfo.binds.map((bind) => {
    return `${bind.platform}` + 'sg' + `${bind.id}`
  })

  useApi().song.postSong(data)
      .then((res) => {
        alertInfo.message = '提交成功'
        alertInfo.type = 'success'
        success.value = true
        songId = res.data.id!
      }).catch((err) => {
        console.log(err)
        alertInfo.message = '提交失败: ' + err.response?.data?.message ? err.message : '未知错误'
        alertInfo.type = 'error'
      }).finally(() => {
        alertInfo.loading = false
      });
}

</script>

<template>
  <v-card :loading="alertInfo.loading" class="mx-auto pa-12 pb-8">
    <v-card-title>
      <h2>提交歌曲</h2>
    </v-card-title>
    <v-card-text>
      <p>提交一首歌曲只需要填写以下信息</p>
    </v-card-text>
    <v-divider></v-divider>
    <v-card-text>
      <v-text-field v-model="songInfo.name" label="歌曲名称" outlined/>
      <v-text-field v-model="songInfo.singer" label="艺术家" outlined/>
      <v-text-field v-model="songInfo.album" label="专辑" outlined/>
      <v-text-field v-model="songInfo.duration" label="时长 (毫秒)" outlined/>
      <v-row v-for="bind in songInfo.binds">
        <v-col cols="4">
          <v-select v-model="bind.platform" :items="KnownMusicPlatforms" density="compact" item-title="name"
                    item-value="id"
                    label="平台"/>
        </v-col>
        <v-col cols="7">
          <v-text-field v-model="bind.id" density="compact" label="歌曲ID" outlined/>
        </v-col>
        <v-col cols="1">
          <v-btn icon="mdi-close" variant="text" @click="songInfo.binds.splice(songInfo.binds.indexOf(bind), 1)"/>
        </v-col>
      </v-row>
      <v-btn class="w-100" prepend-icon="mdi-plus" @click="songInfo.binds.push({platform: 'ncm', id: ''})">添加平台
      </v-btn>
    </v-card-text>
    <v-card-actions>
      <v-btn class="w-100" color="primary" @click="submit">提交</v-btn>
    </v-card-actions>
    <v-alert :type="alertInfo.type" v-if="alertInfo.message" dismissible>
      {{alertInfo.message}}
    </v-alert>
    <v-btn class="w-100" color="primary" @click="gotoSong" v-if="success">查看已提交歌曲</v-btn>
  </v-card>
</template>

<style scoped>

</style>