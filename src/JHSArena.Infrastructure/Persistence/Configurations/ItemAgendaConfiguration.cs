using JHSArena.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JHSArena.Infrastructure.Persistence.Configurations;

/// <summary>
/// Define como a entidade ItemAgenda será armazenada no PostgreSQL.
/// </summary>
public sealed class ItemAgendaConfiguration
    : IEntityTypeConfiguration<ItemAgenda>
{
    public void Configure(EntityTypeBuilder<ItemAgenda> builder)
    {
        builder.ToTable("itens_agenda");

        builder.HasKey(item => item.Id);

        builder.Property(item => item.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(item => item.EmpresaId)
            .HasColumnName("empresa_id")
            .IsRequired();

        builder.Property(item => item.UnidadeId)
            .HasColumnName("unidade_id")
            .IsRequired();

        builder.Property(item => item.EspacoId)
            .HasColumnName("espaco_id")
            .IsRequired();

        builder.Property(item => item.ClienteId)
            .HasColumnName("cliente_id");

        builder.Property(item => item.Tipo)
            .HasColumnName("tipo")
            .HasConversion<int>()
            .IsRequired();

        builder.Property(item => item.Status)
            .HasColumnName("status")
            .HasConversion<int>()
            .IsRequired();

        builder.Property(item => item.Titulo)
            .HasColumnName("titulo")
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(item => item.Descricao)
            .HasColumnName("descricao")
            .HasMaxLength(1000);

        builder.Property(item => item.Inicio)
            .HasColumnName("inicio")
            .HasColumnType("timestamp with time zone")
            .IsRequired();

        builder.Property(item => item.Fim)
            .HasColumnName("fim")
            .HasColumnType("timestamp with time zone")
            .IsRequired();

        builder.Property(item => item.CanceladoEm)
            .HasColumnName("cancelado_em")
            .HasColumnType("timestamp with time zone");

        builder.Property(item => item.CanceladoPor)
            .HasColumnName("cancelado_por");

        builder.Property(item => item.MotivoCancelamento)
            .HasColumnName("motivo_cancelamento")
            .HasMaxLength(500);

        builder.Property(item => item.CriadoEm)
            .HasColumnName("criado_em")
            .HasColumnType("timestamp with time zone")
            .IsRequired();

        builder.Property(item => item.AtualizadoEm)
            .HasColumnName("atualizado_em")
            .HasColumnType("timestamp with time zone");

        builder.Property(item => item.CriadoPor)
            .HasColumnName("criado_por");

        builder.Property(item => item.AtualizadoPor)
            .HasColumnName("atualizado_por");

        builder.Property(item => item.Ativo)
            .HasColumnName("ativo")
            .IsRequired();

        builder.Ignore(item => item.Duracao);

        builder.HasIndex(item => new
        {
            item.EmpresaId,
            item.EspacoId,
            item.Inicio,
            item.Fim
        })
            .HasDatabaseName(
                "ix_itens_agenda_empresa_espaco_periodo");

        builder.HasIndex(item => new
        {
            item.EmpresaId,
            item.UnidadeId,
            item.Inicio
        })
            .HasDatabaseName(
                "ix_itens_agenda_empresa_unidade_inicio");

        builder.HasIndex(item => new
        {
            item.EmpresaId,
            item.Status
        })
            .HasDatabaseName(
                "ix_itens_agenda_empresa_status");
    }
}
