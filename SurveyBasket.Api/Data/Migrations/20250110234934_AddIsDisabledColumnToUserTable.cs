using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SurveyBasket.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddIsDisabledColumnToUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDisabled",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0bfe410c-6723-4bc5-b163-7469b0b8cb35",
                columns: new[] { "IsDisabled", "PasswordHash" },
                values: new object[] { false, "AQAAAAIAAYagAAAAECiYs5sHmjOKUm9Kxgn2sO0gfzFfInNmKeIyfnSQdOuE1hB2SzucNKQ9AwOIyWeTDQ==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDisabled",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0bfe410c-6723-4bc5-b163-7469b0b8cb35",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEGYk+yjRB+Q6cxS1jTNTseGs64zIPEQd7iOrLXS/li/e3TngGyi8Jo+5Th2soCHHyQ==");
        }
    }
}
