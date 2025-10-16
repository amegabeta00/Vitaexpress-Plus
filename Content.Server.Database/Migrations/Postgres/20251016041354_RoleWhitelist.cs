using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Content.Server.Database.Migrations.Postgres
{
    /// <inheritdoc />
    public partial class RoleWhitelist : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
    migrationBuilder.CreateTable(
        name: "role_whitelist",
        columns: table => new
        {
            role_whitelist_id = table.Column<int>(type: "integer", nullable: false)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            player_id = table.Column<Guid>(type: "uuid", nullable: false),
            first_time_added = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            first_time_added_by = table.Column<Guid>(type: "uuid", nullable: false),
            how_many_times_added = table.Column<int>(type: "integer", nullable: false),
            in_whitelist = table.Column<bool>(type: "boolean", nullable: false),
            last_time_added = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            last_time_added_by = table.Column<Guid>(type: "uuid", nullable: false),
            last_time_removed = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
            last_time_removed_by = table.Column<Guid>(type: "uuid", nullable: true)
        },
        constraints: table =>
        {
            table.PrimaryKey("PK_role_whitelist", x => x.role_whitelist_id);
            table.ForeignKey(
                name: "FK_role_whitelist_player_player_id",
                column: x => x.player_id,
                principalTable: "player",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);
        });

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
