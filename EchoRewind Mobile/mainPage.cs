using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Text;
using System.Net;
using System.Text.RegularExpressions;

namespace EchoRelayInstaller {
    public static class GlobalVariables
    {
        public static Entry usernameBox;
        public static Entry passwordBox;
        public static List<OpenSourceLicencesList> openSourceLicencesList = new List<OpenSourceLicencesList>();
        public static ActivityIndicator activityIndicator;

    }
    public class OpenSourceLicencesList
    {
        public string Name { get; set; }
        public string Content { get; set; }
    }

    public partial class MainPage : ContentPage
    {
        Label passwordError;
        ToolbarItem toolBarItem;

        public MainPage()
        {
            var menu = new StackLayout();

           
            //var width = menu.Width;
            var width = 720;

            Title = "Echo Navigator Standalone";

            var header = new Label
            {
                Text = "Create Echo Relay Account",
                HorizontalOptions = LayoutOptions.Center,
                Padding = new Thickness(0, 0, 0, 20),
                FontSize = 24,
                TranslationY = 10,
            };
            menu.Children.Add(header);

#if ANDROID
            toolBarItem = (new ToolbarItem("⋅ ⋅ ⋅", null, () =>
            {
                Title = "";
                ToolbarItems.Clear();

                ToolbarItems.Add(new ToolbarItem("Third-Party Licences", null, () =>
                {
                    Navigation.PushAsync(new OpenSourceLicenses());
                }));

                ToolbarItems.Add(new ToolbarItem("About", null, () =>
                {
                    Navigation.PushAsync(new AboutPage());
                }));

                ToolbarItems.Add(new ToolbarItem("→", null, () =>
                {
                    ToolbarItems.Clear();
                    Title = "Echo Navigator Standalone";
                    ToolbarItems.Add(toolBarItem);
                }));
            }));
            ToolbarItems.Add(toolBarItem);
#endif

#if WINDOWS
            ToolbarItems.Add(new ToolbarItem("Third-Party Licences", null, () =>
            {
                Navigation.PushAsync(new OpenSourceLicenses());
            }));

            ToolbarItems.Add(new ToolbarItem("About", null, () =>
            {
                Navigation.PushAsync(new AboutPage());
            }));
#endif

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

            var nextButton = new Button { Text = "Next", FontSize = 24 };
            nextButton.Clicked += (s, e) =>
            {
                passwordError.IsVisible = false;
                checkCreds();
                if (passwordError.IsVisible)
                    return;

                GlobalVariables.activityIndicator.IsVisible = true;
                Device.BeginInvokeOnMainThread(() =>
                {
                    loadServerBrowser();
                });
            };
            menu.Children.Add(nextButton);

            GlobalVariables.passwordBox.Completed += (s, e) => nextButton.SendClicked();

            passwordError = new Label
            {
                Text = "Text",
                FontSize = 15,
                HorizontalOptions = LayoutOptions.Center,
                Padding = new Thickness(0, 0, 0, 10),
                IsVisible = false,
            };
            menu.Children.Add(passwordError);

            GlobalVariables.activityIndicator = new ActivityIndicator
            {
                IsRunning = true,
                IsVisible = false,
                Color = Colors.Orange,
                TranslationY = 10,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
            };
            menu.Children.Add(GlobalVariables.activityIndicator);

            Content = menu;
        }

        public void checkCreds()
        {
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
        }

        public void loadServerBrowser()
        {
            string json;
            try
            {
                json = new WebClient().DownloadString("https://aldin101.github.io/EchoNavigatorAPI/servers.json");
            }
            catch (WebException ex)
            {
                GlobalVariables.activityIndicator.IsVisible = false;
                DisplayAlert("Error", "The Echo Navigator servers could not be contacted\nThis is usually because your network is blocking the Echo Navigator backend. You can try to use a mobile hotspot or VPN to fix this.", "OK");
                return;
            }

            GlobalVariables.activityIndicator.IsVisible = false;
            Navigation.PushAsync(new ServerBrowser(json));
            return;
        }
    }
}
