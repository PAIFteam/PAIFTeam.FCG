using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PAIFGAMES.FCG.Infra.Migrations
{
    /// <inheritdoc />
    public partial class initialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "fcg");

            migrationBuilder.CreateTable(
                name: "tb_game",
                schema: "fcg",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    uid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "varchar(100)", nullable: false),
                    value = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PKtbgame", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tb_role",
                schema: "fcg",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    uid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    name = table.Column<string>(type: "varchar(50)", maxLength: 256, nullable: false),
                    name_normalized = table.Column<string>(type: "varchar(50)", maxLength: 256, nullable: false),
                    concurrency_stamp = table.Column<string>(type: "varchar(40)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PKtbrole", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tb_user",
                schema: "fcg",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    uid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    fullname = table.Column<string>(type: "varchar(50)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    username = table.Column<string>(type: "varchar(50)", maxLength: 256, nullable: false),
                    normalized_username = table.Column<string>(type: "varchar(50)", maxLength: 256, nullable: false),
                    email = table.Column<string>(type: "varchar(70)", maxLength: 256, nullable: false),
                    normalized_email = table.Column<string>(type: "varchar(70)", maxLength: 256, nullable: false),
                    is_email_confirmed = table.Column<bool>(type: "bit", nullable: false),
                    password_hash = table.Column<string>(type: "varchar(500)", nullable: false),
                    uid_security_stamp = table.Column<string>(type: "varchar(40)", nullable: false),
                    uid_concurrency_stamp = table.Column<string>(type: "varchar(40)", nullable: false),
                    phone_number = table.Column<string>(type: "varchar(25)", nullable: true),
                    is_phone_number_confirmed = table.Column<bool>(type: "bit", nullable: false),
                    is_two_factor_enabled = table.Column<bool>(type: "bit", nullable: false),
                    lockout_end_time = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    is_lockout_enabled = table.Column<bool>(type: "bit", nullable: false),
                    num_access_failed_count = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PKtbuser", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tb_role_claim",
                schema: "fcg",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    role_id = table.Column<long>(type: "bigint", nullable: false),
                    claim_type = table.Column<string>(type: "varchar(50)", nullable: false),
                    claim_value = table.Column<string>(type: "varchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PKtbroleclaim", x => x.id);
                    table.ForeignKey(
                        name: "FK_tb_role_claim_tb_role_role_id",
                        column: x => x.role_id,
                        principalSchema: "fcg",
                        principalTable: "tb_role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tb_user_claim",
                schema: "fcg",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    claim_type = table.Column<string>(type: "varchar(50)", nullable: false),
                    claim_value = table.Column<string>(type: "varchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PKtbuserclaim", x => x.id);
                    table.ForeignKey(
                        name: "FK_tb_user_claim_tb_user_user_id",
                        column: x => x.user_id,
                        principalSchema: "fcg",
                        principalTable: "tb_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tb_user_game",
                schema: "fcg",
                columns: table => new
                {
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    game_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PKtbusergame", x => new { x.user_id, x.game_id });
                    table.ForeignKey(
                        name: "FK_tb_user_game_tb_game_game_id",
                        column: x => x.game_id,
                        principalSchema: "fcg",
                        principalTable: "tb_game",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tb_user_game_tb_user_user_id",
                        column: x => x.user_id,
                        principalSchema: "fcg",
                        principalTable: "tb_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tb_user_login",
                schema: "fcg",
                columns: table => new
                {
                    login_provider = table.Column<string>(type: "varchar(40)", nullable: false),
                    provider_key = table.Column<string>(type: "varchar(40)", nullable: false),
                    provider_display_name = table.Column<string>(type: "varchar(40)", nullable: true),
                    user_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PKtbuserlogin", x => new { x.login_provider, x.provider_key });
                    table.ForeignKey(
                        name: "FK_tb_user_login_tb_user_user_id",
                        column: x => x.user_id,
                        principalSchema: "fcg",
                        principalTable: "tb_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tb_user_role",
                schema: "fcg",
                columns: table => new
                {
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    role_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PKtbuserrole", x => new { x.user_id, x.role_id });
                    table.ForeignKey(
                        name: "FK_tb_user_role_tb_role_role_id",
                        column: x => x.role_id,
                        principalSchema: "fcg",
                        principalTable: "tb_role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tb_user_role_tb_user_user_id",
                        column: x => x.user_id,
                        principalSchema: "fcg",
                        principalTable: "tb_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tb_user_token",
                schema: "fcg",
                columns: table => new
                {
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    login_provider = table.Column<string>(type: "varchar(50)", nullable: false),
                    name = table.Column<string>(type: "varchar(50)", nullable: false),
                    value = table.Column<string>(type: "varchar(250)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PKtbusertoken", x => new { x.user_id, x.login_provider, x.name });
                    table.ForeignKey(
                        name: "FK_tb_user_token_tb_user_user_id",
                        column: x => x.user_id,
                        principalSchema: "fcg",
                        principalTable: "tb_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                schema: "fcg",
                table: "tb_role",
                column: "name_normalized",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tb_role_claim_role_id",
                schema: "fcg",
                table: "tb_role_claim",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                schema: "fcg",
                table: "tb_user",
                column: "normalized_email");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                schema: "fcg",
                table: "tb_user",
                column: "normalized_username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tb_user_claim_user_id",
                schema: "fcg",
                table: "tb_user_claim",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_tb_user_game_game_id",
                schema: "fcg",
                table: "tb_user_game",
                column: "game_id");

            migrationBuilder.CreateIndex(
                name: "IX_tb_user_login_user_id",
                schema: "fcg",
                table: "tb_user_login",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_tb_user_role_role_id",
                schema: "fcg",
                table: "tb_user_role",
                column: "role_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tb_role_claim",
                schema: "fcg");

            migrationBuilder.DropTable(
                name: "tb_user_claim",
                schema: "fcg");

            migrationBuilder.DropTable(
                name: "tb_user_game",
                schema: "fcg");

            migrationBuilder.DropTable(
                name: "tb_user_login",
                schema: "fcg");

            migrationBuilder.DropTable(
                name: "tb_user_role",
                schema: "fcg");

            migrationBuilder.DropTable(
                name: "tb_user_token",
                schema: "fcg");

            migrationBuilder.DropTable(
                name: "tb_game",
                schema: "fcg");

            migrationBuilder.DropTable(
                name: "tb_role",
                schema: "fcg");

            migrationBuilder.DropTable(
                name: "tb_user",
                schema: "fcg");
        }
    }
}
