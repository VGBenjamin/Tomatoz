using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tomatoz.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixSeedDataAndRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TomatoVarietyTags_AspNetUsers_AddedByUserId",
                table: "TomatoVarietyTags");

            migrationBuilder.DropForeignKey(
                name: "FK_TomatoVarietyTags_Tags_TagId1",
                table: "TomatoVarietyTags");

            migrationBuilder.DropIndex(
                name: "IX_TomatoVarietyTags_TagId1",
                table: "TomatoVarietyTags");

            migrationBuilder.DropColumn(
                name: "TagId1",
                table: "TomatoVarietyTags");

            migrationBuilder.UpdateData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "TomatoVarieties",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "TomatoVarieties",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.AddForeignKey(
                name: "FK_TomatoVarietyTags_AspNetUsers_AddedByUserId",
                table: "TomatoVarietyTags",
                column: "AddedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TomatoVarietyTags_AspNetUsers_AddedByUserId",
                table: "TomatoVarietyTags");

            migrationBuilder.AddColumn<int>(
                name: "TagId1",
                table: "TomatoVarietyTags",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 12, 9, 41, 2, 637, DateTimeKind.Utc).AddTicks(6722));

            migrationBuilder.UpdateData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 12, 9, 41, 2, 637, DateTimeKind.Utc).AddTicks(7930));

            migrationBuilder.UpdateData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 12, 9, 41, 2, 637, DateTimeKind.Utc).AddTicks(7932));

            migrationBuilder.UpdateData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 12, 9, 41, 2, 637, DateTimeKind.Utc).AddTicks(7933));

            migrationBuilder.UpdateData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 12, 9, 41, 2, 637, DateTimeKind.Utc).AddTicks(7934));

            migrationBuilder.UpdateData(
                table: "TomatoVarieties",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 12, 9, 41, 2, 638, DateTimeKind.Utc).AddTicks(8017));

            migrationBuilder.UpdateData(
                table: "TomatoVarieties",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 12, 9, 41, 2, 638, DateTimeKind.Utc).AddTicks(8523));

            migrationBuilder.CreateIndex(
                name: "IX_TomatoVarietyTags_TagId1",
                table: "TomatoVarietyTags",
                column: "TagId1");

            migrationBuilder.AddForeignKey(
                name: "FK_TomatoVarietyTags_AspNetUsers_AddedByUserId",
                table: "TomatoVarietyTags",
                column: "AddedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TomatoVarietyTags_Tags_TagId1",
                table: "TomatoVarietyTags",
                column: "TagId1",
                principalTable: "Tags",
                principalColumn: "Id");
        }
    }
}
