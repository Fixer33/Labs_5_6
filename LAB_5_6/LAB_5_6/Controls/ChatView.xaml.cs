using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LAB_5_6.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChatView : ContentView
    {
        public static readonly BindableProperty SendMessageButton =
            BindableProperty.Create("SendMsg", typeof(bool), typeof(ChatView), false);

        private ChatViewModel _model;

        public ChatView()
        {
            InitializeComponent();
            _model = new ChatViewModel();
            BindingContext = _model;
            _model.PropertyChanged += _model_PropertyChanged;
        }

        private void _model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            ScrollDown();   
        }

        private void ScrollDown()
        {
            var items = itemHolder.ItemsSource as IList;
            if (items != null && items.Count > 0)
            {
                var lastItem = items[items.Count - 1];
                itemHolder.ScrollTo(lastItem, ScrollToPosition.End, true);
            }
        }

        public async void OpenChat(int chatId)
        {
            _model.ItemId = chatId.ToString();
            await _model.UpdateMessages();
            ScrollDown();
        }
    }
}