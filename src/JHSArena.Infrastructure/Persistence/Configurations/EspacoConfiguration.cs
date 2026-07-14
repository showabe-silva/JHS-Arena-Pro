using JHSArena.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JHSArena.Infrastructure.Persistence.Configurations;

/// <summary>
/// Define como a entidade Espaco será armazenada no PostgreSQL.
/// </summary>
public sealed class EspacoConfiguration : IEntityTypeConfiguration<Espaco>
{
    public void Configure(EntityTypeBuilder<Espaco> builder)
    {
        builder.ToTable("espacos");

        builder.HasKey(espaco => espaco.Id);

        builder.Property(espaco => espaco.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(espaco => espaco.EmpresaId)
            .HasColumnName("empresa_id")
            .IsRequired();

        builder.Property(espaco => espaco.UnidadeId)
            .HasColumnName("unidade_id")
            .IsRequired();

        builder.Property(espaco => espaco.Nome)
            .HasColumnName("nome")
            .HasMaxLength(120)
            .IsRequired();

        builder.Property(espaco => espaco.Tipo)
            .HasColumnName("tipo")
            .HasConversion<int>()
            .IsRequired();

        builder.Property(espaco => espaco.Descricao)
            .HasColumnName("descricao")
            .HasMaxLength(500);

        builder.Property(espaco => espaco.Capacidade)
            .HasColumnName("capacidade")
            .IsRequired();

        builder.Property(espaco => espaco.ValorHora)
            .HasColumnName("valor_hora")
            .HasPrecision(12, 2)
            .IsRequired();

        builder.Property(espaco => espaco.TempoMinimoReservaMinutos)
            .HasColumnName("tempo_minimo_reserva_minutos")
            .IsRequired();

        builder.Property(espaco => espaco.HorarioAbertura)
            .HasColumnName("horario_abertura")
            .HasColumnType("time without time zone")
            .IsRequired();

        builder.Property(espaco => espaco.HorarioFechamento)
            .HasColumnName("horario_fechamento")
            .HasColumnType("time without time zone")
            .IsRequired();

        builder.Property(espaco => espaco.CorAgenda)
            .HasColumnName("cor_agenda")
            .HasMaxLength(7);

        builder.Property(espaco => espaco.Status)
            .HasColumnName("status")
            .HasConversion<int>()
            .IsRequired();

        builder.Property(espaco => espaco.PermiteAula)
            .HasColumnName("permite_aula")
            .IsRequired();

        builder.Property(espaco => espaco.PermiteRanking)
            .HasColumnName("permite_ranking")
            .IsRequired();

        builder.Property(espaco => espaco.PermiteDayUse)
            .HasColumnName("permite_day_use")
            .IsRequired();

        builder.Property(espaco => espaco.PermiteReservaOnline)
            .HasColumnName("permite_reserva_online")
            .IsRequired();

        builder.Property(espaco => espaco.Observacoes)
            .HasColumnName("observacoes")
            .HasMaxLength(1000);

        builder.Property(espaco => espaco.CriadoEm)
            .HasColumnName("criado_em")
            .IsRequired();

        builder.Property(espaco => espaco.AtualizadoEm)
            .HasColumnName("atualizado_em");

        builder.Property(espaco => espaco.CriadoPor)
            .HasColumnName("criado_por");

        builder.Property(espaco => espaco.AtualizadoPor)
            .HasColumnName("atualizado_por");

        builder.Property(espaco => espaco.Ativo)
            .HasColumnName("ativo")
            .IsRequired();

        builder.Ignore(espaco => espaco.EstaDisponivelParaReserva);

        builder.HasIndex(espaco => new
        {
            espaco.EmpresaId,
            espaco.UnidadeId,
            espaco.Nome
        })
            .IsUnique()
            .HasDatabaseName("ux_espacos_empresa_unidade_nome");

        builder.HasIndex(espaco => new
        {
            espaco.EmpresaId,
            espaco.Status
        })
            .HasDatabaseName("ix_espacos_empresa_status");
    }
}
