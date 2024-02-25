<script lang="ts" setup>
import type {UserInfo} from "~/models/user";
import {HumanizerRole} from "~/utils/Humanizer";
import {roleToColorMapper} from "~/utils/mappers";

let show = ref(false);
const props = defineProps<{
  userInfo: UserInfo,
  isKnown: boolean
}>()
onMounted(() => {
  setInterval(() => {
    show.value = !show.value
  }, 1000)
})

</script>

<template>

  <div class="useravatar">

    <v-badge dot v-if="isKnown" :color="roleToColorMapper(userInfo.role)" class="useravatar-avatar">
      <v-avatar :image="userInfo.avatar"/>
      <v-tooltip :text="HumanizerRole(userInfo.role)" activator="parent" />
    </v-badge>

    <div v-else class="useravatar-avatar">
      <v-avatar icon="mdi-account"/>
    </div>
    <div class="useravatar-info">
      <span class="useravatar-username">
        {{ userInfo.name }}
      </span>
    </div>
  </div>
</template>

<style scoped>

.useravatar {
  display: flex;
  flex-direction: row;
}

.useravatar-avatar {
  align-self: center;
}

.useravatar-username {
  align-self: start;
}

.useravatar-role {
  align-self: end;
  text-align: start;
  width: 100%;
}

.useravatar-info {
  align-self: center;
  margin-left: 8px;
  display: flex;
  flex-direction: column;
}
</style>