using System.ComponentModel.DataAnnotations;
using spitifi.Models.DbModels;

namespace spitifi.Models;

/// <summary>
/// DTO de entrada e saida que contem resultados de pesquisa de acordo com uma string
///
/// Permite obter os resultados de uma procura sobre albuns, musica e artistas
/// </summary>
public class Procura
{
    /// <summary>
    /// String para ser procurada na base de dados
    /// </summary>
    [Required]
    [MaxLength(64)]
    public string TermoDeProcura { get; set; }
    
    /// <summary>
    /// Albuns cujo nome deu match ao termo de pesquisa
    /// </summary>
    public List<Album> Albums { get; set; } = new List<Album>();
    
    /// <summary>
    /// Musicas cujo nome deu match ao termo de pesquisa
    /// </summary>
    public List<Musica> Musicas { get; set; } = new List<Musica>();
    
    /// <summary>
    /// Utilizadores cujo nome deu match ao termo de pesquisa
    /// </summary>
    public List<Utilizadores> Artistas { get; set; } = new List<Utilizadores>();

}