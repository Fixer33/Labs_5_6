using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared
{
    public class UserData
    {
        public string Id { get; private set; }
        public string Name { get; private set; }

        public UserData(string id, string name)
        {
            Id = id;
            Name = name;
        }

        private UserData()
        {
            Id = "";
            Name = "";
        }

        public void UpdateName(string newName)
        {
            Name = newName;
        }

        public string Serialize()
        {
            UserDataSerializable data = new UserDataSerializable()
            {
                Id = Id,
                Name = Name,
            };
            return JsonConvert.SerializeObject(data);
        }

        public static UserData Deserialize(string sData)
        {
            UserDataSerializable saved;
            try
            {
                saved = JsonConvert.DeserializeObject<UserDataSerializable>(sData);
            }
            catch
            {
                return null;
            }

            UserData res = new UserData();
            res.Name = saved.Name;
            res.Id = saved.Id;

            return res;
        }

        [Serializable]
        public struct UserDataSerializable
        {
            public string Id;
            public string Name;
        }
    }
}
