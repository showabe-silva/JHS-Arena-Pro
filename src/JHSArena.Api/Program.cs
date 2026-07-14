using JHSArena.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString =
    builder.Configuration.GetConnectionString("JHSArenaDatabase")
    ?? throw new InvalidOperationException(
        "A string de conexão 'JHSArenaDatabase' não foi encontrada.");

builder.Services.AddDbContext<JHSArenaDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/", () => Results.Ok(new
{
    application = "JHS Arena Pro API",
    status = "online"
}));

app.Run();
