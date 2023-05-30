using Labs_5_6.Services;
using Labs_5_6.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SuperSimpleTcp;
using System.Threading.Tasks;

namespace Labs_5_6
{
    public partial class App : Application
    {
        private static readonly string USER_ID_KEY = "used_id".GetHashCode().ToString();
        private static readonly string USER_NAME_KEY = "used_name".GetHashCode().ToString();
        private static readonly string SERVER_IP_KEY = "server_ip".GetHashCode().ToString();

        public static string UserId { get; private set; }
        public static string UserName { get; private set; }
        public static string ServerIp { get; private set; } = "192.168.1.11";
        public static App Instance { get; private set; }

        public static void ChangeName(string newName)
        {

        }


        public App()
        {
            Instance = this;
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new AppShell();

            InitUser();
            Network.Initialize(ServerIp);
        }

        private void InitUser()
        {
            UserId = Xamarin.Essentials.Preferences.Get(USER_ID_KEY, "");
            if (string.IsNullOrEmpty(UserId))
            {
                string result = DateTime.Now.ToString("yyMMddHH");
                result += Guid.NewGuid();
                result += "1s" + DateTime.Now.Millisecond;
                UserId = Math.Abs(result.GetHashCode()).ToString();
                Xamarin.Essentials.Preferences.Set(USER_ID_KEY, UserId);
            }

            UserName = Xamarin.Essentials.Preferences.Get(USER_NAME_KEY, UserId);
        }

        public void SetName(string newName)
        {
            if (string.IsNullOrEmpty(newName))
                return;

            UserName = newName;
            Xamarin.Essentials.Preferences.Set(USER_NAME_KEY, UserName);
            Network.ChangeName(UserName);
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
