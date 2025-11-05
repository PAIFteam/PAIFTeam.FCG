using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PAIFGAMES.FCG.Infra.Migrations
{
    /// <inheritdoc />
    public partial class SeedDataMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "tb_user",
                schema: "fcg",
                columns: new[]
                {
            "id",
            "uid",
            "fullname",
            "created_at",
            "updated_at",
            "username",
            "normalized_username",
            "email",
            "normalized_email",
            "is_email_confirmed",
            "password_hash",
            "uid_security_stamp",
            "uid_concurrency_stamp",
            "phone_number",
            "is_phone_number_confirmed",
            "is_two_factor_enabled",
            "lockout_end_time",
            "is_lockout_enabled",
            "num_access_failed_count"
                },
                values: new object[]
                {
            1,
            Guid.Parse("59758824-921E-4990-BB7B-62FF357CF6ED"),
            "Conta Admin",
            DateTime.Parse("2025-11-04 19:40:11.0474049"),
            DateTime.Parse("2025-11-04 19:40:11.0474085"),
            "admin",
            "ADMIN",
            "admin",
            "ADMIN",
            false,
            "AQAAAAIAAYagAAAAEAGZRSBKxmKl1bQFI9fvkTgsBcGHbCNEIjoiKbiAyrORJgWQJVE5Dzt48sQ4XI/wdg==",
            "CZGHH3KWLXX4CWGS3VK6HMNZPI25YFBI",
            "86fa3faa-b0f9-4ebc-89d3-9faae0143e47",
            null,
            false,
            false,
            null,
            false,
            0
                }
            );

            // Inserção dos papéis (roles)
            migrationBuilder.InsertData(
                table: "tb_role",
                schema: "fcg",
                columns: new[]
                {
            "id",
            "uid",
            "created_at",
            "updated_at",
            "name",
            "name_normalized",
            "concurrency_stamp"
                },
                values: new object[]
                {
            1,
            Guid.Parse("a1e1b2c3-d4f5-6789-abcd-ef0123456789"),
            DateTime.UtcNow,
            DateTime.UtcNow,
            "Admin",
            "ADMIN",
            Guid.NewGuid().ToString()
                }
            );

            migrationBuilder.InsertData(
                table: "tb_role",
                schema: "fcg",
                columns: new[]
                {
            "id",
            "uid",
            "created_at",
            "updated_at",
            "name",
            "name_normalized",
            "concurrency_stamp"
                },
                values: new object[]
                {
            2,
            Guid.Parse("b2e2c3d4-e5f6-789a-bcde-f12345678901"),
            DateTime.UtcNow,
            DateTime.UtcNow,
            "User",
            "USER",
            Guid.NewGuid().ToString()
                }
            );

            // Relacionamento user-role
            migrationBuilder.InsertData(
                table: "tb_user_role",
                schema: "fcg",
                columns: new[] { "user_id", "role_id" },
                values: new object[] { 1, 1 }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
