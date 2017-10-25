using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace oventy
{
    public partial class WebviewPage : ContentPage
    {
        public WebviewPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            var jsAccessTokenString = $"localStorage.setItem('ls.accessToken', '{Settings.AccessToken}')";
            var jsRefreshTokenString = $"localStorage.setItem('ls.refreshToken', '{Settings.RefreshToken}')";

            Webview.Navigated += (o, s) => {
                Webview.Eval(jsAccessTokenString);
                Webview.Eval(jsRefreshTokenString);
            };

            Webview.Navigating += Webview_Navigating;
        }

        private void Webview_Navigating(object sender, WebNavigatingEventArgs e)
        {
            if(e.Url.StartsWith("https://www.oventy.com/login"))
            {
                e.Cancel = true;

                Settings.RemoveAccessToken();
                Settings.RemoveRefreshToken();

                Settings.RemoveUsername();
                Settings.RemovePassword();

                Navigation.PopAsync();
            }

            if (e.Url.StartsWith("mailto") || e.Url.StartsWith("https://www.facebook.com/sharer") || !e.Url.StartsWith("https://www.oventy.com/"))
            {
                e.Cancel = true;

                Device.OpenUri(new Uri(e.Url));
            }
        }

        protected override bool OnBackButtonPressed()
        {
            Webview.GoBack();
            return true;
        }

    }
}
