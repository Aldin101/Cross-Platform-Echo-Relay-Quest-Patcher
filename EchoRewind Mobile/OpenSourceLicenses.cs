using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EchoRelayInstaller
{

    public partial class OpenSourceLicenses : ContentPage
    {
        public OpenSourceLicenses()
        {

            if (GlobalVariables.openSourceLicencesList == null)
            {
                GlobalVariables.openSourceLicencesList = new List<OpenSourceLicencesList>();
            }

            while (GlobalVariables.openSourceLicencesList.Count() < 6)
            {
                GlobalVariables.openSourceLicencesList.Add(new OpenSourceLicencesList());
            }

            GlobalVariables.openSourceLicencesList[0].Name = "Bouncy Castle";
            GlobalVariables.openSourceLicencesList[0].Content = "Copyright (c) 2000 - 2023 The Legion of the Bouncy Castle Inc. (https://www.bouncycastle.org)\n\nPermission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the \"Software\"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:\n\nThe above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.\n\nTHE SOFTWARE IS PROVIDED \"AS IS\", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.";

            GlobalVariables.openSourceLicencesList[1].Name = "bsdiff";
            GlobalVariables.openSourceLicencesList[1].Content = "Copyright 2003-2005 Colin Percival\nCopyright 2012 Matthew Endsley\nAll rights reserved\n\nRedistribution and use in source and binary forms, with or without modification, are permitted providing that the following conditions are met:\n1. Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.\n2. Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.\n\nTHIS SOFTWARE IS PROVIDED BY THE AUTHOR ``AS IS'' AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.";

            GlobalVariables.openSourceLicencesList[2].Name = "EchoRewind";
            GlobalVariables.openSourceLicencesList[2].Content = "MIT License\n\nCopyright (c) 2023 Christian Luddy\n\nPermission is hereby granted, free of charge, to any person obtaining a copyof this software and associated documentation files (the \"Software\"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:\n\nThe above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.\n\nTHE SOFTWARE IS PROVIDED \"AS IS\", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.";

            GlobalVariables.openSourceLicencesList[3].Name = "Microsoft MAUI";
            GlobalVariables.openSourceLicencesList[3].Content = "The MIT License (MIT)\n\nCopyright (c) .NET Foundation and Contributors\n\nAll rights reserved.\n\nPermission is hereby granted, free of charge, to any person obtaining a copyof this software and associated documentation files (the \"Software\"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:\n\nThe above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.\n\nTHE SOFTWARE IS PROVIDED \"AS IS\", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.";

            GlobalVariables.openSourceLicencesList[4].Name = ".NET";
            GlobalVariables.openSourceLicencesList[4].Content = "The MIT License (MIT)\n\nCopyright (c) 2019 Microsoft\n\nPermission is hereby granted, free of charge, to any person obtaining a copyof this software and associated documentation files (the \"Software\"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:\n\nThe above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.\n\nTHE SOFTWARE IS PROVIDED \"AS IS\", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.";

            GlobalVariables.openSourceLicencesList[5].Name = "Newtonsoft.Json";
            GlobalVariables.openSourceLicencesList[5].Content = "The MIT License (MIT)\n\nCopyright (c) 2007 James Newton-King\n\nPermission is hereby granted, free of charge, to any person obtaining a copyof this software and associated documentation files (the \"Software\"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:\n\nThe above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.\n\nTHE SOFTWARE IS PROVIDED \"AS IS\", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.";


            var menu = new StackLayout();

            Title = "Echo Navigator Standalone";

            var header = new Label
            {
                Text = "Third-Party Licences",
                HorizontalOptions = LayoutOptions.Center,
                Padding = new Thickness(0, 0, 0, 20),
                FontSize = 24,
                TranslationY = 10,
            };

            menu.Children.Add(header);

            var openSourceLicenses = new Label
            {
                Text = "Echo Navigator Standalone uses the following third-party libraries and software:\n\n"
            };

            menu.Children.Add(openSourceLicenses);
#if ANDROID
            var software = new ListView
            {
                ItemsSource = GlobalVariables.openSourceLicencesList.Select(s => s.Name).ToList(),
                BackgroundColor = Color.FromRgb(0, 0, 0),
            };
#else
            var software = new ListView
            {
                ItemsSource = GlobalVariables.openSourceLicencesList.Select(s => s.Name).ToList(),
            };
#endif

            menu.Children.Add(software);

            software.ItemSelected += async (sender, e) =>
            {
                if (e.SelectedItem == null)
                    return;

                await Navigation.PushAsync(new OpenSourceLicensesPage(e.SelectedItemIndex));

                ((ListView)sender).SelectedItem = null;
            };

            Content = menu;
        }
    }
}
