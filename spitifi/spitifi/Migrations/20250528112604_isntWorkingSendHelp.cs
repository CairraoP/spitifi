using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace spitifi.Migrations
{
    /// <inheritdoc />
    public partial class isntWorkingSendHelp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Utilizadores",
                keyColumn: "Username",
                keyValue: null,
                column: "Username",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "Utilizadores",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Musica",
                keyColumn: "Nome",
                keyValue: null,
                column: "Nome",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "Musica",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4f58a2a5-5148-48f6-bc1c-74cae86a2172", "AQAAAAIAAYagAAAAEHCl6YNIAIidZEK8bC0IyTibUV8iFvGT6j728rArm6qB8KKuK65RTXTWxT5PJIJGTQ==", "29c12eb3-07f7-4cd8-a5d2-ab4e40e6d064" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "jonas",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "9f58a2a5-5148-48f6-bc1c-74cae86a2172", "AQAAAAIAAYagAAAAEM5pJ2Mgy10Lx+tU61AdDazBp1VkMphmdn5MO1J99qESI5BhNWH5GaN1lIDF712tkw==", "99c12eb3-07f7-4cd8-a5d2-ab4e40e6d064" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "Utilizadores",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "Musica",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "be271d4d-244f-4467-aafa-c78872a66c19", "AQAAAAIAAYagAAAAEMYxz+8BsfYncb9jyxbcFLEn4nm11JIq7SC+lIgNo/vH9MBCKL2AQn7jd+tCASBBiw==", "d7d8d738-47f9-4e54-990a-77b6124bcdc1" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "jonas",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "81a6a2a1-569b-4b3e-8436-d898a6079e0f", "AQAAAAIAAYagAAAAEPn6AwZvPEMV/lr1fUCQNbbC0MV7UfVpSttw7CJMHB0HO3+nUz9IeLOC2vtvNMotQw==", "3236e72b-a054-437e-a993-d44b2d0b382d" });
        }
    }
}
