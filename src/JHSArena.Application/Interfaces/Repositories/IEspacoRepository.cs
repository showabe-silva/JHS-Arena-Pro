using JHSArena.Domain.Entities;

namespace JHSArena.Application.Interfaces.Repositories;

/// <summary>
/// Define as operações de persistência utilizadas pelo módulo de espaços.
/// </summary>
public interface IEspacoRepository
{
    Task<bool> ExisteNomeAsync(
        Guid empresaId,
        Guid unidadeId,
        string nome,
        Guid? ignorarEspacoId = null,
        CancellationToken cancellationToken = default);

    Task<Espaco?> ObterPorIdAsync(
        Guid empresaId,
        Guid espacoId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Espaco>> ListarAsync(
        Guid empresaId,
        Guid? unidadeId = null,
        CancellationToken cancellationToken = default);

    Task AdicionarAsync(
        Espaco espaco,
        CancellationToken cancellationToken = default);

    Task SalvarAlteracoesAsync(
        CancellationToken cancellationToken = default);
}
