using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared;
using SuperSimpleTcp;

namespace Server
{
    public class ServerLogic
    {
        private SimpleTcpServer _server;
        private List<string> _unregisteredIps;
        private Dictionary<string, UserData> _users;

        public ServerLogic(SimpleTcpServer server)
        {
            _server = server;
            _server.Events.ClientConnected += Events_ClientConnected;
            _server.Events.ClientDisconnected += Events_ClientDisconnected;
            _server.Events.DataReceived += Events_DataReceived;
            _server.Events.DataSent += Events_DataSent;

            _unregisteredIps = new List<string>();
            _users = new Dictionary<string, UserData>();

            CommandService.DeviceIdentified += CommandService_DeviceIdentified;
            CommandService.DeviceNameChanged += CommandService_DeviceNameChanged;
        }

        private void CommandService_DeviceNameChanged(string ip, string newName)
        {
            if (_users.ContainsKey(ip))
            {
                var user = _users[ip];
                user.UpdateName(newName);
                _users[ip] = user;
                Database.UpdateUserName(user.Id, user.Name);
                Console.WriteLine("Changed name of user " + user.Id + " to " + newName);
                return;
            }

            Console.WriteLine("No such user");
        }

        private void CommandService_DeviceIdentified(string ip, string id)
        {
            int index = -1;
            for (int i = 0; i < _unregisteredIps.Count; i++)
            {
                if (_unregisteredIps[i].Equals(ip))
                {
                    Console.WriteLine(1);
                    index = i;
                    break;
                }
            }

            if (index < 0)
                return;

            _unregisteredIps.RemoveAt(index);

            var user = Database.GetUser(id);
            if (user == null)
            {
                user = new UserData(id, id);
                Database.AddUser(id);
            }
            _users.Add(ip, user);

            Console.WriteLine("Authorised user " + id);
        }

        private void Events_DataSent(object? sender, DataSentEventArgs e)
        {
            Console.WriteLine($"Sent {e.BytesSent} bytes to {e.IpPort}");
        }

        private void Events_DataReceived(object? sender, DataReceivedEventArgs e)
        {
            Console.WriteLine($"Recieved {e.Data.Count} bytes from {e.IpPort}");
            CommandService.HandleCommand(e.Data.Array, e.IpPort);
        }

        private void Events_ClientConnected(object? sender, ConnectionEventArgs e)
        {
            Console.WriteLine($"Client connected {e.IpPort}");
            _unregisteredIps.Add(e.IpPort);
        }

        private void Events_ClientDisconnected(object? sender, ConnectionEventArgs e)
        {
            Console.WriteLine($"Client disconnected {e.IpPort} because of: {e.Reason}");

            if (_unregisteredIps.Contains(e.IpPort))
            {
                _unregisteredIps.Remove(e.IpPort);
            }

            if (_users.ContainsKey(e.IpPort))
            {
                _users.Remove(e.IpPort);
            }
        }

        public void Stop()
        {

        }
    }
}
