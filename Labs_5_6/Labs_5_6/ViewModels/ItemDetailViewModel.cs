using Labs_5_6.Models;
using Labs_5_6.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Labs_5_6.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class ItemDetailViewModel : BaseViewModel, INotifyPropertyChanged
    {
        private string itemId;
        private ObservableCollection<ChatMessage> messages;
        private string _newMessage;
        private bool _messageUpdateBusy = false;
        private MockDataStore _dataStore;

        public ICommand SendMessageCommand { get; }

        public ObservableCollection<ChatMessage> Messages
        {
            get { return messages; }
            set
            {
                messages = value;
                OnPropertyChanged(nameof(Messages));
            }
        }

        public string ItemId
        {
            get
            {
                return itemId;
            }
            set
            {
                itemId = value;
                LoadItemId(value);
            }
        }

        public string NewMessage
        {
            get => _newMessage;
            set => SetProperty(ref _newMessage, value);
        }

        public ItemDetailViewModel()
        {
            Messages = new ObservableCollection<ChatMessage>();
            SendMessageCommand = new Command(SendMessage);

            _dataStore = (MockDataStore)DataStore;
            _dataStore.ChatMessagesUpdated += ChatMessageRecieved;
        }

        private void ChatMessageRecieved(int chatId)
        {
            if (itemId.Equals(chatId.ToString()))
            {
                UpdateMessages();
            }
        }

        public async void LoadItemId(string itemId)
        {
            try
            {
                var item = await DataStore.GetItemAsync(itemId);
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }

        private void SendMessage()
        {
            if (!string.IsNullOrWhiteSpace(NewMessage))
            {
                Network.SendChatMessage(int.Parse(ItemId), NewMessage);
                NewMessage = string.Empty;
                OnPropertyChanged(nameof(NewMessage));
            }
        }

        private async void UpdateMessages()
        {
            while (_messageUpdateBusy)
            {
                await Task.Delay(100);
            }

            _messageUpdateBusy = true;

            Messages.Clear();
            int chatId = int.Parse(ItemId);

            var messages = _dataStore.GetChatMessageHistory(chatId);
            for (int i = 0; i < messages.Count; i++)
            {
                var message = new ChatMessage();
                message.Sender = await _dataStore.GetUserName(messages[i].senderId);
                message.Message = messages[i].message;
                Messages.Add(message);
            }

            _messageUpdateBusy = false;
        }

        public void Appearing()
        {
            UpdateMessages();
        }

        public async void GoBack()
        {
            await Shell.Current.GoToAsync("..");
        }

        // INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public class ChatMessage
        {
            public string Sender { get; set; }
            public string Message { get; set; }
        }
    }
}
