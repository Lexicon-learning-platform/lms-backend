using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lms_backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRequiredSubmissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<List<string>>(
                name: "RequiredSubmissions",
                schema: "identity",
                table: "Activities",
                type: "text[]",
                nullable: false,
                defaultValue: new List<string>());

            migrationBuilder.UpdateData(
                schema: "identity",
                table: "Activities",
                keyColumn: "Id",
                keyValue: new Guid("66666666-0000-0000-0000-000000000001"),
                column: "RequiredSubmissions",
                value: new List<string>());

            migrationBuilder.UpdateData(
                schema: "identity",
                table: "Activities",
                keyColumn: "Id",
                keyValue: new Guid("66666666-0000-0000-0000-000000000002"),
                column: "RequiredSubmissions",
                value: new List<string>());

            migrationBuilder.UpdateData(
                schema: "identity",
                table: "Activities",
                keyColumn: "Id",
                keyValue: new Guid("66666666-0000-0000-0000-000000000003"),
                column: "RequiredSubmissions",
                value: new List<string>());

            migrationBuilder.UpdateData(
                schema: "identity",
                table: "Activities",
                keyColumn: "Id",
                keyValue: new Guid("66666666-0000-0000-0000-000000000004"),
                column: "RequiredSubmissions",
                value: new List<string>());

            migrationBuilder.UpdateData(
                schema: "identity",
                table: "Activities",
                keyColumn: "Id",
                keyValue: new Guid("66666666-0000-0000-0000-000000000005"),
                column: "RequiredSubmissions",
                value: new List<string>());

            migrationBuilder.UpdateData(
                schema: "identity",
                table: "Activities",
                keyColumn: "Id",
                keyValue: new Guid("66666666-0000-0000-0000-000000000006"),
                column: "RequiredSubmissions",
                value: new List<string>());

            migrationBuilder.UpdateData(
                schema: "identity",
                table: "Activities",
                keyColumn: "Id",
                keyValue: new Guid("66666666-0000-0000-0000-000000000007"),
                column: "RequiredSubmissions",
                value: new List<string>());

            migrationBuilder.UpdateData(
                schema: "identity",
                table: "Activities",
                keyColumn: "Id",
                keyValue: new Guid("66666666-0000-0000-0000-000000000008"),
                column: "RequiredSubmissions",
                value: new List<string>());

            migrationBuilder.UpdateData(
                schema: "identity",
                table: "Activities",
                keyColumn: "Id",
                keyValue: new Guid("66666666-0000-0000-0000-000000000009"),
                column: "RequiredSubmissions",
                value: new List<string>());
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequiredSubmissions",
                schema: "identity",
                table: "Activities");
        }
    }
}


