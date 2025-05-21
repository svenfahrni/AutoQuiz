<script setup>
import { ref } from 'vue'
import router from '@/router'
import axios from 'axios'
import { storeToRefs } from 'pinia'
import { useDeckStore } from '@/stores/deck'

const file = ref(null)
const isDragging = ref(false)
const error = ref('')
const isLoading = ref(false)

const deckStore = useDeckStore()
const { currentDeck } = storeToRefs(deckStore)

const handleFileUpload = (event) => {
  const eventFile = event.target.files[0]
  if (!eventFile) return

  const allowedTypes = ['application/pdf', 'application/msword', 'application/vnd.openxmlformats-officedocument.wordprocessingml.document']
  if (!allowedTypes.includes(eventFile.type)) {
    error.value = 'Please upload a PDF or Word document'
    return
  }

  file.value = eventFile
}

const handleDrop = (event) => {
  event.preventDefault()
  isDragging.value = false
  
  const eventFile = event.dataTransfer.files[0]
  if (!eventFile) return

  const allowedTypes = ['application/pdf', 'application/msword', 'application/vnd.openxmlformats-officedocument.wordprocessingml.document']
  if (!allowedTypes.includes(eventFile.type)) {
    error.value = 'Please upload a PDF or Word document'
    return
  }

  file.value = eventFile
}

const handleDragOver = (event) => {
  event.preventDefault()
  isDragging.value = true
}

const handleDragLeave = () => {
  isDragging.value = false
}

const handleCreateSet = () => {
  isLoading.value = true
  let formData = new FormData();
  formData.append('formFile', file.value);  
  axios.post('/api/files', formData, {
    headers: {
      'Content-Type': 'multipart/form-data'
    }
  })
    .then(response => {
      const deck = response.data;
      deckStore.addDeck(deck)
      deckStore.setCurrentDeck(deck)
      router.push({ name: 'Learn' })
    })
    .catch(error => {
      console.error(error)
    })
    .finally(() => {
      isLoading.value = false
    })
}
</script>

<template>
  <div class="max-w-4xl mx-auto space-y-6">
    <div class="bg-white rounded-lg shadow-sm p-6">
      <h2 class="text-2xl font-bold text-gray-800 mb-4">Create New Set</h2>
      
      <!-- Upload Section -->
      <div v-if="!file"
        class="border-2 border-dashed rounded-lg p-8 text-center transition-colors duration-200"
        :class="[
          isDragging ? 'border-blue-500 bg-blue-50' : 'border-gray-300 hover:border-blue-500',
          error ? 'border-red-500 bg-red-50' : ''
        ]"
        @dragover="handleDragOver"
        @dragleave="handleDragLeave"
        @drop="handleDrop"
      >
        <div class="space-y-4">
          <div class="text-gray-600">
            <svg class="mx-auto h-12 w-12 text-gray-400" stroke="currentColor" fill="none" viewBox="0 0 48 48">
              <path d="M28 8H12a4 4 0 00-4 4v20m32-12v8m0 0v8a4 4 0 01-4 4H12a4 4 0 01-4-4v-4m32-4l-3.172-3.172a4 4 0 00-5.656 0L28 28M8 32l9.172-9.172a4 4 0 015.656 0L28 28m0 0l4 4m4-24h8m-4-4v8m-12 4h.02" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" />
            </svg>
            <p class="mt-2">Drag and drop your document here, or</p>
            <label class="mt-2 inline-block">
              <span class="text-blue-500 hover:text-blue-600 cursor-pointer">browse</span>
              <input type="file" class="hidden" accept=".pdf,.doc,.docx" @change="handleFileUpload">
            </label>
            <p class="text-sm text-gray-500 mt-2">Supported formats: PDF, Word (.doc, .docx)</p>
          </div>
          
          <div v-if="error" class="text-red-500 text-sm">
            {{ error }}
          </div>
        </div>
      </div>

      <div v-else>
        <div class="flex items-center space-x-4 p-4 bg-gray-50 rounded-lg">
          <svg class="h-8 w-8 text-gray-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M7 21h10a2 2 0 002-2V9.414a1 1 0 00-.293-.707l-5.414-5.414A1 1 0 0012.586 3H7a2 2 0 00-2 2v14a2 2 0 002 2z" />
          </svg>
          <div>
            <p class="text-sm font-medium text-gray-900">{{ file.name }}</p>
            <p class="text-xs text-gray-500">{{ (file.size / 1024).toFixed(1) }} KB</p>
          </div>
          <button 
            class="ml-auto text-gray-400 hover:text-gray-500"
            @click="file = null"
          >
            <svg class="h-5 w-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
            </svg>
          </button>
        </div>
      </div>

      <!-- Action Buttons -->
      <div class="mt-8 flex justify-end space-x-4">
        <button 
          class="px-4 py-2 text-gray-700 hover:text-gray-900"
          @click="$router.push('/')"
          :disabled="isLoading"
        >
          Cancel
        </button>
        <button 
          class="px-4 py-2 bg-blue-500 text-white rounded-md hover:bg-blue-600 transition-colors duration-200 disabled:opacity-50 disabled:cursor-not-allowed flex items-center space-x-2"
          @click="handleCreateSet"
          :disabled="isLoading"
        >
          <svg v-if="isLoading" class="animate-spin h-5 w-5 text-white" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
            <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle>
            <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
          </svg>
          <span>{{ isLoading ? 'Creating...' : 'Create Set' }}</span>
        </button>
      </div>
    </div>
  </div>
</template> 