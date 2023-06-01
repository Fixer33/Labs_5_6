using Labs_5_6.Models;
using Labs_5_6.ViewModels;
using Labs_5_6.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Labs_5_6.Views
{
    public partial class ItemsPage : ContentPage
    {
        ItemsViewModel _viewModel;

        public ItemsPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new ItemsViewModel();
            _viewModel.ItemSelected += ItemSelected;
        }

        private void ItemSelected(Item obj)
        {
            chatView.OpenChat(int.Parse(obj.Id));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }

        private void ItemsListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            _viewModel.ItemTapped.Execute(e.Item);
        }

        private void DeleteChatClicked(object sender, EventArgs e)
        {
            var menuItem = (MenuItem)sender;
            var item = (Item)menuItem.BindingContext;
            _viewModel.DeleteItemCommand.Execute(item);
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            _viewModel.IsPortrait = width < height;
            grid.ColumnDefinitions[1].Width = width > height ? new GridLength(2, GridUnitType.Star) : new GridLength(0, GridUnitType.Star);
            chatView.IsVisible = width > height;
        }
    }
}