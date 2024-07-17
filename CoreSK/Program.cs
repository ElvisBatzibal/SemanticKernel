using CoreSK.API.Services;
using Microsoft.SemanticKernel;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddKernel()
    .AddOpenAIChatCompletion("gpt-4", builder.Configuration["AI:OpenAI:ApiKey"])
    .AddOpenAITextEmbeddingGeneration("text-embedding-3-small", builder.Configuration["AI:OpenAI:ApiKey"]);

builder.Services.AddTransient<IOpenAIService, OpenAIService>();


// Configurar política de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins("https://localhost:7168") // Agrega la URL de tu aplicación cliente aquí
                          .AllowAnyHeader()
                          .AllowAnyMethod());
});


builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
// Usar la política de CORS
app.UseCors("AllowSpecificOrigin");
app.MapControllers();
// Configurar el servidor Kestrel para escuchar en el puerto 7009
//app.Urls.Add("http://localhost:7009");
app.Run();

