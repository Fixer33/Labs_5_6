using Labs_5_6.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace Labs_5_6.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();

            
        }
    }
}