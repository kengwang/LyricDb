<script lang="ts" setup>
import type {SongInfoDisplay} from "~/models/song";
import type {LyricInfoResponse} from "~/utils/LyricDbApi";

const context = reactive({
  loading: false,
  status: 0,
  page: 1,
  totalPage: 0,
  total: 0,
  lyrics: [] as Array<LyricInfoResponse>,
  haveResult: true
})

let lyricStatusItems = reactive([
  {text: '所有', value: -1},
  {text: '未审核', value: 0},
  {text: '已审核', value: 1},
  {text: '已拒绝', value: 2},
])

function fireSongFetchEvent() {
  context.loading = true
  useApi().lyric.getPagedLyrics({
    status: context.status,
    page: context.page - 1
  })
      .then((res) => {
        if (res.data.items?.length) {
          context.haveResult = true
          context.totalPage = res.data.totalPages ?? 0;
          context.total = res.data.totalCount ?? 0;
          context.lyrics = res.data.items!
        } else {
          context.lyrics = []
          context.haveResult = false
        }
      }).catch((err) => {
    console.log(err)
  }).finally(() => {
    context.loading = false
  });


}

fireSongFetchEvent()

function statusToIconMapper(status: number) {
  switch (status) {
    case 0:
      return 'mdi-file-document-outline'
    case 1:
      return 'mdi-file-document-check-outline'
    case 2:
      return 'mdi-file-document-remove-outline'
  }
}

function onStatusChanged(e: any) {
  context.status = e
  fireSongFetchEvent()
}

</script>

<template>
  <v-card :loading="context.loading" class="mx-auto pa-12 pb-8">
    <v-card-title>
      <h2>歌词列表</h2>
    </v-card-title>

    <v-select
        @update:modelValue="onStatusChanged"
        @input="fireSongFetchEvent"
        v-model="context.status"
        :items="lyricStatusItems"
        item-title="text"
        item-value="value"
        label="歌词状态"/>
    <NuxtLink to="/song/submit">
      <v-btn class="w-100" color="primary">提交新歌曲</v-btn>
    </NuxtLink>
    <div class="my-8">
      <div v-if="!context.haveResult">
        <v-alert dense type="info">
          <p>没有找到任何歌词</p>
        </v-alert>
      </div>

      <v-list v-else>
        <NuxtLink v-for="lyric in context.lyrics" :to="`/song/${lyric.song?.id}/lyric/${lyric.id}`">
          <v-list-item
              lines="three"
          >
            <template v-slot:prepend>
              <v-icon :icon="statusToIconMapper(lyric.status??0)"/>
            </template>
            <v-list-item-title v-text="lyric.id"/>
            <v-list-item-subtitle>
              <div>歌曲: {{ lyric.song?.name }}</div>
              <div>提交者: {{ lyric.submitter!.userName }}</div>
              <div>提交时间: {{ new Date(lyric.createTime).toLocaleString() }}</div>
            </v-list-item-subtitle>
          </v-list-item>
        </NuxtLink>
      </v-list>
    </div>
    <v-pagination v-model="context.page" :length="context.totalPage" @update:modelValue="fireSongFetchEvent"/>
    <div class="text-center">共 {{ context.total }} 个</div>
  </v-card>
</template>

<style scoped>

</style>