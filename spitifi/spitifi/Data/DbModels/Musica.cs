using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using spitifi.Data.DbModels;

namespace spitifi.Data.DbModels;
public class Musica
{
    /// <summary>
    /// identificador da música
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Nome da Música
    /// </summary>
    [Display(Name = "Título")]
    [Required(ErrorMessage = "A Música é de preenchimento obrigatório.")]
    [StringLength(255)] 
    public string Nome { get; set; }
    
    /// <summary>
    /// Nome do Albúm a que pertence a música, ainda podendo ser usado a referência da FK para importar a foto associada ao albúm.
    /// Relação de 1-N 
    /// </summary>
    [ForeignKey(nameof(Album))]
    public int AlbumFK { get; set; }
    
    [Display(Name = "Albúm")]
    public Album Album { get; set; }
    
    /// <summary>
    /// Coluna/campo onde ficará guardado o caminho para o ficheiro da música
    /// </summary>
    [Display(Name = "Música")]
    public string FilePath { get; set; }
    
    
    /* *************************
     * Definção dos relacionamentos
     * **************************
     */
    
    /// <summary>
    /// FK para a classe "Utilizadores"
    /// </summary>
    [Display(Name = "Artista")]
    [ForeignKey(nameof(Dono))]
    public int DonoFK { get; set; }
    
    
    /// <summary>
    /// Dono da fotografia
    /// </summary>
    public Utilizadores Dono { get; set; }
    
    /// <summary>
    /// Lista de utilizadores que colaboraram na música
    /// </summary>
    public ICollection<Colabs> ListaColab { get; set; } = [];
    
    /// <summary>
    /// Lista de utilizadores que gostaram da música
    /// </summary>
    public ICollection<Gostos> ListaGostos { get; set; } = [];
    
    public ICollection<AlbumPlayList> ListaPlayList { get; set; } = [];
}