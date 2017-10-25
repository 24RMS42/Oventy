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
            NavigationPage.SetHasNavigationBar(this, false);

            webview.Navigated += (o, s) => {
                Console.WriteLine("login webview loaded");
                loginButton.IsEnabled = true;
            };
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (!string.IsNullOrWhiteSpace(Settings.Username) && !string.IsNullOrWhiteSpace(Settings.Password))
            {
                EmailEntry.Text = Settings.Username;
                PasswordEntry.Text = Settings.Password;
                await LoginAsync(Settings.Username, Settings.Password);
            }
            else
            {
                EmailEntry.Text = "";
                PasswordEntry.Text = "";
            }
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
                var login_result = await httpHandler.LoginAsync(useremail, userpassword);

                if (login_result)
                {
                    var register_device_result = await httpHandler.InstallDeviceAsync();
                    if (register_device_result)
                    {
                        var jsAccessTokenString = $"localStorage.setItem('ls.accessToken', '{Settings.AccessToken}')";
                        var jsRefreshTokenString = $"localStorage.setItem('ls.refreshToken', '{Settings.RefreshToken}')";

                        webview.Eval(jsAccessTokenString);
                        webview.Eval(jsRefreshTokenString);

                        Settings.Username = useremail;
                        Settings.Password = userpassword;

                        await Navigation.PushAsync(new WebviewPage());
                    }
                    else
                    {
                        Settings.Username = "";
                        Settings.Password = "";
                    }
                }
                else
                {
                    Settings.Username = "";
                    Settings.Password = "";
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
