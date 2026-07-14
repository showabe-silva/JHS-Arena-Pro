using JHSArena.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace JHSArena.Infrastructure.Persistence;

/// <summary>
/// Representa a sessão de comunicação entre o JHS Arena
/// e o banco de dados PostgreSQL.
/// </summary>
public sealed class JHSArenaDbContext : DbContext
{
    /// <summary>
    /// Recebe as configurações preparadas pela aplicação,
    /// como o provedor PostgreSQL e a string de conexão.
    /// </summary>
    public JHSArenaDbContext(
        DbContextOptions<JHSArenaDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Representa a coleção persistida de espaços.
    /// </summary>
    public DbSet<Espaco> Espacos => Set<Espaco>();

    /// <summary>
    /// Representa a coleção persistida de itens da agenda.
    /// </summary>
    public DbSet<ItemAgenda> ItensAgenda => Set<ItemAgenda>();

    /// <summary>
    /// Aplica automaticamente os mapeamentos das entidades
    /// encontrados no projeto Infrastructure.
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(JHSArenaDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
