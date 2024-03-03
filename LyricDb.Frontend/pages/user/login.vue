<script lang="ts" setup>
import {useApi} from "~/composables/api";

definePageMeta({
  layout: "login"
})
let loginInfo = reactive({
  account: '',
  password: ''
})
let alertInfo = reactive({
  type: undefined as "error" | "success" | "warning" | "info" | undefined,
  message: '登录即代表您同意我们的服务条款和隐私政策',
  title: '',
  loading: false
})

function doLogin() {
  alertInfo.loading = true
  if (loginInfo.account === '' || loginInfo.password === '') {
    alertInfo.type = 'error';
    alertInfo.message = '用户名和密码不能为空';
    alertInfo.title = '错误';
    alertInfo.loading = false;
    return
  }
  useApi().user.postLogin(
      {
        account: loginInfo.account,
        password: loginInfo.password
      }).then(res => {
    updateUserInfo()
    alertInfo.type = 'success';
    alertInfo.message = '登录成功，欢迎回来 ' + res.data.userName;
    alertInfo.title = '登录成功';
    alertInfo.loading = false;
    setTimeout(() => {
      useRouter().push('/')
    }, 3000)
  }).catch(err => {
    alertInfo.type = 'error'
    alertInfo.title = '登录失败'
    alertInfo.message = err.response?.data?.detail ?? err.message ?? '未知错误'
  }).finally(() => {
    alertInfo.loading = false
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
    <div>
      <v-btn :disabled="alertInfo.loading" class="w-100" color="primary" variant="tonal" @click="doLogin">登录</v-btn>
    </div>
    <NuxtLink to="/user/register">
      <v-btn class="float-right my-8" variant="plain" color="primary" text="前往注册"/>
    </NuxtLink>
    <div>
      <v-alert :text="alertInfo.message" :title="alertInfo.title" :type="alertInfo.type" class="mt-8 w-100"/>
    </div>
  </v-card>
</template>

<style scoped>

</style>