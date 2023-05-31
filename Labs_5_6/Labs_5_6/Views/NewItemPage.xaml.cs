using Labs_5_6.Models;
using Labs_5_6.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Labs_5_6.Views
{
    public partial class NewItemPage : ContentPage
    {
        public Item Item { get; set; }

        private NewItemViewModel _model;

        public NewItemPage()
        {
            InitializeComponent();
            _model = new NewItemViewModel();
            BindingContext = _model;
        }

        private async void AddMemberClicked(object sender, EventArgs e)
        {
            string id = await DisplayPromptAsync("Добавить пользователя", "Введите Id пользователя, которого хотите добавить");
            if (string.IsNullOrEmpty(id))
                return;

            bool userExists = await Network.IsUserIdExists(id);
            if (userExists)
            {
                _model.AddMember(id);
                if (MemberList.Text.Length > 0)
                    MemberList.Text += ';';
                MemberList.Text += id;
            }
        }
    }
}