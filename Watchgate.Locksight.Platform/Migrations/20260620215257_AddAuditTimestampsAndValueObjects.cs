using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Watchgate.Locksight.Platform.Migrations
{
    /// <inheritdoc />
    public partial class AddAuditTimestampsAndValueObjects : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "p_k_warehouses",
                table: "warehouses");

            migrationBuilder.DropPrimaryKey(
                name: "p_k_warehouse_zones",
                table: "warehouse_zones");

            migrationBuilder.DropPrimaryKey(
                name: "p_k_users",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "p_k_sensors",
                table: "sensors");

            migrationBuilder.DropPrimaryKey(
                name: "p_k_security_alerts",
                table: "security_alerts");

            migrationBuilder.DropPrimaryKey(
                name: "p_k_companies",
                table: "companies");

            migrationBuilder.DropPrimaryKey(
                name: "p_k_alert_incidents",
                table: "alert_incidents");

            migrationBuilder.DropPrimaryKey(
                name: "p_k_alert_incident_security_alerts",
                table: "alert_incident_security_alerts");

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "warehouses",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "warehouses",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "users",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "users",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "sensors",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "sensors",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "security_alerts",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "security_alerts",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "companies",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "companies",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "alert_incidents",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_warehouses",
                table: "warehouses",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_warehouse_zones",
                table: "warehouse_zones",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_users",
                table: "users",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_sensors",
                table: "sensors",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_security_alerts",
                table: "security_alerts",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_companies",
                table: "companies",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_alert_incidents",
                table: "alert_incidents",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_alert_incident_security_alerts",
                table: "alert_incident_security_alerts",
                columns: new[] { "alert_incident_id", "related_alerts_id" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_warehouses",
                table: "warehouses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_warehouse_zones",
                table: "warehouse_zones");

            migrationBuilder.DropPrimaryKey(
                name: "PK_users",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_sensors",
                table: "sensors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_security_alerts",
                table: "security_alerts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_companies",
                table: "companies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_alert_incidents",
                table: "alert_incidents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_alert_incident_security_alerts",
                table: "alert_incident_security_alerts");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "warehouses");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "warehouses");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "users");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "users");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "sensors");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "sensors");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "security_alerts");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "security_alerts");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "companies");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "companies");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "alert_incidents");

            migrationBuilder.AddPrimaryKey(
                name: "p_k_warehouses",
                table: "warehouses",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "p_k_warehouse_zones",
                table: "warehouse_zones",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "p_k_users",
                table: "users",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "p_k_sensors",
                table: "sensors",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "p_k_security_alerts",
                table: "security_alerts",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "p_k_companies",
                table: "companies",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "p_k_alert_incidents",
                table: "alert_incidents",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "p_k_alert_incident_security_alerts",
                table: "alert_incident_security_alerts",
                columns: new[] { "alert_incident_id", "related_alerts_id" });
        }
    }
}
