<script setup lang="ts"></script>

<template>
    <header>
        <nav
            class="navbar navbar-expand-sm navbar-toggleable-sm navbar-light bg-white border-bottom box-shadow mb-3"
        >
            <div class="container">
                <router-link class="navbar-brand" to="/"
                    >自动化任务管理平台</router-link
                >
                <button
                    class="navbar-toggler"
                    type="button"
                    data-bs-toggle="collapse"
                    data-bs-target=".navbar-collapse"
                    aria-controls="navbarSupportedContent"
                    aria-expanded="false"
                    aria-label="Toggle navigation"
                >
                    <span class="navbar-toggler-icon"></span>
                </button>
                <div
                    class="navbar-collapse collapse d-sm-inline-flex justify-content-between"
                >
                    <ul class="navbar-nav flex-grow-1">
                        <li class="nav-item">
                            <router-link class="nav-link text-dark" to="/group">
                                分组
                            </router-link>
                        </li>
                        <li class="nav-item">
                            <router-link class="nav-link text-dark" to="/work"
                                >工作项</router-link
                            >
                        </li>
                        <li class="nav-item">
                            <router-link
                                class="nav-link text-dark"
                                to="/setting"
                                >设置</router-link
                            >
                        </li>
                    </ul>
                </div>
            </div>
        </nav>
    </header>
</template>
