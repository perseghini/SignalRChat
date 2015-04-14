using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRChat.Hubs
{
    public class ChatHub : Hub
    {
        private static readonly Dictionary<string, string> Users;
        private static readonly List<MessageLog> MessageLogs;

        static ChatHub()
        {
            Users = new Dictionary<string, string>();
            MessageLogs = new List<MessageLog>();
        }

        public void Send(string message, string color)
        {
            var msg = new MessageLog
            {
                UserConnectionId = Context.ConnectionId,
                Username = Clients.Caller.userName,
                TimeStamp = DateTime.Now,
                Message = message
            };
            MessageLogs.Add(msg);
            // Save MessageLogs (to a database, a file, etc..)

            Clients.All.addNewMessageToPage(Clients.Caller.userName, message, color);
        }

        public void Connected()
        {
            Users.Add(Context.ConnectionId, Clients.Caller.userName);
            Clients.All.newConnectedUser(Clients.Caller.userName, Users.Select(p => p.Value));
        }

        public void GetAllConnectedUsers()
        {
            Clients.Caller.allUsers(Users.Select(p => p.Value));
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            string userName = "Unknown";
            if (Users.ContainsKey(Context.ConnectionId))
            {
                userName = Users[Context.ConnectionId];
                Users.Remove(Context.ConnectionId);
            }

            Clients.All.disconnectedUser(userName, Users.Select(p => p.Value));
            return base.OnDisconnected(stopCalled);
        }
    }

    public class MessageLog
    {
        public string UserConnectionId { get; set; }

        public string Username { get; set; }

        public DateTime TimeStamp { get; set; }

        public string Message { get; set; }
    }
}