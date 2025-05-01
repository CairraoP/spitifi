using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace spitifi.Migrations
{
    /// <inheritdoc />
    public partial class booleanUtilizador : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MusicaPlaylist");

            migrationBuilder.DropTable(
                name: "UtilizadorPlaylist");

            migrationBuilder.DropTable(
                name: "Playlist");

            migrationBuilder.AddColumn<bool>(
                name: "IsArtista",
                table: "Utilizadores",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsArtista",
                table: "Utilizadores");

            migrationBuilder.CreateTable(
                name: "Playlist",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DonoFK = table.Column<int>(type: "int", nullable: false),
                    Nome = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Playlist", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Playlist_Utilizadores_DonoFK",
                        column: x => x.DonoFK,
                        principalTable: "Utilizadores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MusicaPlaylist",
                columns: table => new
                {
                    ListaMusicaId = table.Column<int>(type: "int", nullable: false),
                    ListaPlaylistId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MusicaPlaylist", x => new { x.ListaMusicaId, x.ListaPlaylistId });
                    table.ForeignKey(
                        name: "FK_MusicaPlaylist_Musica_ListaPlaylistId",
                        column: x => x.ListaPlaylistId,
                        principalTable: "Musica",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MusicaPlaylist_Playlist_ListaMusicaId",
                        column: x => x.ListaMusicaId,
                        principalTable: "Playlist",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UtilizadorPlaylist",
                columns: table => new
                {
                    UtilizadorFK = table.Column<int>(type: "int", nullable: false),
                    PlaylistFK = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UtilizadorPlaylist", x => new { x.UtilizadorFK, x.PlaylistFK });
                    table.ForeignKey(
                        name: "FK_UtilizadorPlaylist_Playlist_PlaylistFK",
                        column: x => x.PlaylistFK,
                        principalTable: "Playlist",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UtilizadorPlaylist_Utilizadores_UtilizadorFK",
                        column: x => x.UtilizadorFK,
                        principalTable: "Utilizadores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_MusicaPlaylist_ListaPlaylistId",
                table: "MusicaPlaylist",
                column: "ListaPlaylistId");

            migrationBuilder.CreateIndex(
                name: "IX_Playlist_DonoFK",
                table: "Playlist",
                column: "DonoFK");

            migrationBuilder.CreateIndex(
                name: "IX_UtilizadorPlaylist_PlaylistFK",
                table: "UtilizadorPlaylist",
                column: "PlaylistFK");
        }
    }
}
