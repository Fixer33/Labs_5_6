using Shared;
using SuperSimpleTcp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal static class CommandSender
    {
        private static SimpleTcpServer _server;

        public static void Initialize(ref SimpleTcpServer server)
        {
            _server = server;
        }

        public static void SendCheckIdReply(string toIp, bool exists)
        {
            _server.Send(toIp, new byte[] { (byte)ServerPacket.IdExistsResponse, Convert.ToByte(exists) });
        }

        public static void SendChatArray(string toIp, ChatArray chats)
        {
            string dataToSend = chats.Serialize();
            List<byte> dataToSendList = new List<byte>();
            dataToSendList.Add((byte)ServerPacket.ChatDataNotification);
            dataToSendList.AddRange(Encoding.UTF8.GetBytes(dataToSend));
            _server.Send(toIp, dataToSendList.ToArray());
        }

        public static void SendChatData(string toIp, ChatData chat)
        {
            string dataToSend = chat.Serialize();

            List<byte> dataToSendList = new List<byte>();
            dataToSendList.Add((byte)ServerPacket.ChatCreated);
            dataToSendList.AddRange(Encoding.UTF8.GetBytes(dataToSend));
            _server.Send(toIp, dataToSendList.ToArray());
        }
    }
}
