using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace EchoRelayInstaller
{

    public class Servers
    {
        public string name { get; set; }
        public string IP { get; set; }
        public string port { get; set; }
        public string description { get; set; }
        public string longDescription { get; set; }
        public string image { get; set; }
    }
    public partial class ServerBrowser : ContentPage
    {

        Servers[] servers;
        String apkFile;
        ToolbarItem toolBarItem;
        ListView serverList;
        StackLayout serverBrowserMenu;
        StackLayout serverInfo;
        int selectedServer;

        public ServerBrowser()
        {

            var json = new WebClient().DownloadString("https://aldin101.github.io/EchoNavigatorAPI/servers.json");

            JObject jsonObject = JObject.Parse(json);
            JArray jsondata = (JArray)jsonObject["online"];

            servers = jsondata.ToObject<Servers[]>();

            var serverNames = servers.Select(s => s.name).ToList();


            serverBrowserMenu = new StackLayout();

            Title = "Echo Navigator Standalone";
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
            var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;

            var header = new Label
            {
                Text = "Select a Server",
                HorizontalOptions = LayoutOptions.Center,
                Padding = new Thickness(0, 0, 0, 20),
                FontSize = 24,
                TranslationY = 10,
            };
            serverBrowserMenu.Children.Add(header);

            serverInfo = new StackLayout();

#if ANDROID
            serverList = new ListView
            {
                ItemsSource = serverNames,
                BackgroundColor = Color.FromRgb(0, 0, 0),
                TranslationX = 10,
            };

#else
            serverList = new ListView
            {
                ItemsSource = serverNames,
                TranslationX = 10,
            };
#endif


            serverList.ItemTapped += async (sender, e) =>
            {
                serverBrowserMenu.Children.Remove(serverInfo);

                if (e.ItemIndex == selectedServer)
                {
                    serverList.SelectedItem = null;
                    selectedServer = 420;
                    return;
                }

                selectedServer = e.ItemIndex;
                var server = servers[e.ItemIndex];
                var serverIP = server.IP;
                var serverPort = server.port;
                var serverDescription = server.description;
                var serverLongDescription = server.longDescription;
                var serverImage = server.image;

                serverInfo = new StackLayout();

                var serverInfoHeader = new Label
                {
                    Text = server.name,
                    HorizontalOptions = LayoutOptions.Center,
                    Padding = new Thickness(0, 0, 0, 20),
                    FontSize = 24,
                };
                serverInfo.Children.Add(serverInfoHeader);

                var serverInfoImage = new Image
                {
                    Source = "resources/images/loading.gif",
                    HorizontalOptions = LayoutOptions.Center,
                    MaximumHeightRequest = 250,
                    TranslationY = 10,
                };
                serverInfo.Children.Add(serverInfoImage);

                var serverInfoDescription = new Label
                {
                    Text = serverLongDescription,
                    HorizontalOptions = LayoutOptions.Center,
                    Padding = new Thickness(0, 0, 0, 20),
                    FontSize = 24,
                    TranslationY = 10,
                };
                serverInfo.Children.Add(serverInfoDescription);

                var connectButton = new Button
                {
                    Text = "Patch APK",
                    HorizontalOptions = LayoutOptions.Center,
                    FontSize = 24,
                    WidthRequest = 200,
                    TranslationY = -10,
                };
                serverInfo.Children.Add(connectButton);
                connectButton.Clicked += filePicking;

                serverBrowserMenu.Children.Add(serverInfo);

                serverInfoImage.Source = serverImage;
            };

            serverBrowserMenu.Children.Add(serverList);

            selectedServer = 420;

            Content = serverBrowserMenu;
        }

        protected override bool OnBackButtonPressed()
        {
            if (selectedServer != 420)
            {
                selectedServer = 420;
                serverList.SelectedItem = null;
                serverBrowserMenu.Children.Remove(serverInfo);
                return true;
            } else
            {
                return base.OnBackButtonPressed();
            }
        }

        public async void filePicking(object sender, System.EventArgs e)
        {
/*            PickOptions pickOptionsObb = new PickOptions
            {
                PickerTitle = "Select OBB File",
            };
            obbFile = await FileSelector(pickOptionsObb, true);

            if (obbFile == null)
            {
                await alert("Error", "File no selected");
                return;
            }*/

            PickOptions pickOptionsApk = new PickOptions
            {
                PickerTitle = "Select APK File",
            };
            apkFile = await FileSelector(pickOptionsApk, false);

            if (apkFile == null)
            {
                await alert("Error", "File no selected");
                return;
            }

            await Task.Delay(1000);

            await Navigation.PushAsync(new patchGame(apkFile, servers, selectedServer));
        }

        public async Task<string> FileSelector(PickOptions options, bool obb)
        {
            try
            {
                if (obb)
                {
                    await alert("", "Please select downloaded OBB file from OculusDB");
                }
                else
                {
                    await alert("", "Please select downloaded APK file from OculusDB");
                }
                var result = await FilePicker.Default.PickAsync(options);
                return result.FullPath;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task alert(string title, string message)
        {
            await DisplayAlert(title, message, "OK");
        }
    }
}
