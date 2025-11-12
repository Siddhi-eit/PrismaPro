using MDFusionLabHaute.Domain.Entities;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace MDFusionLabHaute.API.Helper
{
    public class ConnectionHub : Hub
    {
        private readonly IHubContext<ConnectionHub> _connectionHub;

        public ConnectionHub(IHubContext<ConnectionHub> hubContext)
        {
            _connectionHub = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
            PublicFile._connection ??= new ConnectionMapping<string>();
        }

        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception ex)
        {
            return base.OnDisconnectedAsync(ex);
        }

        public Task SetUserID(string userID)
        {
            if (!string.IsNullOrEmpty(userID))
            {
                PublicFile._connection.Remove(userID, Context.ConnectionId);
                PublicFile._connection.Add(userID, Context.ConnectionId);
            }
            return Task.CompletedTask;
        }

        public static async Task DispenserNow(IHubContext<ConnectionHub> hubContext, string userID, string message)
        {
            foreach (var connectionId in PublicFile._connection.GetConnections(userID))
            {
                await hubContext.Clients.Client(connectionId).SendAsync("DispenseNow", message);
            }
        }

        public Task DispenseCompleted(string[] dispenseCompletedParm)
        {
            if (!string.IsNullOrEmpty(dispenseCompletedParm[5]))
            {
                foreach (var connectionId in PublicFile._connection.GetConnections(dispenseCompletedParm[5]))
                {
                    _connectionHub.Clients.Client(connectionId).SendAsync("DispenseSuccess", dispenseCompletedParm);
                }
            }
            return Task.CompletedTask;
        }

        public Task DispenseError(string desktopUserID, string userID)
        {
            if (!string.IsNullOrEmpty(userID))
            {
                foreach (var connectionId in PublicFile._connection.GetConnections(userID))
                {
                    _connectionHub.Clients.Client(connectionId).SendAsync("DispenseError", desktopUserID);
                }
            }
            return Task.CompletedTask;
        }
    }
}