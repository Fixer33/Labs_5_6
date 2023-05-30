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

        public List<ChatData> GetChats()
        {
            List<ChatData> res = new List<ChatData>();
            for (int i = 0; i < SerializedChats.Count; i++)
            {
                res.Add(ChatData.Deserialize(SerializedChats[i]));
            }
            return res;
        }
    }
}
