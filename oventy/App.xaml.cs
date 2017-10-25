using System;
using Xamarin.Forms;

namespace oventy
{
    public partial class App : Application
    {
        public static string DeviceType = "";

        public App()
        {
            InitializeComponent();

            if (string.IsNullOrEmpty(Settings.InstallationId))
            {
                Settings.InstallationId = Guid.NewGuid().ToString();
            }

            Xamarin.Forms.PlatformConfiguration.AndroidSpecific.Application.SetWindowSoftInputModeAdjust(this, Xamarin.Forms.PlatformConfiguration.AndroidSpecific.WindowSoftInputModeAdjust.Resize);
            MainPage = new NavigationPage(new LoginPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
