using JHSArena.Application.Features.Espacos.DTOs;
using JHSArena.Application.Interfaces.Repositories;
using JHSArena.Domain.Exceptions;

namespace JHSArena.Application.Features.Espacos.Queries;

public sealed class ObterEspacoPorIdHandler
{
    private readonly IEspacoRepository _espacoRepository;

    public ObterEspacoPorIdHandler(
        IEspacoRepository espacoRepository)
    {
        _espacoRepository = espacoRepository;
    }

    public async Task<EspacoDto> HandleAsync(
        Guid empresaId,
        Guid espacoId,
        CancellationToken cancellationToken = default)
    {
        var espaco = await _espacoRepository.ObterPorIdAsync(
            empresaId,
            espacoId,
            cancellationToken);

        if (espaco is null)
        {
            throw new DomainException("Espaço não encontrado.");
        }

        return EspacoDto.FromEntity(espaco);
    }
}
