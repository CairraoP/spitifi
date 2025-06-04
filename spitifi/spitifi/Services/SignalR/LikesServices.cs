using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace spitifi.Services.SignalR;

public class LikesServices : Hub
{
    [Authorize]
    public override Task OnConnectedAsync()
    {
        Clients.All.SendAsync("whatever", "fsfsdff"+Context.ConnectionId);  
        return base.OnConnectedAsync();
    }
    

}