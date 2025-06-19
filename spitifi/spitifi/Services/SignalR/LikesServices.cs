using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using spitifi.Data;
using spitifi.Models.DbModels;

namespace spitifi.Services.SignalR;

public class LikesServices : Hub
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly ILogger<LikesServices> _logger;

    public LikesServices(ApplicationDbContext applicationDbContext, ILogger<LikesServices> logger)
    {
        _applicationDbContext = applicationDbContext;
        _logger = logger;
    }

    [Authorize]
    public override async Task OnConnectedAsync()
    {
        try
        {
            var username = Context.User.Identity.Name;
            var user = await _applicationDbContext.Utilizadores
                .Include(u => u.ListaGostos)
                .FirstOrDefaultAsync(u => u.Username == username);
        
            // Get all songs the user might see (simplified for demo - in real app, you'd want to scope this)
            var allSongs = await _applicationDbContext.Musica
                .Include(m => m.ListaGostos)
                .ToListAsync();
        
            var initialStates = allSongs.Select(song => new
            {
                songId = song.Id,
                count = song.ListaGostos.Count,
                liked = user?.ListaGostos?.Any(g => g.MusicaFK == song.Id) ?? false
            }).ToList();
        
            // Send initial states to the newly connected client
            await Clients.Caller.SendAsync("ReceiveInitialStates", initialStates);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending initial states");
        }
    
        await base.OnConnectedAsync();
    }

    
    [Authorize]
    public void AtualizarGostos(int idMusica)
    {
        // 1 - verificar se a foto existe
        var musica= _applicationDbContext.Musica.Find(idMusica);
        if (musica == null)
            return;
        
        // 2 - validar se o utilizador já gostou da foto 
        
        var utilizador = _applicationDbContext.Utilizadores
            .Include(u => u.ListaGostos)
            .First(u => u.Username == Context.User.Identity.Name);

        var listaGostoMusica = utilizador.ListaGostos
            .Where(ul => ul.MusicaFK == idMusica);

        // se entra aqui é porque existe um gosto
        if (listaGostoMusica.Any())
        {
            _applicationDbContext.Gostos.Remove(listaGostoMusica.First());
        }
        // se entra aqui é porque não existe um gosto
        else
        {
            var novoGosto = new Gostos(){MusicaFK = idMusica, UtilizadorFK = utilizador.Id};
            _applicationDbContext.Gostos.Add(novoGosto);
        }
    
        _applicationDbContext.SaveChanges();
        
        var numGostos = _applicationDbContext.Gostos.Where(g => g.MusicaFK==idMusica).Count();
        
        // get current user's new liked state
        var newState = _applicationDbContext.Gostos
            .Any(g => g.MusicaFK == idMusica && 
                      g.Utilizador.Username == Context.User.Identity.Name);
        
        Clients.All.SendAsync("AtualizarGostos", idMusica, numGostos);
        
        // send new state only to the caller
        Clients.Caller.SendAsync("AtualizarGostoState", idMusica, newState);
    }
}