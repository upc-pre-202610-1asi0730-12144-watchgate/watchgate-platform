using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace Watchgate.Locksight.Platform.Migrations
{
    /// <inheritdoc />
    public partial class InitialSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "alert_incidents",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    title = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false),
                    status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, defaultValue: "OPEN"),
                    priority = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, defaultValue: "MEDIUM"),
                    company_id = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    closed_at = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_alert_incidents", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "companies",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    trade_name = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    tax_id = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_companies", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "security_alerts",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    type = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    severity = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, defaultValue: "LOW"),
                    status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, defaultValue: "OPEN"),
                    description = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    sensor_id = table.Column<int>(type: "int", nullable: false),
                    company_id = table.Column<int>(type: "int", nullable: false),
                    triggered_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    resolved_at = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_security_alerts", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "sensors",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    type = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, defaultValue: "ACTIVE"),
                    unit = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    last_reading = table.Column<double>(type: "double", nullable: true),
                    last_reading_at = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    zone_id = table.Column<int>(type: "int", nullable: false),
                    company_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_sensors", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "warehouses",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    location = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false),
                    capacity = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, defaultValue: "ACTIVE"),
                    operation_start = table.Column<TimeSpan>(type: "time(6)", nullable: true),
                    operation_end = table.Column<TimeSpan>(type: "time(6)", nullable: true),
                    company_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_warehouses", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    full_name = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    email = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    password_hash = table.Column<string>(type: "longtext", nullable: false),
                    role = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, defaultValue: "Visitor"),
                    company_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_users", x => x.id);
                    table.ForeignKey(
                        name: "f_k_users_companies_company_id",
                        column: x => x.company_id,
                        principalTable: "companies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "alert_incident_security_alerts",
                columns: table => new
                {
                    alert_incident_id = table.Column<int>(type: "int", nullable: false),
                    related_alerts_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_alert_incident_security_alerts", x => new { x.alert_incident_id, x.related_alerts_id });
                    table.ForeignKey(
                        name: "fk_aisa_alert_incident",
                        column: x => x.alert_incident_id,
                        principalTable: "alert_incidents",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_aisa_security_alert",
                        column: x => x.related_alerts_id,
                        principalTable: "security_alerts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "warehouse_zones",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    area = table.Column<double>(type: "double", nullable: false),
                    risk_level = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, defaultValue: "LOW"),
                    warehouse_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_warehouse_zones", x => x.id);
                    table.ForeignKey(
                        name: "f_k_warehouse_zones_warehouses_warehouse_id",
                        column: x => x.warehouse_id,
                        principalTable: "warehouses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "i_x_alert_incident_security_alerts_related_alerts_id",
                table: "alert_incident_security_alerts",
                column: "related_alerts_id");

            migrationBuilder.CreateIndex(
                name: "i_x_users_company_id",
                table: "users",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "i_x_users_email",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_warehouse_zones_warehouse_id",
                table: "warehouse_zones",
                column: "warehouse_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "alert_incident_security_alerts");

            migrationBuilder.DropTable(
                name: "sensors");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "warehouse_zones");

            migrationBuilder.DropTable(
                name: "alert_incidents");

            migrationBuilder.DropTable(
                name: "security_alerts");

            migrationBuilder.DropTable(
                name: "companies");

            migrationBuilder.DropTable(
                name: "warehouses");
        }
    }
}
