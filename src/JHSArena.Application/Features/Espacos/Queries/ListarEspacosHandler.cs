using JHSArena.Application.Features.Espacos.DTOs;
using JHSArena.Application.Interfaces.Repositories;

namespace JHSArena.Application.Features.Espacos.Queries;

public sealed class ListarEspacosHandler
{
    private readonly IEspacoRepository _espacoRepository;

    public ListarEspacosHandler(
        IEspacoRepository espacoRepository)
    {
        _espacoRepository = espacoRepository;
    }

    public async Task<IReadOnlyList<EspacoDto>> HandleAsync(
        Guid empresaId,
        Guid? unidadeId = null,
        CancellationToken cancellationToken = default)
    {
        var espacos = await _espacoRepository.ListarAsync(
            empresaId,
            unidadeId,
            cancellationToken);

        return espacos
            .OrderBy(espaco => espaco.Nome)
            .Select(EspacoDto.FromEntity)
            .ToList();
    }
}
