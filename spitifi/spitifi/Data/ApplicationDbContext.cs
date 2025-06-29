using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using spitifi.Models.DbModels;

namespace spitifi.Data;


public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        //importa todo o comportamento do metodo 
        // aquando da sua definição na SuperClasse
        base.OnModelCreating(modelBuilder);
 
        // criar os perfis de utilizador da nossa app
        modelBuilder.Entity<IdentityRole>().HasData(
            new IdentityRole { Id = "a", Name = "Administrador", NormalizedName = "ADMINISTRADOR" },
            new IdentityRole { Id = "ar", Name = "Artista", NormalizedName = "ARTISTA" });
 
        // criar um utilizador para funcionar como ADMIN
        // função para codificar a password
        var hasher = new PasswordHasher<IdentityUser>();
        // criação do utilizador
        modelBuilder.Entity<IdentityUser>().HasData(
            new IdentityUser
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
            },
            new IdentityUser
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
            },
            new IdentityUser
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
            }
        );
        // Associar este utilizador à role ADMIN
        modelBuilder.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string> { UserId = "admin", RoleId = "a" },
            new IdentityUserRole<string> { UserId = "John Mayer", RoleId = "ar" });
    }

    public DbSet<Gostos> Gostos { get; set; }
    public DbSet<Musica> Musica { get; set; }
    public DbSet<Utilizadores> Utilizadores { get; set; }
    public DbSet<Album> Album { get; set; }
    
    public DbSet<PlayList> PlayList { get; set; }
}