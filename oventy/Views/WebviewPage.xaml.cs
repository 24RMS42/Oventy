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
        }

        void Logout_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
            Settings.RemoveAccessToken();
            Settings.RemoveRefreshToken();
        }
    }
}
