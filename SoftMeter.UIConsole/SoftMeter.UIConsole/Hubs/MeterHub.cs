using Microsoft.AspNet.SignalR;
using SoftMeter.UIConsole.Models;
using System.Threading.Tasks;

namespace SoftMeter.UIConsole.Hubs
{
    public class MeterHub : Hub
    {
        public void Send(string name, SoftMeterNotification message)
        {
            Clients
                .Group(Clients.Caller.group)
                .notify(name, message);
        }

        public Task JoinGroup(string groupName)
        {
            return Groups.Add(Context.ConnectionId, groupName);
        }

        public Task LeaveGroup(string groupName)
        {
            return Groups.Remove(Context.ConnectionId, groupName);
        }
    }
}