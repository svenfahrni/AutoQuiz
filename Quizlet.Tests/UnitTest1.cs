namespace Quizlet.Tests;

using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;

public class Example
{
    [Fact]
    public async Task GET_returns_ok()
    {
        await using var application = new WebApplicationFactory<Program>();
        using var client = application.CreateClient();
 
        var response = await client.GetAsync("/");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadAsStringAsync();
        content.Should().Be("Hello World!");
    }
}