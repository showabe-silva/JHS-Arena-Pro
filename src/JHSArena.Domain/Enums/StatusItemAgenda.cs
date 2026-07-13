namespace JHSArena.Domain.Enums;

/// <summary>
/// Define o estado atual de um item da agenda.
/// </summary>
public enum StatusItemAgenda
{
    PreReserva = 1,
    AguardandoConfirmacao = 2,
    Confirmado = 3,
    EmAndamento = 4,
    Concluido = 5,
    Cancelado = 6,
    NaoCompareceu = 7,
    Bloqueado = 8
}
