using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Content.Server.Database.Migrations.Postgres
{
    /// <inheritdoc />
    public partial class EuropaRoleWhitelist : Migration
    {
        private const string RoleWhitelists = "role_whitelists";
        private const string RoleWhitelist = "role_whitelist";
        private const string RoleWhitelistLog = "role_whitelist_log";

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(RoleWhitelists);

            migrationBuilder.CreateTable(
                name: RoleWhitelist,
                columns: table => new
                {
                    role_whitelist_id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    player_id = table.Column<Guid>(type: "uuid", nullable: false),
                    in_whitelist = table.Column<bool>(type: "boolean", defaultValue: true, nullable: false),
                    how_many_times_added = table.Column<int>(type: "integer", defaultValue: 1, nullable: false),
                    first_time_added = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    last_time_added = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    last_time_removed = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    first_time_added_by = table.Column<Guid>(type: "uuid", nullable: false),
                    last_time_added_by = table.Column<Guid>(type: "uuid", nullable: false),
                    last_time_removed_by = table.Column<Guid>(type: "uuid", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_" + RoleWhitelistLog, x => x.role_whitelist_id);
                });

            migrationBuilder.CreateTable(
                name: RoleWhitelistLog,
                columns: table => new
                {
                    role_whitelist_log_id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    admin_id = table.Column<Guid>(type: "uuid", nullable: false),
                    player_id = table.Column<Guid>(type: "uuid", nullable: false),
                    role_whitelist_action = table.Column<string>(type: "text", nullable: false),
                    reason = table.Column<string>(type: "text", nullable: false),
                    time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_" + RoleWhitelistLog, x => x.role_whitelist_log_id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_role_whitelist_player_id",
                table: RoleWhitelist,
                column: "player_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_role_whitelist_log_player_id",
                table: RoleWhitelistLog,
                column: "player_id");

            migrationBuilder.CreateIndex(
                name: "IX_role_whitelist_log_admin_id",
                table: RoleWhitelistLog,
                column: "admin_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(RoleWhitelist);
            migrationBuilder.DropTable(RoleWhitelistLog);
        }
    }
}
