using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SurveyBasket.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class seedIdentityTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "IsDefault", "IsDeleted", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4097f06e-a127-4960-8a9b-e4cbba84bf4f", "0439e59f-86fb-4706-b2b4-de1e795fda32", true, false, "Member", "MEMBER" },
                    { "7507f5d6-d69d-4ce4-955e-49e5611f46dc", "c230c815-ddbd-420e-adf7-f270056c2d59", false, false, "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "0bfe410c-6723-4bc5-b163-7469b0b8cb35", 0, "c2a24c0c-6e74-4bce-858e-b190dbc77025", "Admin@survey-Basket.com", true, "Survey Basket", "Admin", false, null, "ADMIN@SURVEY-BASKET.COM", "ADMIN@SURVEY-BASKET.COM", "AQAAAAIAAYagAAAAEGYk+yjRB+Q6cxS1jTNTseGs64zIPEQd7iOrLXS/li/e3TngGyi8Jo+5Th2soCHHyQ==", null, false, "75a8ae76fbd443f99afcfd3883674e06", false, "Admin@survey-Basket.com" });

            migrationBuilder.InsertData(
                table: "AspNetRoleClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "RoleId" },
                values: new object[,]
                {
                    { 1, "permissions", "Polls:read", "7507f5d6-d69d-4ce4-955e-49e5611f46dc" },
                    { 2, "permissions", "Polls:add", "7507f5d6-d69d-4ce4-955e-49e5611f46dc" },
                    { 3, "permissions", "Polls:update", "7507f5d6-d69d-4ce4-955e-49e5611f46dc" },
                    { 4, "permissions", "Polls:delete", "7507f5d6-d69d-4ce4-955e-49e5611f46dc" },
                    { 5, "permissions", "Questions:read", "7507f5d6-d69d-4ce4-955e-49e5611f46dc" },
                    { 6, "permissions", "Questions:add", "7507f5d6-d69d-4ce4-955e-49e5611f46dc" },
                    { 7, "permissions", "Questions:update", "7507f5d6-d69d-4ce4-955e-49e5611f46dc" },
                    { 8, "permissions", "Users:read", "7507f5d6-d69d-4ce4-955e-49e5611f46dc" },
                    { 9, "permissions", "Users:add", "7507f5d6-d69d-4ce4-955e-49e5611f46dc" },
                    { 10, "permissions", "Users:update", "7507f5d6-d69d-4ce4-955e-49e5611f46dc" },
                    { 11, "permissions", "Roles:read", "7507f5d6-d69d-4ce4-955e-49e5611f46dc" },
                    { 12, "permissions", "Roles:add", "7507f5d6-d69d-4ce4-955e-49e5611f46dc" },
                    { 13, "permissions", "Roles:update", "7507f5d6-d69d-4ce4-955e-49e5611f46dc" },
                    { 14, "permissions", "Results:read", "7507f5d6-d69d-4ce4-955e-49e5611f46dc" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "7507f5d6-d69d-4ce4-955e-49e5611f46dc", "0bfe410c-6723-4bc5-b163-7469b0b8cb35" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4097f06e-a127-4960-8a9b-e4cbba84bf4f");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "7507f5d6-d69d-4ce4-955e-49e5611f46dc", "0bfe410c-6723-4bc5-b163-7469b0b8cb35" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7507f5d6-d69d-4ce4-955e-49e5611f46dc");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0bfe410c-6723-4bc5-b163-7469b0b8cb35");
        }
    }
}
