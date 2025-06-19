using spitifi.Data.DbInitializerDev;
using Microsoft.AspNetCore.Identity;

namespace spitifi.Data.DbInitializerDev;

public class DbInitializerDev
{
    internal static async void Initialize(ApplicationDbContext dbContext)
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

        if (await dbContext.Users.FindAsync(u1.Id) == null)
        {
            dbContext.Users.Add(u1);
            haAdicao = true;
        }
        
        if (await dbContext.Users.FindAsync(u2.Id) == null)
        {
            dbContext.Users.Add(u2);
            haAdicao = true;
        }


        IdentityRole role_admin = new IdentityRole { Id = "a", Name = "Administrador", NormalizedName = "ADMINISTRADOR" };
        IdentityRole role_artista = new IdentityRole { Id = "ar", Name = "Artista", NormalizedName = "ARTISTA" };
        IdentityRole role_user = new IdentityRole { Id = "user", Name = "User", NormalizedName = "USER" };
        
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
        
        if (await dbContext.Roles.FindAsync(role_user.Id) == null)
        {
            dbContext.Add(role_user);
            haAdicao = true;
        }

        IdentityUserRole<string> user_role_1 = new IdentityUserRole<string> { UserId = "admin", RoleId = "a" };
        IdentityUserRole<string> user_role_2 = new IdentityUserRole<string> { UserId = "jonas", RoleId = "user" };

        if (await dbContext.UserRoles.FindAsync(user_role_1.UserId, user_role_1.RoleId) == null)
        {
            dbContext.UserRoles.Add(user_role_1);
            haAdicao = true;
        } 
        
        if (await dbContext.UserRoles.FindAsync(user_role_2.UserId, user_role_2.RoleId) == null)
        {
            dbContext.UserRoles.Add(user_role_2);
            haAdicao = true;
        }
        
    }
}