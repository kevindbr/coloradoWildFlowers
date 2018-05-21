using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PortableApp
{
    public partial class MontaneDetailsPage : ViewHelpers
    {
        protected async override void OnAppearing()
        {

            base.OnAppearing();
        }

        public MontaneDetailsPage()
        {
            // Turn off navigation bar and initialize pageContainer
            NavigationPage.SetHasNavigationBar(this, false);
            AbsoluteLayout pageContainer = ConstructPageContainer();

            // Initialize grid for inner container
            Grid innerContainer = new Grid { Padding = new Thickness(0, Device.OnPlatform(0, 0, 0), 0, 0), RowSpacing = 0, BackgroundColor = Color.White };
            innerContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            // Add navigationBar to inner container
            Grid navigationBar = ConstructNavigationBarMain("Montane Zone Details");
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });
            innerContainer.Children.Add(navigationBar, 0, 0);


         

            //html snippet
            var browser = new WebView();
            var htmlSource = new HtmlWebViewSource();
            htmlSource.Html = @"
                <html><body>
                    <p style = 'text-indent: 2.0em; margin:0'>Name</p>
                    <p style = 'text-indent: 2.0em; margin:0; color:#808080'>Montane</p>
                    <hr width='85%'>
                    <p style = 'text-indent: 2.0em; margin:0'>Elevation</p>
                    <p style = 'text-indent: 2.0em; margin:0; color:#808080'>8,000 to 10,000 feet</p>
                    <hr width='85%'>
                    <p style = 'text-indent: 2.0em; margin:0'>Description</p>
                    <p style = 'padding-left: 2.0em; margin:0; color:#808080'>Open Aspen forests, sometimes with heavy undergrowth of shrubs (Snowberry, Currants, Elderberry). Colorado Blue Spruce in moist areas. At lower elevations some large stands of Ponderosa Pine with scattered Douglas Fir on north facing slopes. Annually about 18-30 inches of moisture, 1/2 to 3/4 from snow. Moderate to lush wildflower growth from June-August.</p>
                </body></html>";
            browser.Source = htmlSource;
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(400) });
            innerContainer.Children.Add(browser, 0, 1);

            //Flowers in Zone Button
            Button showFlowers = new Button
            {
                Style = Application.Current.Resources["showFlowersButton"] as Style,
                Text = "Show Flowers in this Zone",
                HorizontalOptions = LayoutOptions.Fill
                //Margin = new Thickness(0, 10, 0, 10)
            };
            showFlowers.Clicked += ToZones;
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(45) });
            innerContainer.Children.Add(showFlowers, 0, 2);

            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            // Add inner container to page container and set as page content
            pageContainer.Children.Add(innerContainer, new Rectangle(0, 0, 1, 1), AbsoluteLayoutFlags.All);
            Content = pageContainer;
        }
    }
}
