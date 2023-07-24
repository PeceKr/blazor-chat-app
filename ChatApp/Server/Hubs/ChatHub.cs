using Microsoft.AspNetCore.SignalR;

namespace ChatApp.Hubs
{
    public class ChatHub : Hub
    {
        private static Dictionary<string, string> Users = new Dictionary<string, string>();

        public override async Task OnConnectedAsync()
        {
            var userName = Context.GetHttpContext().Request.Query["username"];
            Users.Add(Context.ConnectionId, userName);
            await SendMessage(string.Empty, $"{userName} joined the channel");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            string userName = Users.FirstOrDefault(x=>x.Key == Context.ConnectionId).Value;
            await SendMessage(string.Empty, $"{userName} left the channel");
        }
        
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("GetMessage", user, message);
        }
    }
}