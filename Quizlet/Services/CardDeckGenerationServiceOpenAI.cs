using System.Threading.Tasks;
using Quizlet.Interfaces;
using Quizlet.Models;
using OpenAI.Chat;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Schema;
using System.Text;

namespace Quizlet.Services;

public class CardDeckGenerationServiceOpenAI : ICardDeckGenerationService
{

  private readonly ChatClient _client;
  private readonly ChatResponseFormat _chatResponseFormat;
  private readonly string _systemMessage = "You are a helpful assistant that generates flashcards from text. Generate a card deck with a name and 10 cards, each with a front and back.";

  private readonly CardDeck _fallbackCardDeck = new()
  {
    Title = "Empty Card Deck",
    Cards = new List<Card>{}
  };

  public CardDeckGenerationServiceOpenAI()
  {
    // Initialize the OpenAI client
    _client = new ChatClient(model: "gpt-4o", apiKey: Environment.GetEnvironmentVariable("OPENAI_API_KEY"));

    // Generate a JSON schema for the CardDeck class
    JsonSerializerOptions options = JsonSerializerOptions.Default;
    JsonSchemaExporterOptions exporterOptions = new()
    {
      TreatNullObliviousAsNonNullable = true,
    };
    JsonNode jsonSchema = options.GetJsonSchemaAsNode(typeof(CardDeck), exporterOptions);
    BinaryData jsonSchemaBinary = BinaryData.FromBytes(Encoding.UTF8.GetBytes(jsonSchema.ToJsonString()));
    _chatResponseFormat = ChatResponseFormat.CreateJsonSchemaFormat(jsonSchemaFormatName: "json_schema", jsonSchema: jsonSchemaBinary);
  }


  public async Task<CardDeck> GenerateCardsFromTextAsync(string content)
  {

    // Generate a prompt for the OpenAI API
    List<ChatMessage> messages = [
        new SystemChatMessage(_systemMessage),
        new UserChatMessage(content)
    ];

    // Generate a completion for the OpenAI API
    ChatCompletion completion = await _client.CompleteChatAsync(
      messages,
      options: new ChatCompletionOptions
      {
        ResponseFormat = _chatResponseFormat
      }
    );

    // Parse the completion into a CardDeck
    using JsonDocument structuredJson = JsonDocument.Parse(completion.Content[0].Text);
    return JsonSerializer.Deserialize<CardDeck>(structuredJson) ?? _fallbackCardDeck;
  }
}
