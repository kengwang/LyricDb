<script lang="ts" setup>
import type {SongInfoDisplay} from "~/models/song";

const context = reactive({
  loading: false,
  search: '',
  page: 1,
  totalPage: 0,
  total: 0,
  songs: [] as Array<SongInfoDisplay>,
  haveResult: true,
  onlyNoLyric: false
})

function fireSongFetchEvent() {
  context.loading = true
  useApi().song.getPagedSongs({
    search: context.search,
    page: context.page - 1,
    unlyriced : context.onlyNoLyric
  })
      .then((res) => {
        if (res.data.items?.length) {
          context.haveResult = true
          context.totalPage = res.data.totalPages ?? 0;
          context.total = res.data.totalCount ?? 0;
          context.songs = res.data.items!.map((item) => {
            return <SongInfoDisplay>{
              id: item.id,
              name: item.name,
              artists: item.artists,
              album: item.album,
              cover: item.cover
            }
          })
        } else {
          context.songs = []
          context.haveResult = false
        }
      }).catch((err) => {
    console.log(err)
  }).finally(() => {
    context.loading = false
  });


}

fireSongFetchEvent()

function onSearch(event: KeyboardEvent) {
  context.page = 1
  fireSongFetchEvent()
}

</script>

<template>
  <v-card :loading="context.loading" class="mx-auto pa-12 pb-8">
    <v-card-title>
      <h2>歌曲列表</h2>
    </v-card-title>

    <v-text-field v-model="context.search" class="mt-4" density="compact" label="搜索" prepend-icon="mdi-magnify"
                  @keyup.enter="onSearch"/>
    <v-checkbox label="只看无歌词" class="my-1" v-model="context.onlyNoLyric" @change="fireSongFetchEvent"/>
    <NuxtLink to="/song/submit">
      <v-btn class="w-100" color="primary">提交新歌曲</v-btn>
    </NuxtLink>
    <div class="my-8">
      <div v-if="!context.haveResult">
        <v-alert dense type="info">
          <p>没有找到任何歌曲</p>
        </v-alert>
      </div>
      <div v-else class="flex-column" style="display: flex">
        <div v-for="song in context.songs" class="w-100 my-2">
          <NuxtLink :to="`/song/${song.id}`">
            <v-card>
              <div class="flex-row" style="display: flex">
                <v-img :src="song.cover" max-height="100" max-width="100" min-height="100"
                       min-width="100"/>
                <div class="flex-column pl-4" style="display: flex">
                  <div class="font-weight-bold">{{ song.name }}</div>
                  <div>{{ song.artists }}</div>
                  <div>{{ song.album }}</div>
                </div>
              </div>
            </v-card>
          </NuxtLink>
        </div>
      </div>
    </div>
    <v-pagination v-model="context.page" :length="context.totalPage" @update:modelValue="fireSongFetchEvent"/>
    <div class="text-center">共 {{ context.total }} 首</div>
  </v-card>
</template>

<style scoped>

</style>