using AndroidX.Core.App;
using AndroidX.Core.Content;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
//using Microsoft.UI.Xaml.Controls;
using System.Text.RegularExpressions;

namespace EchoRelayInstaller {
    public static class GlobalVariables
    {
        public static Entry usernameBox;
        public static Entry passwordBox;
    }
    public partial class MainPage : ContentPage
    {
        Label passwordError;

        public MainPage()
        {
            var menu = new StackLayout();

           
            //var width = menu.Width;
            var width = 720;

            Title = "Echo Relay Quest Patcher";

            var header = new Label
            {
                Text = "Create Echo Relay Account",
                HorizontalOptions = LayoutOptions.Center,
                Padding = new Thickness(0, 0, 0, 20),
                FontSize = 24,
                TranslationY = 10,
            };
            menu.Children.Add(header);

            var usernameLabel = new Label
            {
                Text = "Username",
                FontSize = 12,
                TranslationX = 10,
                MaximumWidthRequest = width - 20,

                Padding = new Thickness(0, 0, 0, 10),
            };
            menu.Children.Add(usernameLabel);

            GlobalVariables.usernameBox = new Entry
            {
                Text = "",
                Placeholder = "Enter username",
                MaximumWidthRequest = width - 20,
                TranslationX = 10,
                TranslationY = -5,
            };
            menu.Children.Add(GlobalVariables.usernameBox);

            var passwordLabel = new Label
            {
                Text = "Password",
                FontSize = 12,
                MaximumWidthRequest = width - 20,
                TranslationX = 10,
                Padding = new Thickness(0, 0, 0, 10),
            };
            menu.Children.Add(passwordLabel);

            GlobalVariables.passwordBox = new Entry {
                Text = "",
                Placeholder = "Enter password",
                MaximumWidthRequest = width - 20,
                TranslationX = 10,
                IsPassword = true,
                TranslationY = -5,
            };
            menu.Children.Add(GlobalVariables.passwordBox);

            var nextButton = new Button { Text = "Next", Padding = new Thickness(0,0,0,10) };
            nextButton.Clicked += checkCreds;
            menu.Children.Add(nextButton);

            passwordError = new Label
            {
                Text = "Text",
                FontSize = 12,

                Padding = new Thickness(0, 0, 0, 10),
                IsVisible = false,
            };
            menu.Children.Add(passwordError);

            Content = menu;
        }


        public async Task CheckForPermissions()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.StorageWrite>();
            }
        }

        public void checkCreds(object sender, System.EventArgs e)
        {
            //CheckForPermissions();

            if (GlobalVariables.usernameBox.Text == "" || GlobalVariables.passwordBox.Text == "")
            {
                passwordError.Text = "Please enter a username and password";
                passwordError.IsVisible = true;
                return;
            }

            if (!Regex.IsMatch(GlobalVariables.passwordBox.Text, @"[a-zA-Z]"))
            {
                passwordError.Text = "Password must contain at least 1 letter";
                passwordError.IsVisible = true;
                return;
            }

            if (!Regex.IsMatch(GlobalVariables.passwordBox.Text, @"\d"))
            {
                passwordError.Text = "Password must contain at least 1 number";
                passwordError.IsVisible = true;
                return;
            }

            if (Regex.IsMatch(GlobalVariables.passwordBox.Text, @" ") || Regex.IsMatch(GlobalVariables.usernameBox.Text, @" "))
            {
                passwordError.Text = "Username and password cannot contain spaces";
                passwordError.IsVisible = true;
                return;
            }

            if (GlobalVariables.passwordBox.Text.Length < 8)
            {
                passwordError.Text = "Password must be at least 8 characters";
                passwordError.IsVisible = true;
                return;
            }

            Navigation.PushAsync(new ServerBrowser());

        }
    }
}
