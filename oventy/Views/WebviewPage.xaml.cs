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

            var jsAccessTokenString = $"localStorage.setItem('ls.accessToken', '{Settings.AccessToken}')";
            var jsRefreshTokenString = $"localStorage.setItem('ls.refreshToken', '{Settings.RefreshToken}')";

            webview.Navigated += (o, s) => {
                webview.Eval(jsAccessTokenString);
                webview.Eval(jsRefreshTokenString);
            };
        }
    }
}
