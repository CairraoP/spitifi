using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace spitifi.Migrations
{
    /// <inheritdoc />
    public partial class AjustePlaylistsModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlbumPlayListMusica");

            migrationBuilder.DropTable(
                name: "AlbumPlayList");

            migrationBuilder.CreateTable(
                name: "PlayList",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DonoFK = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayList", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayList_Utilizadores_DonoFK",
                        column: x => x.DonoFK,
                        principalTable: "Utilizadores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MusicaPlayList",
                columns: table => new
                {
                    ListaPlayListId = table.Column<int>(type: "int", nullable: false),
                    MusicasId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MusicaPlayList", x => new { x.ListaPlayListId, x.MusicasId });
                    table.ForeignKey(
                        name: "FK_MusicaPlayList_Musica_MusicasId",
                        column: x => x.MusicasId,
                        principalTable: "Musica",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_MusicaPlayList_PlayList_ListaPlayListId",
                        column: x => x.ListaPlayListId,
                        principalTable: "PlayList",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEERbPkVuxmaBeuKDStvr1NmXFvHyu/314Kx4DBKBUeOKl2VAMd79bqL6tLaSGd9YqA==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "jonas",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAENCpEXYyjMG3OMuEYSAy2Ue2iuKjnVp0X4/eIGpyhz/Uo0PSNRCPmGuv/AoqgN29tQ==");

            migrationBuilder.CreateIndex(
                name: "IX_MusicaPlayList_MusicasId",
                table: "MusicaPlayList",
                column: "MusicasId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayList_DonoFK",
                table: "PlayList",
                column: "DonoFK");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MusicaPlayList");

            migrationBuilder.DropTable(
                name: "PlayList");

            migrationBuilder.CreateTable(
                name: "AlbumPlayList",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PlayListFK = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlbumPlayList", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AlbumPlayList_Album_PlayListFK",
                        column: x => x.PlayListFK,
                        principalTable: "Album",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AlbumPlayListMusica",
                columns: table => new
                {
                    ListaPlayListId = table.Column<int>(type: "int", nullable: false),
                    MusicasId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlbumPlayListMusica", x => new { x.ListaPlayListId, x.MusicasId });
                    table.ForeignKey(
                        name: "FK_AlbumPlayListMusica_AlbumPlayList_ListaPlayListId",
                        column: x => x.ListaPlayListId,
                        principalTable: "AlbumPlayList",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AlbumPlayListMusica_Musica_MusicasId",
                        column: x => x.MusicasId,
                        principalTable: "Musica",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEHOqrpwOq1NamK1oJBLr43qRWBHLRxxrVG3DoLWseIDCiQiljPnRiJpSdXokZXMdmQ==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "jonas",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEPOERIWu1WULFFbUqPovySqW6Yo8aGD53yNQqvIL3pvWBe87eq8Edb9vtJVZooiw9Q==");

            migrationBuilder.CreateIndex(
                name: "IX_AlbumPlayList_PlayListFK",
                table: "AlbumPlayList",
                column: "PlayListFK");

            migrationBuilder.CreateIndex(
                name: "IX_AlbumPlayListMusica_MusicasId",
                table: "AlbumPlayListMusica",
                column: "MusicasId");
        }
    }
}
