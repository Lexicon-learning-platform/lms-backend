using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Lms_backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ExtendSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "identity",
                table: "Activities",
                columns: new[] { "Id", "ActivityType", "CreatedAt", "Description", "DurationMinutes", "ModuleId", "Name", "StartTimeOffset", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("66666666-0000-0000-0000-000000000010"), 3, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "Apply branching, merging, and commit conventions to complete a guided multi-branch Git workflow.", 120, new Guid("22222222-0000-0000-0000-000000000001"), "Git Workflow Assignment", 150, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("66666666-0000-0000-0000-000000000011"), 0, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "Semantic HTML structure and the CSS box model, Flexbox, and Grid layout basics.", 90, new Guid("22222222-0000-0000-0000-000000000002"), "HTML & CSS Fundamentals", 0, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("66666666-0000-0000-0000-000000000012"), 2, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "Hands-on practice building a mobile-first, responsive page layout with Flexbox and media queries.", 120, new Guid("22222222-0000-0000-0000-000000000002"), "Responsive Layout Exercise", 90, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("66666666-0000-0000-0000-000000000013"), 3, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "Build and publish a responsive personal portfolio page using semantic HTML and CSS.", 240, new Guid("22222222-0000-0000-0000-000000000002"), "Personal Portfolio Page", 210, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("66666666-0000-0000-0000-000000000014"), 0, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "Building reusable UI components and passing data between them with props.", 90, new Guid("22222222-0000-0000-0000-000000000003"), "React Components & Props", 0, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("66666666-0000-0000-0000-000000000015"), 2, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "Hands-on practice managing component state and side effects with useState and useEffect.", 120, new Guid("22222222-0000-0000-0000-000000000003"), "State & Hooks Exercise", 90, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("66666666-0000-0000-0000-000000000016"), 3, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "Build a functional todo list application using React components, state, and hooks.", 240, new Guid("22222222-0000-0000-0000-000000000003"), "Todo App Assignment", 210, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("66666666-0000-0000-0000-000000000017"), 0, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "Designing and implementing RESTful endpoints with ASP.NET Core controllers and routing.", 90, new Guid("22222222-0000-0000-0000-000000000005"), "Building REST APIs", 0, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("66666666-0000-0000-0000-000000000018"), 2, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "Hands-on practice modeling entities and managing schema changes with EF Core migrations.", 120, new Guid("22222222-0000-0000-0000-000000000005"), "EF Core Migrations Exercise", 90, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("66666666-0000-0000-0000-000000000019"), 3, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "Build a complete CRUD REST API with ASP.NET Core and EF Core backed by PostgreSQL.", 240, new Guid("22222222-0000-0000-0000-000000000005"), "CRUD API Assignment", 210, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("66666666-0000-0000-0000-000000000020"), 2, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "Hands-on practice writing a Dockerfile and building a custom image for a sample application.", 90, new Guid("22222222-0000-0000-0000-000000000006"), "Build a Custom Image", 90, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("66666666-0000-0000-0000-000000000021"), 3, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "Containerize a sample application with a Dockerfile and a Docker Compose setup.", 180, new Guid("22222222-0000-0000-0000-000000000006"), "Dockerize an Application", 180, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("66666666-0000-0000-0000-000000000022"), 0, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "Overview of continuous integration and deployment concepts, stages, and common tooling.", 60, new Guid("22222222-0000-0000-0000-000000000007"), "CI/CD Pipeline Concepts", 0, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("66666666-0000-0000-0000-000000000023"), 2, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "Hands-on practice writing a GitHub Actions workflow to build and test an application.", 90, new Guid("22222222-0000-0000-0000-000000000007"), "GitHub Actions Workflow", 60, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("66666666-0000-0000-0000-000000000024"), 3, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "Build an automated CI/CD pipeline that builds, tests, and deploys a sample application.", 180, new Guid("22222222-0000-0000-0000-000000000007"), "Deployment Pipeline Assignment", 150, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                schema: "identity",
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CourseId", "CreatedAt", "Email", "EmailConfirmed", "GivenName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "Role", "SecurityStamp", "TwoFactorEnabled", "UpdatedAt", "UserName" },
                values: new object[,]
                {
                    { new Guid("44444444-0000-0000-0000-000000000006"), 0, "8b87ae00-f38f-4dfd-9470-fae499b9d999", new Guid("11111111-0000-0000-0000-000000000001"), new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), null, false, "Emma", "Karlsson", false, null, null, null, null, null, false, "Student", null, false, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), null },
                    { new Guid("44444444-0000-0000-0000-000000000007"), 0, "d5ed9a83-ce9a-444a-9e5e-48dbc96d605f", new Guid("11111111-0000-0000-0000-000000000001"), new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), null, false, "Oskar", "Lindberg", false, null, null, null, null, null, false, "Student", null, false, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), null },
                    { new Guid("44444444-0000-0000-0000-000000000008"), 0, "76347267-0422-4483-a70e-c72338e79a71", new Guid("11111111-0000-0000-0000-000000000001"), new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), null, false, "Lina", "Hakansson", false, null, null, null, null, null, false, "Student", null, false, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), null },
                    { new Guid("44444444-0000-0000-0000-000000000009"), 0, "f48ed268-1ee5-4d2f-9683-04b6f5c1bf79", new Guid("11111111-0000-0000-0000-000000000001"), new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), null, false, "Viktor", "Astrom", false, null, null, null, null, null, false, "Student", null, false, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), null },
                    { new Guid("44444444-0000-0000-0000-000000000010"), 0, "626c18fe-9584-4585-a6e9-394aba0ba1b9", new Guid("11111111-0000-0000-0000-000000000002"), new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), null, false, "Erik", "Holm", false, null, null, null, null, null, false, "Student", null, false, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), null },
                    { new Guid("44444444-0000-0000-0000-000000000011"), 0, "c2ea1c6d-87ff-4890-85d7-db834a55cf1e", new Guid("11111111-0000-0000-0000-000000000002"), new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), null, false, "Sofia", "Bergstrom", false, null, null, null, null, null, false, "Student", null, false, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), null },
                    { new Guid("44444444-0000-0000-0000-000000000012"), 0, "09fa74cb-b7c6-4c36-8f6c-b23094594822", new Guid("11111111-0000-0000-0000-000000000002"), new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), null, false, "Anders", "Nystrom", false, null, null, null, null, null, false, "Student", null, false, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), null },
                    { new Guid("44444444-0000-0000-0000-000000000013"), 0, "8b5e418d-9348-4f50-aaa1-7a343c1cdaea", new Guid("11111111-0000-0000-0000-000000000002"), new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), null, false, "Elin", "Forsberg", false, null, null, null, null, null, false, "Student", null, false, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), null },
                    { new Guid("44444444-0000-0000-0000-000000000014"), 0, "ea445ccf-fbef-43b4-acc1-ffe255d95097", new Guid("11111111-0000-0000-0000-000000000003"), new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), null, false, "Fredrik", "Dahl", false, null, null, null, null, null, false, "Student", null, false, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), null },
                    { new Guid("44444444-0000-0000-0000-000000000015"), 0, "d004077b-65a6-4a0e-87aa-d37d8df1570c", new Guid("11111111-0000-0000-0000-000000000003"), new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), null, false, "Nina", "Ekstrom", false, null, null, null, null, null, false, "Student", null, false, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), null },
                    { new Guid("44444444-0000-0000-0000-000000000016"), 0, "ffb50c6e-e744-49af-9328-511be1055c17", new Guid("11111111-0000-0000-0000-000000000003"), new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), null, false, "Josefin", "Lund", false, null, null, null, null, null, false, "Student", null, false, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), null },
                    { new Guid("44444444-0000-0000-0000-000000000017"), 0, "01d6c3ba-34cb-426e-a711-c8a6b6ce29fc", new Guid("11111111-0000-0000-0000-000000000003"), new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), null, false, "Martin", "Oberg", false, null, null, null, null, null, false, "Student", null, false, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), null }
                });

            migrationBuilder.InsertData(
                schema: "identity",
                table: "Resources",
                columns: new[] { "Id", "CreatedAt", "Data", "Description", "Name", "OwnerId", "ResourceType", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("55555555-0000-0000-0000-000000000011"), new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "https://github.com/maria-svensson/git-workflow-assignment", "Maria Svensson's submitted solution for the Git branching and merging workflow assignment.", "Git Workflow Assignment Submission", new Guid("44444444-0000-0000-0000-000000000002"), 2, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("55555555-0000-0000-0000-000000000015"), new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "https://github.com/johan-berg/git-workflow-assignment", "Johan Berg's submitted solution for the Git branching and merging workflow assignment.", "Git Workflow Assignment Submission", new Guid("44444444-0000-0000-0000-000000000003"), 2, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("55555555-0000-0000-0000-000000000022"), new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "https://github.com/maria-svensson/personal-portfolio-page", "Maria Svensson's submitted solution for the personal portfolio page assignment.", "Personal Portfolio Page Submission", new Guid("44444444-0000-0000-0000-000000000002"), 2, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("55555555-0000-0000-0000-000000000026"), new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "https://github.com/maria-svensson/todo-app-assignment", "Maria Svensson's submitted solution for the React todo app assignment.", "Todo App Assignment Submission", new Guid("44444444-0000-0000-0000-000000000002"), 2, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("55555555-0000-0000-0000-000000000030"), new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "https://github.com/johan-berg/crud-api-assignment", "Johan Berg's submitted solution for the CRUD API assignment.", "CRUD API Assignment Submission", new Guid("44444444-0000-0000-0000-000000000003"), 2, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("55555555-0000-0000-0000-000000000034"), new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "https://github.com/sara-lindqvist/dockerize-an-application", "Sara Lindqvist's submitted solution for the dockerize-an-application assignment.", "Dockerize an Application Submission", new Guid("44444444-0000-0000-0000-000000000004"), 2, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("55555555-0000-0000-0000-000000000038"), new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "https://github.com/sara-lindqvist/deployment-pipeline-assignment", "Sara Lindqvist's submitted solution for the CI/CD deployment pipeline assignment.", "Deployment Pipeline Assignment Submission", new Guid("44444444-0000-0000-0000-000000000004"), 2, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                schema: "identity",
                table: "ActivityResources",
                columns: new[] { "Id", "ActivityId", "ResourceId" },
                values: new object[,]
                {
                    { new Guid("99999999-0000-0000-0000-000000000005"), new Guid("66666666-0000-0000-0000-000000000010"), new Guid("55555555-0000-0000-0000-000000000011") },
                    { new Guid("99999999-0000-0000-0000-000000000009"), new Guid("66666666-0000-0000-0000-000000000010"), new Guid("55555555-0000-0000-0000-000000000015") },
                    { new Guid("99999999-0000-0000-0000-000000000016"), new Guid("66666666-0000-0000-0000-000000000013"), new Guid("55555555-0000-0000-0000-000000000022") },
                    { new Guid("99999999-0000-0000-0000-000000000020"), new Guid("66666666-0000-0000-0000-000000000016"), new Guid("55555555-0000-0000-0000-000000000026") },
                    { new Guid("99999999-0000-0000-0000-000000000024"), new Guid("66666666-0000-0000-0000-000000000019"), new Guid("55555555-0000-0000-0000-000000000030") },
                    { new Guid("99999999-0000-0000-0000-000000000028"), new Guid("66666666-0000-0000-0000-000000000021"), new Guid("55555555-0000-0000-0000-000000000034") },
                    { new Guid("99999999-0000-0000-0000-000000000032"), new Guid("66666666-0000-0000-0000-000000000024"), new Guid("55555555-0000-0000-0000-000000000038") }
                });

            migrationBuilder.InsertData(
                schema: "identity",
                table: "Resources",
                columns: new[] { "Id", "CreatedAt", "Data", "Description", "Name", "OwnerId", "ResourceType", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("55555555-0000-0000-0000-000000000012"), new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "https://github.com/emma-karlsson/git-workflow-assignment", "Emma Karlsson's submitted solution for the Git branching and merging workflow assignment.", "Git Workflow Assignment Submission", new Guid("44444444-0000-0000-0000-000000000006"), 2, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("55555555-0000-0000-0000-000000000013"), new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "https://github.com/oskar-lindberg/git-workflow-assignment", "Oskar Lindberg's submitted solution for the Git branching and merging workflow assignment.", "Git Workflow Assignment Submission", new Guid("44444444-0000-0000-0000-000000000007"), 2, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("55555555-0000-0000-0000-000000000014"), new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "https://github.com/lina-hakansson/git-workflow-assignment", "Lina Hakansson's submitted solution for the Git branching and merging workflow assignment.", "Git Workflow Assignment Submission", new Guid("44444444-0000-0000-0000-000000000008"), 2, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("55555555-0000-0000-0000-000000000016"), new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "https://github.com/erik-holm/git-workflow-assignment", "Erik Holm's submitted solution for the Git branching and merging workflow assignment.", "Git Workflow Assignment Submission", new Guid("44444444-0000-0000-0000-000000000010"), 2, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("55555555-0000-0000-0000-000000000017"), new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "https://github.com/sofia-bergstrom/git-workflow-assignment", "Sofia Bergstrom's submitted solution for the Git branching and merging workflow assignment.", "Git Workflow Assignment Submission", new Guid("44444444-0000-0000-0000-000000000011"), 2, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("55555555-0000-0000-0000-000000000018"), new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "https://github.com/anders-nystrom/git-workflow-assignment", "Anders Nystrom's submitted solution for the Git branching and merging workflow assignment.", "Git Workflow Assignment Submission", new Guid("44444444-0000-0000-0000-000000000012"), 2, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("55555555-0000-0000-0000-000000000019"), new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "https://github.com/erik-holm/console-app-assignment", "Erik Holm's submitted solution for the console application assignment.", "Console App Assignment Submission", new Guid("44444444-0000-0000-0000-000000000010"), 2, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("55555555-0000-0000-0000-000000000020"), new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "https://github.com/sofia-bergstrom/console-app-assignment", "Sofia Bergstrom's submitted solution for the console application assignment.", "Console App Assignment Submission", new Guid("44444444-0000-0000-0000-000000000011"), 2, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("55555555-0000-0000-0000-000000000021"), new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "https://github.com/anders-nystrom/console-app-assignment", "Anders Nystrom's submitted solution for the console application assignment.", "Console App Assignment Submission", new Guid("44444444-0000-0000-0000-000000000012"), 2, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("55555555-0000-0000-0000-000000000023"), new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "https://github.com/emma-karlsson/personal-portfolio-page", "Emma Karlsson's submitted solution for the personal portfolio page assignment.", "Personal Portfolio Page Submission", new Guid("44444444-0000-0000-0000-000000000006"), 2, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("55555555-0000-0000-0000-000000000024"), new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "https://github.com/oskar-lindberg/personal-portfolio-page", "Oskar Lindberg's submitted solution for the personal portfolio page assignment.", "Personal Portfolio Page Submission", new Guid("44444444-0000-0000-0000-000000000007"), 2, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("55555555-0000-0000-0000-000000000025"), new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "https://github.com/lina-hakansson/personal-portfolio-page", "Lina Hakansson's submitted solution for the personal portfolio page assignment.", "Personal Portfolio Page Submission", new Guid("44444444-0000-0000-0000-000000000008"), 2, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("55555555-0000-0000-0000-000000000027"), new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "https://github.com/emma-karlsson/todo-app-assignment", "Emma Karlsson's submitted solution for the React todo app assignment.", "Todo App Assignment Submission", new Guid("44444444-0000-0000-0000-000000000006"), 2, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("55555555-0000-0000-0000-000000000028"), new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "https://github.com/oskar-lindberg/todo-app-assignment", "Oskar Lindberg's submitted solution for the React todo app assignment.", "Todo App Assignment Submission", new Guid("44444444-0000-0000-0000-000000000007"), 2, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("55555555-0000-0000-0000-000000000029"), new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "https://github.com/viktor-astrom/todo-app-assignment", "Viktor Astrom's submitted solution for the React todo app assignment.", "Todo App Assignment Submission", new Guid("44444444-0000-0000-0000-000000000009"), 2, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("55555555-0000-0000-0000-000000000031"), new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "https://github.com/erik-holm/crud-api-assignment", "Erik Holm's submitted solution for the CRUD API assignment.", "CRUD API Assignment Submission", new Guid("44444444-0000-0000-0000-000000000010"), 2, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("55555555-0000-0000-0000-000000000032"), new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "https://github.com/anders-nystrom/crud-api-assignment", "Anders Nystrom's submitted solution for the CRUD API assignment.", "CRUD API Assignment Submission", new Guid("44444444-0000-0000-0000-000000000012"), 2, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("55555555-0000-0000-0000-000000000033"), new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "https://github.com/elin-forsberg/crud-api-assignment", "Elin Forsberg's submitted solution for the CRUD API assignment.", "CRUD API Assignment Submission", new Guid("44444444-0000-0000-0000-000000000013"), 2, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("55555555-0000-0000-0000-000000000035"), new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "https://github.com/fredrik-dahl/dockerize-an-application", "Fredrik Dahl's submitted solution for the dockerize-an-application assignment.", "Dockerize an Application Submission", new Guid("44444444-0000-0000-0000-000000000014"), 2, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("55555555-0000-0000-0000-000000000036"), new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "https://github.com/nina-ekstrom/dockerize-an-application", "Nina Ekstrom's submitted solution for the dockerize-an-application assignment.", "Dockerize an Application Submission", new Guid("44444444-0000-0000-0000-000000000015"), 2, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("55555555-0000-0000-0000-000000000037"), new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "https://github.com/josefin-lund/dockerize-an-application", "Josefin Lund's submitted solution for the dockerize-an-application assignment.", "Dockerize an Application Submission", new Guid("44444444-0000-0000-0000-000000000016"), 2, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("55555555-0000-0000-0000-000000000039"), new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "https://github.com/fredrik-dahl/deployment-pipeline-assignment", "Fredrik Dahl's submitted solution for the CI/CD deployment pipeline assignment.", "Deployment Pipeline Assignment Submission", new Guid("44444444-0000-0000-0000-000000000014"), 2, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("55555555-0000-0000-0000-000000000040"), new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "https://github.com/josefin-lund/deployment-pipeline-assignment", "Josefin Lund's submitted solution for the CI/CD deployment pipeline assignment.", "Deployment Pipeline Assignment Submission", new Guid("44444444-0000-0000-0000-000000000016"), 2, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("55555555-0000-0000-0000-000000000041"), new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "https://github.com/martin-oberg/deployment-pipeline-assignment", "Martin Oberg's submitted solution for the CI/CD deployment pipeline assignment.", "Deployment Pipeline Assignment Submission", new Guid("44444444-0000-0000-0000-000000000017"), 2, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                schema: "identity",
                table: "ActivityResources",
                columns: new[] { "Id", "ActivityId", "ResourceId" },
                values: new object[,]
                {
                    { new Guid("99999999-0000-0000-0000-000000000006"), new Guid("66666666-0000-0000-0000-000000000010"), new Guid("55555555-0000-0000-0000-000000000012") },
                    { new Guid("99999999-0000-0000-0000-000000000007"), new Guid("66666666-0000-0000-0000-000000000010"), new Guid("55555555-0000-0000-0000-000000000013") },
                    { new Guid("99999999-0000-0000-0000-000000000008"), new Guid("66666666-0000-0000-0000-000000000010"), new Guid("55555555-0000-0000-0000-000000000014") },
                    { new Guid("99999999-0000-0000-0000-000000000010"), new Guid("66666666-0000-0000-0000-000000000010"), new Guid("55555555-0000-0000-0000-000000000016") },
                    { new Guid("99999999-0000-0000-0000-000000000011"), new Guid("66666666-0000-0000-0000-000000000010"), new Guid("55555555-0000-0000-0000-000000000017") },
                    { new Guid("99999999-0000-0000-0000-000000000012"), new Guid("66666666-0000-0000-0000-000000000010"), new Guid("55555555-0000-0000-0000-000000000018") },
                    { new Guid("99999999-0000-0000-0000-000000000013"), new Guid("66666666-0000-0000-0000-000000000007"), new Guid("55555555-0000-0000-0000-000000000019") },
                    { new Guid("99999999-0000-0000-0000-000000000014"), new Guid("66666666-0000-0000-0000-000000000007"), new Guid("55555555-0000-0000-0000-000000000020") },
                    { new Guid("99999999-0000-0000-0000-000000000015"), new Guid("66666666-0000-0000-0000-000000000007"), new Guid("55555555-0000-0000-0000-000000000021") },
                    { new Guid("99999999-0000-0000-0000-000000000017"), new Guid("66666666-0000-0000-0000-000000000013"), new Guid("55555555-0000-0000-0000-000000000023") },
                    { new Guid("99999999-0000-0000-0000-000000000018"), new Guid("66666666-0000-0000-0000-000000000013"), new Guid("55555555-0000-0000-0000-000000000024") },
                    { new Guid("99999999-0000-0000-0000-000000000019"), new Guid("66666666-0000-0000-0000-000000000013"), new Guid("55555555-0000-0000-0000-000000000025") },
                    { new Guid("99999999-0000-0000-0000-000000000021"), new Guid("66666666-0000-0000-0000-000000000016"), new Guid("55555555-0000-0000-0000-000000000027") },
                    { new Guid("99999999-0000-0000-0000-000000000022"), new Guid("66666666-0000-0000-0000-000000000016"), new Guid("55555555-0000-0000-0000-000000000028") },
                    { new Guid("99999999-0000-0000-0000-000000000023"), new Guid("66666666-0000-0000-0000-000000000016"), new Guid("55555555-0000-0000-0000-000000000029") },
                    { new Guid("99999999-0000-0000-0000-000000000025"), new Guid("66666666-0000-0000-0000-000000000019"), new Guid("55555555-0000-0000-0000-000000000031") },
                    { new Guid("99999999-0000-0000-0000-000000000026"), new Guid("66666666-0000-0000-0000-000000000019"), new Guid("55555555-0000-0000-0000-000000000032") },
                    { new Guid("99999999-0000-0000-0000-000000000027"), new Guid("66666666-0000-0000-0000-000000000019"), new Guid("55555555-0000-0000-0000-000000000033") },
                    { new Guid("99999999-0000-0000-0000-000000000029"), new Guid("66666666-0000-0000-0000-000000000021"), new Guid("55555555-0000-0000-0000-000000000035") },
                    { new Guid("99999999-0000-0000-0000-000000000030"), new Guid("66666666-0000-0000-0000-000000000021"), new Guid("55555555-0000-0000-0000-000000000036") },
                    { new Guid("99999999-0000-0000-0000-000000000031"), new Guid("66666666-0000-0000-0000-000000000021"), new Guid("55555555-0000-0000-0000-000000000037") },
                    { new Guid("99999999-0000-0000-0000-000000000033"), new Guid("66666666-0000-0000-0000-000000000024"), new Guid("55555555-0000-0000-0000-000000000039") },
                    { new Guid("99999999-0000-0000-0000-000000000034"), new Guid("66666666-0000-0000-0000-000000000024"), new Guid("55555555-0000-0000-0000-000000000040") },
                    { new Guid("99999999-0000-0000-0000-000000000035"), new Guid("66666666-0000-0000-0000-000000000024"), new Guid("55555555-0000-0000-0000-000000000041") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "identity",
                table: "Activities",
                keyColumn: "Id",
                keyValue: new Guid("66666666-0000-0000-0000-000000000011"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "Activities",
                keyColumn: "Id",
                keyValue: new Guid("66666666-0000-0000-0000-000000000012"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "Activities",
                keyColumn: "Id",
                keyValue: new Guid("66666666-0000-0000-0000-000000000014"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "Activities",
                keyColumn: "Id",
                keyValue: new Guid("66666666-0000-0000-0000-000000000015"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "Activities",
                keyColumn: "Id",
                keyValue: new Guid("66666666-0000-0000-0000-000000000017"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "Activities",
                keyColumn: "Id",
                keyValue: new Guid("66666666-0000-0000-0000-000000000018"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "Activities",
                keyColumn: "Id",
                keyValue: new Guid("66666666-0000-0000-0000-000000000020"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "Activities",
                keyColumn: "Id",
                keyValue: new Guid("66666666-0000-0000-0000-000000000022"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "Activities",
                keyColumn: "Id",
                keyValue: new Guid("66666666-0000-0000-0000-000000000023"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "ActivityResources",
                keyColumn: "Id",
                keyValue: new Guid("99999999-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "ActivityResources",
                keyColumn: "Id",
                keyValue: new Guid("99999999-0000-0000-0000-000000000006"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "ActivityResources",
                keyColumn: "Id",
                keyValue: new Guid("99999999-0000-0000-0000-000000000007"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "ActivityResources",
                keyColumn: "Id",
                keyValue: new Guid("99999999-0000-0000-0000-000000000008"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "ActivityResources",
                keyColumn: "Id",
                keyValue: new Guid("99999999-0000-0000-0000-000000000009"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "ActivityResources",
                keyColumn: "Id",
                keyValue: new Guid("99999999-0000-0000-0000-000000000010"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "ActivityResources",
                keyColumn: "Id",
                keyValue: new Guid("99999999-0000-0000-0000-000000000011"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "ActivityResources",
                keyColumn: "Id",
                keyValue: new Guid("99999999-0000-0000-0000-000000000012"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "ActivityResources",
                keyColumn: "Id",
                keyValue: new Guid("99999999-0000-0000-0000-000000000013"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "ActivityResources",
                keyColumn: "Id",
                keyValue: new Guid("99999999-0000-0000-0000-000000000014"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "ActivityResources",
                keyColumn: "Id",
                keyValue: new Guid("99999999-0000-0000-0000-000000000015"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "ActivityResources",
                keyColumn: "Id",
                keyValue: new Guid("99999999-0000-0000-0000-000000000016"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "ActivityResources",
                keyColumn: "Id",
                keyValue: new Guid("99999999-0000-0000-0000-000000000017"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "ActivityResources",
                keyColumn: "Id",
                keyValue: new Guid("99999999-0000-0000-0000-000000000018"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "ActivityResources",
                keyColumn: "Id",
                keyValue: new Guid("99999999-0000-0000-0000-000000000019"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "ActivityResources",
                keyColumn: "Id",
                keyValue: new Guid("99999999-0000-0000-0000-000000000020"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "ActivityResources",
                keyColumn: "Id",
                keyValue: new Guid("99999999-0000-0000-0000-000000000021"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "ActivityResources",
                keyColumn: "Id",
                keyValue: new Guid("99999999-0000-0000-0000-000000000022"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "ActivityResources",
                keyColumn: "Id",
                keyValue: new Guid("99999999-0000-0000-0000-000000000023"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "ActivityResources",
                keyColumn: "Id",
                keyValue: new Guid("99999999-0000-0000-0000-000000000024"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "ActivityResources",
                keyColumn: "Id",
                keyValue: new Guid("99999999-0000-0000-0000-000000000025"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "ActivityResources",
                keyColumn: "Id",
                keyValue: new Guid("99999999-0000-0000-0000-000000000026"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "ActivityResources",
                keyColumn: "Id",
                keyValue: new Guid("99999999-0000-0000-0000-000000000027"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "ActivityResources",
                keyColumn: "Id",
                keyValue: new Guid("99999999-0000-0000-0000-000000000028"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "ActivityResources",
                keyColumn: "Id",
                keyValue: new Guid("99999999-0000-0000-0000-000000000029"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "ActivityResources",
                keyColumn: "Id",
                keyValue: new Guid("99999999-0000-0000-0000-000000000030"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "ActivityResources",
                keyColumn: "Id",
                keyValue: new Guid("99999999-0000-0000-0000-000000000031"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "ActivityResources",
                keyColumn: "Id",
                keyValue: new Guid("99999999-0000-0000-0000-000000000032"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "ActivityResources",
                keyColumn: "Id",
                keyValue: new Guid("99999999-0000-0000-0000-000000000033"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "ActivityResources",
                keyColumn: "Id",
                keyValue: new Guid("99999999-0000-0000-0000-000000000034"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "ActivityResources",
                keyColumn: "Id",
                keyValue: new Guid("99999999-0000-0000-0000-000000000035"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "Activities",
                keyColumn: "Id",
                keyValue: new Guid("66666666-0000-0000-0000-000000000010"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "Activities",
                keyColumn: "Id",
                keyValue: new Guid("66666666-0000-0000-0000-000000000013"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "Activities",
                keyColumn: "Id",
                keyValue: new Guid("66666666-0000-0000-0000-000000000016"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "Activities",
                keyColumn: "Id",
                keyValue: new Guid("66666666-0000-0000-0000-000000000019"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "Activities",
                keyColumn: "Id",
                keyValue: new Guid("66666666-0000-0000-0000-000000000021"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "Activities",
                keyColumn: "Id",
                keyValue: new Guid("66666666-0000-0000-0000-000000000024"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "Resources",
                keyColumn: "Id",
                keyValue: new Guid("55555555-0000-0000-0000-000000000011"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "Resources",
                keyColumn: "Id",
                keyValue: new Guid("55555555-0000-0000-0000-000000000012"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "Resources",
                keyColumn: "Id",
                keyValue: new Guid("55555555-0000-0000-0000-000000000013"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "Resources",
                keyColumn: "Id",
                keyValue: new Guid("55555555-0000-0000-0000-000000000014"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "Resources",
                keyColumn: "Id",
                keyValue: new Guid("55555555-0000-0000-0000-000000000015"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "Resources",
                keyColumn: "Id",
                keyValue: new Guid("55555555-0000-0000-0000-000000000016"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "Resources",
                keyColumn: "Id",
                keyValue: new Guid("55555555-0000-0000-0000-000000000017"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "Resources",
                keyColumn: "Id",
                keyValue: new Guid("55555555-0000-0000-0000-000000000018"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "Resources",
                keyColumn: "Id",
                keyValue: new Guid("55555555-0000-0000-0000-000000000019"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "Resources",
                keyColumn: "Id",
                keyValue: new Guid("55555555-0000-0000-0000-000000000020"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "Resources",
                keyColumn: "Id",
                keyValue: new Guid("55555555-0000-0000-0000-000000000021"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "Resources",
                keyColumn: "Id",
                keyValue: new Guid("55555555-0000-0000-0000-000000000022"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "Resources",
                keyColumn: "Id",
                keyValue: new Guid("55555555-0000-0000-0000-000000000023"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "Resources",
                keyColumn: "Id",
                keyValue: new Guid("55555555-0000-0000-0000-000000000024"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "Resources",
                keyColumn: "Id",
                keyValue: new Guid("55555555-0000-0000-0000-000000000025"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "Resources",
                keyColumn: "Id",
                keyValue: new Guid("55555555-0000-0000-0000-000000000026"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "Resources",
                keyColumn: "Id",
                keyValue: new Guid("55555555-0000-0000-0000-000000000027"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "Resources",
                keyColumn: "Id",
                keyValue: new Guid("55555555-0000-0000-0000-000000000028"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "Resources",
                keyColumn: "Id",
                keyValue: new Guid("55555555-0000-0000-0000-000000000029"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "Resources",
                keyColumn: "Id",
                keyValue: new Guid("55555555-0000-0000-0000-000000000030"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "Resources",
                keyColumn: "Id",
                keyValue: new Guid("55555555-0000-0000-0000-000000000031"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "Resources",
                keyColumn: "Id",
                keyValue: new Guid("55555555-0000-0000-0000-000000000032"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "Resources",
                keyColumn: "Id",
                keyValue: new Guid("55555555-0000-0000-0000-000000000033"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "Resources",
                keyColumn: "Id",
                keyValue: new Guid("55555555-0000-0000-0000-000000000034"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "Resources",
                keyColumn: "Id",
                keyValue: new Guid("55555555-0000-0000-0000-000000000035"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "Resources",
                keyColumn: "Id",
                keyValue: new Guid("55555555-0000-0000-0000-000000000036"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "Resources",
                keyColumn: "Id",
                keyValue: new Guid("55555555-0000-0000-0000-000000000037"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "Resources",
                keyColumn: "Id",
                keyValue: new Guid("55555555-0000-0000-0000-000000000038"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "Resources",
                keyColumn: "Id",
                keyValue: new Guid("55555555-0000-0000-0000-000000000039"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "Resources",
                keyColumn: "Id",
                keyValue: new Guid("55555555-0000-0000-0000-000000000040"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "Resources",
                keyColumn: "Id",
                keyValue: new Guid("55555555-0000-0000-0000-000000000041"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("44444444-0000-0000-0000-000000000006"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("44444444-0000-0000-0000-000000000007"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("44444444-0000-0000-0000-000000000008"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("44444444-0000-0000-0000-000000000009"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("44444444-0000-0000-0000-000000000010"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("44444444-0000-0000-0000-000000000011"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("44444444-0000-0000-0000-000000000012"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("44444444-0000-0000-0000-000000000013"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("44444444-0000-0000-0000-000000000014"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("44444444-0000-0000-0000-000000000015"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("44444444-0000-0000-0000-000000000016"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("44444444-0000-0000-0000-000000000017"));
        }
    }
}
