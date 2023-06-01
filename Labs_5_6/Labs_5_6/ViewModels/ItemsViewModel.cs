using Labs_5_6.Models;
using Labs_5_6.Services;
using Labs_5_6.Views;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Labs_5_6.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        private Item _selectedItem;

        public bool IsPortrait;
        public event Action<Item> ItemSelected;

        public ObservableCollection<Item> Items { get; }
        public Command LoadItemsCommand { get; }
        public Command AddItemCommand { get; }
        public Command DeleteItemCommand { get; }
        public Command<Item> ItemTapped { get; }

        public ItemsViewModel()
        {
            Title = "Чаты";
            Items = new ObservableCollection<Item>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            ItemTapped = new Command<Item>(OnItemSelected);
            DeleteItemCommand = new Command<Item>(OnItemDeleted);

            AddItemCommand = new Command(OnAddItem);

            Network.ChatCreated += Network_ChatCreated;
            Network.ChatDeleted += Network_ChatDeleted;
        }

        private async void Network_ChatDeleted(int obj)
        {
            await Task.Delay(1000);
            await ExecuteLoadItemsCommand();
        }

        ~ItemsViewModel()
        {
            Network.ChatCreated -= Network_ChatCreated;
            Network.ChatDeleted -= Network_ChatDeleted;
        }

        private async void Network_ChatCreated(Shared.ChatData obj)
        {
            await ExecuteLoadItemsCommand();
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await DataStore.GetItemsAsync(true);
                foreach (var item in items)
                {
                    Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
            SelectedItem = null;
            LoadItemsCommand.Execute(null);
        }

        public Item SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnItemSelected(value);
            }
        }

        private async void OnAddItem(object obj)
        {
            await Shell.Current.GoToAsync(nameof(NewItemPage));
        }

        async void OnItemSelected(Item item)
        {
            if (item == null)
                return;

            if (IsPortrait)
            {
                // This will push the ItemDetailPage onto the navigation stack
                await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.ItemId)}={item.Id}");
            }
            else
            {
                ItemSelected?.Invoke(item);
            }
        }

        async void OnItemDeleted(Item item)
        {
            if (item == null)
                return;

            var dataStore = (MockDataStore)DataStore;
            await dataStore.DeleteItemAsync(item.Id);
        }
    }
}