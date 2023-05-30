using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Labs_5_6.ViewModels;

namespace Labs_5_6.Views
{
    public partial class AboutPage : ContentPage
    {
        public static AboutPage Instance { get; private set; }

        private AboutViewModel _model;

        public AboutPage()
        {
            Instance = this;

            InitializeComponent();
            _model = new AboutViewModel();
            BindingContext = _model;

            Appearing += OnAppear;
        }

        private void OnAppear(object sender, EventArgs e)
        {
            _model.Id = App.UserId;
            _model.Name = App.UserName;
        }

        public async System.Threading.Tasks.Task<string> GetPopupInpText(string header, string message)
        {
            string result = await DisplayPromptAsync(header, message);
            return result;
        }

        private async void NameTapped(object sender, EventArgs e)
        {
            _model.Name = await DisplayPromptAsync("Изменение имени", "Измените отображаемое имя", "Сохранить", "Отмена", initialValue: _model.Name);
            App.Instance.SetName(_model.Name);
        }
    }
}