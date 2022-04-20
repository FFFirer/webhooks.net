<script setup lang="ts">
// This starter template is using Vue 3 <script setup> SFCs
// Check out https://vuejs.org/api/sfc-script-setup.html#script-setup
import AppHeader from "./components/AppHeader.vue";
import AppFooter from "./components/AppFooter.vue";
</script>

<template>
    <app-header></app-header>
    <div class="container">
        <main role="main">
            <router-view></router-view>
        </main>
    </div>
    <app-footer></app-footer>
</template>

<style>
#app {
}
</style>
