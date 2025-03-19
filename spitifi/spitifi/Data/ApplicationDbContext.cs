using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using spitifi.Data.DbModels;

namespace spitifi.Data;


public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<Gostos> Gostos { get; set; }
    public DbSet<Musica> Musica { get; set; }
    public DbSet<Playlist> Playlist { get; set; }
    public DbSet<Utilizadores> Utilizadores { get; set; }
    public DbSet<UtilizadorPlaylist> UtilizadorPlaylist { get; set; }
    public DbSet<Colabs> Colabs { get; set; }
}