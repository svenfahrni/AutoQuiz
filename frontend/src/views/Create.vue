<script setup>
import { ref } from 'vue'

const fileContent = ref('')
const fileName = ref('')
const isDragging = ref(false)
const error = ref('')

const handleFileUpload = (event) => {
  const file = event.target.files[0]
  if (!file) return

  const allowedTypes = ['application/pdf', 'application/msword', 'application/vnd.openxmlformats-officedocument.wordprocessingml.document']
  if (!allowedTypes.includes(file.type)) {
    error.value = 'Please upload a PDF or Word document'
    return
  }

  fileName.value = file.name
  const reader = new FileReader()
  
  reader.onload = (e) => {
    fileContent.value = e.target.result
    error.value = ''
  }
  
  reader.onerror = () => {
    error.value = 'Error reading file'
  }
  
  reader.readAsText(file)
}

const handleDrop = (event) => {
  event.preventDefault()
  isDragging.value = false
  
  const file = event.dataTransfer.files[0]
  if (!file) return

  const allowedTypes = ['application/pdf', 'application/msword', 'application/vnd.openxmlformats-officedocument.wordprocessingml.document']
  if (!allowedTypes.includes(file.type)) {
    error.value = 'Please upload a PDF or Word document'
    return
  }

  fileName.value = file.name
  const reader = new FileReader()
  
  reader.onload = (e) => {
    fileContent.value = e.target.result
    error.value = ''
  }
  
  reader.onerror = () => {
    error.value = 'Error reading file'
  }
  
  reader.readAsText(file)
}

const handleDragOver = (event) => {
  event.preventDefault()
  isDragging.value = true
}

const handleDragLeave = () => {
  isDragging.value = false
}
</script>

<template>
  <div class="max-w-4xl mx-auto space-y-6">
    <div class="bg-white rounded-lg shadow-sm p-6">
      <h2 class="text-2xl font-bold text-gray-800 mb-4">Create New Set</h2>
      
      <!-- Upload Section -->
      <div 
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

      <!-- Action Buttons -->
      <div class="mt-8 flex justify-end space-x-4">
        <button 
          class="px-4 py-2 text-gray-700 hover:text-gray-900"
          @click="$router.push('/')"
        >
          Cancel
        </button>
        <button 
          class="px-4 py-2 bg-blue-500 text-white rounded-md hover:bg-blue-600 transition-colors duration-200"
          :disabled="!fileContent"
        >
          Create Set
        </button>
      </div>
    </div>
  </div>
</template> 