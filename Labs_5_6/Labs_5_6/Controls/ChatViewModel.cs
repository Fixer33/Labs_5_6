using Labs_5_6.Services;
using Labs_5_6.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Labs_5_6.Controls
{
    public class ChatViewModel : BaseViewModel, INotifyPropertyChanged
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
            }
        }

        public string NewMessage
        {
            get => _newMessage;
            set => SetProperty(ref _newMessage, value);
        }

        public ChatViewModel()
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
                OnPropertyChanged(nameof(NewMessage));
            }
        }

        private void SendMessage()
        {
            if (string.IsNullOrEmpty(ItemId))
                return;

            if (!string.IsNullOrWhiteSpace(NewMessage))
            {
                Network.SendChatMessage(int.Parse(ItemId), NewMessage);
                NewMessage = string.Empty;
                OnPropertyChanged(nameof(NewMessage));
            }
        }

        public async Task UpdateMessages()
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
