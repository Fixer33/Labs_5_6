using Shared;
using SuperSimpleTcp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LAB_5_6
{
    public static class Network
    {
        public static event Action<ChatData> ChatDataAccepted;
        public static event Action<ChatData> ChatCreated;
        public static event Action<ChatMessageData> ChatMessageRecieved;
        public static event Action<int> ChatDeleted;

        private static SimpleTcpClient _client;

        private static bool _userIdLocked;
        private static Action<bool> _userIdExistsResponse;

        private static bool _userNameReqLock;
        private static Action<string> _userNameReqResponse;

        public static void Initialize(string ip)
        {
            _client = new SimpleTcpClient(ip, 25566);
            _client.Events.DataReceived += DataReceived;
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

        public static void RequestChatsData()
        {
            _client.Send(new byte[] { (byte)ClientPacket.ChatDataRequest });
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

        public static void CreateChat(string name, List<string> members)
        {
            if (_client.IsConnected)
            {
                List<byte> toSend = new List<byte>();
                toSend.Add((byte)ClientPacket.ChatCreate);
                ChatData chat = new ChatData(-1, name, members.ToArray());
                toSend.AddRange(Encoding.UTF8.GetBytes(chat.Serialize()));
                _client.Send(toSend.ToArray());
            }
        }

        public static void DeleteChat(int chatId)
        {
            if (_client.IsConnected)
            {
                List<byte> toSend = new List<byte>();
                toSend.Add((byte)ClientPacket.ChatDelete);
                toSend.Add(Convert.ToByte(chatId));
                _client.Send(toSend.ToArray());
            }
        }

        public static async Task<bool> IsUserIdExists(string id)
        {
            if (_userIdLocked)
                return false;

            if (_client.IsConnected)
            {
                _userIdLocked = true;
                bool result = false;
                _userIdExistsResponse = (exists) =>
                {
                    result = exists;
                    _userIdLocked = false;
                    _userIdExistsResponse = null;
                };

                List<byte> toSend = new List<byte>();
                toSend.Add((byte)ClientPacket.CheckIdExists);
                toSend.AddRange(Encoding.UTF8.GetBytes(id));
                _client.Send(toSend.ToArray());

                while (_userIdLocked)
                {
                    await Task.Delay(100);
                }
                return result;
            }

            return false;
        }

        public static async Task<string> GetUserName(string id)
        {
            if (_userNameReqLock)
                throw new Exception("RequestAlreadyAwaiting");

            if (_client.IsConnected)
            {
                _userNameReqLock = true;
                string result = "";
                _userNameReqResponse = (name) =>
                {
                    result = name;
                    _userNameReqLock = false;
                    _userNameReqResponse = null;
                };

                List<byte> toSend = new List<byte>();
                toSend.Add((byte)ClientPacket.UserNameRequest);
                toSend.AddRange(Encoding.UTF8.GetBytes(id));
                _client.Send(toSend.ToArray());

                while (_userNameReqLock)
                {
                    await Task.Delay(100);
                }
                return result;
            }

            throw new Exception("NOT CONNECTED CLIENT");
        }

        public static void SendChatMessage(int chatId, string message)
        {
            if (_client.IsConnected)
            {
                List<byte> toSend = new List<byte>();
                toSend.Add((byte)ClientPacket.ChatMessageSent);
                ChatMessageData chatMsg = new ChatMessageData(chatId, App.UserId, message);
                toSend.AddRange(Encoding.UTF8.GetBytes(chatMsg.Serialize()));
                _client.Send(toSend.ToArray());
            }
        }

        private static void DataReceived(object sender, DataReceivedEventArgs e)
        {
            byte[] rawData = e.Data.Array;
            if (rawData == null || rawData.Length <= 0)
            {
                Console.WriteLine("Invalid data");
                return;
            }

            List<byte> data = rawData.ToList();
            ServerPacket commType = (ServerPacket)data[0];
            data.RemoveAt(0);

            switch (commType)
            {
                case ServerPacket.IdExistsResponse:
                    IdExistsResponse(data);
                    break;
                case ServerPacket.ChatCreated:
                    ChatCreatedResponse(data);
                    break;
                case ServerPacket.ChatUpdated:

                    break;
                case ServerPacket.ChatDeleted:
                    ChatDeletedHandler(data);
                    break;
                case ServerPacket.UserNameResponse:
                    UserNameResponse(data);
                    break;
                case ServerPacket.UserNameChanged:

                    break;
                case ServerPacket.ChatDataNotification:
                    ChatDataAcceptedHandle(data);
                    break;
                case ServerPacket.ChatMessageSent:
                    ChatMessageRecievedHandler(data);
                    break;
                default:
                    break;
            }
        }

        private static void ChatCreatedResponse(List<byte> data)
        {
            string chatData = GetString(data.ToArray());

            ChatData chat;

            try
            {
                chat = ChatData.Deserialize(chatData);
            }
            catch
            {
                return;
            }

            ChatCreated?.Invoke(chat);
        }

        private static void IdExistsResponse(List<byte> data)
        {
            _userIdExistsResponse?.Invoke(Convert.ToBoolean(data[0]));
        }

        private static void UserNameResponse(List<byte> data)
        {
            _userNameReqResponse?.Invoke(GetString(data.ToArray()));
        }

        private static void ChatDataAcceptedHandle(List<byte> data)
        {
            string serializedChatArray = GetString(data.ToArray());
            ChatArray chats = ChatArray.Deserialize(serializedChatArray);
            for (int i = 0; i < chats.SerializedChats.Count; i++)
            {
                var chatData = ChatData.Deserialize(chats.SerializedChats[i]);
                if (chatData != null)
                {
                    ChatDataAccepted?.Invoke(chatData);
                }
            }
        }

        private static void ChatMessageRecievedHandler(List<byte> data)
        {
            string rawData = GetString(data.ToArray());
            ChatMessageRecieved?.Invoke(ChatMessageData.Deserialize(rawData));
        }

        private static void ChatDeletedHandler(List<byte> data)
        {
            int chatId = Convert.ToInt32(data[0]);
            ChatDeleted?.Invoke(chatId);
        }

        private static string GetString(byte[] data)
        {
            string res = Encoding.UTF8.GetString(data.ToArray());
            return res.Trim('\0');
        }

    }
}
