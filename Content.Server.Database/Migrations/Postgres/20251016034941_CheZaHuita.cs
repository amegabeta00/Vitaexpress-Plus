using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Content.Server.Database.Migrations.Postgres
{
    /// <inheritdoc />
    public partial class CheZaHuita : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_role_whitelists_player_player_user_id",
                table: "role_whitelists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_role_whitelists",
                table: "role_whitelists");

            migrationBuilder.DropColumn(
                name: "role_id",
                table: "role_whitelists");

            migrationBuilder.RenameTable(
                name: "role_whitelists",
                newName: "role_whitelist");

            migrationBuilder.RenameColumn(
                name: "player_user_id",
                table: "role_whitelist",
                newName: "player_id");

            migrationBuilder.AddColumn<int>(
                name: "role_whitelist_id",
                table: "role_whitelist",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<DateTime>(
                name: "first_time_added",
                table: "role_whitelist",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "first_time_added_by",
                table: "role_whitelist",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "how_many_times_added",
                table: "role_whitelist",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "in_whitelist",
                table: "role_whitelist",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "last_time_added",
                table: "role_whitelist",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "last_time_added_by",
                table: "role_whitelist",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "last_time_removed",
                table: "role_whitelist",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "last_time_removed_by",
                table: "role_whitelist",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_role_whitelist",
                table: "role_whitelist",
                column: "role_whitelist_id");

            migrationBuilder.CreateTable(
                name: "role_whitelist_log",
                columns: table => new
                {
                    role_whitelist_log_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    admin_id = table.Column<Guid>(type: "uuid", nullable: false),
                    player_id = table.Column<Guid>(type: "uuid", nullable: false),
                    role_whitelist_action = table.Column<string>(type: "text", nullable: false),
                    time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_role_whitelist_log", x => x.role_whitelist_log_id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_role_whitelist_player_id",
                table: "role_whitelist",
                column: "player_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_role_whitelist_log_admin_id",
                table: "role_whitelist_log",
                column: "admin_id");

            migrationBuilder.CreateIndex(
                name: "IX_role_whitelist_log_player_id",
                table: "role_whitelist_log",
                column: "player_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "role_whitelist_log");

            migrationBuilder.DropPrimaryKey(
                name: "PK_role_whitelist",
                table: "role_whitelist");

            migrationBuilder.DropIndex(
                name: "IX_role_whitelist_player_id",
                table: "role_whitelist");

            migrationBuilder.DropColumn(
                name: "role_whitelist_id",
                table: "role_whitelist");

            migrationBuilder.DropColumn(
                name: "first_time_added",
                table: "role_whitelist");

            migrationBuilder.DropColumn(
                name: "first_time_added_by",
                table: "role_whitelist");

            migrationBuilder.DropColumn(
                name: "how_many_times_added",
                table: "role_whitelist");

            migrationBuilder.DropColumn(
                name: "in_whitelist",
                table: "role_whitelist");

            migrationBuilder.DropColumn(
                name: "last_time_added",
                table: "role_whitelist");

            migrationBuilder.DropColumn(
                name: "last_time_added_by",
                table: "role_whitelist");

            migrationBuilder.DropColumn(
                name: "last_time_removed",
                table: "role_whitelist");

            migrationBuilder.DropColumn(
                name: "last_time_removed_by",
                table: "role_whitelist");

            migrationBuilder.RenameTable(
                name: "role_whitelist",
                newName: "role_whitelists");

            migrationBuilder.RenameColumn(
                name: "player_id",
                table: "role_whitelists",
                newName: "player_user_id");

            migrationBuilder.AddColumn<string>(
                name: "role_id",
                table: "role_whitelists",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_role_whitelists",
                table: "role_whitelists",
                columns: new[] { "player_user_id", "role_id" });

            migrationBuilder.AddForeignKey(
                name: "FK_role_whitelists_player_player_user_id",
                table: "role_whitelists",
                column: "player_user_id",
                principalTable: "player",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
