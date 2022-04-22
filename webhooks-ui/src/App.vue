<script setup lang="ts">
// This starter template is using Vue 3 <script setup> SFCs
// Check out https://vuejs.org/api/sfc-script-setup.html#script-setup
import AppHeader from "./components/AppHeader.vue";
import AppFooter from "./components/AppFooter.vue";
import GlobalMessage from "./components/Shared/GlobalMessage/GlobalMessage.vue";
import { provideGlobalMessage } from "./components/Shared/GlobalMessage/GlobalMessageProxy";

const proxy = provideGlobalMessage();
</script>

<template>
    <app-header></app-header>
    <div class="container">
        <main role="main">
            <router-view></router-view>
        </main>
        <global-message :proxy="proxy"></global-message>
    </div>
    <app-footer></app-footer>
</template>

<style></style>
