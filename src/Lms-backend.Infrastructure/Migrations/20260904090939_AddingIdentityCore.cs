using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Lms_backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddingIdentityCore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUser_Courses_CourseId",
                table: "ApplicationUser");

            migrationBuilder.DropForeignKey(
                name: "FK_Resources_ApplicationUser_OwnerId",
                table: "Resources");

            migrationBuilder.DropForeignKey(
                name: "FK_UserResource_ApplicationUser_UserId",
                table: "UserResource");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplicationUser",
                table: "ApplicationUser");

            migrationBuilder.EnsureSchema(
                name: "identity");

            migrationBuilder.RenameTable(
                name: "UserResource",
                newName: "UserResource",
                newSchema: "identity");

            migrationBuilder.RenameTable(
                name: "Resources",
                newName: "Resources",
                newSchema: "identity");

            migrationBuilder.RenameTable(
                name: "Modules",
                newName: "Modules",
                newSchema: "identity");

            migrationBuilder.RenameTable(
                name: "ModuleResources",
                newName: "ModuleResources",
                newSchema: "identity");

            migrationBuilder.RenameTable(
                name: "Courses",
                newName: "Courses",
                newSchema: "identity");

            migrationBuilder.RenameTable(
                name: "CourseResources",
                newName: "CourseResources",
                newSchema: "identity");

            migrationBuilder.RenameTable(
                name: "CourseModules",
                newName: "CourseModules",
                newSchema: "identity");

            migrationBuilder.RenameTable(
                name: "ActivityResources",
                newName: "ActivityResources",
                newSchema: "identity");

            migrationBuilder.RenameTable(
                name: "Activities",
                newName: "Activities",
                newSchema: "identity");

            migrationBuilder.RenameTable(
                name: "ApplicationUser",
                newName: "AspNetUsers",
                newSchema: "identity");

            migrationBuilder.RenameColumn(
                name: "ResourceId",
                schema: "identity",
                table: "Resources",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                schema: "identity",
                table: "AspNetUsers",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_ApplicationUser_CourseId",
                schema: "identity",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_CourseId");

            migrationBuilder.AddColumn<int>(
                name: "AccessFailedCount",
                schema: "identity",
                table: "AspNetUsers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                schema: "identity",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                schema: "identity",
                table: "AspNetUsers",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EmailConfirmed",
                schema: "identity",
                table: "AspNetUsers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "LockoutEnabled",
                schema: "identity",
                table: "AspNetUsers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LockoutEnd",
                schema: "identity",
                table: "AspNetUsers",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NormalizedEmail",
                schema: "identity",
                table: "AspNetUsers",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NormalizedUserName",
                schema: "identity",
                table: "AspNetUsers",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                schema: "identity",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                schema: "identity",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PhoneNumberConfirmed",
                schema: "identity",
                table: "AspNetUsers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SecurityStamp",
                schema: "identity",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "TwoFactorEnabled",
                schema: "identity",
                table: "AspNetUsers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                schema: "identity",
                table: "AspNetUsers",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUsers",
                schema: "identity",
                table: "AspNetUsers",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                schema: "identity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                schema: "identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "identity",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                schema: "identity",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "identity",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                schema: "identity",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "identity",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                schema: "identity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    TokenHash = table.Column<string>(type: "text", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RevokedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeviceInfo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    LastUsedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "identity",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                schema: "identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "identity",
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                schema: "identity",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "identity",
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "identity",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                schema: "identity",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("44444444-0000-0000-0000-000000000001"),
                columns: new[] { "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { 0, "268d2cf5-1946-4e8e-b915-7f66ea6abed8", null, false, false, null, null, null, null, null, false, null, false, null });

            migrationBuilder.UpdateData(
                schema: "identity",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("44444444-0000-0000-0000-000000000002"),
                columns: new[] { "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { 0, "01f429a1-8204-4597-bc2a-e763ee8e1e9b", null, false, false, null, null, null, null, null, false, null, false, null });

            migrationBuilder.UpdateData(
                schema: "identity",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("44444444-0000-0000-0000-000000000003"),
                columns: new[] { "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { 0, "48fd668d-3a3b-40d5-9c9e-e96de93a4458", null, false, false, null, null, null, null, null, false, null, false, null });

            migrationBuilder.UpdateData(
                schema: "identity",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("44444444-0000-0000-0000-000000000004"),
                columns: new[] { "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { 0, "4a08b3ba-e848-4ca6-aee6-51fa69ded960", null, false, false, null, null, null, null, null, false, null, false, null });

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                schema: "identity",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                schema: "identity",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                schema: "identity",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                schema: "identity",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                schema: "identity",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                schema: "identity",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                schema: "identity",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                schema: "identity",
                table: "RefreshTokens",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Courses_CourseId",
                schema: "identity",
                table: "AspNetUsers",
                column: "CourseId",
                principalSchema: "identity",
                principalTable: "Courses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Resources_AspNetUsers_OwnerId",
                schema: "identity",
                table: "Resources",
                column: "OwnerId",
                principalSchema: "identity",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserResource_AspNetUsers_UserId",
                schema: "identity",
                table: "UserResource",
                column: "UserId",
                principalSchema: "identity",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Courses_CourseId",
                schema: "identity",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Resources_AspNetUsers_OwnerId",
                schema: "identity",
                table: "Resources");

            migrationBuilder.DropForeignKey(
                name: "FK_UserResource_AspNetUsers_UserId",
                schema: "identity",
                table: "UserResource");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "RefreshTokens",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "AspNetRoles",
                schema: "identity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUsers",
                schema: "identity",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "EmailIndex",
                schema: "identity",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "UserNameIndex",
                schema: "identity",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AccessFailedCount",
                schema: "identity",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                schema: "identity",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Email",
                schema: "identity",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EmailConfirmed",
                schema: "identity",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LockoutEnabled",
                schema: "identity",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LockoutEnd",
                schema: "identity",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "NormalizedEmail",
                schema: "identity",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "NormalizedUserName",
                schema: "identity",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                schema: "identity",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                schema: "identity",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PhoneNumberConfirmed",
                schema: "identity",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SecurityStamp",
                schema: "identity",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TwoFactorEnabled",
                schema: "identity",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserName",
                schema: "identity",
                table: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "UserResource",
                schema: "identity",
                newName: "UserResource");

            migrationBuilder.RenameTable(
                name: "Resources",
                schema: "identity",
                newName: "Resources");

            migrationBuilder.RenameTable(
                name: "Modules",
                schema: "identity",
                newName: "Modules");

            migrationBuilder.RenameTable(
                name: "ModuleResources",
                schema: "identity",
                newName: "ModuleResources");

            migrationBuilder.RenameTable(
                name: "Courses",
                schema: "identity",
                newName: "Courses");

            migrationBuilder.RenameTable(
                name: "CourseResources",
                schema: "identity",
                newName: "CourseResources");

            migrationBuilder.RenameTable(
                name: "CourseModules",
                schema: "identity",
                newName: "CourseModules");

            migrationBuilder.RenameTable(
                name: "ActivityResources",
                schema: "identity",
                newName: "ActivityResources");

            migrationBuilder.RenameTable(
                name: "Activities",
                schema: "identity",
                newName: "Activities");

            migrationBuilder.RenameTable(
                name: "AspNetUsers",
                schema: "identity",
                newName: "ApplicationUser");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Resources",
                newName: "ResourceId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ApplicationUser",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_CourseId",
                table: "ApplicationUser",
                newName: "IX_ApplicationUser_CourseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplicationUser",
                table: "ApplicationUser",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUser_Courses_CourseId",
                table: "ApplicationUser",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Resources_ApplicationUser_OwnerId",
                table: "Resources",
                column: "OwnerId",
                principalTable: "ApplicationUser",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserResource_ApplicationUser_UserId",
                table: "UserResource",
                column: "UserId",
                principalTable: "ApplicationUser",
                principalColumn: "UserId");
        }
    }
}
