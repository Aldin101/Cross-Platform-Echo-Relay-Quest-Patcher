using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EchoRelayInstaller
{
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            Title = "Echo Relay Quest Patcher";

            var menu = new StackLayout();

            var header = new Label
            {
                Text = "About",
                HorizontalOptions = LayoutOptions.Center,
                Padding = new Thickness(0, 0, 0, 20),
                FontSize = 24,
                TranslationY = 10,
            };

            menu.Children.Add(header);

            var aboutText = new Label
            {
                Text = "Echo Relay Quest Patcher is a tools created by Aldin101 that allows you to patch Echo VR APKs for use with Echo Relay.\n\nNeed help? Contact me on Dicord: @aldin101"
            };

            menu.Children.Add(aboutText);

            Content = menu;
        }

    }
}
