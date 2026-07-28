using JHSArena.Application.Interfaces.Repositories;
using JHSArena.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace JHSArena.Infrastructure.Persistence.Repositories;

/// <summary>
/// Implementa as operações de persistência do módulo de espaços
/// utilizando Entity Framework Core e PostgreSQL.
/// </summary>
public sealed class EspacoRepository : IEspacoRepository
{
    private readonly JHSArenaDbContext _dbContext;

    public EspacoRepository(JHSArenaDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<bool> ExisteNomeAsync(
        Guid empresaId,
        Guid unidadeId,
        string nome,
        Guid? ignorarEspacoId = null,
        CancellationToken cancellationToken = default)
    {
        var nomeNormalizado = nome.Trim().ToUpper();

        return _dbContext.Espacos.AnyAsync(
            espaco =>
                espaco.EmpresaId == empresaId &&
                espaco.UnidadeId == unidadeId &&
                espaco.Nome.ToUpper() == nomeNormalizado &&
                (!ignorarEspacoId.HasValue ||
                 espaco.Id != ignorarEspacoId.Value),
            cancellationToken);
    }

    public Task<Espaco?> ObterPorIdAsync(
        Guid empresaId,
        Guid espacoId,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.Espacos
            .FirstOrDefaultAsync(
                espaco =>
                    espaco.EmpresaId == empresaId &&
                    espaco.Id == espacoId,
                cancellationToken);
    }

    public async Task<IReadOnlyList<Espaco>> ListarAsync(
        Guid empresaId,
        Guid? unidadeId = null,
        CancellationToken cancellationToken = default)
    {
        var consulta = _dbContext.Espacos
            .AsNoTracking()
            .Where(espaco => espaco.EmpresaId == empresaId);

        if (unidadeId.HasValue)
        {
            consulta = consulta.Where(
                espaco => espaco.UnidadeId == unidadeId.Value);
        }

        return await consulta
            .OrderBy(espaco => espaco.Nome)
            .ToListAsync(cancellationToken);
    }

    public Task AdicionarAsync(
        Espaco espaco,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.Espacos.AddAsync(
            espaco,
            cancellationToken).AsTask();
    }

    public Task SalvarAlteracoesAsync(
        CancellationToken cancellationToken = default)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}
