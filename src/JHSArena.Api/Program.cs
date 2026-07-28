using JHSArena.Application.Features.Espacos.Commands.AlterarStatusEspaco;
using JHSArena.Application.Features.Espacos.Commands.CriarEspaco;
using JHSArena.Application.Features.Espacos.Queries;
using JHSArena.Application.Interfaces.Repositories;
using JHSArena.Infrastructure.Persistence;
using JHSArena.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString =
    builder.Configuration.GetConnectionString("JHSArenaDatabase")
    ?? throw new InvalidOperationException(
        "A string de conexão 'JHSArenaDatabase' não foi encontrada.");

builder.Services.AddDbContext<JHSArenaDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<IEspacoRepository, EspacoRepository>();

builder.Services.AddScoped<CriarEspacoHandler>();
builder.Services.AddScoped<ObterEspacoPorIdHandler>();
builder.Services.AddScoped<ListarEspacosHandler>();
builder.Services.AddScoped<AlterarStatusEspacoHandler>();

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();

app.MapGet("/", () => Results.Ok(new
{
    application = "JHS Arena Pro API",
    status = "online"
}));

app.Run();
