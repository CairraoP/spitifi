using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using spitifi.Data.DbModels;

namespace spitifi.Data.DbModels;
public class Musica
{
    /// <summary>
    /// chave prmária auto-increment
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Nome da Música
    /// </summary>
    public string Nome { get; set; }
    
    /// <summary>
    /// Nome do Albúm a que pertence a música, ainda podendo ser usado a referência da FK para importar a foto associada ao albúm.
    /// Relação de 1-N 
    /// </summary>
    [Display(Name = "Albúm")]
    [ForeignKey(nameof(Album))]
    public int AlbumFK { get; set; }
    
    public Album Album { get; set; }
    
    /// <summary>
    /// Coluna/campo onde ficará guardado o caminho para o ficheiro da música
    /// </summary>
    [Display(Name = "Música")]
    public string FilePath { get; set; }
    
    /// <summary>
    /// FK para a classe "Utilizadores"
    /// </summary>
    [Display(Name = "Artista")]
    [ForeignKey(nameof(Dono))]
    public int DonoFK { get; set; }
    /*
     * Relação 1 - N com participação obrigatória do lado N
     */
    public Utilizadores Dono { get; set; }
    
    /// <summary>
    /// Lista de utilizadores que colaboraram numa música
    /// </summary>
    public ICollection<Colabs> ListaColab { get; set; } = [];
    
    /// <summary>
    /// Lista de playlists onde aparece esta música
    ///
    /// NOTA: Este campo é/foi usado para a construção da tabela/relação Playlists-Musica
    /// </summary>
    public ICollection<Playlist> ListaMusica { get; set; } = [];

    /// <summary>
    /// Relação N-M sem participação obrigatória de nenhum dos lados
    /// 
    /// Lista de utilizadores que gostaram desta música
    ///
    /// NOTA: Este ICollection é/foi usado para a construção da tabela/relação N-M Utilizadores-Música
    /// </summary>
    public ICollection<Gostos> ListaGostos { get; set; } = [];
}