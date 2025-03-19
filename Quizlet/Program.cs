using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
    
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
     c.SwaggerDoc("v1", new OpenApiInfo { Title = "AutoQuiz API", Description = "Creating quizzes from your Slides.", Version = "v1" });
});
    
var app = builder.Build();
    
if (app.Environment.IsDevelopment())
{
   app.UseSwagger();
   app.UseSwaggerUI(c =>
   {
      c.SwaggerEndpoint("/swagger/v1/swagger.json", "AutoQuiz API V1");
   });
}
    
app.UseStaticFiles();
app.UseDefaultFiles();
app.MapFallbackToFile("index.html");
app.MapGet("/", () => "Hello World!");
    
app.Run();

public partial class Program { }