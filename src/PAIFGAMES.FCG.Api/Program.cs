using PAIFGAMES.FCG.Api.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.AddLoggerConfig();
builder.Services.AddApiConfig(builder.Configuration);
builder.Services.AddSwaggerConfig();
builder.Services.ResolveDependencies(builder.Configuration);
builder.Services.AddDataConfigurations(builder.Configuration);
builder.Services.AddAuthConfiguration(builder.Configuration);

var app = builder.Build();

app.UseApiConfiguration(app.Environment);
app.UseSwaggerConfig();
app.ApplyMigrations();
app.Run();

