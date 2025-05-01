using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using spitifi.Data.DbModels;
using Microsoft.AspNetCore.Identity;

namespace spitifi.Data;


public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // 'importa' todo o comportamento do método, 
        // aquando da sua definição na SuperClasse
        base.OnModelCreating(modelBuilder);
 
        // criar os perfis de utilizador da nossa app
        modelBuilder.Entity<IdentityRole>().HasData(
            new IdentityRole { Id = "a", Name = "Administrador", NormalizedName = "ADMINISTRADOR" },
            new IdentityRole { Id = "ar", Name = "Artista", NormalizedName = "ARTISTA" },
            new IdentityRole { Id = "user", Name = "User", NormalizedName = "USER" });
 
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
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                PasswordHash = hasher.HashPassword(null, "Aa0_aa")
            },
            new IdentityUser
            {
                Id = "jonas",
                UserName = "jonas@mail.pt",
                NormalizedUserName = "JONAS@MAIL.PT",
                Email = "jonas@mail.pt",
                NormalizedEmail = "ADMIN@MAIL.PT",
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                PasswordHash = hasher.HashPassword(null, "Bb0_bb")
            }
        );
        // Associar este utilizador à role ADMIN
        modelBuilder.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string> { UserId = "admin", RoleId = "a" },
            new IdentityUserRole<string> { UserId = "jonas", RoleId = "user" });
         
    }
    
    public DbSet<Gostos> Gostos { get; set; }
    public DbSet<Musica> Musica { get; set; }
    public DbSet<Utilizadores> Utilizadores { get; set; }
    public DbSet<Colabs> Colabs { get; set; }
    public DbSet<Album> Album { get; set; }
}