using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lms_backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAssignmentTurnInResource : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Resources",
                columns: new[] { "ResourceId", "CreatedAt", "Data", "Description", "Name", "OwnerId", "ResourceType", "UpdatedAt" },
                values: new object[] { new Guid("55555555-0000-0000-0000-000000000010"), new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "https://github.com/johan-berg/console-app-assignment", "Johan Berg's submitted solution for the console application assignment.", "Console App Assignment Submission", new Guid("44444444-0000-0000-0000-000000000003"), 2, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.InsertData(
                table: "ActivityResources",
                columns: new[] { "Id", "ActivityId", "ResourceId" },
                values: new object[] { new Guid("99999999-0000-0000-0000-000000000004"), new Guid("66666666-0000-0000-0000-000000000007"), new Guid("55555555-0000-0000-0000-000000000010") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ActivityResources",
                keyColumn: "Id",
                keyValue: new Guid("99999999-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "Resources",
                keyColumn: "ResourceId",
                keyValue: new Guid("55555555-0000-0000-0000-000000000010"));
        }
    }
}
