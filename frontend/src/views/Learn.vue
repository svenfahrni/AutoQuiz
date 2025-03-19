<script setup>
import { ref } from 'vue'

// Demo data - replace with actual data from backend
const currentSet = ref('Spanisch Vokabeln')
const cards = ref([
  { id: 1, front: 'Was ist die Hauptstadt von Frankreich?', back: 'Paris' },
  { id: 2, front: 'Was gibt 2 + 2?', back: '4' },
  { id: 3, front: 'Welche Frabe hat Globi?', back: 'Blue' },
  { id: 4, front: 'Welches ist der grösse Planet?', back: 'Jupiter' },
])

const currentIndex = ref(0)
const showAnswer = ref(false)

const nextCard = () => {
  if (currentIndex.value < cards.value.length - 1) {
    currentIndex.value++
    showAnswer.value = false
  }
}

const previousCard = () => {
  if (currentIndex.value > 0) {
    currentIndex.value--
    showAnswer.value = false
  }
}

const toggleAnswer = () => {
  showAnswer.value = !showAnswer.value
}
</script>

<template>
  <div class="max-w-2xl mx-auto space-y-6">
    <div class="flex justify-between items-center">
      <h2 class="text-2xl font-bold text-gray-800">{{ currentSet }}</h2>
      <div class="text-sm text-gray-500">
        Karte {{ currentIndex + 1 }} von {{ cards.length }}
      </div>
    </div>

    <!-- Single Card -->
    <div class="relative aspect-[3/2]">
      <div 
        class="absolute inset-0 bg-white rounded-lg shadow-md p-6 cursor-pointer"
        @click="toggleAnswer"
      >
        <div class="h-full flex items-center justify-center text-center">
          <p class="text-xl font-medium text-gray-800">
            {{ showAnswer ? cards[currentIndex].back : cards[currentIndex].front }}
          </p>
        </div>
      </div>
    </div>

    <!-- Navigation Controls -->
    <div class="flex justify-center gap-4">
      <button 
        @click="previousCard"
        :disabled="currentIndex === 0"
        class="px-4 py-2 bg-gray-100 text-gray-700 rounded-md hover:bg-gray-200 transition-colors duration-200 disabled:opacity-50 disabled:cursor-not-allowed"
      >
        Zurück
      </button>
      <button 
        @click="nextCard"
        :disabled="currentIndex === cards.length - 1"
        class="px-4 py-2 bg-blue-500 text-white rounded-md hover:bg-blue-600 transition-colors duration-200 disabled:opacity-50 disabled:cursor-not-allowed"
      >
        Weiter
      </button>
    </div>
  </div>
</template>

<style scoped>
.rotate-y-180 {
  transform: rotateY(180deg);
}
</style> 