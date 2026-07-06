using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace Watchgate.Locksight.Platform.Migrations
{
    /// <inheritdoc />
    public partial class AddIamUserAccessManagement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "user_access_profiles",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    company_id = table.Column<int>(type: "int", nullable: false),
                    role = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    permissions = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    restricted_zone_id = table.Column<int>(type: "int", nullable: true),
                    status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    email_notifications_enabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    push_notifications_enabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    critical_only_notifications = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_access_profiles", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "user_invitations",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    company_id = table.Column<int>(type: "int", nullable: false),
                    email = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    role = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    permissions = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    zone_id = table.Column<int>(type: "int", nullable: true),
                    token = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false),
                    status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    expires_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    accepted_at = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_invitations", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "i_x_user_access_profiles_user_id",
                table: "user_access_profiles",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_user_invitations_token",
                table: "user_invitations",
                column: "token",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user_access_profiles");

            migrationBuilder.DropTable(
                name: "user_invitations");
        }
    }
}
