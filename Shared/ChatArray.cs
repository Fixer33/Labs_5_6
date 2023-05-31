using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared
{
    [Serializable]
    public class ChatArray
    {
        public List<string> SerializedChats;

        public ChatArray(List<ChatData> chats)
        {
            SerializedChats = new List<string>();
            for (int i = 0; i < chats.Count; i++)
            {
                SerializedChats.Add(chats[i].Serialize());
            }
        }

        private ChatArray()
        {
            SerializedChats = new List<string>();
        }

        public List<ChatData> GetChats()
        {
            List<ChatData> res = new List<ChatData>();
            for (int i = 0; i < SerializedChats.Count; i++)
            {
                res.Add(ChatData.Deserialize(SerializedChats[i]));
            }
            return res;
        }

        public string Serialize()
        {
            SerializedChatArrayData toSave = new SerializedChatArrayData();
            toSave.SerializedChats = SerializedChats.ToArray();
            return JsonConvert.SerializeObject(toSave);
        }

        public static ChatArray Deserialize(string rawData)
        {
            SerializedChatArrayData saved;
            try
            {
                saved = JsonConvert.DeserializeObject<SerializedChatArrayData>(rawData);
            }
            catch
            {
                return null;
            }


            ChatArray res = new ChatArray();
            res.SerializedChats.AddRange(saved.SerializedChats);
            return res;
        }

        [Serializable]
        public struct SerializedChatArrayData
        {
            public string[] SerializedChats;
        }
    }
}
