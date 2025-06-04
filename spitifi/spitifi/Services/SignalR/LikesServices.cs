using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using spitifi.Data;
using spitifi.Data.DbModels;

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
    public override Task OnConnectedAsync()
    {
        Clients.All.SendAsync("OnConnectedAsync", "Nova conexão: "+Context.ConnectionId);  
        return base.OnConnectedAsync();
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
        
        Clients.All.SendAsync("AtualizarGostos", idMusica, numGostos);

    }
}