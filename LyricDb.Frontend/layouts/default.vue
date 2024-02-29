<template>
  <AppHeader/>
  <v-main class="app-main">
    <v-container class="pa-4 pa-sm-6 pa-md-8" style="max-width: 960px;">
      <slot/>
    </v-container>
  </v-main>
</template>

<style scoped>
.app-main {
  background: url("https://s2.loli.net/2024/02/24/SbXBluRIVmJGofP.jpg");
  background-size: cover;
}
</style>