using PortableApp.Models;
using Xamarin.Forms;


namespace PortableApp
{
    public partial class WildFlowerEcologyPage : ViewHelpers
    {
        WildFlower plant;
        TransparentWebView browser;


        public WildFlowerEcologyPage(WildFlower plant)
        {
            System.GC.Collect();


            // Turn off navigation bar and initialize pageContainer
            NavigationPage.SetHasNavigationBar(this, false);
            AbsoluteLayout pageContainer = ConstructPageContainer();

            // Initialize grid for inner container
            Grid innerContainer = new Grid { Padding = new Thickness(0, Device.OnPlatform(10, 0, 0), 0, 0) };
            innerContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            // Add header to inner container
            Grid navigationBar = ConstructNavigationBarMain(plant.genusSpeciesWeber);
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });
            innerContainer.Children.Add(navigationBar, 0, 0);

            ScrollView contentScrollView = new ScrollView
            {
                BackgroundColor = Color.FromHex("88000000"),
                Padding = new Thickness(20, 5, 20, 5),
                Margin = new Thickness(0, 0, 0, 0)
            };

            TransparentWebView browser = ConstructHTMLContent(plant);
            //browser.Navigating += ToWetlandType;

            contentScrollView.Content = browser;
            innerContainer.RowDefinitions.Add(new RowDefinition { });
            innerContainer.Children.Add(contentScrollView, 0, 1);

            //var wetlandTypes = ConstructWetlandTypes(plant.ecologicalsystems);
            //innerContainer.RowDefinitions.Add(new RowDefinition { });
            //innerContainer.Children.Add(wetlandTypes, 0, 2);

            // Add inner container to page container and set as page content
            pageContainer.Children.Add(innerContainer, new Rectangle(0, 0, 1, 1), AbsoluteLayoutFlags.All);
            Content = pageContainer;

            base.OnAppearing();
            System.GC.Collect();

        }

        public TransparentWebView ConstructHTMLContent(WildFlower plant)
        {
            browser = new TransparentWebView();
            var htmlSource = new HtmlWebViewSource();
            string html = "";
            /*
            html += "<!DOCTYPE html><html lang='en' xmlns='http://www.w3.org/1999/xhtml'><head><meta charset = 'utf-8' /><title>Plant Info Page</title></head><body>";
            html += "<style>body, a { color: white; font-size: 0.9em; } .section_header { font-weight: bold; border-bottom: 1px solid white; margin: 10px 0; } .embedded_table { width: 100%; margin-left: 10px; } .iconImg { height: 40px; }</style>";

            html += "<div class='section_header'>HABITAT & ECOLOGY</div>" + plant.habitat;

            html += "<div class='section_header'>COMMENTS</div>" + plant.comments;

            html += "<div class='section_header'>WETLAND TYPES</div>" + ReconstructWetlandTypes(plant.ecologicalsystems);

            html += "<div class='section_header'>ANIMAL USE</div>" + plant.animaluse.Replace("resources/images/animals/", "");

            html += "</body></html>";

            htmlSource.Html = html;
            browser.Source = htmlSource;
            */
            return browser;
        }
    }
}
