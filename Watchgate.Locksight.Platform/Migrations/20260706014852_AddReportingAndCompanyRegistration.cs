using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace Watchgate.Locksight.Platform.Migrations
{
    /// <inheritdoc />
    public partial class AddReportingAndCompanyRegistration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "company_accounts",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    company_id = table.Column<int>(type: "int", nullable: false),
                    trade_name = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    tax_id = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    legal_name = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true),
                    industry = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    contact_phone = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    address = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: true),
                    website_url = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: true),
                    status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, defaultValue: "ACTIVE"),
                    is_profile_completed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    is_administrator_email_verified = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    email_verification_code = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_company_accounts", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "i_x_company_accounts_company_id",
                table: "company_accounts",
                column: "company_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "company_accounts");
        }
    }
}
