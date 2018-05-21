using PortableApp.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace PortableApp
{
    public partial class WildFlowerInfoPage : ViewHelpers
    {

        public WildFlowerInfoPage(WildFlower plant)
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

            contentScrollView.Content = browser;
            innerContainer.RowDefinitions.Add(new RowDefinition { });
            innerContainer.Children.Add(contentScrollView, 0, 1);

            // Add inner container to page container and set as page content
            pageContainer.Children.Add(innerContainer, new Rectangle(0, 0, 1, 1), AbsoluteLayoutFlags.All);
            Content = pageContainer;
            System.GC.Collect();
        }


        public TransparentWebView ConstructHTMLContent(WildFlower plant)
        {
            var browser = new TransparentWebView();
            var htmlSource = new HtmlWebViewSource();
            string html = "";
            /*
            html += "<!DOCTYPE html><html lang='en' xmlns='http://www.w3.org/1999/xhtml'><head><meta charset = 'utf-8' /><title>Plant Info Page</title></head><body>";
            html += "<style>body { color: white; font-size: 0.9em; } .section_header { font-weight: bold; border-bottom: 1px solid white; margin: 10px 0; } .embedded_table { width: 100%; margin-left: 10px; }</style>";

            html += "<div class='section_header'>NOMENCLATURE</div>";
            html += "<strong>Scientific Name: </strong>" + plant.scinameauthor + "<br/>";
            html += "<strong>Family: </strong>" + plant.family + "<br/>";
            html += "<strong>Common Name: </strong>" + plant.commonname + "<br/>";
            html += "<strong>Synonyms: </strong>" + plant.synonyms + "<br/>";
            html += "<strong>USDA Plants Symbol: </strong>" + plant.plantscode + "<br/>";
            html += "<strong>ITIS TSN: </strong>" + plant.itiscode + "<br/>";

            html += "<div class='section_header'>CONSERVATION STATUS</div>";
            html += "<strong>Federal Status: </strong>" + plant.federalstatus + "<br/>";
            html += "<strong>Global Rank: </strong>" + plant.grank + "<br/>";
            html += "<strong>State Ranks</strong><br/>";
            html += "<table class='embedded_table'><tbody>";
            html += "<tr><td><strong>CO: </strong>" + plant.cosrank + "</td><td><strong>MT: </strong>" + plant.mtsrank + "</td></tr>";
            html += "<tr><td><strong>WY: </strong>" + plant.wysrank + "</td><td><strong>ND: </strong>" + plant.ndsrank + "</td></tr>";
            html += "<tr><td><strong>UT: </strong>" + plant.utsrank + "</td></tr>";
            html += "</tbody></table>";

            html += "<div class='section_header'>BIOLOGY</div>";
            html += "<strong>C-Value: </strong>" + plant.cvalue + "<br/>";
            html += "<strong>Duration: </strong>" + plant.duration + "<br/>";
            html += "<strong>Native Status: </strong>" + plant.nativity + "<br/>";
            html += "<strong>Wetland Indicator Status</strong><br/>";
            html += "<table class='embedded_table'><tbody>";
            html += "<tr><td><strong>AW: </strong>" + plant.awwetcode + "</td></tr>";
            html += "<tr><td><strong>WM: </strong>" + plant.wmvcwetcode + "</td></tr>";
            html += "<tr><td><strong>GP: </strong>" + plant.gpwetcode + "</td></tr>";
            html += "</tbody></table>";

            html += "<div class='section_header'>KEY CHARACTERISTICS</div>";

            html += "<ul>";
            if (plant.keychar1 != null && plant.keychar1 != "") { html += "<li>" + plant.keychar1 + "</li>"; };
            if (plant.keychar2 != null && plant.keychar2 != "") { html += "<li>" + plant.keychar2 + "</li>"; };
            if (plant.keychar3 != null && plant.keychar3 != "") { html += "<li>" + plant.keychar3 + "</li>"; };
            if (plant.keychar4 != null && plant.keychar4 != "") { html += "<li>" + plant.keychar4 + "</li>"; };
            if (plant.keychar5 != null && plant.keychar5 != "") { html += "<li>" + plant.keychar5 + "</li>"; };
            if (plant.keychar6 != null && plant.keychar6 != "") { html += "<li>" + plant.keychar6 + "</li>"; };
            html += "</ul>";

            html += "<div class='section_header'>REFERENCES</div>";

            html += "<ul>";
            foreach (WildFlowerReference reference in plant.References)
            {
                html += "<li>" + reference.fullcitation + "</li>";
            }
            html += "</ul>";

            html += "</body></html>";

            htmlSource.Html = html;
            browser.Source = htmlSource;*/
            return browser;
        }

    }
}
