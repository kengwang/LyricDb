<script setup lang="ts">

</script>

<template>
  <AppHeader/>
  <v-main class="app-main">
      <v-row>
        <v-col class="pt-md-16" cols="12" md="4" offset-md="7">
          <slot />
        </v-col>
      </v-row>
  </v-main>
</template>

<style scoped>
.app-main {
  background: url("https://s2.loli.net/2024/02/24/SbXBluRIVmJGofP.jpg");
  background-size: cover;
}
</style>