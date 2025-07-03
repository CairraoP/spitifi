using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using spitifi.Models.DbModels;

namespace spitifi.Data;


public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<Gostos> Gostos { get; set; }
    public DbSet<Musica> Musica { get; set; }
    public DbSet<Utilizadores> Utilizadores { get; set; }
    public DbSet<Album> Album { get; set; }
    
    public DbSet<PlayList> PlayList { get; set; }
}