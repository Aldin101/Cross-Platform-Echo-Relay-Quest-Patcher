using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EchoRelayInstaller
{
    public partial class OpenSourceLicensesPage : ContentPage
    {
        public OpenSourceLicensesPage(int license)
        {
            Title = "Echo Navigator Standalone";

            var menu = new StackLayout();

            var header = new Label
            {
                Text = GlobalVariables.openSourceLicencesList[license].Name,
                HorizontalOptions = LayoutOptions.Center,
                Padding = new Thickness(0, 0, 0, 20),
                FontSize = 24,
                TranslationY = 10,
            };

            menu.Children.Add(header);

            var licenseText = new Label
            {
                Text = GlobalVariables.openSourceLicencesList[license].Content,
                HorizontalOptions = LayoutOptions.Center,
                Padding = new Thickness(0, 0, 0, 20),
                FontSize = 12,
                TranslationY = 10,
            };

            menu.Children.Add(licenseText);

            Content = menu;
        }
    }
}
