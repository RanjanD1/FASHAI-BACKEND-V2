using Microsoft.AspNetCore.SignalR;
namespace FashAI.Hubs
{
    public class SwapHub : Hub
    {
        public async Task NotifySwapRequest(string userId, string message)
        {
            await Clients.User(userId).SendAsync("ReceiveSwapRequest", message);
        }

        public async Task NotifySwapResponse(string userId, string message)
        {
            await Clients.User(userId).SendAsync("ReceiveSwapResponse", message);
        }
    }
}
