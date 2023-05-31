using Newtonsoft.Json;
using System;

namespace Shared
{
    public class ChatMessageData
    {
        public int ChatId { get; private set; }
        public string SenderId { get; private set; }
        public string Message { get; private set; }

        private ChatMessageData()
        {

        }

        public ChatMessageData(int chatId, string senderId, string message)
        {
            ChatId = chatId;
            SenderId = senderId;
            Message = message;
        }

        public string Serialize()
        {
            SerializableMessageData toSave = new SerializableMessageData();
            toSave.Message = Message;
            toSave.SenderId = SenderId;
            toSave.ChatId = ChatId;

            return JsonConvert.SerializeObject(toSave);
        }

        public static ChatMessageData Deserialize(string raw)
        {
            SerializableMessageData saved;

            try
            {
                saved = JsonConvert.DeserializeObject<SerializableMessageData>(raw);
            }
            catch
            {
                return null;
            }

            ChatMessageData data = new ChatMessageData();
            data.Message = saved.Message;
            data.SenderId = saved.SenderId;
            data.ChatId = saved.ChatId;
            return data;
        }

        [Serializable]
        public struct SerializableMessageData
        {
            public int ChatId;
            public string SenderId;
            public string Message;
        }
    }
}
