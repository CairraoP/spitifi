using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace spitifi.Migrations
{
    /// <inheritdoc />
    public partial class paracetamolOverdose2000 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEDwUwLW0hDDRIZ8Aij8OMm/cvNqc5rP39aSxePBemMzqN6XlaLYvPs9GdehmJCqJCQ==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "jonas",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAELq2QajnmisEec4NytoR6vOZBrnfxfPZv4nRiq02a+3eFdmU4l1yvQvb/AsqWtsYMA==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEFlD4rWdfnejmxUQKtzTOPTmPam7Yu/J72fGU+oiFQTk2TgMCMRBNFng7zZ2SF0xpA==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "jonas",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEFKLw0uBBh9txvVxKuA+BqRA+fJNCa7MkdBTAnk7uj5/ZB/NyYA1N5befK1jj2FjnQ==");
        }
    }
}
