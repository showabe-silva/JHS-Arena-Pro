using JHSArena.Domain.Common;
using JHSArena.Domain.Enums;
using JHSArena.Domain.Exceptions;

namespace JHSArena.Domain.Entities;

/// <summary>
/// Representa um espaço físico que pode ser reservado,
/// como uma quadra de tênis, quadra de beach tennis ou estúdio.
/// </summary>
public sealed class Espaco : EmpresaEntity
{
    private Espaco()
    {
    }

    public Espaco(
        Guid empresaId,
        Guid unidadeId,
        string nome,
        TipoEspaco tipo,
        int capacidade,
        decimal valorHora,
        TimeOnly horarioAbertura,
        TimeOnly horarioFechamento)
        : base(empresaId)
    {
        ValidarIdentificador(unidadeId, "A unidade é obrigatória.");
        ValidarNome(nome);
        ValidarCapacidade(capacidade);
        ValidarValor(valorHora);
        ValidarHorario(horarioAbertura, horarioFechamento);

        UnidadeId = unidadeId;
        Nome = nome.Trim();
        Tipo = tipo;
        Capacidade = capacidade;
        ValorHora = valorHora;
        HorarioAbertura = horarioAbertura;
        HorarioFechamento = horarioFechamento;
        TempoMinimoReservaMinutos = 60;
        Status = StatusEspaco.Ativo;
        PermiteAula = true;
        PermiteRanking = true;
        PermiteReservaOnline = false;
        PermiteDayUse = false;
    }

    public Guid UnidadeId { get; private set; }

    public string Nome { get; private set; } = string.Empty;

    public TipoEspaco Tipo { get; private set; }

    public string? Descricao { get; private set; }

    public int Capacidade { get; private set; }

    public decimal ValorHora { get; private set; }

    public int TempoMinimoReservaMinutos { get; private set; }

    public TimeOnly HorarioAbertura { get; private set; }

    public TimeOnly HorarioFechamento { get; private set; }

    public string? CorAgenda { get; private set; }

    public StatusEspaco Status { get; private set; }

    public bool PermiteAula { get; private set; }

    public bool PermiteRanking { get; private set; }

    public bool PermiteDayUse { get; private set; }

    public bool PermiteReservaOnline { get; private set; }

    public string? Observacoes { get; private set; }

    public bool EstaDisponivelParaReserva =>
        Ativo && Status == StatusEspaco.Ativo;

    public void AlterarDadosBasicos(
        string nome,
        TipoEspaco tipo,
        int capacidade,
        decimal valorHora)
    {
        ValidarNome(nome);
        ValidarCapacidade(capacidade);
        ValidarValor(valorHora);

        Nome = nome.Trim();
        Tipo = tipo;
        Capacidade = capacidade;
        ValorHora = valorHora;
        AtualizadoEm = DateTimeOffset.UtcNow;
    }

    public void AlterarHorarioFuncionamento(
        TimeOnly horarioAbertura,
        TimeOnly horarioFechamento)
    {
        ValidarHorario(horarioAbertura, horarioFechamento);

        HorarioAbertura = horarioAbertura;
        HorarioFechamento = horarioFechamento;
        AtualizadoEm = DateTimeOffset.UtcNow;
    }

    public void DefinirTempoMinimoReserva(int minutos)
    {
        if (minutos <= 0)
        {
            throw new DomainException(
                "O tempo mínimo da reserva deve ser maior que zero.");
        }

        if (minutos > 1440)
        {
            throw new DomainException(
                "O tempo mínimo da reserva não pode ultrapassar 24 horas.");
        }

        TempoMinimoReservaMinutos = minutos;
        AtualizadoEm = DateTimeOffset.UtcNow;
    }

    public void AlterarDescricao(string? descricao)
    {
        if (!string.IsNullOrWhiteSpace(descricao) &&
            descricao.Trim().Length > 500)
        {
            throw new DomainException(
                "A descrição deve possuir no máximo 500 caracteres.");
        }

        Descricao = NormalizarTextoOpcional(descricao);
        AtualizadoEm = DateTimeOffset.UtcNow;
    }

    public void AlterarObservacoes(string? observacoes)
    {
        if (!string.IsNullOrWhiteSpace(observacoes) &&
            observacoes.Trim().Length > 1000)
        {
            throw new DomainException(
                "As observações devem possuir no máximo 1000 caracteres.");
        }

        Observacoes = NormalizarTextoOpcional(observacoes);
        AtualizadoEm = DateTimeOffset.UtcNow;
    }

    public void DefinirCorAgenda(string? corHexadecimal)
    {
        if (string.IsNullOrWhiteSpace(corHexadecimal))
        {
            CorAgenda = null;
            AtualizadoEm = DateTimeOffset.UtcNow;
            return;
        }

        var cor = corHexadecimal.Trim();

        if (cor.Length != 7 ||
            cor[0] != '#' ||
            !cor.Skip(1).All(Uri.IsHexDigit))
        {
            throw new DomainException(
                "A cor da agenda deve estar no formato hexadecimal #RRGGBB.");
        }

        CorAgenda = cor.ToUpperInvariant();
        AtualizadoEm = DateTimeOffset.UtcNow;
    }

    public void ConfigurarRecursos(
        bool permiteAula,
        bool permiteRanking,
        bool permiteDayUse,
        bool permiteReservaOnline)
    {
        PermiteAula = permiteAula;
        PermiteRanking = permiteRanking;
        PermiteDayUse = permiteDayUse;
        PermiteReservaOnline = permiteReservaOnline;
        AtualizadoEm = DateTimeOffset.UtcNow;
    }

    public void ColocarEmManutencao()
    {
        Status = StatusEspaco.EmManutencao;
        AtualizadoEm = DateTimeOffset.UtcNow;
    }

    public void MarcarComoEmConstrucao()
    {
        Status = StatusEspaco.EmConstrucao;
        AtualizadoEm = DateTimeOffset.UtcNow;
    }

    public void LiberarParaUso()
    {
        Status = StatusEspaco.Ativo;
        Ativar();
    }

    public void Desativar()
    {
        Status = StatusEspaco.Inativo;
        Inativar();
    }

    public bool FuncionaNoHorario(
        TimeOnly horarioInicio,
        TimeOnly horarioFim)
    {
        if (horarioFim <= horarioInicio)
        {
            return false;
        }

        return horarioInicio >= HorarioAbertura &&
               horarioFim <= HorarioFechamento;
    }

    private static void ValidarNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
        {
            throw new DomainException("O nome do espaço é obrigatório.");
        }

        if (nome.Trim().Length > 120)
        {
            throw new DomainException(
                "O nome do espaço deve possuir no máximo 120 caracteres.");
        }
    }

    private static void ValidarCapacidade(int capacidade)
    {
        if (capacidade <= 0)
        {
            throw new DomainException(
                "A capacidade do espaço deve ser maior que zero.");
        }
    }

    private static void ValidarValor(decimal valorHora)
    {
        if (valorHora < 0)
        {
            throw new DomainException(
                "O valor por hora não pode ser negativo.");
        }
    }

    private static void ValidarHorario(
        TimeOnly horarioAbertura,
        TimeOnly horarioFechamento)
    {
        if (horarioFechamento <= horarioAbertura)
        {
            throw new DomainException(
                "O horário de fechamento deve ser posterior ao horário de abertura.");
        }
    }

    private static void ValidarIdentificador(
        Guid identificador,
        string mensagem)
    {
        if (identificador == Guid.Empty)
        {
            throw new DomainException(mensagem);
        }
    }

    private static string? NormalizarTextoOpcional(string? texto)
    {
        return string.IsNullOrWhiteSpace(texto)
            ? null
            : texto.Trim();
    }
}
