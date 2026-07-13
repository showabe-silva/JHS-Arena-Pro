using JHSArena.Domain.Entities;
using JHSArena.Domain.Enums;
using JHSArena.Domain.Exceptions;

namespace JHSArena.UnitTests.Domain;

public sealed class ItemAgendaTests
{
    [Fact]
    public void CriarItemAgenda_ComDadosValidos_DeveCriarPreReserva()
    {
        var inicio = DateTimeOffset.UtcNow.AddDays(1);
        var fim = inicio.AddHours(1);

        var item = CriarItem(inicio, fim);

        Assert.NotEqual(Guid.Empty, item.Id);
        Assert.Equal(StatusItemAgenda.PreReserva, item.Status);
        Assert.Equal(TimeSpan.FromHours(1), item.Duracao);
        Assert.True(item.Ativo);
    }

    [Fact]
    public void CriarItemAgenda_ComFimAnteriorAoInicio_DeveFalhar()
    {
        var inicio = DateTimeOffset.UtcNow.AddDays(1);
        var fim = inicio.AddMinutes(-30);

        var excecao = Assert.Throws<DomainException>(
            () => CriarItem(inicio, fim));

        Assert.Equal(
            "O horário final deve ser posterior ao horário inicial.",
            excecao.Message);
    }

    [Fact]
    public void Confirmar_PreReserva_DeveAlterarStatus()
    {
        var inicio = DateTimeOffset.UtcNow.AddDays(1);
        var item = CriarItem(inicio, inicio.AddHours(1));

        item.Confirmar();

        Assert.Equal(StatusItemAgenda.Confirmado, item.Status);
    }

    [Fact]
    public void Cancelar_SemMotivo_DeveFalhar()
    {
        var inicio = DateTimeOffset.UtcNow.AddDays(1);
        var item = CriarItem(inicio, inicio.AddHours(1));

        Assert.Throws<DomainException>(
            () => item.Cancelar(Guid.NewGuid(), string.Empty));
    }

    [Fact]
    public void ItemCancelado_NaoDeveGerarSobreposicao()
    {
        var inicio = DateTimeOffset.UtcNow.AddDays(1);
        var item = CriarItem(inicio, inicio.AddHours(1));

        item.Cancelar(Guid.NewGuid(), "Cliente solicitou cancelamento.");

        var sobrepoe = item.Sobrepoe(
            inicio.AddMinutes(15),
            inicio.AddMinutes(45));

        Assert.False(sobrepoe);
    }

    [Fact]
    public void PeriodosSobrepostos_DevemSerIdentificados()
    {
        var inicio = DateTimeOffset.UtcNow.AddDays(1);
        var item = CriarItem(inicio, inicio.AddHours(1));

        var sobrepoe = item.Sobrepoe(
            inicio.AddMinutes(30),
            inicio.AddMinutes(90));

        Assert.True(sobrepoe);
    }

    [Fact]
    public void PeriodosEncostados_NaoDevemSerConsideradosConflito()
    {
        var inicio = DateTimeOffset.UtcNow.AddDays(1);
        var item = CriarItem(inicio, inicio.AddHours(1));

        var sobrepoe = item.Sobrepoe(
            inicio.AddHours(1),
            inicio.AddHours(2));

        Assert.False(sobrepoe);
    }

    private static ItemAgenda CriarItem(
        DateTimeOffset inicio,
        DateTimeOffset fim)
    {
        return new ItemAgenda(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            TipoItemAgenda.ReservaQuadra,
            "Reserva da Quadra 1",
            inicio,
            fim,
            Guid.NewGuid());
    }
}
