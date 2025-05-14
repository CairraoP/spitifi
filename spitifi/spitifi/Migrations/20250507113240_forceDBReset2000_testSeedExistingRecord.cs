using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace spitifi.Migrations
{
    /// <inheritdoc />
    public partial class forceDBReset2000_testSeedExistingRecord : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEPir2/XWuoczmfbhBFDMa6S6lNkdjuU6pLi4sYpBK3quEHuwNErMk0iOuQFOKqJxnw==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "jonas",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAELswAGHtPoWEYTI9+y7I2T7BsIqgfJm69lRVzBlU4nxXqtdkhmRd49RoPTc3sUlz5w==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAENMkn+dutNQroaqi62ysFFaXvlONpiuz4g7wu5/574fe6e0whCweY6hkd/1h1JZnrQ==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "jonas",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEA17Ajb1quyew4K6exLA6k5Wv2guDz38SiRu+iX2Aoqae63LxDMn7OzsASq2ERWpzQ==");
        }
    }
}
