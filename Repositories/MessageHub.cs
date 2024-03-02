using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Query;
using Models.Auth;
using Repositories.Contracts;
using System.ComponentModel;

namespace Repositories
{
    public class MessageHub : Hub, IMessageHub
    {

        private readonly IHubContext<MessageHub> _messageHub;

        public MessageHub(IHubContext<MessageHub> messageHub) 
        {
            _messageHub = messageHub;
        }

        public override Task OnConnectedAsync()
        {
            if(Context.User.Claims.ElementAt(2).Value == "Admin" || Context.User.Claims.ElementAt(1).Value == "Admin") 
            {
                Groups.AddToGroupAsync(Context.ConnectionId, "admin");
            }
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendAddMessage(string userName)
        {
            await _messageHub.Clients.Group("admin").SendAsync("ReceiveData", $"New User Added: {userName}");     
        }
    }
}
