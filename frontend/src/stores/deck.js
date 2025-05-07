import { defineStore } from 'pinia'

export const useDeckStore = defineStore('deck', {
  state: () => ({
    decks: [],
    currentDeck: null
  }),

  actions: {
    addDeck(deck) {
      this.decks.push(deck)
    },
    
    setCurrentDeck(deck) {
      this.currentDeck = deck
    },

    removeDeck(deckId) {
      this.decks = this.decks.filter(deck => deck.id !== deckId)
      if (this.currentDeck?.id === deckId) {
        this.currentDeck = null
      }
    }
  },

  getters: {
    getDeckById: (state) => {
      return (id) => state.decks.find(deck => deck.id === id)
    }
  }
})
