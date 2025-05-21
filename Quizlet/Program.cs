using Microsoft.OpenApi.Models;
using Quizlet.Interfaces;
using Quizlet.Readers;
using Quizlet.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1",
        new OpenApiInfo
        { Title = "AutoQuiz API", Description = "Creating quizzes from your Slides.", Version = "v1" });
});

// Healthcheck
builder.Services.AddHealthChecks();

// Register Services
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<ICardDeckGenerationService, CardDeckGenerationServiceOpenAI>();

// Register Readers
builder.Services.AddTransient<IFileReaderStrategy, TxtFileReader>();
builder.Services.AddTransient<IFileReaderStrategy, PdfFileReader>();
builder.Services.AddTransient<IFileReaderStrategy, DocxFileReader>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "AutoQuiz API V1"); });
    app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
}

app.UseStaticFiles();
app.UseDefaultFiles();
app.MapFallbackToFile("index.html");
app.UseRouting();
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health");

await app.RunAsync();

public abstract partial class Program
{
}
