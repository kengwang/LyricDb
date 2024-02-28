<script setup lang="ts">
let verifyToken = useRoute().query.token as string
let userId = useRoute().params.id as string
let message = ref("正在验证您的邮箱")
let verifyInfo = reactive({
  isVerified: false,
  isLoading: true
})
useApi().user.confirmEmail(userId, {token: verifyToken}).then(() => {
  verifyInfo.isVerified = true
  verifyInfo.isLoading = false
  message.value = "邮箱验证成功"
  updateUserInfo()
}).catch(() => {
  verifyInfo.isVerified = false
  verifyInfo.isLoading = false
  message.value = "邮箱验证失败"
})
</script>

<template>
  <v-card>
    <v-card-title>验证邮箱</v-card-title>
    <v-divider></v-divider>
    <div class="py-12 text-center">
      <v-icon
          v-if="!verifyInfo.isLoading && verifyInfo.isVerified"
          class="mb-6"
          color="success"
          icon="mdi-check-circle-outline"
          size="128"
      ></v-icon>
      <v-icon
          v-if="!verifyInfo.isLoading && !verifyInfo.isVerified"
          class="mb-6"
          color="error"
          icon="mdi-alert-circle-outline"
          size="128"/>
      <v-progress-circular v-if="verifyInfo.isLoading" class="mb-6" indeterminate size="128" />
      <div class="text-h4 font-weight-bold">{{ message }}</div>
    </div>
  </v-card>
</template>

<style scoped>

</style>