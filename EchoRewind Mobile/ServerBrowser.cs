﻿using GoogleGson;
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
        String obbFile;
        int selectedServer;

        public ServerBrowser()
        {

            var json = new WebClient().DownloadString("https://aldin101.github.io/echo-relay-server-browser/servers.json");

            JObject jsonObject = JObject.Parse(json);
            JArray jsondata = (JArray)jsonObject["online"];

            servers = jsondata.ToObject<Servers[]>();

            var serverNames = servers.Select(s => s.name).ToList();

            var serverBrowserMenu = new StackLayout();

            Title = "Echo Relay Quest Patcher";

            var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;

            var width = mainDisplayInfo.Width;

            var height = mainDisplayInfo.Height;

            var header = new Label
            {
                Text = "Select a Server",
                HorizontalOptions = LayoutOptions.Center,
                Padding = new Thickness(0, 0, 0, 20),
                FontSize = 24,
                TranslationY = 10,
            };
            serverBrowserMenu.Children.Add(header);

/*            var serverSelectLabel = new Label
            {
                Text = "Select a server",
                FontSize = 12,
                TranslationX = 10,
                MaximumWidthRequest = width - 20,

                Padding = new Thickness(0, 0, 0, 10),
            };
            serverBrowserMenu.Children.Add(serverSelectLabel);*/

            var serverInfo = new StackLayout();

            ListView serverList = new ListView
            {
                ItemsSource = serverNames,
                TranslationX = 10,
                MaximumWidthRequest = width - 20,
            };

            serverList.ItemSelected += async (sender, e) =>
            {
                if (serverBrowserMenu.Children.Contains(serverInfo))
                {
                    serverBrowserMenu.Children.Remove(serverInfo);
                }
                selectedServer = e.SelectedItemIndex;
                var server = servers[e.SelectedItemIndex];
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
                    MaximumHeightRequest = (width - 20) * (16 / 9),
                    MaximumWidthRequest = width - 20,
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
                    Text = "Join on Quest",
                    HorizontalOptions = LayoutOptions.Center,
                    Padding = new Thickness(0, 0, 0, 20),
                    FontSize = 24,
                    TranslationY = 10,
                };
                serverInfo.Children.Add(connectButton);
                connectButton.Clicked += filePicking;

                serverBrowserMenu.Children.Add(serverInfo);

                serverInfoImage.Source = serverImage;
            };

            serverBrowserMenu.Children.Add(serverList);

            Content = serverBrowserMenu;
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
