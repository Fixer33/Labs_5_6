using Labs_5_6.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Labs_5_6.Services
{
    public class MockDataStore : IDataStore<Item>
    {
        private static readonly string CACHE_CHATS_PATH = Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ChatCache");

        readonly List<Item> items;
        Dictionary<int, List<(string, string)>> chatMessages;

        public MockDataStore()
        {
            App.OnAppPaused += App_OnAppPaused;

            items = new List<Item>();
            LoadChatHistory();

            Network.ChatDataAccepted += Network_ChatDataAccepted;
            Network.ChatCreated += Network_ChatDataAccepted;

            Network.RequestChatsData();
        }

        private async void Network_ChatDataAccepted(Shared.ChatData obj)
        {
            await AddItemAsync(new Item(obj.Id.ToString(), obj.Name));
            if (chatMessages.ContainsKey(obj.Id) == false)
            {
                chatMessages.Add(obj.Id, new List<(string, string)>());
            }
        }

        ~MockDataStore()
        {
            App.OnAppPaused -= App_OnAppPaused;
            Network.ChatCreated -= Network_ChatDataAccepted;
        }

        private void LoadChatHistory()
        {
            chatMessages = new Dictionary<int, List<(string, string)>>();
            if (File.Exists(CACHE_CHATS_PATH) == false)
            {
                return;
            }

            SerializableChatHistory saved;
            try
            {
                saved = JsonConvert.DeserializeObject<SerializableChatHistory>(File.ReadAllText(CACHE_CHATS_PATH));
            }
            catch
            {
                return;
            }

            for (int i = 0; i < saved.Chats.Count; i++)
            {
                var messages = new List<(string, string)>();
                for (int k = 0; k < saved.Chats[i].Senders.Count; k++)
                {
                    messages.Add((saved.Chats[i].Senders[k], saved.Chats[i].Messages[k]));
                }

                chatMessages.Add(saved.Chats[i].Id, messages);
            }
        }

        private void SaveChatHistory()
        {
            if (File.Exists(CACHE_CHATS_PATH))
                File.Delete(CACHE_CHATS_PATH);

            File.Create(CACHE_CHATS_PATH).Close();

            SerializableChatHistory toSave = new SerializableChatHistory();
            toSave.Chats = new List<SerializableChatMessage>();
            foreach (var chat in chatMessages)
            {
                SerializableChatMessage msg = new SerializableChatMessage();
                msg.Id = chat.Key;
                msg.Senders = new List<string>();
                msg.Messages = new List<string>();
                foreach (var message in chat.Value)
                {
                    msg.Senders.Add(message.Item1);
                    msg.Messages.Add(message.Item2);
                }
            }
            File.WriteAllText(CACHE_CHATS_PATH, JsonConvert.SerializeObject(toSave));
        }

        private void App_OnAppPaused()
        {
            SaveChatHistory();
        }

        public async Task<bool> AddItemAsync(Item item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Item item)
        {
            var oldItem = items.Where((Item arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(oldItem);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldItem = items.Where((Item arg) => arg.Id == id).FirstOrDefault();
            items.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<Item> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Item>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }

        [Serializable]
        public struct SerializableChatHistory
        {
            public List<SerializableChatMessage> Chats;
        }

        [Serializable]
        public struct SerializableChatMessage
        {
            public int Id;
            public List<string> Senders;
            public List<string> Messages;
        }
    }
}