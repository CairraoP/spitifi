using spitifi.Data.DbInitializerDev;
using Microsoft.AspNetCore.Identity;
using NuGet.Protocol;
using spitifi.Models.DbModels;

namespace spitifi.Data.DbInitializerDev;

public class DbInitializerDev
{
    internal static async Task Initialize(ApplicationDbContext dbContext)
    {
        /*
         * https://stackoverflow.com/questions/70581816/how-to-seed-data-in-net-core-6-with-entity-framework
         *
         * https://learn.microsoft.com/en-us/aspnet/core/data/ef-mvc/intro?view=aspnetcore-6.0#initialize-db-with-test-data
         * https://github.com/dotnet/AspNetCore.Docs/blob/main/aspnetcore/data/ef-mvc/intro/samples/5cu/Program.cs
         * https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/style-rules/ide0300
         */


        ArgumentNullException.ThrowIfNull(dbContext, nameof(dbContext));
        dbContext.Database.EnsureCreated();

        // var auxiliar
        bool haAdicao = false;

        var hasher = new PasswordHasher<IdentityUser>();

        IdentityUser u1 = new IdentityUser
        {
            Id = "admin",
            UserName = "admin@mail.pt",
            NormalizedUserName = "ADMIN@MAIL.PT",
            Email = "admin@mail.pt",
            NormalizedEmail = "ADMIN@MAIL.PT",
            EmailConfirmed = true,
            SecurityStamp = "29c12eb3-07f7-4cd8-a5d2-ab4e40e6d064",
            ConcurrencyStamp = "4f58a2a5-5148-48f6-bc1c-74cae86a2172",
            PasswordHash = hasher.HashPassword(null, "Aa0_aa")
        };
        IdentityUser u2 = new IdentityUser
        {
            Id = "jonas",
            UserName = "jonas@mail.pt",
            NormalizedUserName = "JONAS@MAIL.PT",
            Email = "jonas@mail.pt",
            NormalizedEmail = "JONAS@MAIL.PT",
            EmailConfirmed = true,
            SecurityStamp = "99c12eb3-07f7-4cd8-a5d2-ab4e40e6d064",
            ConcurrencyStamp = "9f58a2a5-5148-48f6-bc1c-74cae86a2172",
            PasswordHash = hasher.HashPassword(null, "Bb0_bb")
        };

        IdentityUser u3 = new IdentityUser
        {
            Id = "John Mayer",
            UserName = "johnM@mail.pt",
            NormalizedUserName = "JOHNM@MAIL.PT",
            Email = "johnM@mail.pt",
            NormalizedEmail = "JOHNM@MAIL.PT",
            EmailConfirmed = true,
            SecurityStamp = "39c12eb3-07f7-4cd8-a5d2-ab4e40e6d064",
            ConcurrencyStamp = "2f58a2a5-5148-48f6-bc1c-74cae86a2172",
            PasswordHash = hasher.HashPassword(null, "Cc0_cc")
        };
        Console.Write("Before if");
        if (await dbContext.Users.FindAsync(u1.Id) == null)
        {
            Console.Write("Inside if; before add");
            dbContext.Users.Add(u1);
            Console.Write("Inside if; after add; bafore addAsync");
            await dbContext.Utilizadores.AddAsync(new Utilizadores
            {
                Id = 50,
                Username = u1.UserName
            });
            Console.Write("Inside if; after add; bafore afterAsync");
            haAdicao = true;
        }

        if (await dbContext.Users.FindAsync(u2.Id) == null)
        {
            dbContext.Users.Add(u2);
            await dbContext.Utilizadores.AddAsync(new Utilizadores
            {
                Id = 51,
                Username = u2.UserName
            });
            haAdicao = true;
        }

        if (await dbContext.Users.FindAsync(u3.Id) == null)
        {
            dbContext.Users.Add(u3);
            await dbContext.Utilizadores.AddAsync(new Utilizadores
            {
                Id = 52,
                Username = u3.UserName
            });
            haAdicao = true;
        }


        IdentityRole role_admin = new IdentityRole
            { Id = "a", Name = "Administrador", NormalizedName = "ADMINISTRADOR" };
        IdentityRole role_artista = new IdentityRole { Id = "ar", Name = "Artista", NormalizedName = "ARTISTA" };

        if (await dbContext.Roles.FindAsync(role_admin.Id) == null)
        {
            dbContext.Roles.Add(role_admin);
            haAdicao = true;
        }

        if (await dbContext.Roles.FindAsync(role_artista.Id) == null)
        {
            dbContext.Roles.Add(role_artista);
            haAdicao = true;
        }

        IdentityUserRole<string> user_role_1 = new IdentityUserRole<string> { UserId = "admin", RoleId = "a" };
        IdentityUserRole<string> user_role_3 = new IdentityUserRole<string> { UserId = "John Mayer", RoleId = "ar" };

        if (await dbContext.UserRoles.FindAsync(user_role_1.UserId, user_role_1.RoleId) == null)
        {
            dbContext.UserRoles.Add(user_role_1);
            haAdicao = true;
        }

        if (await dbContext.UserRoles.FindAsync(user_role_3.UserId, user_role_3.RoleId) == null)
        {
            dbContext.UserRoles.Add(user_role_3);
            haAdicao = true;
        }

        if (haAdicao)
        {
            await dbContext.SaveChangesAsync();
        }

        var dono = dbContext.Utilizadores.FirstOrDefault(u => u.Id == 52);
        if (dono != null)
        {
            Album a1 = new Album
            {
                Id = 60,
                Titulo = "Seed1",
                DonoFK = dono.Id,
                Foto = @"\assets\imagesSeed\texto.PNG"
            };

            if (!dbContext.Album.Any(a => a.Id == a1.Id))
            {
                dbContext.Album.Add(a1);
                haAdicao = true;
            }

            Musica m1 = new Musica
            {
                Id = 70,
                Nome = "Seed11",
                DonoFK = dono.Id,
                AlbumFK = 60,
                FilePath = @"\assets\musicsSeed\3 Doors Down - Here Without You.mp3"
            };
            Musica m2 = new Musica
            {
                Id = 71,
                Nome = "Seed12",
                DonoFK = dono.Id,
                AlbumFK = 60,
                FilePath = @"\assets\musicsSeed\Bee Gees - More Than A Woman (Lyric Video).mp3"
            };

            a1.Musicas.Add(m1);
            a1.Musicas.Add(m2);

            Album a2 = new Album
            {
                Id = 70,
                Titulo = "Seed2",
                DonoFK = dono.Id,
                Foto = @"\assets\imagesSeed\Pilha Alcalina.PNG"
            };

            if (!dbContext.Album.Any(a => a.Id == a2.Id))
            {
                dbContext.Album.Add(a2);
                haAdicao = true;
            }

            Musica m3 = new Musica
            {
                Id = 72,
                Nome = "Seed21",
                DonoFK = dono.Id,
                AlbumFK = 70,
                FilePath =
                    @"\assets\musicsSeed\Come and get Your Love(Guardians of the Galaxy Intro song) - Redbone.mp3"
            };
            Musica m4 = new Musica
            {
                Id = 73,
                Nome = "Seed22",
                DonoFK = dono.Id,
                AlbumFK = 70,
                FilePath = @"\assets\musicsSeed\Encore.mp3"
            };
            Musica m5 = new Musica
            {
                Id = 74,
                Nome = "Seed23",
                DonoFK = dono.Id,
                AlbumFK = 70,
                FilePath = @"\assets\musicsSeed\Gravity by John Mayer.mp3"
            };

            a2.Musicas.Add(m3);
            a2.Musicas.Add(m4);
            a2.Musicas.Add(m5);

            dono.Albums.Add(a1);
            dono.Albums.Add(a2);

            if (haAdicao)
            {
                await dbContext.SaveChangesAsync();
            }
        }
    }
}