﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PortableApp
{
    public partial class SubalpineDetailsPage : ViewHelpers
    {
        protected async override void OnAppearing()
        {

            base.OnAppearing();
        }

        public SubalpineDetailsPage()
        {
            // Turn off navigation bar and initialize pageContainer
            NavigationPage.SetHasNavigationBar(this, false);
            AbsoluteLayout pageContainer = ConstructPageContainer();

            // Initialize grid for inner container
            Grid innerContainer = new Grid { Padding = new Thickness(0, Device.OnPlatform(0, 0, 0), 0, 0), RowSpacing = 0, BackgroundColor = Color.White };
            innerContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            // Add navigationBar to inner container
            Grid navigationBar = ConstructNavigationBarMain("Subalpine Zone Details");
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });
            innerContainer.Children.Add(navigationBar, 0, 0);


         

            //html snippet
            var browser = new WebView();
            var htmlSource = new HtmlWebViewSource();
            htmlSource.Html = @"
                <html><body>
                    <p style = 'text-indent: 2.0em; margin:0'>Name</p>
                    <p style = 'text-indent: 2.0em; margin:0; color:#808080'>Subalpine</p>
                    <hr width='85%'>
                    <p style = 'text-indent: 2.0em; margin:0'>Elevation</p>
                    <p style = 'text-indent: 2.0em; margin:0; color:#808080'>10,000 to 11,500 feet</p>
                    <hr width='85%'>
                    <p style = 'text-indent: 2.0em; margin:0'>Description</p>
                    <p style = 'padding-left: 2.0em; margin:0; color:#808080'>Characterized by thick Spruce/Fir forests. Aspens grow at lower elevations in this zone. Annually about 25-40 inches of moisture, most from snow (about 250-350 inches). Lush wildflower growth mid-June through August.</p>
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
