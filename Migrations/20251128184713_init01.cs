using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AvvalOnline.Shop.Api.Migrations
{
    /// <inheritdoc />
    public partial class init01 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "ProductCategories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 1,
                column: "GrantedAt",
                value: new DateTime(2025, 11, 28, 18, 47, 13, 327, DateTimeKind.Utc).AddTicks(2846));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 2,
                column: "GrantedAt",
                value: new DateTime(2025, 11, 28, 18, 47, 13, 327, DateTimeKind.Utc).AddTicks(2850));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 3,
                column: "GrantedAt",
                value: new DateTime(2025, 11, 28, 18, 47, 13, 327, DateTimeKind.Utc).AddTicks(2851));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 4,
                column: "GrantedAt",
                value: new DateTime(2025, 11, 28, 18, 47, 13, 327, DateTimeKind.Utc).AddTicks(2851));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 5,
                column: "GrantedAt",
                value: new DateTime(2025, 11, 28, 18, 47, 13, 327, DateTimeKind.Utc).AddTicks(2852));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 6,
                column: "GrantedAt",
                value: new DateTime(2025, 11, 28, 18, 47, 13, 327, DateTimeKind.Utc).AddTicks(2854));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 7,
                column: "GrantedAt",
                value: new DateTime(2025, 11, 28, 18, 47, 13, 327, DateTimeKind.Utc).AddTicks(2854));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 8,
                column: "GrantedAt",
                value: new DateTime(2025, 11, 28, 18, 47, 13, 327, DateTimeKind.Utc).AddTicks(2855));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 9,
                column: "GrantedAt",
                value: new DateTime(2025, 11, 28, 18, 47, 13, 327, DateTimeKind.Utc).AddTicks(2856));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 10,
                column: "GrantedAt",
                value: new DateTime(2025, 11, 28, 18, 47, 13, 327, DateTimeKind.Utc).AddTicks(2857));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 11,
                column: "GrantedAt",
                value: new DateTime(2025, 11, 28, 18, 47, 13, 327, DateTimeKind.Utc).AddTicks(2858));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 12,
                column: "GrantedAt",
                value: new DateTime(2025, 11, 28, 18, 47, 13, 327, DateTimeKind.Utc).AddTicks(2858));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 28, 18, 47, 13, 268, DateTimeKind.Utc).AddTicks(9112));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 28, 18, 47, 13, 268, DateTimeKind.Utc).AddTicks(9114));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 28, 18, 47, 13, 268, DateTimeKind.Utc).AddTicks(9115));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 28, 18, 47, 13, 268, DateTimeKind.Utc).AddTicks(9117));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 28, 18, 47, 13, 268, DateTimeKind.Utc).AddTicks(9118));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "AssignedAt",
                value: new DateTime(2025, 11, 28, 18, 47, 13, 327, DateTimeKind.Utc).AddTicks(2675));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 28, 18, 47, 13, 327, DateTimeKind.Utc).AddTicks(2321), "AQAAAAIAAYagAAAAEP90W8fazDibP1JEbEbt7EYe8ZQs/5Tld7LHGtBWVWlft/c0NOBKbNqOkGprKpwhnQ==", new DateTime(2025, 11, 28, 18, 47, 13, 268, DateTimeKind.Utc).AddTicks(9624) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "ProductCategories");

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 1,
                column: "GrantedAt",
                value: new DateTime(2025, 11, 28, 17, 0, 47, 383, DateTimeKind.Utc).AddTicks(250));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 2,
                column: "GrantedAt",
                value: new DateTime(2025, 11, 28, 17, 0, 47, 383, DateTimeKind.Utc).AddTicks(255));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 3,
                column: "GrantedAt",
                value: new DateTime(2025, 11, 28, 17, 0, 47, 383, DateTimeKind.Utc).AddTicks(256));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 4,
                column: "GrantedAt",
                value: new DateTime(2025, 11, 28, 17, 0, 47, 383, DateTimeKind.Utc).AddTicks(257));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 5,
                column: "GrantedAt",
                value: new DateTime(2025, 11, 28, 17, 0, 47, 383, DateTimeKind.Utc).AddTicks(258));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 6,
                column: "GrantedAt",
                value: new DateTime(2025, 11, 28, 17, 0, 47, 383, DateTimeKind.Utc).AddTicks(261));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 7,
                column: "GrantedAt",
                value: new DateTime(2025, 11, 28, 17, 0, 47, 383, DateTimeKind.Utc).AddTicks(262));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 8,
                column: "GrantedAt",
                value: new DateTime(2025, 11, 28, 17, 0, 47, 383, DateTimeKind.Utc).AddTicks(263));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 9,
                column: "GrantedAt",
                value: new DateTime(2025, 11, 28, 17, 0, 47, 383, DateTimeKind.Utc).AddTicks(263));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 10,
                column: "GrantedAt",
                value: new DateTime(2025, 11, 28, 17, 0, 47, 383, DateTimeKind.Utc).AddTicks(266));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 11,
                column: "GrantedAt",
                value: new DateTime(2025, 11, 28, 17, 0, 47, 383, DateTimeKind.Utc).AddTicks(267));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 12,
                column: "GrantedAt",
                value: new DateTime(2025, 11, 28, 17, 0, 47, 383, DateTimeKind.Utc).AddTicks(267));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 28, 17, 0, 47, 305, DateTimeKind.Utc).AddTicks(8984));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 28, 17, 0, 47, 305, DateTimeKind.Utc).AddTicks(8987));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 28, 17, 0, 47, 305, DateTimeKind.Utc).AddTicks(8989));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 28, 17, 0, 47, 305, DateTimeKind.Utc).AddTicks(8991));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 28, 17, 0, 47, 305, DateTimeKind.Utc).AddTicks(8993));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "AssignedAt",
                value: new DateTime(2025, 11, 28, 17, 0, 47, 383, DateTimeKind.Utc).AddTicks(120));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 28, 17, 0, 47, 382, DateTimeKind.Utc).AddTicks(9387), "AQAAAAIAAYagAAAAELqcj9MH4al7SjgggDvcq1WAlXwdl70p3Yw9UtbOCKzO0SwZLsbX3SIzuA44B6ApIg==", new DateTime(2025, 11, 28, 17, 0, 47, 305, DateTimeKind.Utc).AddTicks(9234) });
        }
    }
}
