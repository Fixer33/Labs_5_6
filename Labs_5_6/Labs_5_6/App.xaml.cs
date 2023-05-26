using Labs_5_6.Services;
using Labs_5_6.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Labs_5_6
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
