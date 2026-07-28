using JHSArena.Application.Features.Espacos.DTOs;
using JHSArena.Application.Interfaces.Repositories;
using JHSArena.Domain.Entities;
using JHSArena.Domain.Exceptions;

namespace JHSArena.Application.Features.Espacos.Commands.CriarEspaco;

public sealed class CriarEspacoHandler
{
    private readonly IEspacoRepository _espacoRepository;

    public CriarEspacoHandler(IEspacoRepository espacoRepository)
    {
        _espacoRepository = espacoRepository;
    }

    public async Task<EspacoDto> HandleAsync(
        CriarEspacoCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command.EmpresaId == Guid.Empty)
        {
            throw new DomainException("A empresa é obrigatória.");
        }

        if (command.UnidadeId == Guid.Empty)
        {
            throw new DomainException("A unidade é obrigatória.");
        }

        var nomeJaUtilizado =
            await _espacoRepository.ExisteNomeAsync(
                command.EmpresaId,
                command.UnidadeId,
                command.Nome.Trim(),
                cancellationToken: cancellationToken);

        if (nomeJaUtilizado)
        {
            throw new DomainException(
                "Já existe um espaço com esse nome na unidade.");
        }

        var espaco = new Espaco(
            command.EmpresaId,
            command.UnidadeId,
            command.Nome,
            command.Tipo,
            command.Capacidade,
            command.ValorHora,
            command.HorarioAbertura,
            command.HorarioFechamento);

        espaco.DefinirTempoMinimoReserva(
            command.TempoMinimoReservaMinutos);

        espaco.AlterarDescricao(command.Descricao);
        espaco.DefinirCorAgenda(command.CorAgenda);
        espaco.AlterarObservacoes(command.Observacoes);

        espaco.ConfigurarRecursos(
            command.PermiteAula,
            command.PermiteRanking,
            command.PermiteDayUse,
            command.PermiteReservaOnline);

        await _espacoRepository.AdicionarAsync(
            espaco,
            cancellationToken);

        await _espacoRepository.SalvarAlteracoesAsync(
            cancellationToken);

        return EspacoDto.FromEntity(espaco);
    }
}
