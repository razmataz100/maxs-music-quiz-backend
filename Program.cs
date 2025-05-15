using MaxsMusicQuiz.Backend.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddDatabase()
    .AddCorsPolicy()
    .AddApplicationServices()
    .AddHttpClient()
    .AddJwtAuthentication(builder.Configuration)
    .AddApiServices();

var app = builder.Build();

app.ConfigurePipeline();

app.Run();