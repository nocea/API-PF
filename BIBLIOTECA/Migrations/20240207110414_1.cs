using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BIBLIOTECA.Migrations
{
    /// <inheritdoc />
    public partial class _1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tokens",
                columns: table => new
                {
                    id_token = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    cadena_token = table.Column<string>(type: "text", nullable: true),
                    fechaFin_token = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tokens", x => x.id_token);
                });

            migrationBuilder.CreateTable(
                name: "usuarios",
                columns: table => new
                {
                    id_usuario = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombreCompleto_usuario = table.Column<string>(type: "text", nullable: true),
                    rol_usuario = table.Column<string>(type: "text", nullable: true),
                    movil_usuario = table.Column<int>(type: "integer", nullable: true),
                    alias_usuario = table.Column<string>(type: "text", nullable: true),
                    email_usuario = table.Column<string>(type: "text", nullable: true),
                    passwd_usuario = table.Column<string>(type: "text", nullable: true),
                    token_usuario = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuarios", x => x.id_usuario);
                });

            migrationBuilder.InsertData(
                table: "usuarios",
                columns: new[] { "id_usuario", "alias_usuario", "email_usuario", "movil_usuario", "nombreCompleto_usuario", "passwd_usuario", "rol_usuario", "token_usuario" },
                values: new object[] { 1, null, "admin@admin", null, "Administrador", "eb31b34db2f22a0030aa4f9306b77bfbbda728967e9feb88ea79ac206e657d29", "ADMIN", null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tokens");

            migrationBuilder.DropTable(
                name: "usuarios");
        }
    }
}
