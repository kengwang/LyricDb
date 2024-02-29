<script lang="ts" setup>
import type {SongInfoDisplay} from "~/models/song";
import type {LyricInfoResponse} from "~/utils/LyricDbApi";

let context = reactive({
  song: {} as SongInfoDisplay,
  lyrics: [] as LyricInfoResponse[],
  loading: false
})

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

useApi().song.getSong(useRoute().params.id as string).then((res) => {
  context.song.id = res.data.id!
  context.song.name = res.data.name!
  context.song.artists = res.data.artists!
  context.song.album = res.data.album!
  context.song.cover = res.data.cover!
}).catch((err) => {
  console.log(err)
}).finally(() => {
  context.loading = false
})

useApi().song.getSongLyrics(useRoute().params.id as string).then((res) => {
  context.lyrics = res.data
})
</script>

<template>
  <v-card class="mx-auto pa-12 pb-8">
    <NuxtLink :to="`/song/${context.song.id}`" class="my-4">
      <v-card>
        <div class="flex-row" style="display: flex">
          <v-img :src="context.song.cover" max-height="100" max-width="100" min-height="100"
                 min-width="100"/>
          <div class="flex-column pl-4" style="display: flex">
            <div class="font-weight-bold">{{ context.song.name }}</div>
            <div>{{ context.song.artists }}</div>
            <div>{{ context.song.album }}</div>
          </div>
        </div>
      </v-card>
    </NuxtLink>
    <v-divider class="my-4"/>
    <v-card-title>
      <h2>歌词历史</h2>
    </v-card-title>

    <v-list>
      <NuxtLink v-for="lyric in context.lyrics" :to="`/song/${context.song.id}/lyric/${lyric.id}`">
        <v-list-item
            lines="three"
        >
          <template v-slot:prepend>
            <v-icon :icon="statusToIconMapper(lyric.status ?? 0)"/>
          </template>
          <v-list-item-title v-text="lyric.id"/>
          <v-list-item-subtitle>
            <div>提交者: {{ lyric.submitter!.userName }}</div>
            <div>提交时间: {{ new Date(lyric.createTime).toLocaleString() }}</div>
          </v-list-item-subtitle>
        </v-list-item>
      </NuxtLink>
    </v-list>
  </v-card>
</template>

<style scoped>

</style>