using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Quizlet.Interfaces;

namespace Quizlet.Tests
{
    public class TestSetup
    {
        public static WebApplicationFactory<Program> CreateTestApplication()
        {
            return new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        // Replace the real service with our fake service
                        services.Remove(services.First(d => d.ServiceType == typeof(ICardDeckGenerationService)));
                        services.AddScoped<ICardDeckGenerationService, FakeCardGenerator>();
                    });
                });
        }
    }
}