<script setup>
import router from '@/router'
import { ref } from 'vue'

// demo cards, get the real data from the backend...
const recentSets = ref([
  { id: 1, title: 'Spanisch Vokabeln', cards: 50, lastStudied: new Date().toLocaleDateString()},
  { id: 2, title: 'Mathematik', cards: 30, lastStudied: new Date().toLocaleDateString() },
  { id: 3, title: 'Geschichte', cards: 45, lastStudied: new Date().toLocaleDateString() },
])

const studyStats = ref({
  totalSets: 12,
  totalCards: 450
})

const navigateToCreate = () => {
  router.push('/create')
}

</script>

<template>
  <div class="space-y-6">
    <!-- Welcome Section -->
    <div class="bg-white rounded-lg shadow-sm p-6">
      <h2 class="text-2xl font-bold text-gray-800 mb-2">Willkommen zurück</h2>
      <p class="text-gray-600">Viel Spaß beim Lernen!</p>
    </div>

    <!-- Stats Grid -->
    <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
      <div class="bg-white rounded-lg shadow-sm p-6">
        <h3 class="text-sm font-medium text-gray-500 mb-1">Anzahl Sets</h3>
        <p class="text-2xl font-bold text-gray-800">{{ studyStats.totalSets }}</p>
      </div>
      <div class="bg-white rounded-lg shadow-sm p-6">
        <h3 class="text-sm font-medium text-gray-500 mb-1">Anzahl Karten</h3>
        <p class="text-2xl font-bold text-gray-800">{{ studyStats.totalCards }}</p>
      </div>
    </div>

    <!-- Recent Sets -->
    <div class="bg-white rounded-lg shadow-sm p-6">
      <div class="flex justify-between items-center mb-4">
        <h3 class="text-lg font-semibold text-gray-800">Letzte Sets</h3>
        <button class="text-blue-500 hover:text-blue-600 font-medium">Alle anzeigen</button>
      </div>
      <div class="space-y-4">
        <div v-for="set in recentSets" :key="set.id" 
             class="flex items-center justify-between p-4 bg-gray-50 rounded-lg hover:bg-gray-100 transition-colors duration-200">
          <div>
            <h4 class="font-medium text-gray-800">{{ set.title }}</h4>
            <p class="text-sm text-gray-500">{{ set.cards }} Karten</p>
          </div>
          <div class="text-right">
            <p class="text-sm text-gray-500">Zuletzt besucht am {{ set.lastStudied }}</p>
            <button class="text-blue-500 hover:text-blue-600 text-sm font-medium mt-1">Jetzt lernen</button>
          </div>
        </div>
      </div>
    </div>

    <!-- Create Set Button -->
    <div class="bg-white rounded-lg shadow-sm p-6">
      <h3 class="text-lg font-semibold text-gray-800 mb-2">Neues Set erstellen</h3>
      <button class="bg-blue-500 text-white px-4 py-2 rounded-md hover:bg-blue-600 transition-colors duration-200" @click="navigateToCreate">
        Set erstellen
      </button>
    </div>
  </div>
</template> 