using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Shared
{
    public class ChatData
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public List<string> Members { get; private set; }

        private ChatData()
        {
            Members = new List<string>();
            Name = Id.ToString();
        }

        public ChatData(int id, string name, params string[] members)
        {
            Id = id;
            Name = name;
            Members = new List<string>();
            Members.AddRange(members);
        }

        public ChatData(int id, ChatData cloneFrom)
        {
            Id = id;
            Name = cloneFrom.Name;
            Members = cloneFrom.Members;
        }

        public string Serialize()
        {
            ChatDataSerializable data = new ChatDataSerializable()
            {
                Id = Id,
                Members = Members,
                Name = Name,
            };
            return JsonConvert.SerializeObject(data);
        }

        public static ChatData Deserialize(string sData)
        {
            ChatDataSerializable saved;
            try
            {
                saved = JsonConvert.DeserializeObject<ChatDataSerializable>(sData);
            }
            catch
            {
                return null;
            }
            ChatData data = new ChatData();
            data.Id = saved.Id;
            data.Name = saved.Name;
            data.Members = saved.Members;
            return data;
        }

        [Serializable]
        public struct ChatDataSerializable
        {
            public int Id;
            public string Name;
            public List<string> Members;
        }
    }
}
