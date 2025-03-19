<script setup>
import { ref } from 'vue'

const appName = ref('Quizlet Clone')
</script>

<template>
  <div class="min-h-screen flex flex-col">
    <header class="bg-white shadow-sm px-8 py-4 flex justify-between items-center">
      <div class="flex items-center gap-4">
        <img src="./assets/logo.svg" alt="Quizlet Logo" class="h-10 w-auto" />
        <h1 class="text-2xl font-semibold text-gray-800 m-0">{{ appName }}</h1>
      </div>
      <nav class="hidden md:block">
        <ul class="flex gap-8 list-none m-0 p-0">
          <li><router-link to="/" class="text-gray-800 font-medium hover:text-blue-500 transition-colors duration-200">Home</router-link></li>
        </ul>
      </nav>
    </header>

    <main class="flex-1 p-8 bg-gray-50">
      <router-view></router-view>
    </main>
  </div>
</template>
