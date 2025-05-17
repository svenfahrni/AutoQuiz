using System.Threading.Tasks;
using Quizlet.Models;

namespace Quizlet.Interfaces
{
    public interface ICardDeckGenerationService
    {
        /// <summary>
        /// Generates a card deck from the provided text content
        /// </summary>
        /// <param name="content">The text content to generate cards from</param>
        /// <returns>A CardDeck containing the generated cards</returns>
        Task<CardDeck> GenerateCardsFromTextAsync(string content);
    }
}
