using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Lms_backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedExampleData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ApplicationUser",
                columns: new[] { "UserId", "CourseId", "CreatedAt", "GivenName", "LastName", "UpdatedAt" },
                values: new object[] { new Guid("44444444-0000-0000-0000-000000000001"), null, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "Alex", "Nilsson", new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "Id", "CreatedAt", "Description", "Duration", "Name", "StartDate", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("11111111-0000-0000-0000-000000000001"), new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "Learn to build modern web applications end-to-end, from responsive front-ends to REST APIs and databases.", 12, "Full-Stack Web Development", new DateOnly(2026, 2, 2), new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("11111111-0000-0000-0000-000000000002"), new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "Deep dive into C#, object-oriented design, and building robust web APIs with ASP.NET Core and EF Core.", 10, "Backend Development with C# & .NET", new DateOnly(2026, 2, 2), new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("11111111-0000-0000-0000-000000000003"), new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "Containerize, automate, and deploy applications using Docker, CI/CD pipelines, and cloud platforms.", 8, "Cloud & DevOps Engineering", new DateOnly(2026, 3, 2), new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "Modules",
                columns: new[] { "Id", "CreatedAt", "Description", "Duration", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("22222222-0000-0000-0000-000000000001"), new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "Version control fundamentals: repositories, commits, branching, merging, and collaborative workflows.", 1, "Git & Version Control", new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("22222222-0000-0000-0000-000000000002"), new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "Core building blocks of the web: semantic HTML, CSS layout & styling, and JavaScript basics.", 3, "Frontend Fundamentals", new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("22222222-0000-0000-0000-000000000003"), new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "Building interactive user interfaces with components, props, state, and hooks in React.", 3, "React Fundamentals", new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("22222222-0000-0000-0000-000000000004"), new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "Core C# syntax, types, control flow, and object-oriented programming concepts.", 3, "C# Language Fundamentals", new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("22222222-0000-0000-0000-000000000005"), new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "Building REST APIs with ASP.NET Core and persisting data with Entity Framework Core.", 4, "ASP.NET Core & EF Core", new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("22222222-0000-0000-0000-000000000006"), new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "Packaging and running applications in containers using Docker images and Compose.", 2, "Containers with Docker", new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("22222222-0000-0000-0000-000000000007"), new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "Automating build, test, and deployment pipelines and deploying to cloud platforms.", 3, "CI/CD & Cloud Deployment", new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "Activities",
                columns: new[] { "Id", "ActivityType", "CreatedAt", "Description", "DurationMinutes", "ModuleId", "Name", "StartTimeOffset", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("66666666-0000-0000-0000-000000000001"), 0, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "Overview of version control concepts and setting up your first Git repository.", 60, new Guid("22222222-0000-0000-0000-000000000001"), "Introduction to Git", 0, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("66666666-0000-0000-0000-000000000002"), 2, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "Hands-on practice creating branches, merging changes, and resolving conflicts.", 90, new Guid("22222222-0000-0000-0000-000000000001"), "Git Branching Exercise", 60, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("66666666-0000-0000-0000-000000000003"), 1, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "Self-paced reading covering Git basics, branching, and the Git workflow.", 120, new Guid("22222222-0000-0000-0000-000000000001"), "Read Pro Git Book (Ch. 1-3)", 30, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("66666666-0000-0000-0000-000000000004"), 0, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "Variables, data types, operators, and control flow in C#.", 90, new Guid("22222222-0000-0000-0000-000000000004"), "C# Syntax & Types", 0, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("66666666-0000-0000-0000-000000000005"), 0, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "Classes, objects, inheritance, interfaces, and encapsulation.", 90, new Guid("22222222-0000-0000-0000-000000000004"), "Object-Oriented Programming in C#", 90, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("66666666-0000-0000-0000-000000000006"), 2, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "Practice designing classes and interfaces for a small console application.", 120, new Guid("22222222-0000-0000-0000-000000000004"), "OOP Practice Exercise", 60, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("66666666-0000-0000-0000-000000000007"), 3, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "Build a small console application applying the OOP principles covered in this module.", 180, new Guid("22222222-0000-0000-0000-000000000004"), "Console App Assignment", 180, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("66666666-0000-0000-0000-000000000008"), 0, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "Images, containers, volumes, and networking basics with Docker.", 60, new Guid("22222222-0000-0000-0000-000000000006"), "Docker Fundamentals", 0, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("66666666-0000-0000-0000-000000000009"), 4, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "Group review and feedback on Dockerfiles written for the sample project.", 45, new Guid("22222222-0000-0000-0000-000000000006"), "Dockerfile Review Session", 45, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "ApplicationUser",
                columns: new[] { "UserId", "CourseId", "CreatedAt", "GivenName", "LastName", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("44444444-0000-0000-0000-000000000002"), new Guid("11111111-0000-0000-0000-000000000001"), new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "Maria", "Svensson", new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("44444444-0000-0000-0000-000000000003"), new Guid("11111111-0000-0000-0000-000000000002"), new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "Johan", "Berg", new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("44444444-0000-0000-0000-000000000004"), new Guid("11111111-0000-0000-0000-000000000003"), new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "Sara", "Lindqvist", new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "CourseModules",
                columns: new[] { "Id", "CourseId", "ModuleId", "StartTimeOffset" },
                values: new object[,]
                {
                    { new Guid("33333333-0000-0000-0000-000000000001"), new Guid("11111111-0000-0000-0000-000000000001"), new Guid("22222222-0000-0000-0000-000000000001"), 0 },
                    { new Guid("33333333-0000-0000-0000-000000000002"), new Guid("11111111-0000-0000-0000-000000000001"), new Guid("22222222-0000-0000-0000-000000000002"), 7 },
                    { new Guid("33333333-0000-0000-0000-000000000003"), new Guid("11111111-0000-0000-0000-000000000001"), new Guid("22222222-0000-0000-0000-000000000003"), 28 },
                    { new Guid("33333333-0000-0000-0000-000000000004"), new Guid("11111111-0000-0000-0000-000000000002"), new Guid("22222222-0000-0000-0000-000000000001"), 0 },
                    { new Guid("33333333-0000-0000-0000-000000000005"), new Guid("11111111-0000-0000-0000-000000000002"), new Guid("22222222-0000-0000-0000-000000000004"), 7 },
                    { new Guid("33333333-0000-0000-0000-000000000006"), new Guid("11111111-0000-0000-0000-000000000002"), new Guid("22222222-0000-0000-0000-000000000005"), 28 },
                    { new Guid("33333333-0000-0000-0000-000000000007"), new Guid("11111111-0000-0000-0000-000000000003"), new Guid("22222222-0000-0000-0000-000000000006"), 0 },
                    { new Guid("33333333-0000-0000-0000-000000000008"), new Guid("11111111-0000-0000-0000-000000000003"), new Guid("22222222-0000-0000-0000-000000000007"), 14 }
                });

            migrationBuilder.InsertData(
                table: "Resources",
                columns: new[] { "ResourceId", "CreatedAt", "Data", "Description", "Name", "OwnerId", "ResourceType", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("55555555-0000-0000-0000-000000000001"), new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "https://git-scm.com/book/en/v2", "Free online book covering everything from Git basics to advanced workflows.", "Pro Git Book", new Guid("44444444-0000-0000-0000-000000000001"), 1, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("55555555-0000-0000-0000-000000000002"), new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "git init | git add . | git commit -m \"msg\" | git branch <name> | git checkout <name> | git merge <name> | git status | git log", "Quick reference for common Git commands.", "Git Cheat Sheet", new Guid("44444444-0000-0000-0000-000000000001"), 0, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("55555555-0000-0000-0000-000000000003"), new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "https://developer.mozilla.org/en-US/docs/Web/JavaScript", "Comprehensive reference and guides for HTML, CSS, and JavaScript.", "MDN Web Docs - JavaScript", new Guid("44444444-0000-0000-0000-000000000001"), 1, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("55555555-0000-0000-0000-000000000004"), new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "Use PascalCase for classes/methods, camelCase for locals/params, prefix interfaces with 'I', keep methods short and single-purpose.", "Notes on naming, formatting, and style conventions used in this course.", "C# Coding Conventions", new Guid("44444444-0000-0000-0000-000000000001"), 0, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("55555555-0000-0000-0000-000000000005"), new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "https://learn.microsoft.com/aspnet/core", "Official Microsoft documentation and tutorials for ASP.NET Core.", "Microsoft Learn - ASP.NET Core", new Guid("44444444-0000-0000-0000-000000000001"), 1, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("55555555-0000-0000-0000-000000000006"), new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "https://docs.docker.com/", "Official documentation for Docker Engine, images, and Compose.", "Docker Official Docs", new Guid("44444444-0000-0000-0000-000000000001"), 1, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("55555555-0000-0000-0000-000000000007"), new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "1. Define an interface IShape with an Area() method. 2. Implement Circle and Rectangle. 3. Compute the total area of a list of shapes.", "Step-by-step instructions for the object-oriented programming exercise.", "OOP Practice Instructions", new Guid("44444444-0000-0000-0000-000000000001"), 0, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("55555555-0000-0000-0000-000000000008"), new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "Week 1: Git. Weeks 2-4: Frontend Fundamentals. Weeks 5-8: React. Weeks 9-12: Capstone project.", "Full syllabus and schedule for the Full-Stack Web Development course.", "Course Syllabus", new Guid("44444444-0000-0000-0000-000000000001"), 0, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "ActivityResources",
                columns: new[] { "Id", "ActivityId", "ResourceId" },
                values: new object[,]
                {
                    { new Guid("99999999-0000-0000-0000-000000000001"), new Guid("66666666-0000-0000-0000-000000000001"), new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("99999999-0000-0000-0000-000000000002"), new Guid("66666666-0000-0000-0000-000000000002"), new Guid("55555555-0000-0000-0000-000000000002") },
                    { new Guid("99999999-0000-0000-0000-000000000003"), new Guid("66666666-0000-0000-0000-000000000006"), new Guid("55555555-0000-0000-0000-000000000007") }
                });

            migrationBuilder.InsertData(
                table: "CourseResources",
                columns: new[] { "Id", "CourseId", "ResourceId" },
                values: new object[] { new Guid("77777777-0000-0000-0000-000000000001"), new Guid("11111111-0000-0000-0000-000000000001"), new Guid("55555555-0000-0000-0000-000000000008") });

            migrationBuilder.InsertData(
                table: "ModuleResources",
                columns: new[] { "Id", "ModuleId", "ResourceId" },
                values: new object[,]
                {
                    { new Guid("88888888-0000-0000-0000-000000000001"), new Guid("22222222-0000-0000-0000-000000000001"), new Guid("55555555-0000-0000-0000-000000000001") },
                    { new Guid("88888888-0000-0000-0000-000000000002"), new Guid("22222222-0000-0000-0000-000000000002"), new Guid("55555555-0000-0000-0000-000000000003") },
                    { new Guid("88888888-0000-0000-0000-000000000003"), new Guid("22222222-0000-0000-0000-000000000004"), new Guid("55555555-0000-0000-0000-000000000004") },
                    { new Guid("88888888-0000-0000-0000-000000000004"), new Guid("22222222-0000-0000-0000-000000000005"), new Guid("55555555-0000-0000-0000-000000000005") },
                    { new Guid("88888888-0000-0000-0000-000000000005"), new Guid("22222222-0000-0000-0000-000000000006"), new Guid("55555555-0000-0000-0000-000000000006") }
                });

            migrationBuilder.InsertData(
                table: "Resources",
                columns: new[] { "ResourceId", "CreatedAt", "Data", "Description", "Name", "OwnerId", "ResourceType", "UpdatedAt" },
                values: new object[] { new Guid("55555555-0000-0000-0000-000000000009"), new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "Remember: commit early and often. Use feature branches. Run 'git status' before every commit. Ask Alex about rebase vs merge.", "Personal notes from the Git & Version Control module.", "My Git Notes", new Guid("44444444-0000-0000-0000-000000000002"), 0, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.InsertData(
                table: "UserResource",
                columns: new[] { "Id", "ResourceId", "UserId" },
                values: new object[] { new Guid("aaaaaaaa-0000-0000-0000-000000000001"), new Guid("55555555-0000-0000-0000-000000000009"), new Guid("44444444-0000-0000-0000-000000000002") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: new Guid("66666666-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: new Guid("66666666-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: new Guid("66666666-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: new Guid("66666666-0000-0000-0000-000000000007"));

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: new Guid("66666666-0000-0000-0000-000000000008"));

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: new Guid("66666666-0000-0000-0000-000000000009"));

            migrationBuilder.DeleteData(
                table: "ActivityResources",
                keyColumn: "Id",
                keyValue: new Guid("99999999-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "ActivityResources",
                keyColumn: "Id",
                keyValue: new Guid("99999999-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "ActivityResources",
                keyColumn: "Id",
                keyValue: new Guid("99999999-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "ApplicationUser",
                keyColumn: "UserId",
                keyValue: new Guid("44444444-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "ApplicationUser",
                keyColumn: "UserId",
                keyValue: new Guid("44444444-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "CourseModules",
                keyColumn: "Id",
                keyValue: new Guid("33333333-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "CourseModules",
                keyColumn: "Id",
                keyValue: new Guid("33333333-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "CourseModules",
                keyColumn: "Id",
                keyValue: new Guid("33333333-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "CourseModules",
                keyColumn: "Id",
                keyValue: new Guid("33333333-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "CourseModules",
                keyColumn: "Id",
                keyValue: new Guid("33333333-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "CourseModules",
                keyColumn: "Id",
                keyValue: new Guid("33333333-0000-0000-0000-000000000006"));

            migrationBuilder.DeleteData(
                table: "CourseModules",
                keyColumn: "Id",
                keyValue: new Guid("33333333-0000-0000-0000-000000000007"));

            migrationBuilder.DeleteData(
                table: "CourseModules",
                keyColumn: "Id",
                keyValue: new Guid("33333333-0000-0000-0000-000000000008"));

            migrationBuilder.DeleteData(
                table: "CourseResources",
                keyColumn: "Id",
                keyValue: new Guid("77777777-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "ModuleResources",
                keyColumn: "Id",
                keyValue: new Guid("88888888-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "ModuleResources",
                keyColumn: "Id",
                keyValue: new Guid("88888888-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "ModuleResources",
                keyColumn: "Id",
                keyValue: new Guid("88888888-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "ModuleResources",
                keyColumn: "Id",
                keyValue: new Guid("88888888-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "ModuleResources",
                keyColumn: "Id",
                keyValue: new Guid("88888888-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "UserResource",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: new Guid("66666666-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: new Guid("66666666-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: new Guid("66666666-0000-0000-0000-000000000006"));

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: new Guid("11111111-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: new Guid("11111111-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "Id",
                keyValue: new Guid("22222222-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "Id",
                keyValue: new Guid("22222222-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "Id",
                keyValue: new Guid("22222222-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "Id",
                keyValue: new Guid("22222222-0000-0000-0000-000000000006"));

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "Id",
                keyValue: new Guid("22222222-0000-0000-0000-000000000007"));

            migrationBuilder.DeleteData(
                table: "Resources",
                keyColumn: "ResourceId",
                keyValue: new Guid("55555555-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Resources",
                keyColumn: "ResourceId",
                keyValue: new Guid("55555555-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Resources",
                keyColumn: "ResourceId",
                keyValue: new Guid("55555555-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "Resources",
                keyColumn: "ResourceId",
                keyValue: new Guid("55555555-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "Resources",
                keyColumn: "ResourceId",
                keyValue: new Guid("55555555-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "Resources",
                keyColumn: "ResourceId",
                keyValue: new Guid("55555555-0000-0000-0000-000000000006"));

            migrationBuilder.DeleteData(
                table: "Resources",
                keyColumn: "ResourceId",
                keyValue: new Guid("55555555-0000-0000-0000-000000000007"));

            migrationBuilder.DeleteData(
                table: "Resources",
                keyColumn: "ResourceId",
                keyValue: new Guid("55555555-0000-0000-0000-000000000008"));

            migrationBuilder.DeleteData(
                table: "Resources",
                keyColumn: "ResourceId",
                keyValue: new Guid("55555555-0000-0000-0000-000000000009"));

            migrationBuilder.DeleteData(
                table: "ApplicationUser",
                keyColumn: "UserId",
                keyValue: new Guid("44444444-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "ApplicationUser",
                keyColumn: "UserId",
                keyValue: new Guid("44444444-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "Id",
                keyValue: new Guid("22222222-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "Id",
                keyValue: new Guid("22222222-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: new Guid("11111111-0000-0000-0000-000000000001"));
        }
    }
}
