<script setup lang="ts">
// This starter template is using Vue 3 <script setup> SFCs
// Check out https://vuejs.org/api/sfc-script-setup.html#script-setup
import AppHeader from "./views/AppHeader.vue";
import AppFooter from "./views/AppFooter.vue";
import GlobalMessage from "./components/GlobalMessage/GlobalMessage.vue";
import { provideGlobalMessage } from "./components/GlobalMessage/GlobalMessageProxy";
import GlobalSpinner from "./components/GlobalSpinner/GlobalSpinner.vue";
import { provideGlobalSpinner } from "./components/GlobalSpinner/GlobalSpinnerProxy";

const proxy = provideGlobalMessage();
const spinner = provideGlobalSpinner();
</script>

<template>
    <app-header></app-header>
    <div class="container">
        <main role="main">
            <router-view></router-view>
        </main>
        <global-message :proxy="proxy"></global-message>
        <global-spinner :proxy="spinner"></global-spinner>
    </div>
    <app-footer></app-footer>
</template>

<style></style>
