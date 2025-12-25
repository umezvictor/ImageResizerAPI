using Microsoft.AspNetCore.SignalR;

namespace ImageResizerAPI;

public class ChatHub : Hub
{
    public async Task SendMessage(string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", message);
    }
}
