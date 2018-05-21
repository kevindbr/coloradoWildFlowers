using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PortableApp
{
    public partial class PlainsDetailsPage : ViewHelpers
    {
        protected async override void OnAppearing()
        {

            base.OnAppearing();
        }

        public PlainsDetailsPage()
        {
            // Turn off navigation bar and initialize pageContainer
            NavigationPage.SetHasNavigationBar(this, false);
            AbsoluteLayout pageContainer = ConstructPageContainer();

            // Initialize grid for inner container
            Grid innerContainer = new Grid { Padding = new Thickness(0, Device.OnPlatform(0, 0, 0), 0, 0), RowSpacing = 0, BackgroundColor = Color.White };
            innerContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            // Add navigationBar to inner container
            Grid navigationBar = ConstructNavigationBarMain("Plains Zone Details");
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });
            innerContainer.Children.Add(navigationBar, 0, 0);


         

            //html snippet
            var browser = new WebView();
            var htmlSource = new HtmlWebViewSource();
            htmlSource.Html = @"
                <html><body>
                    <p style = 'text-indent: 2.0em; margin:0'>Name</p>
                    <p style = 'text-indent: 2.0em; margin:0; color:#808080'>Plains</p>
                    <hr width='85%'>
                    <p style = 'text-indent: 2.0em; margin:0'>Elevation</p>
                    <p style = 'text-indent: 2.0em; margin:0; color:#808080'> 3,500 to 6,500 feet</p>
                    <hr width='85%'>
                    <p style = 'text-indent: 2.0em; margin:0'>Description</p>
                    <p style = 'padding-left: 2.0em; margin:0; color:#808080'>The plains are, where undisturbed by agriculture, a mixed grass prairie. Sandy hills may have shrubs such as Sagebrush and Rabbitbrush mixed with Cacti and Yucca. Cottonwoods, willows, and Box Elder are found along streams. Percentage of days with sunshine is very high and humidity is quite low. Annual precipitation is less than 20 inches with about a quarter of that from snow. Wildflowers in undisturbed areas can be very good from March through June.</p>
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
