using Labs_5_6.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace Labs_5_6.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        private ItemDetailViewModel _model;
        private bool _isVisible = false;

        public ItemDetailPage()
        {
            InitializeComponent();
            _model = new ItemDetailViewModel();
            BindingContext = _model;

            Appearing += ItemDetailPage_Appearing;
            Disappearing += ItemDetailPage_Disappearing;

            Network.ChatDeleted += Network_ChatDeleted;
        }

        ~ItemDetailPage()
        {
            Network.ChatDeleted -= Network_ChatDeleted;
        }

        private void Network_ChatDeleted(int chatId)
        {
            if (_isVisible && _model.ItemId.Equals(chatId.ToString()))
            {
                _model.GoBack();
            }
        }

        private void ItemDetailPage_Disappearing(object sender, System.EventArgs e)
        {
            _isVisible = false;
        }

        private void ItemDetailPage_Appearing(object sender, System.EventArgs e)
        {
            _isVisible = true;
            _model.Appearing();
        }
    }
}