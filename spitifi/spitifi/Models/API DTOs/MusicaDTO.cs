using System.ComponentModel.DataAnnotations;
using spitifi.Models.DbModels;

namespace spitifi.Models.ApiModels;

/// <summary>
/// DTO para output de dados de API
///
/// Utilizado para devolver informação de uma musica, contendo o ID do respetivo album e artista 
///
/// Contexto: Não podemos devolver o objeto normal, por causa do atributo Dono e Album
///     Atributo Dono e Album causam loops relacionais
/// 
/// Os atributos desta classe replicam algumas anotações da classe Musica. Cuidado durante uma alteração
/// </summary>
public class MusicaDTO
{
    /// <summary>
    /// identificador da música
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Nome da Música
    /// </summary>
    [StringLength(255)] 
    public string Nome { get; set; }
    
    /// <summary>
    /// Nome do Albúm a que pertence a música, ainda podendo ser usado a referência da FK para importar a foto associada ao albúm.
    /// Relação de 1-N 
    /// </summary>
    public int AlbumFK { get; set; }
    
    /// <summary>
    /// Caminho no wwwroot onde a imagem será guardada
    /// </summary>
    public string FilePath { get; set; }
    
    /// <summary>
    /// FK para a classe "Utilizadores"
    /// Artista que criou o album
    /// Musica tem de pertencer a 1 album
    /// </summary>
    public int DonoFK { get; set; }
    
    public MusicaDTO() {}
    
    public MusicaDTO(Musica musica)
    {
        Id = musica.Id;
        Nome = musica.Nome;
        FilePath = musica.FilePath;
        DonoFK = musica.DonoFK;
    }
}