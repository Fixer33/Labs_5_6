using Shared;
using SuperSimpleTcp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Labs_5_6
{
    public static class Network
    {
        private static SimpleTcpClient _client;

        public static void Initialize(string ip)
        {
            _client = new SimpleTcpClient(ip, 25566);
            _client.Events.Connected += (s, e) =>
            {
                List<byte> msg = new List<byte>();
                msg.Add((byte)ClientPacket.ClientIdInit);
                msg.AddRange(Encoding.UTF8.GetBytes(App.UserId));
                _client.Send(msg.ToArray());
            };
            _client.Events.Disconnected += (s, e) =>
            {
                RetryConnect();
            };
            try
            {
                _client.Connect();
            }
            catch (Exception e)
            {
                RetryConnect();
            }
        }

        public static void ChangeName(string newName)
        {
            if (_client.IsConnected)
            {
                List<byte> dataToSend = new List<byte>();
                dataToSend.Add((byte)ClientPacket.ClientNameChange);
                dataToSend.AddRange(Encoding.UTF8.GetBytes(newName));
                _client.Send(dataToSend.ToArray());
            }
        }

        private static async void RetryConnect()
        {
            if (_client.IsConnected)
                return;

            await Task.Delay(1000);
            try
            {
                _client.Connect();
            }
            catch
            {
                RetryConnect();
                return;
            }
        }
    }
}
