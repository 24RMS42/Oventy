using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace oventy
{
	public partial class SettingsPage : ContentPage
	{
		public SettingsPage ()
		{
			InitializeComponent();
        }
        void Logout_Clicked(object sender, EventArgs e)
        {
            Settings.RemoveAccessToken();
            Settings.RemoveRefreshToken();

            Settings.RemoveUsername();
            Settings.RemovePassword();

            Navigation.PopAsync();
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}