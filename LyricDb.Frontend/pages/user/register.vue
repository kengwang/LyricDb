<script setup lang="ts">
definePageMeta({
  layout: "login"
})

let registerInfo = reactive({
  email: '',
  username: '',
  password: ''
})
let alertInfo = reactive({
  type: undefined as "error" | "success" | "warning" | "info" | undefined,
  message: '注册即代表您同意我们的服务条款和隐私政策',
  title: '',
  loading: false
})

function doRegister() {
  alertInfo.loading = true
  if (registerInfo.email === '' || registerInfo.username === '' || registerInfo.password === '') {
    alertInfo.type = 'error';
    alertInfo.message = '用户名、邮箱和密码不能为空';
    alertInfo.title = '错误';
    alertInfo.loading = false;
    return
  }
  useApi().user.postRegister(
      {
        email: registerInfo.email,
        name: registerInfo.username,
        password: registerInfo.password
      }).then(res => {
    alertInfo.type = 'success';
    alertInfo.message = '注册成功，欢迎加入 LyricDB，即将跳转到登录页面';
    alertInfo.title = '注册成功';
    alertInfo.loading = false;
    setTimeout(() => {
      useRouter().push('/user/login')
    }, 3000)
  }).catch(err => {
    alertInfo.type = 'error'
    alertInfo.title = '注册失败'
    alertInfo.message = err.response.data.errors.register[0] ?? err?.message ?? '未知错误'
  }).finally(()=>{
    alertInfo.loading = false
  });
}
</script>

<template>
  <v-card :loading="alertInfo.loading" class="mx-auto pa-12 pb-8">
    <v-card-title style="font-weight: bold">注册 LyricDB</v-card-title>
    <v-card-subtitle>欢迎成为新的一员呀~</v-card-subtitle>
    <div class="px-4 pt-4">
      <v-text-field v-model="registerInfo.username" label="用户名" variant="outlined"/>
      <v-text-field v-model="registerInfo.email" type="email" label="邮箱" variant="outlined"/>
      <v-text-field v-model="registerInfo.password" label="密码" type="password" variant="outlined"></v-text-field>
    </div>
    <div>

      <v-btn class="w-100" color="primary" variant="tonal" @click="doRegister" :disabled="alertInfo.loading">注册</v-btn>
      <NuxtLink to="/user/login">
        <v-btn class="float-right my-4" variant="plain" color="primary" text="前往登录"/>
      </NuxtLink>
    </div>
    <v-alert :text="alertInfo.message" :title="alertInfo.title" :type="alertInfo.type" class="w-100 mt-8" />
  </v-card>
</template>

<style scoped>

</style>