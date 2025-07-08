using spitifi.Models.DbModels;

namespace spitifi.Models;

public class Procura
{
    public string TermoDeProcura { get; set; }
    public List<Album> Albums { get; set; } = new List<Album>();
    public List<Musica> Musicas { get; set; } = new List<Musica>();
    public List<Utilizadores> Artistas { get; set; } = new List<Utilizadores>();

}