using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using spitifi.Data;
using spitifi.Models.DbModels;

namespace spitifi.Models.ApiModels
{
    [Route("api/album")]
    [ApiController]
    public class ApiAlbumController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ApiAlbumController(ApplicationDbContext context, UserManager<IdentityUser> userManager,
            IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: /api/album
        /// <summary>
        /// Retorna os dados de todos os albuns
        /// </summary>
        /// <remarks>
        /// Requer token de autenticação
        /// Não requer autorizações especiais (roles)
        /// 
        /// </remarks>
        /// <returns>
        /// <para>200 OK: Retorna dados dos albuns</para>
        /// <para>401 Unauthorized: Quando o request não contem um token de autenticação</para>
        /// <para>403 Forbidden: Quando o API Client não possui role de admin</para>
        /// </returns>
        /// <response code="200">Retorna array de JSON com albuns</response>
        /// <response code="401">Token de autenticação não presente</response>
        /// <response code="403">API client não é um administrador</response>
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<IEnumerable<AlbumDTO>>> Albuns()
        {
            return await _context.Album
                .Include(a => a.Musicas)
                .Select(a => new AlbumDTO
                {
                    Id = a.Id,
                    Titulo = a.Titulo,
                    Foto = a.Foto,
                    DonoFK = a.DonoFK,
                    Musicas = a.Musicas.Select(m => new MusicaDTO
                    {
                        Id = m.Id,
                        Nome = m.Nome,
                        AlbumFK = m.AlbumFK,
                        FilePath = m.FilePath,
                        DonoFK = m.DonoFK
                    }).ToList()
                })
                .ToListAsync();
        }

        // GET: api/album/{id}
        /// <summary>
        /// Retorna os dados de um album 
        /// </summary>
        /// <remarks>
        /// Requer token de autenticação
        /// Não requer autorizações especiais (roles)
        ///
        /// Exemplo:
        /// 
        ///     GET api/album/{id}
        ///     {
        ///         "Username": "60",
        ///     }
        /// 
        /// </remarks>
        /// <returns>
        /// <para>200 OK: Retorna dados do album</para>
        /// <para>401 Unauthorized: Quando o request não contem um token de autenticação</para>
        /// <para>403 Forbidden: Quando o API Client não possui role de admin</para>
        /// <para>404 Not Found: ID de album não coincide com registos existentes</para>
        /// </returns>
        /// <response code="200">Retorna JSON COM albuns</response>
        /// <response code="401">Token de autenticação não presente</response>
        /// <response code="403">API client não é um administrador</response>
        /// <response code="404">Album não existe</response>
        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<AlbumDTO>> PesquisarAlbum(int id)
        {
            var album = await _context.Album
                .Include(a => a.Musicas)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (album == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, "Album não existe"); //NotFound();
            }

            return new AlbumDTO
            {
                Id = album.Id,
                Titulo = album.Titulo,
                Foto = album.Foto,
                DonoFK = album.DonoFK,
                Musicas = album.Musicas.Select(m => new MusicaDTO
                {
                    Id = m.Id,
                    Nome = m.Nome,
                    AlbumFK = m.AlbumFK,
                    FilePath = m.FilePath,
                    DonoFK = m.DonoFK
                }).ToList()
            };
        }

        // PUT: api/album/{albumId}/musicas
        /// <summary>
        /// Insere musicas num album
        /// </summary>
        /// <remarks>
        /// Requer token de autenticação
        /// Requer autorização de artista ou administrador
        ///
        /// Exemplo:
        /// 
        ///     PUT api/album/{albumId}/musicas
        ///     {
        ///         "musicasNovas" : [
        ///             IFormFile,
        ///             IFormFile
        ///         ]
        ///     }
        /// 
        /// </remarks>
        /// <returns>
        /// <para>200 OK: Retorna album</para>
        /// <para>400 Bad Request: Informação invalída/errada</para>
        /// <para>401 Unauthorized: Quando o request não contem um token de autenticação</para>
        /// <para>403 Forbidden: Quando o API Client não possui role de admin</para>
        /// <para>404 Not Found: ID de album não coincide com registos existentes</para>
        /// </returns>
        /// <response code="200">Retorna JSON com albuns</response>    
        /// <response code="400">Deve enviar pelo menos uma música</response>    
        /// <response code="400">Ficheiro inválido. Use .mp3 ou .wav</response>    
        /// <response code="400">Operação não pode ser concluída</response>    
        /// <response code="401">Utilizador não encontrado</response>
        /// <response code="403">Não tem permissão para modificar este álbum</response>
        /// <response code="404">Album não encontrado</response>
        [HttpPut("{albumId}/musicas")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Artista,Administrador")]
        public async Task<ActionResult<AlbumDTO>> AdicionarMusicasAlbum(
            int albumId,
            [FromForm] AdicionarMusicasDTO request)
        {
            try
            {
                var currentUser = await UtilizadorAtual();
                if (currentUser == null)
                {
                    return StatusCode(StatusCodes.Status401Unauthorized,
                        "Utilizador não encontrado"); //Unauthorized("Utilizador não encontrado");
                }

                var album = await _context.Album
                    .Include(a => a.Musicas)
                    .FirstOrDefaultAsync(a => a.Id == albumId);
                
                var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (currentUserId == null)
                {
                    return Unauthorized();
                }

                if (!User.IsInRole("Administrador") &&
                    !(User.IsInRole("Artista") && album.Dono.IdentityUser == currentUserId))
                {
                    return StatusCode(StatusCodes.Status403Forbidden,
                        "Não tem permissão para modificar este álbum"); //Forbid();
                }

                if (album == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound,
                        "Album não encontrado"); //NotFound("Album não encontrado");
                }

                // input com todos os atributos presentes?
                if (request.musicasNovas == null || request.musicasNovas.Count == 0)
                {
                    return StatusCode(StatusCodes.Status404NotFound,
                        "Deve enviar pelo menos uma música"); //BadRequest("Deve enviar pelo menos uma música");
                }

                foreach (var file in request.musicasNovas)
                {
                    if (!file.ContentType.StartsWith("audio"))
                        return StatusCode(StatusCodes.Status400BadRequest,
                            $"Ficheiro inválido: {file.FileName}. Use .mp3 ou .wav"); //BadRequest($"Ficheiro inválido: {file.FileName}. Use .mp3 ou .wav");
                }

                // criar lista de músicas do album
                var newSongs = new List<Musica>();

                foreach (var songFile in request.musicasNovas)
                {
                    // gerar nome unico
                    var fileName = $"{Guid.NewGuid()}{Path.GetExtension(songFile.FileName)}";
                    var relativePath = Path.Combine("musicas", fileName);
                    var fullPath = Path.Combine(_webHostEnvironment.WebRootPath, relativePath);

                    // guardar ficheiro no wwwroot
                    Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await songFile.CopyToAsync(stream);
                    }

                    newSongs.Add(new Musica
                    {
                        Nome = songFile.FileName,
                        FilePath = relativePath,
                        AlbumFK = albumId,
                        DonoFK = album.DonoFK
                    });
                }

                // adicionar musicas novas
                album.Musicas.AddRange(newSongs);
                await _context.SaveChangesAsync();

                return Ok(new AlbumDTO(album));
            }
            catch (Exception ex)
            {
                return StatusCode(400, "Operação não pode ser concluída");
            }
        }


        // POST: api/album
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // POST: api/album
        /// <summary>
        /// Criar um album
        /// </summary>
        /// <remarks>
        /// Requer token de autenticação
        /// Requer autorização de artista ou administrador
        ///
        /// Exemplo:
        /// 
        ///     POST api/album
        ///     {
        ///         "Titulo" : "string",
        ///         "FotoAlbum" : IFormFile,
        ///         "MusicasNovas" : [
        ///             IFormFile,
        ///             IFormFile
        ///         ]
        ///     }
        /// 
        /// </remarks>
        /// <returns>
        /// <para>201 OK: Retorna album criado</para>
        /// <para>400 Bad Request: Informação invalída/errada</para>
        /// <para>401 Unauthorized: Quando o request não contem um token de autenticação</para>
        /// <para>403 Forbidden: Quando o API Client não possui role de admin</para>
        /// <para>404 Not Found: ID de album não coincide com registos existentes</para>
        /// </returns>
        /// <response code="201">Retorna JSON do album criado</response>    
        /// <response code="400">Dados do album invalidos</response>    
        /// <response code="401">Utilizador não encontrado</response>
        /// <response code="403">Não tem permissão para modificar este álbum</response>
        /// <response code="404">Album não encontrado</response>
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Artista,Administrador")]
        public async Task<ActionResult<AlbumDTO>> CriarAlbum([FromForm] CreateAlbumRequestDto request)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, "Utilizador não encontrado"); //Unauthorized();
            }

            var utilizador = await _context.Utilizadores
                .FirstOrDefaultAsync(u => u.IdentityUser == user.Id);
            if (utilizador == null)
            {
                return StatusCode(StatusCodes.Status401Unauthorized,
                    "Utilizador não encontrado"); //BadRequest("Alteração incorreta do Dono");
            }

            if (string.IsNullOrWhiteSpace(request.Titulo))
            {
                return BadRequest("Não introduziste um título");
            }

            if (request.FotoAlbum == null)
            {
                return BadRequest("Não introduziste uma foto");
            }

            if (request.MusicasNovas == null || request.MusicasNovas.Count == 0)
            {
                return BadRequest("Não introduziste pelo menos uma musica/ficheiro");
            }

            foreach (var file in request.MusicasNovas)
            {
                if (!file.ContentType.StartsWith("audio"))
                {
                    return BadRequest("Uma ou mais músicas com extensão inválida, use .wav ou .mp3 por favor");
                }
            }

            if (!(request.FotoAlbum.ContentType == "image/png" || request.FotoAlbum.ContentType == "image/jpeg"))
            {
                return BadRequest("Formato Inválido. Insira uma foto com formato JPEG ou PNG");
            }

            var album = new Album
            {
                Titulo = request.Titulo,
                DonoFK = utilizador.Id,
                Musicas = new List<Musica>()
            };

            string fotoPath = null;
            List<string> musicasPaths = new List<string>();

            try
            {
                var fotoFileName = $"{Guid.NewGuid()}{Path.GetExtension(request.FotoAlbum.FileName)}";
                var fotoRelativePath = Path.Combine("imagens", fotoFileName);
                fotoPath = Path.Combine(_webHostEnvironment.WebRootPath, fotoRelativePath);

                Directory.CreateDirectory(Path.GetDirectoryName(fotoPath));
                using (var stream = new FileStream(fotoPath, FileMode.Create))
                {
                    await request.FotoAlbum.CopyToAsync(stream);
                }

                album.Foto = fotoRelativePath;

                foreach (var musicaFile in request.MusicasNovas)
                {
                    var musicaFileName = $"{Guid.NewGuid()}{Path.GetExtension(musicaFile.FileName)}";
                    var musicaRelativePath = Path.Combine("musicas", musicaFileName);
                    var musicaFullPath = Path.Combine(_webHostEnvironment.WebRootPath, musicaRelativePath);

                    Directory.CreateDirectory(Path.GetDirectoryName(musicaFullPath));
                    using (var stream = new FileStream(musicaFullPath, FileMode.Create))
                    {
                        await musicaFile.CopyToAsync(stream);
                    }

                    musicasPaths.Add(musicaFullPath);

                    album.Musicas.Add(new Musica
                    {
                        Nome = musicaFile.FileName,
                        FilePath = musicaRelativePath,
                        DonoFK = utilizador.Id
                    });
                }

                _context.Album.Add(album);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(PesquisarAlbum), new { id = album.Id }, new AlbumDTO(album));
            }
            catch (Exception ex)
            {
                // remover ficheiros inseridos caso algum erro ocorra
                if (fotoPath != null && System.IO.File.Exists(fotoPath))
                {
                    System.IO.File.Delete(fotoPath);
                }

                foreach (var path in musicasPaths.Where(System.IO.File.Exists))
                {
                    System.IO.File.Delete(path);
                }

                return StatusCode(400, "Algo correu mal, por favor tente novamente");
            }
        }

        // DELETE: api/ApiAlbum/{albumId}
        /// <summary>
        /// Remove album
        /// </summary>
        /// <remarks>
        /// Requer token de autenticação
        /// Requer autorização de artista ou administrador
        ///
        /// Exemplo:
        /// 
        ///     Delete api/album/{albumId}
        /// 
        /// </remarks>
        /// <returns>
        /// <para>204 No Content: Album removido</para>
        /// <para>400 Bad Request: Informação invalída/errada</para>
        /// <para>401 Unauthorized: Quando o request não contem um token de autenticação</para>
        /// <para>403 Forbidden: Quando o API Client não possui role de admin</para>
        /// <para>404 Not Found: ID de album não coincide com registos existentes</para>
        /// </returns>
        /// <response code="204">Album removido</response>    
        /// <response code="400">Algo correu mal, por favor tente novamente</response>
        /// <response code="401">Utilizador não encontrado</response>
        /// <response code="403">Não tem permissão para modificar este álbum</response>
        /// <response code="404">Album não encontrado</response>
        [HttpDelete("{albumId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Artista,Administrador")]
        public async Task<IActionResult> RemoverAlbum(int albumId)
        {
            try
            {
                var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (currentUserId == null)
                {
                    return Unauthorized();
                }

                var album = await _context.Album.Include(a => a.Dono).FirstOrDefaultAsync(a => a.Id == albumId);

                // admins podem alterar qualquer album
                // artistas somente podem alterar os seus albuns
                if (!User.IsInRole("Administrador") &&
                    !(User.IsInRole("Artista") && album.Dono.IdentityUser == currentUserId))
                {
                    return StatusCode(StatusCodes.Status403Forbidden,
                        "Não tem permissão para modificar este álbum"); //Forbid(); 
                }

                if (album == null)
                {
                    return NotFound();
                }

                _context.Album.Remove(album);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception e)
            {
                return StatusCode(400, "Algo correu mal, por favor tente novamente");
            }
        }


        // DELETE: api/ApiAlbum/{albumId}/musicas
        /// <summary>
        /// Remove musicas de um album
        /// </summary>
        /// <remarks>
        /// Requer token de autenticação
        /// Requer autorização de artista ou administrador
        ///
        /// Exemplo:
        /// 
        ///     Delete api/album/{albumId}/musicas
        ///     {
        ///         "idsMusicas" : [
        ///             id musicas,
        ///             id musicas,
        ///             id musicas,
        ///         ]
        ///     }
        /// 
        /// </remarks>
        /// <returns>
        /// <para>204 No Content: Album removido</para>
        /// <para>400 Bad Request: Informação invalída/errada</para>
        /// <para>401 Unauthorized: Quando o request não contem um token de autenticação</para>
        /// <para>403 Forbidden: Quando o API Client não possui role de admin</para>
        /// <para>404 Not Found: ID de album não coincide com registos existentes</para>
        /// </returns>
        /// <response code="204">Musicas removidas</response>    
        /// <response code="400">Operação não pode ser realizar. Tente novamente</response>
        /// <response code="401">Utilizador não encontrado</response>
        /// <response code="403">Não tem permissão para modificar este álbum</response>
        /// <response code="404">Album não encontrado</response>
        [HttpDelete("{albumId}/musicas")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Artista,Administrador")]
        public async Task<IActionResult> RemoverMusicasDeAlbum(
            int albumId,
            [FromBody] RemoverMusicasDTO request)
        {
            try
            {
                // valida se utilizador está autenticado
                var currentUser = await UtilizadorAtual();
                if (currentUser == null)
                {
                    return Unauthorized("Utilizador não encontrado");
                }

                var album = await _context.Album
                    .Include(a => a.Musicas)
                    .Include(a => a.Dono)
                    .FirstOrDefaultAsync(a => a.Id == albumId);

                var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (currentUserId == null)
                {
                    return Unauthorized();
                }

                if (!User.IsInRole("Administrador") &&
                    !(User.IsInRole("Artista") && album.Dono.IdentityUser == currentUserId))
                {
                    return StatusCode(StatusCodes.Status403Forbidden,
                        "Não tem permissão para modificar este álbum"); //Forbid();
                }

                if (album == null)
                {
                    return NotFound("Album não encontrado");
                }

                // validacao
                if (request.idsMusicas == null || request.idsMusicas.Count == 0)
                {
                    return BadRequest("Deve especificar pelo menos uma música para remover");
                }

                // encontrarmusicas para remover
                var songsToRemove = album.Musicas
                    .Where(m => request.idsMusicas.Contains(m.Id))
                    .ToList();

                if (songsToRemove.Count != request.idsMusicas.Count)
                {
                    var missingIds = request.idsMusicas.Except(songsToRemove.Select(m => m.Id));
                    return BadRequest($"Músicas não encontradas: {string.Join(", ", missingIds)}");
                }

                // apagas ficheiros do wwwwrot
                foreach (var song in songsToRemove)
                {
                    var filePath = Path.Combine(_webHostEnvironment.WebRootPath, song.FilePath);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }

                    _context.Musica.Remove(song);
                }

                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(400, "Não foi possível concretizar a operação. Tente novamente.");
            }
        }

        // PATCH: api/ApiAlbum/{albumId}
        /// <summary>
        /// Altera informações do album
        /// </summary>
        /// <remarks>
        /// Requer token de autenticação
        /// Requer autorização de artista ou administrador
        ///
        /// Exemplo:
        /// 
        ///     PATCH api/album/{albumId}
        ///     {
        ///         "Titulo" : "duuuuh",
        ///         "FotoAlbum" : IFormFile,
        ///         "ArtistaUsername" : "nerd"
        ///     }
        /// 
        /// </remarks>
        /// <returns>
        /// <para>204 No Content: Alterações efetuadas com sucesso</para>
        /// <para>400 Bad Request: Informação invalída/errada</para>
        /// <para>401 Unauthorized: Quando o request não contem um token de autenticação</para>
        /// <para>403 Forbidden: Quando o API Client não possui role de admin</para>
        /// <para>404 Not Found: ID de album não coincide com registos existentes</para>
        /// </returns>
        /// <response code="204">Alterações efetuadas com sucesso</response>    
        /// <response code="400">Operação não pode ser realizar. Tente novamente</response>
        /// <response code="401">Utilizador não encontrado</response>
        /// <response code="403">Não tem permissão para modificar este álbum</response>
        /// <response code="404">Album não encontrado</response>
        [HttpPatch("{albumId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Artista,Administrador")]
        public async Task<IActionResult> UpdateAlbum(int albumId, [FromBody] AlbumUpdateDto updateDto)
        {
            var album = await _context.Album
                .Include(a => a.Musicas)
                .Include(a => a.Dono)
                .FirstOrDefaultAsync(a => a.Id == albumId);

            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (currentUserId == null)
            {
                return Unauthorized();
            }

            if (!User.IsInRole("Administrador") &&
                !(User.IsInRole("Artista") && album.Dono.IdentityUser == currentUserId))
            {
                return StatusCode(StatusCodes.Status403Forbidden,
                    "Não tem permissão para modificar este álbum"); //Forbid();
            }

            if (album == null)
            {
                return NotFound("Album não encontrado");
            }

            // atualiza titulo (caso esteja presente)
            if (!string.IsNullOrEmpty(updateDto.Titulo))
            {
                album.Titulo = updateDto.Titulo;
            }

            // atualiza foto (caso esteja presente)
            if (!(updateDto.FotoAlbum == null))
            {
                string fotoPath = null;
                album.Foto = updateDto.FotoAlbum.FileName;

                var fotoFileName = $"{Guid.NewGuid()}{Path.GetExtension(updateDto.FotoAlbum.FileName)}";
                var fotoRelativePath = Path.Combine("imagens", fotoFileName);
                fotoPath = Path.Combine(_webHostEnvironment.WebRootPath, fotoRelativePath);

                Directory.CreateDirectory(Path.GetDirectoryName(fotoPath));
                using (var stream = new FileStream(fotoPath, FileMode.Create))
                {
                    await updateDto.FotoAlbum.CopyToAsync(stream);
                }

                album.Foto = fotoRelativePath;
            }

            // atualiza artista (caso esteja presente)
            if (!string.IsNullOrEmpty(updateDto.ArtistaUsername))
            {
                // get identify userid from the artist name
                var identityUser = await _userManager.FindByNameAsync(updateDto.ArtistaUsername);
    
                if (identityUser == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, "Username especificado não corresponde a um artista existente");
                }

                // match the identity userid to a user id
                var newArtist = await _context.Utilizadores
                    .FirstOrDefaultAsync(u => u.IdentityUser == identityUser.Id);

                if (newArtist == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, "Username especificado não corresponde a um artista existente");
                } 

                // Update both FK and navigation property
                album.DonoFK = newArtist.Id;
                album.Dono = newArtist;
            }

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(400, "Não foi possível concretizar a operação. Tente novamente.");
            }
        }

        private async Task<Utilizadores> UtilizadorAtual()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return await _context.Utilizadores
                .FirstOrDefaultAsync(u => u.IdentityUser == userId);
        }
    }
}