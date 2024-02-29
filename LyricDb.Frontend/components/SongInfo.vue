<script setup lang="ts">
import type {SongInfoDisplay} from '~/models/song';

defineProps<{
  song: SongInfoDisplay
}>()
</script>

<template>
<NuxtLink :to="`/song/${song.id}`" class="flex-row" style="display: flex">
  <v-img
      :src="song.cover"
      width="100"
      height="100"
      style="max-width: 100px; max-height: 100px;"
      class="align-start mr-6 text-white"
  />
  <div class="align-start">
    <div class="text-h5 font-weight-bold">{{ song.name }}</div>
    <div class="text-h6">By: {{ song.artists }}</div>
    <div>From: {{ song.album }}</div>
  </div>
</NuxtLink>
</template>

<style scoped>

</style>