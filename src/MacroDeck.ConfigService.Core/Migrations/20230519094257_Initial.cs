using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MacroDeck.ConfigService.Core.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "configservice");

            migrationBuilder.CreateTable(
                name: "configs",
                schema: "configservice",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    cfg_name = table.Column<string>(type: "text", nullable: false),
                    cfg_value = table.Column<string>(type: "text", nullable: false),
                    cfg_access_token = table.Column<string>(type: "text", nullable: false),
                    cfg_access_token_salt = table.Column<string>(type: "text", nullable: false),
                    cfg_version = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    created_timestamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_timestamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_configs", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_configs_cfg_name",
                schema: "configservice",
                table: "configs",
                column: "cfg_name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "configs",
                schema: "configservice");
        }
    }
}
