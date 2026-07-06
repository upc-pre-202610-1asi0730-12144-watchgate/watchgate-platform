using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace Watchgate.Locksight.Platform.Migrations
{
    /// <inheritdoc />
    public partial class AddReportingBoundedContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "scheduled_reports",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    company_id = table.Column<int>(type: "int", nullable: false),
                    warehouse_id = table.Column<int>(type: "int", nullable: true),
                    name = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    frequency = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, defaultValue: "WEEKLY"),
                    format = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, defaultValue: "PDF"),
                    recipient_email = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    is_active = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    starts_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_scheduled_reports", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "security_reports",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    company_id = table.Column<int>(type: "int", nullable: false),
                    warehouse_id = table.Column<int>(type: "int", nullable: true),
                    title = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    from = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    to = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    format = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, defaultValue: "PDF"),
                    status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, defaultValue: "GENERATED"),
                    total_events = table.Column<int>(type: "int", nullable: false),
                    critical_events = table.Column<int>(type: "int", nullable: false),
                    resolved_events = table.Column<int>(type: "int", nullable: false),
                    generated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_security_reports", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "scheduled_reports");

            migrationBuilder.DropTable(
                name: "security_reports");
        }
    }
}
