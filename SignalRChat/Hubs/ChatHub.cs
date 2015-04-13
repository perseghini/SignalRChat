using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SignalRChat.Hubs
{
    public class ChatHub : Hub
    {
        public void Send(string name, string message)
        {
            Clients.All.addNewMessageToPage(Clients.Caller.userName, message);
        }

        public void Connected()
        {
            //Send all client new user connected
            Clients.All.newConnectedUser(Clients.Caller.userName);
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            Clients.All.disconnectedUser(Clients.Caller.userName);
            return base.OnDisconnected(stopCalled);
        }
    }
}