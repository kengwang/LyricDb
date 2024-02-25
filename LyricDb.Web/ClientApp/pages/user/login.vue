<script lang="ts" setup>
import {useApi} from "~/composables/api";
import {Md5} from "ts-md5";

definePageMeta({
  layout: "login"
})
let currentUser = useUserInfo()
let loginInfo = ref({
  account: '',
  password: ''
})
let alertInfo = ref({
  type: undefined as "error" | "success" | "warning" | "info" | undefined,
  message: '登录即代表您同意我们的服务条款和隐私政策',
  title: '',
  loading: false
})

function doLogin() {
  alertInfo.value.loading = true
  if (loginInfo.value.account === '' || loginInfo.value.password === '') {
    alertInfo.value = {
      type: 'error',
      message: '用户名和密码不能为空',
      title: '错误',
      loading: false
    }
    return
  }
  useApi().user.postLogin(
      {
        account: loginInfo.value.account,
        password: loginInfo.value.password
      }).then(res => {
    updateUserInfo()
    alertInfo.value = {
      type: 'success',
      message: '登录成功，欢迎回来 ' + res.data.userName,
      title: '登录成功',
      loading: false
    }
    setTimeout(() => {
      useRouter().push('/')
    }, 3000)
  }).catch(err => {
    alertInfo.value.type = 'error'
    alertInfo.value.title = '登录失败'
    alertInfo.value.message = err.response?.data?.detail ?? err.message ?? '未知错误'
  }).finally(() => {
    alertInfo.value.loading = false
  })
}


</script>

<template>
  <v-card :loading="alertInfo.loading" class="mx-auto pa-12 pb-8">
    <v-card-title style="font-weight: bold">登录到 LyricDB</v-card-title>
    <v-card-subtitle>登录后开启新世界</v-card-subtitle>
    <div class="px-4 pt-4">
      <v-text-field v-model="loginInfo.account" label="用户名" variant="outlined"/>
      <v-text-field v-model="loginInfo.password" label="密码" type="password" variant="outlined"></v-text-field>
    </div>
    <v-card-actions>
      <v-spacer></v-spacer>
      <v-btn :disabled="alertInfo.loading" class="w-100" color="primary" variant="tonal" @click="doLogin">登录</v-btn>
    </v-card-actions>
    <v-alert :text="alertInfo.message" :title="alertInfo.title" :type="alertInfo.type" class="mt-8"/>
  </v-card>
</template>

<style scoped>

</style>