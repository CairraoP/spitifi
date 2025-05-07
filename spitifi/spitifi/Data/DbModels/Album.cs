using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace spitifi.Data.DbModels;

public class Album
{
    /// <summary>
    /// identificador do albúm
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Título da albúm
    /// </summary>
    [Display(Name = "Albúm")]
    public string Titulo { get; set; }
    
    /// <summary>
    /// Fotografia do albúm
    /// Está guardada em wwwroot/imagens
    /// </summary>
    public string Foto { get; set; }
    
    /* *************************
     * Definição dos relacionamentos
     * **************************
     */
    
    /// <summary>
    /// FK para referenciar que utilizador criou o albúm
    /// </summary>
    [Display(Name = "Artista")]
    [ForeignKey(nameof(Dono))]
    public int DonoFK { get; set; }
    
    /// <summary>
    /// Utilizador que criou o albúm
    /// </summary>
    public Utilizadores Dono { get; set; }
    
    
    
    /// <summary>
    /// Lista de músicas pertencentes ao albúm
    /// </summary>
    public List<Musica> Musicas { get; set; } = new  List<Musica>();
}