using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lms_backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class IdentityUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                schema: "identity",
                table: "AspNetUsers",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "GivenName",
                schema: "identity",
                table: "AspNetUsers",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<string>(
                name: "Role",
                schema: "identity",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                schema: "identity",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("44444444-0000-0000-0000-000000000001"),
                columns: new[] { "ConcurrencyStamp", "Role" },
                values: new object[] { "268d2cf5-1946-4e8e-b915-7f66ea6abed8", "Student" });

            migrationBuilder.UpdateData(
                schema: "identity",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("44444444-0000-0000-0000-000000000002"),
                columns: new[] { "ConcurrencyStamp", "Role" },
                values: new object[] { "01f429a1-8204-4597-bc2a-e763ee8e1e9b", "Student" });

            migrationBuilder.UpdateData(
                schema: "identity",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("44444444-0000-0000-0000-000000000003"),
                columns: new[] { "ConcurrencyStamp", "Role" },
                values: new object[] { "48fd668d-3a3b-40d5-9c9e-e96de93a4458", "Student" });

            migrationBuilder.UpdateData(
                schema: "identity",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("44444444-0000-0000-0000-000000000004"),
                columns: new[] { "ConcurrencyStamp", "Role" },
                values: new object[] { "4a08b3ba-e848-4ca6-aee6-51fa69ded960", "Student" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                schema: "identity",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                schema: "identity",
                table: "AspNetUsers",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "GivenName",
                schema: "identity",
                table: "AspNetUsers",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.UpdateData(
                schema: "identity",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("44444444-0000-0000-0000-000000000001"),
                column: "ConcurrencyStamp",
                value: "ad245959-237f-4db1-a469-0e68e761e720");

            migrationBuilder.UpdateData(
                schema: "identity",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("44444444-0000-0000-0000-000000000002"),
                column: "ConcurrencyStamp",
                value: "4bcdc751-c13a-4eff-8407-faecec993c6a");

            migrationBuilder.UpdateData(
                schema: "identity",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("44444444-0000-0000-0000-000000000003"),
                column: "ConcurrencyStamp",
                value: "eb3f8eaf-0998-43f8-9db6-643bb7b2e41a");

            migrationBuilder.UpdateData(
                schema: "identity",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("44444444-0000-0000-0000-000000000004"),
                column: "ConcurrencyStamp",
                value: "9d0b77c8-b4be-4264-9cad-09ecb828e9e8");
        }
    }
}
