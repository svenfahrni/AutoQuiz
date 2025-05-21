using System.Text.Json.Serialization;

namespace Quizlet.Models
{
    public class Card
    {
        [JsonPropertyName("front")]
        public string Front { get; set; }

        [JsonPropertyName("back")]
        public string Back { get; set; }

        public Card()
        {
            Front = string.Empty;
            Back = string.Empty;
        }
    }
}
