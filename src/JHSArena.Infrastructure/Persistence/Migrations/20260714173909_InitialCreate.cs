using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JHSArena.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "espacos",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    unidade_id = table.Column<Guid>(type: "uuid", nullable: false),
                    nome = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    tipo = table.Column<int>(type: "integer", nullable: false),
                    descricao = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    capacidade = table.Column<int>(type: "integer", nullable: false),
                    valor_hora = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: false),
                    tempo_minimo_reserva_minutos = table.Column<int>(type: "integer", nullable: false),
                    horario_abertura = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    horario_fechamento = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    cor_agenda = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: true),
                    status = table.Column<int>(type: "integer", nullable: false),
                    permite_aula = table.Column<bool>(type: "boolean", nullable: false),
                    permite_ranking = table.Column<bool>(type: "boolean", nullable: false),
                    permite_day_use = table.Column<bool>(type: "boolean", nullable: false),
                    permite_reserva_online = table.Column<bool>(type: "boolean", nullable: false),
                    observacoes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    criado_em = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    atualizado_em = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    criado_por = table.Column<Guid>(type: "uuid", nullable: true),
                    atualizado_por = table.Column<Guid>(type: "uuid", nullable: true),
                    ativo = table.Column<bool>(type: "boolean", nullable: false),
                    empresa_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_espacos", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "itens_agenda",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    unidade_id = table.Column<Guid>(type: "uuid", nullable: false),
                    espaco_id = table.Column<Guid>(type: "uuid", nullable: false),
                    cliente_id = table.Column<Guid>(type: "uuid", nullable: true),
                    tipo = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    titulo = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    descricao = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    inicio = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    fim = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    cancelado_em = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    cancelado_por = table.Column<Guid>(type: "uuid", nullable: true),
                    motivo_cancelamento = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    criado_em = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    atualizado_em = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    criado_por = table.Column<Guid>(type: "uuid", nullable: true),
                    atualizado_por = table.Column<Guid>(type: "uuid", nullable: true),
                    ativo = table.Column<bool>(type: "boolean", nullable: false),
                    empresa_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_itens_agenda", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_espacos_empresa_status",
                table: "espacos",
                columns: new[] { "empresa_id", "status" });

            migrationBuilder.CreateIndex(
                name: "ux_espacos_empresa_unidade_nome",
                table: "espacos",
                columns: new[] { "empresa_id", "unidade_id", "nome" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_itens_agenda_empresa_espaco_periodo",
                table: "itens_agenda",
                columns: new[] { "empresa_id", "espaco_id", "inicio", "fim" });

            migrationBuilder.CreateIndex(
                name: "ix_itens_agenda_empresa_status",
                table: "itens_agenda",
                columns: new[] { "empresa_id", "status" });

            migrationBuilder.CreateIndex(
                name: "ix_itens_agenda_empresa_unidade_inicio",
                table: "itens_agenda",
                columns: new[] { "empresa_id", "unidade_id", "inicio" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "espacos");

            migrationBuilder.DropTable(
                name: "itens_agenda");
        }
    }
}
