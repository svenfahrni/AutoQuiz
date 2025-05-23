using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Text.Json;
using Quizlet.Models;

namespace Quizlet.Tests;

public class UploadTests() : IClassFixture<WebApplicationFactory<Program>>
{
    public class FileControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public FileControllerTests()
        {
            _factory = TestSetup.CreateTestApplication();
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task UploadFile_ShouldReturnCards_WhenTxtFileIsProvided()
        {
            // Create fake file content
            var text = "Hello, world!";
            var content = new MultipartFormDataContent();
            var fileContent = new ByteArrayContent(Encoding.UTF8.GetBytes(text));
            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("text/plain");
            content.Add(fileContent, "formFile", "test.txt"); // Key name updated to match the controller parameter

            // Act
            var response = await _client.PostAsync("/api/files/", content);
            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<CardDeck>(responseContent);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.NotNull(result);
            Assert.NotNull(result?.Title);
            Assert.Equal("Test Deck", result?.Title);
            Assert.Equal(2, result?.Cards.Count);
        }

        [Fact]
        public async Task UploadFile_ShouldReturnBadRequest_WhenFileIsNotProvided()
        {
            // Create empty content
            var content = new MultipartFormDataContent();

            // Act
            var response = await _client.PostAsync("/api/files/", content);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task UploadFile_ShouldReturnBadRequest_WhenFileTypeIsNotAllowed()
        {
            // Create fake file content
            var text = "Hello, world!";
            var content = new MultipartFormDataContent();
            var fileContent = new ByteArrayContent(Encoding.UTF8.GetBytes(text));
            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/octet-stream");
            content.Add(fileContent, "formFile", "test.exe"); // Invalid file type

            // Act
            var response = await _client.PostAsync("/api/files/", content);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}