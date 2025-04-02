using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Quizlet.Tests;

public class UploadTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task UploadFile_ShouldReturnText_WhenTxtFileIsProvided()
    {
        // Create fake file content
        var text = "Hello, world!";
        var content = new MultipartFormDataContent();
        var fileContent = new ByteArrayContent(Encoding.UTF8.GetBytes(text));
        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("text/plain");
        content.Add(fileContent, "formFile", "test.txt"); // Key name updated to match the controller parameter

        // Act
        var response = await _client.PostAsync("/api/files/", content);

        // Assert
        response.EnsureSuccessStatusCode();
        var responseText = await response.Content.ReadAsStringAsync();
        Console.WriteLine(responseText);
        Assert.Contains(text, responseText);
    }

    [Fact]
    public async Task UploadFile_ShouldReturnBadRequest_WhenFileIsNotProvided()
    {
        // Create empty content
        var content = new MultipartFormDataContent();

        // Act
        var response = await _client.PostAsync("/api/files/", content);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
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
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}