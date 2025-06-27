using Microsoft.EntityFrameworkCore;
using spitifi.Data;

namespace spitifi.Services.AlbumEraser;

//Classe para apagar um Album, como a lógica ia ser dupplicada nos utilizadores e no controlador do Album, criou-se esta classe
public class AlbumEraser
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public AlbumEraser(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task AlbumEraserFunction(int id)
    {
        
        //buscar os albuns e as musicas associadas ao album
        var album =  _context.Album.Include(a =>a.Musicas).FirstOrDefault(a=> a.Id == id)!;

        if (album != null)
        {
            //apagar os recursos relacionados ao album, neste caso a foto e as musicas, para isso usamos também o DELETE do
            //Model das músicas
            var partialPath = album.Foto; // buscar o caminho relativo da foto

            //juntar como caminho do wwwroot
            var fullPath = Path.Combine(_webHostEnvironment.WebRootPath,
                partialPath.Replace("/", Path.DirectorySeparatorChar.ToString()));

            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }

            //Se eliminarmos um album, eliminamos também as músicas que estão presentes no album e o seu ficheiro no wwwroot.
            foreach (var musica in album.Musicas)
            {
                var partialPathMusic = musica.FilePath; // e.g. "albumcover.jpg"

                var fullPathMusic = Path.Combine(_webHostEnvironment.WebRootPath,
                    partialPathMusic.Replace("/", Path.DirectorySeparatorChar.ToString()));

                if (System.IO.File.Exists(fullPathMusic))
                {
                    System.IO.File.Delete(fullPathMusic);
                }

                _context.Musica.Remove(musica);
            }
            
            _context.Album.Remove(album);
        }
    }
}