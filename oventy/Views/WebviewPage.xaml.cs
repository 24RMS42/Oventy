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

            CustomWebview webview = new CustomWebview {
                Source = "https://www.oventy.com/login",
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            this.Content = new StackLayout {
                Children = {
                     webview
                }
            };
        }
    }
}
