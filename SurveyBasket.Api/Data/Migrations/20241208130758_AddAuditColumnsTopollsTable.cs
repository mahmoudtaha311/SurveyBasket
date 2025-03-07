using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SurveyBasket.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAuditColumnsTopollsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "polls",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "polls",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedById",
                table: "polls",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "polls",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_polls_CreatedById",
                table: "polls",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_polls_UpdatedById",
                table: "polls",
                column: "UpdatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_polls_AspNetUsers_CreatedById",
                table: "polls",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_polls_AspNetUsers_UpdatedById",
                table: "polls",
                column: "UpdatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_polls_AspNetUsers_CreatedById",
                table: "polls");

            migrationBuilder.DropForeignKey(
                name: "FK_polls_AspNetUsers_UpdatedById",
                table: "polls");

            migrationBuilder.DropIndex(
                name: "IX_polls_CreatedById",
                table: "polls");

            migrationBuilder.DropIndex(
                name: "IX_polls_UpdatedById",
                table: "polls");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "polls");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "polls");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                table: "polls");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "polls");
        }
    }
}
