using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace oventy
{
    public partial class MainPage : TabbedPage
    {
        public MainPage ()
        {
            NavigationPage.SetHasNavigationBar(this, false);

            Children.Add(new WebviewPage());
            Children.Add(new SettingsPage());
        }

        protected override bool OnBackButtonPressed()
        {
            return CurrentPage.SendBackButtonPressed();
        }
    }
}