using System.ComponentModel.DataAnnotations.Schema;

namespace spitifi.Data;

public class Playlist
{
    /// <summary>
    /// PK de uma playlist com auto-increment
    ///
    /// [key] caso queiramos outro nome pa PK com auto increment
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Nome da Playlist
    /// </summary>
    public string Nome { get; set; }
    
    /// <summary>
    /// Relação 1-N com participação obrigatória do lado N
    /// 
    /// FK para a entidade "Utilizador"
    /// </summary>
    [ForeignKey(nameof(Dono))]
    public int DonoFK { get; set; }
    
    public Utilizadores Dono { get; set; }
    
    /// <summary>
    /// Relação N-M com participação obrigatória da entidade "Música"
    /// 
    /// Lista de músicas pertencentes a uma playlist
    ///
    /// NOTA: Este ICollection foi/é usado para construir a relação de N-M  PlayList-Música
    /// </summary>
    public ICollection<Musica> ListaPlaylist { get; set; }
    
    /// <summary>
    /// Lista de utilizadores que seguem a playlist
    ///
    /// NOTA: Este ICollection é/foi usado para construir a tabela da relação N-M entre as entidades Utilizador - Playlist
    /// </summary>
    public ICollection<UtilizadorPlaylist> SeguePlaylist { get; set; }
}