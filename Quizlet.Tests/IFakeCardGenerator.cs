
using System.Threading.Tasks;
using Quizlet.Interfaces;
using Quizlet.Models;

namespace Quizlet.Tests
{
    public class FakeCardGenerator : ICardDeckGenerationService
    {
        private readonly CardDeck _fixedResponse;

        public FakeCardGenerator()
        {
            _fixedResponse = new CardDeck
            {
                Title = "Test Deck",
                Cards = new List<Card>
                {
                    new Card { Front = "Test Question 1", Back = "Test Answer 1" },
                    new Card { Front = "Test Question 2", Back = "Test Answer 2" }
                }
            };
        }

        public Task<CardDeck> GenerateCardsFromTextAsync(string content)
        {
            return Task.FromResult(_fixedResponse);
        }
    }
}
