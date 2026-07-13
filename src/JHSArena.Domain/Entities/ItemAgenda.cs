using JHSArena.Domain.Common;
using JHSArena.Domain.Enums;
using JHSArena.Domain.Exceptions;

namespace JHSArena.Domain.Entities;

/// <summary>
/// Representa uma ocupação de espaço e tempo na agenda central.
/// </summary>
public sealed class ItemAgenda : EmpresaEntity
{
    private ItemAgenda()
    {
    }

    public ItemAgenda(
        Guid empresaId,
        Guid unidadeId,
        Guid espacoId,
        TipoItemAgenda tipo,
        string titulo,
        DateTimeOffset inicio,
        DateTimeOffset fim,
        Guid? clienteId = null)
        : base(empresaId)
    {
        ValidarIdentificador(unidadeId, "A unidade é obrigatória.");
        ValidarIdentificador(espacoId, "O espaço é obrigatório.");
        ValidarTitulo(titulo);
        ValidarPeriodo(inicio, fim);

        UnidadeId = unidadeId;
        EspacoId = espacoId;
        ClienteId = clienteId;
        Tipo = tipo;
        Titulo = titulo.Trim();
        Inicio = inicio;
        Fim = fim;
        Status = StatusItemAgenda.PreReserva;
    }

    public Guid UnidadeId { get; private set; }

    public Guid EspacoId { get; private set; }

    public Guid? ClienteId { get; private set; }

    public TipoItemAgenda Tipo { get; private set; }

    public StatusItemAgenda Status { get; private set; }

    public string Titulo { get; private set; } = string.Empty;

    public string? Descricao { get; private set; }

    public DateTimeOffset Inicio { get; private set; }

    public DateTimeOffset Fim { get; private set; }

    public DateTimeOffset? CanceladoEm { get; private set; }

    public Guid? CanceladoPor { get; private set; }

    public string? MotivoCancelamento { get; private set; }

    public TimeSpan Duracao => Fim - Inicio;

    public void AlterarPeriodo(DateTimeOffset inicio, DateTimeOffset fim)
    {
        GarantirQuePodeSerAlterado();
        ValidarPeriodo(inicio, fim);

        Inicio = inicio;
        Fim = fim;
        AtualizadoEm = DateTimeOffset.UtcNow;
    }

    public void AlterarDescricao(string? descricao)
    {
        GarantirQuePodeSerAlterado();

        Descricao = string.IsNullOrWhiteSpace(descricao)
            ? null
            : descricao.Trim();

        AtualizadoEm = DateTimeOffset.UtcNow;
    }

    public void AguardarConfirmacao()
    {
        GarantirStatus(
            StatusItemAgenda.PreReserva,
            "Somente uma pré-reserva pode aguardar confirmação.");

        Status = StatusItemAgenda.AguardandoConfirmacao;
        AtualizadoEm = DateTimeOffset.UtcNow;
    }

    public void Confirmar()
    {
        if (Status is not StatusItemAgenda.PreReserva
            and not StatusItemAgenda.AguardandoConfirmacao)
        {
            throw new DomainException(
                "O item da agenda não pode ser confirmado no status atual.");
        }

        Status = StatusItemAgenda.Confirmado;
        AtualizadoEm = DateTimeOffset.UtcNow;
    }

    public void Iniciar()
    {
        GarantirStatus(
            StatusItemAgenda.Confirmado,
            "Somente um item confirmado pode ser iniciado.");

        Status = StatusItemAgenda.EmAndamento;
        AtualizadoEm = DateTimeOffset.UtcNow;
    }

    public void Concluir()
    {
        GarantirStatus(
            StatusItemAgenda.EmAndamento,
            "Somente um item em andamento pode ser concluído.");

        Status = StatusItemAgenda.Concluido;
        AtualizadoEm = DateTimeOffset.UtcNow;
    }

    public void RegistrarNaoComparecimento()
    {
        GarantirStatus(
            StatusItemAgenda.Confirmado,
            "O não comparecimento somente pode ser registrado em um item confirmado.");

        Status = StatusItemAgenda.NaoCompareceu;
        AtualizadoEm = DateTimeOffset.UtcNow;
    }

    public void Cancelar(Guid usuarioId, string motivo)
    {
        if (Status is StatusItemAgenda.Concluido
            or StatusItemAgenda.Cancelado)
        {
            throw new DomainException(
                "Um item concluído ou já cancelado não pode ser cancelado.");
        }

        if (usuarioId == Guid.Empty)
        {
            throw new DomainException(
                "O usuário responsável pelo cancelamento é obrigatório.");
        }

        if (string.IsNullOrWhiteSpace(motivo))
        {
            throw new DomainException(
                "O motivo do cancelamento é obrigatório.");
        }

        Status = StatusItemAgenda.Cancelado;
        CanceladoPor = usuarioId;
        CanceladoEm = DateTimeOffset.UtcNow;
        MotivoCancelamento = motivo.Trim();
        AtualizadoPor = usuarioId;
        AtualizadoEm = DateTimeOffset.UtcNow;
    }

    public bool Sobrepoe(DateTimeOffset inicio, DateTimeOffset fim)
    {
        ValidarPeriodo(inicio, fim);

        if (Status == StatusItemAgenda.Cancelado)
        {
            return false;
        }

        return inicio < Fim && fim > Inicio;
    }

    private void GarantirQuePodeSerAlterado()
    {
        if (Status is StatusItemAgenda.Concluido
            or StatusItemAgenda.Cancelado)
        {
            throw new DomainException(
                "Itens concluídos ou cancelados não podem ser alterados.");
        }
    }

    private void GarantirStatus(
        StatusItemAgenda statusEsperado,
        string mensagem)
    {
        if (Status != statusEsperado)
        {
            throw new DomainException(mensagem);
        }
    }

    private static void ValidarPeriodo(
        DateTimeOffset inicio,
        DateTimeOffset fim)
    {
        if (fim <= inicio)
        {
            throw new DomainException(
                "O horário final deve ser posterior ao horário inicial.");
        }
    }

    private static void ValidarTitulo(string titulo)
    {
        if (string.IsNullOrWhiteSpace(titulo))
        {
            throw new DomainException("O título é obrigatório.");
        }

        if (titulo.Trim().Length > 150)
        {
            throw new DomainException(
                "O título deve possuir no máximo 150 caracteres.");
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
}
