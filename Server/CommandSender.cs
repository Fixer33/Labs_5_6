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

        public static void SendChatDeleteConfirmation(string toIp, int chatId)
        {
            List<byte> dataToSendList = new List<byte>();
            dataToSendList.Add((byte)ServerPacket.ChatDeleted);
            dataToSendList.Add(Convert.ToByte(chatId));
            _server.Send(toIp, dataToSendList.ToArray());
        }

        public static void SendChatMessage(string toIp, ChatMessageData message)
        {
            string dataToSend = message.Serialize();

            List<byte> dataToSendList = new List<byte>();
            dataToSendList.Add((byte)ServerPacket.ChatMessageSent);
            dataToSendList.AddRange(Encoding.UTF8.GetBytes(dataToSend));
            _server.Send(toIp, dataToSendList.ToArray());
        }

        public static void SendUserName(string toIp, string name)
        {
            List<byte> dataToSendList = new List<byte>();
            dataToSendList.Add((byte)ServerPacket.UserNameResponse);
            dataToSendList.AddRange(Encoding.UTF8.GetBytes(name));
            _server.Send(toIp, dataToSendList.ToArray());
        }
    }
}
