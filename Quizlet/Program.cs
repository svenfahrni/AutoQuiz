using Microsoft.OpenApi.Models;
using Quizlet.Interfaces;
using Quizlet.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "AutoQuiz API", Description = "Creating quizzes from your Slides.", Version = "v1" });
});

// Register Services
builder.Services.AddScoped<IFileService, FileService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "AutoQuiz API V1");
    });
}

app.MapGet("/", () => "Hello World!");

app.UseRouting();
app.UseAuthorization();
app.MapControllers();
app.Run();