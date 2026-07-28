using JHSArena.Application.Features.Espacos.DTOs;
using JHSArena.Application.Interfaces.Repositories;
using JHSArena.Domain.Entities;
using JHSArena.Domain.Exceptions;

namespace JHSArena.Application.Features.Espacos.Commands.AlterarStatusEspaco;

public sealed class AlterarStatusEspacoHandler
{
    private readonly IEspacoRepository _espacoRepository;

    public AlterarStatusEspacoHandler(
        IEspacoRepository espacoRepository)
    {
        _espacoRepository = espacoRepository;
    }

    public Task<EspacoDto> ColocarEmManutencaoAsync(
        Guid empresaId,
        Guid espacoId,
        CancellationToken cancellationToken = default)
    {
        return AlterarAsync(
            empresaId,
            espacoId,
            espaco => espaco.ColocarEmManutencao(),
            cancellationToken);
    }

    public Task<EspacoDto> LiberarParaUsoAsync(
        Guid empresaId,
        Guid espacoId,
        CancellationToken cancellationToken = default)
    {
        return AlterarAsync(
            empresaId,
            espacoId,
            espaco => espaco.LiberarParaUso(),
            cancellationToken);
    }

    public Task<EspacoDto> DesativarAsync(
        Guid empresaId,
        Guid espacoId,
        CancellationToken cancellationToken = default)
    {
        return AlterarAsync(
            empresaId,
            espacoId,
            espaco => espaco.Desativar(),
            cancellationToken);
    }

    private async Task<EspacoDto> AlterarAsync(
        Guid empresaId,
        Guid espacoId,
        Action<Espaco> alteracao,
        CancellationToken cancellationToken)
    {
        var espaco = await _espacoRepository.ObterPorIdAsync(
            empresaId,
            espacoId,
            cancellationToken);

        if (espaco is null)
        {
            throw new DomainException("Espaço não encontrado.");
        }

        alteracao(espaco);

        await _espacoRepository.SalvarAlteracoesAsync(
            cancellationToken);

        return EspacoDto.FromEntity(espaco);
    }
}
