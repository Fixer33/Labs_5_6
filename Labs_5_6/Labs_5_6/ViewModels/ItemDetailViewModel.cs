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

        public ItemDetailViewModel()
        {
            
        }

        public void Appearing()
        {
            //UpdateMessages();
        }

        public async void GoBack()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
