using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace Quizlet.Models
{
    public class CardDeck
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("cards")]
        public List<Card> Cards { get; set; }

        public CardDeck()
        {
            Title = string.Empty;
            Cards = new List<Card>();
        }
    }
}