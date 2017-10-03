using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace oventy
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        async void OnLoginButtonClicked(object sender, EventArgs args)
        {
            await LoginAsync(EmailEntry.Text, PasswordEntry.Text);
        }

        async Task LoginAsync(string useremail, string userpassword)
        {
            if (CheckValidate())
            {
                var httpHandler = new HttpHandler();
                var result = await httpHandler.LoginAsync(useremail, userpassword);

                if (result)
                {
                    await Navigation.PushAsync(new WebviewPage());
                }
            }
        }

        private bool CheckValidate()
        {
            if (string.IsNullOrEmpty(EmailEntry.Text) || string.IsNullOrEmpty(PasswordEntry.Text))
            {
                DisplayAlert("Warning!", "Email and password is required!", "OK");
                return false;
            }
            else if (!EmailEntry.Text.IsValidEmail())
            {
                DisplayAlert("Warning!", "Please input valid email", "OK");
                return false;
            }
            else
                return true;
        }
    }
}
