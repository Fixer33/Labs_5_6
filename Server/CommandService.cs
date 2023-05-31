using Shared;
using System.Text;

namespace Server
{
    public static class CommandService
    {
        public static event Action<string, string> DeviceIdentified;
        public static event Action<string, string> DeviceNameChanged;
        public static event Action<ChatData> ChatCreateRequest;
        public static event Action<string> ChatDataRequested;

        public static void HandleCommand(byte[] rawData, string from)
        {
            if (rawData == null || rawData.Length <= 0)
            {
                Console.WriteLine("Invalid data");
                return;
            }

            List<byte> data = rawData.ToList();
            ClientPacket commType = (ClientPacket)data[0];
            data.RemoveAt(0);

            switch (commType)
            {
                case ClientPacket.ClientIdInit:
                    ClientIdInit(data, from);
                    break;
                case ClientPacket.ClientNameChange:
                    ClientNameChange(data, from);
                    break;
                case ClientPacket.CheckIdExists:
                    CheckIdExists(data, from);
                    break;
                case ClientPacket.ChatCreate:
                    ChatCreate(data, from);
                    break;
                case ClientPacket.ChatUpdate:
                    ChatUpdate(data, from);
                    break;
                case ClientPacket.ChatDelete:
                    ChatDelete(data, from);
                    break;
                case ClientPacket.ChatDataRequest:
                    ChatDataRequestedHandler(data, from);
                    break;
                default:
                    break;
            }
        }

        private static string GetString(byte[] data)
        {
            string res = Encoding.UTF8.GetString(data.ToArray());
            return res.Trim('\0');
        }

        private static void ClientIdInit(List<byte> data, string from)
        {
            string id = null;
            try
            {
                id = GetString(data.ToArray());
            }
            catch
            {
                return;
            }
            DeviceIdentified?.Invoke(from, id);
        }

        private static void ClientNameChange(List<byte> data, string from)
        {
            string newName = null;
            try
            {
                newName = GetString(data.ToArray());
            }
            catch
            {
                Console.WriteLine("Invalid client name change request from " + from);
                return;
            }
            DeviceNameChanged?.Invoke(from, newName);
        }

        private static void CheckIdExists(List<byte> data, string from)
        {
            string userId = GetString(data.ToArray());
            CommandSender.SendCheckIdReply(from, Database.CheckUserIdExistance(userId));
        }

        private static void ChatCreate(List<byte> data, string from)
        {
            ChatData chat = null;
            try
            {
                chat = ChatData.Deserialize(GetString(data.ToArray()));
            }
            catch
            {
                Console.WriteLine("Invalid chat create request! from " + from);
            }

            ChatCreateRequest?.Invoke(chat);
        }

        private static void ChatUpdate(List<byte> data, string from)
        {

        }

        private static void ChatDelete(List<byte> data, string from)
        {

        }

        private static void ChatDataRequestedHandler(List<byte> data, string from)
        {
            ChatDataRequested?.Invoke(from);
        }
    }
}
