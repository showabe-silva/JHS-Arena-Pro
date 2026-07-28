using JHSArena.Domain.Entities;
using JHSArena.Domain.Enums;

namespace JHSArena.Application.Features.Espacos.DTOs;

public sealed record EspacoDto(
    Guid Id,
    Guid EmpresaId,
    Guid UnidadeId,
    string Nome,
    TipoEspaco Tipo,
    string? Descricao,
    int Capacidade,
    decimal ValorHora,
    int TempoMinimoReservaMinutos,
    TimeOnly HorarioAbertura,
    TimeOnly HorarioFechamento,
    string? CorAgenda,
    StatusEspaco Status,
    bool PermiteAula,
    bool PermiteRanking,
    bool PermiteDayUse,
    bool PermiteReservaOnline,
    string? Observacoes,
    bool EstaDisponivelParaReserva)
{
    public static EspacoDto FromEntity(Espaco espaco)
    {
        return new EspacoDto(
            espaco.Id,
            espaco.EmpresaId,
            espaco.UnidadeId,
            espaco.Nome,
            espaco.Tipo,
            espaco.Descricao,
            espaco.Capacidade,
            espaco.ValorHora,
            espaco.TempoMinimoReservaMinutos,
            espaco.HorarioAbertura,
            espaco.HorarioFechamento,
            espaco.CorAgenda,
            espaco.Status,
            espaco.PermiteAula,
            espaco.PermiteRanking,
            espaco.PermiteDayUse,
            espaco.PermiteReservaOnline,
            espaco.Observacoes,
            espaco.EstaDisponivelParaReserva);
    }
}
