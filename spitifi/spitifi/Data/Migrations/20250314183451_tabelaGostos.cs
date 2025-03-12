using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace spitifi.Data.Migrations
{
    /// <inheritdoc />
    public partial class tabelaGostos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Utilizadores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Utilizadores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Musica",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", nullable: false),
                    Album = table.Column<string>(type: "TEXT", nullable: false),
                    FilePath = table.Column<string>(type: "TEXT", nullable: false),
                    DonoFK = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Musica", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Musica_Utilizadores_DonoFK",
                        column: x => x.DonoFK,
                        principalTable: "Utilizadores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Playlist",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", nullable: false),
                    DonoFK = table.Column<int>(type: "INTEGER", nullable: false)
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
                });

            migrationBuilder.CreateTable(
                name: "Colabs",
                columns: table => new
                {
                    UtilizadorFK = table.Column<int>(type: "INTEGER", nullable: false),
                    MusicaFK = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Colabs", x => new { x.UtilizadorFK, x.MusicaFK });
                    table.ForeignKey(
                        name: "FK_Colabs_Musica_MusicaFK",
                        column: x => x.MusicaFK,
                        principalTable: "Musica",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Colabs_Utilizadores_UtilizadorFK",
                        column: x => x.UtilizadorFK,
                        principalTable: "Utilizadores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Gostos",
                columns: table => new
                {
                    UtilizadorFK = table.Column<int>(type: "INTEGER", nullable: false),
                    MusicaFK = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gostos", x => new { x.UtilizadorFK, x.MusicaFK });
                    table.ForeignKey(
                        name: "FK_Gostos_Musica_MusicaFK",
                        column: x => x.MusicaFK,
                        principalTable: "Musica",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Gostos_Utilizadores_UtilizadorFK",
                        column: x => x.UtilizadorFK,
                        principalTable: "Utilizadores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MusicaPlaylist",
                columns: table => new
                {
                    ListaMusicaId = table.Column<int>(type: "INTEGER", nullable: false),
                    ListaPlaylistId = table.Column<int>(type: "INTEGER", nullable: false)
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
                });

            migrationBuilder.CreateTable(
                name: "UtilizadorPlaylist",
                columns: table => new
                {
                    UtilizadorFK = table.Column<int>(type: "INTEGER", nullable: false),
                    PlaylistFK = table.Column<int>(type: "INTEGER", nullable: false)
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
                });

            migrationBuilder.CreateIndex(
                name: "IX_Colabs_MusicaFK",
                table: "Colabs",
                column: "MusicaFK");

            migrationBuilder.CreateIndex(
                name: "IX_Gostos_MusicaFK",
                table: "Gostos",
                column: "MusicaFK");

            migrationBuilder.CreateIndex(
                name: "IX_Musica_DonoFK",
                table: "Musica",
                column: "DonoFK");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Colabs");

            migrationBuilder.DropTable(
                name: "Gostos");

            migrationBuilder.DropTable(
                name: "MusicaPlaylist");

            migrationBuilder.DropTable(
                name: "UtilizadorPlaylist");

            migrationBuilder.DropTable(
                name: "Musica");

            migrationBuilder.DropTable(
                name: "Playlist");

            migrationBuilder.DropTable(
                name: "Utilizadores");
        }
    }
}
