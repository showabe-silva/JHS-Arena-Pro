using JHSArena.Domain.Enums;

namespace JHSArena.Application.Features.Espacos.Commands.CriarEspaco;

public sealed record CriarEspacoCommand(
    Guid EmpresaId,
    Guid UnidadeId,
    string Nome,
    TipoEspaco Tipo,
    int Capacidade,
    decimal ValorHora,
    TimeOnly HorarioAbertura,
    TimeOnly HorarioFechamento,
    int TempoMinimoReservaMinutos = 60,
    string? Descricao = null,
    string? CorAgenda = null,
    string? Observacoes = null,
    bool PermiteAula = true,
    bool PermiteRanking = true,
    bool PermiteDayUse = false,
    bool PermiteReservaOnline = false);
