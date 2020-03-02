using Microsoft.AspNetCore.SignalR;
using NLog;
using System.Threading.Tasks;

namespace Crossroads.Service.Contact.Hubs
{
    //[Authorize]
    public class EventHub : Hub
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public override async Task OnConnectedAsync()
        {
            _logger.Info($"Client connection added: {Context.ConnectionId}");
            await Groups.AddToGroupAsync(Context.ConnectionId, "Event_Checkin");
            await base.OnConnectedAsync();
        }
    }
}
