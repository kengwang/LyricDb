<script setup lang="ts">
import type {SongInfoDisplay} from "~/models/song";
import type {LyricInfoResponse} from "~/utils/LyricDbApi";
import {mapUserInfoResponseToUserInfo} from "~/utils/mappers";
import UserAvatar from "~/components/UserAvatar.vue";

function fireUserInfo(){
  useApi().user.getUserInfo(context.userId)
      .then((res) => {
        context.user = res.data
      }).catch((err) => {
    console.log(err)
  });
}

const context = reactive({
  userId: useRoute().params.id as string,
  loading: false,
  search: '',
  page: 1,
  totalPage: 0,
  total: 0,
  lyrics: [] as Array<LyricInfoResponse>,
  haveResult: true,
  user: undefined as UserInfoResponse | undefined
})

function fireSongFetchEvent() {
  context.loading = true
  useApi().user.getUserPagedLyrics(context.userId,{
    userId: context.userId,
    page: context.page - 1
  })
      .then((res) => {
        if (res.data.items?.length) {
          context.haveResult = true
          context.totalPage = res.data.totalPages ?? 0;
          context.total = res.data.totalCount ?? 0;
          context.lyrics = res.data.items
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
fireUserInfo()
fireSongFetchEvent()

</script>

<template>
<v-card :loading="context.loading" class="mx-auto pa-12 pb-8">
  <v-row>
    <v-col cols="12" md="4">
      <v-img
          v-if="context.user"
          :src="`https://cravatar.cn/avatar/${context.user?.avatar ?? ''}?s=200`"
          class="align-end text-white"
      />
    </v-col>

    <v-col cols="12" md="8">
      <div v-if="context.user">
        <div class="text-h5 font-weight-bold">{{ context.user?.userName }}</div>
        <div>用户组: {{ context.user?.role }}</div>
        <div>用户ID: {{ context.user?.id }}</div>
        <div>用户贡献: {{ context.user?.contributionPoint }}</div>
        <div>
          <UserAvatar :is-known="true" :user-info="mapUserInfoResponseToUserInfo(context.user!)" class="ml-8"
                      style="align-self: start"/>
        </div>


        <v-divider class="my-4"/>
      </div>
    </v-col>
  </v-row>
  <v-card-title>
    <h2>用户贡献</h2>
  </v-card-title>
  <v-divider></v-divider>
  <v-list>
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
  <v-divider />
  <v-card-actions>
    <v-pagination
        v-model="context.page"
        :length="context.totalPage"
        @input="fireSongFetchEvent"
    ></v-pagination>
  </v-card-actions>
</v-card>
</template>

<style scoped>

</style>