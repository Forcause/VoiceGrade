using System.Reflection;
using Microsoft.OpenApi.Models;
using VoiceGradeApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "0.0.1",
        Title = "VoiceGradeApi v0.0.1",
        Description = "API для выставления оценок ученикам с использованием распознавания речи."
    });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

builder.Services.AddSingleton<TranscriberService>();
builder.Services.AddTransient<ProcessingService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1 API"));
    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();
}

app.Run();