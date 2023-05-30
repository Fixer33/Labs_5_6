using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Labs_5_6.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        private string _id;
        private string _name;

        public string Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public AboutViewModel()
        {
            Title = "Функциональное меню";
        }
    }
}