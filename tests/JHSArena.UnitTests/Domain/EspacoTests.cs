using JHSArena.Domain.Entities;
using JHSArena.Domain.Enums;
using JHSArena.Domain.Exceptions;

namespace JHSArena.UnitTests.Domain;

public sealed class EspacoTests
{
    [Fact]
    public void CriarEspaco_ComDadosValidos_DeveCriarAtivo()
    {
        var espaco = CriarEspaco();

        Assert.NotEqual(Guid.Empty, espaco.Id);
        Assert.Equal("Quadra de Tênis 1", espaco.Nome);
        Assert.Equal(TipoEspaco.QuadraTenis, espaco.Tipo);
        Assert.Equal(StatusEspaco.Ativo, espaco.Status);
        Assert.True(espaco.EstaDisponivelParaReserva);
        Assert.Equal(60, espaco.TempoMinimoReservaMinutos);
    }

    [Fact]
    public void CriarEspaco_SemNome_DeveFalhar()
    {
        var excecao = Assert.Throws<DomainException>(
            () => new Espaco(
                Guid.NewGuid(),
                Guid.NewGuid(),
                string.Empty,
                TipoEspaco.QuadraTenis,
                4,
                80m,
                new TimeOnly(7, 0),
                new TimeOnly(22, 0)));

        Assert.Equal(
            "O nome do espaço é obrigatório.",
            excecao.Message);
    }

    [Fact]
    public void CriarEspaco_ComValorNegativo_DeveFalhar()
    {
        Assert.Throws<DomainException>(
            () => new Espaco(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Quadra 1",
                TipoEspaco.QuadraTenis,
                4,
                -10m,
                new TimeOnly(7, 0),
                new TimeOnly(22, 0)));
    }

    [Fact]
    public void CriarEspaco_ComHorarioInvalido_DeveFalhar()
    {
        Assert.Throws<DomainException>(
            () => new Espaco(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Quadra 1",
                TipoEspaco.QuadraTenis,
                4,
                80m,
                new TimeOnly(22, 0),
                new TimeOnly(7, 0)));
    }

    [Fact]
    public void ColocarEmManutencao_DeveImpedirReserva()
    {
        var espaco = CriarEspaco();

        espaco.ColocarEmManutencao();

        Assert.Equal(StatusEspaco.EmManutencao, espaco.Status);
        Assert.False(espaco.EstaDisponivelParaReserva);
    }

    [Fact]
    public void LiberarParaUso_DeveAtivarEspaco()
    {
        var espaco = CriarEspaco();
        espaco.ColocarEmManutencao();

        espaco.LiberarParaUso();

        Assert.Equal(StatusEspaco.Ativo, espaco.Status);
        Assert.True(espaco.Ativo);
        Assert.True(espaco.EstaDisponivelParaReserva);
    }

    [Fact]
    public void FuncionaNoHorario_DentroDoExpediente_DeveRetornarVerdadeiro()
    {
        var espaco = CriarEspaco();

        var resultado = espaco.FuncionaNoHorario(
            new TimeOnly(18, 0),
            new TimeOnly(19, 0));

        Assert.True(resultado);
    }

    [Fact]
    public void FuncionaNoHorario_ForaDoExpediente_DeveRetornarFalso()
    {
        var espaco = CriarEspaco();

        var resultado = espaco.FuncionaNoHorario(
            new TimeOnly(21, 30),
            new TimeOnly(22, 30));

        Assert.False(resultado);
    }

    [Fact]
    public void DefinirCorAgenda_ComFormatoValido_DeveSalvarCor()
    {
        var espaco = CriarEspaco();

        espaco.DefinirCorAgenda("#0ea5e9");

        Assert.Equal("#0EA5E9", espaco.CorAgenda);
    }

    [Fact]
    public void DefinirCorAgenda_ComFormatoInvalido_DeveFalhar()
    {
        var espaco = CriarEspaco();

        Assert.Throws<DomainException>(
            () => espaco.DefinirCorAgenda("azul"));
    }

    private static Espaco CriarEspaco()
    {
        return new Espaco(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "Quadra de Tênis 1",
            TipoEspaco.QuadraTenis,
            4,
            80m,
            new TimeOnly(7, 0),
            new TimeOnly(22, 0));
    }
}
