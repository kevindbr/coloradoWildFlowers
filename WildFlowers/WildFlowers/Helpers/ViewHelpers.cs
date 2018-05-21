using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms;
using PortableApp.Models;
using PortableApp.Views;
using System.ComponentModel;
using System.Diagnostics;

namespace PortableApp
{
    public class TransparentWebView : WebView
    {
    }

    public class ViewHelpers : ContentPage, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string pName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(pName));
        }


        //
        // VIEWS
        //

        public ExternalDBConnection externalConnection = new ExternalDBConnection();
        //public bool downloadImages = (bool)App.WoodySettingsRepo.GetSetting("Download Images").valuebool;

        public bool downloadImages = true;

        public AbsoluteLayout ConstructPageContainer()
        {
            AbsoluteLayout pageContainer = new AbsoluteLayout { BackgroundColor = Color.Black };
            Image backgroundImage = new Image
            {
                Source = ImageSource.FromResource("WildFlowers.Resources.Images.background.jpg"),
                Aspect = Aspect.AspectFill,
                Opacity = 0.7
            };
            pageContainer.Children.Add(backgroundImage, new Rectangle(0, 0, 1, 1), AbsoluteLayoutFlags.All);
            return pageContainer;
        }

        public Grid ConstructNavigationBarHome(string titleText)
        {
            Grid gridLayout = new Grid { VerticalOptions = LayoutOptions.FillAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand, ColumnSpacing = 0, BackgroundColor = Color.Transparent };
            gridLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });

            gridLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            //Title
            gridLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(6, GridUnitType.Star) });
            gridLayout.Children.Add(TitleConstructor(titleText), 1, 0);

            //Home
            gridLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            gridLayout.Children.Add(HomeImageConstructor(), 2, 0);

            return gridLayout;
        }

        public Grid ConstructNavigationBarPlants(string titleText)
        {
            Grid gridLayout = new Grid { VerticalOptions = LayoutOptions.FillAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand, ColumnSpacing = 0, BackgroundColor = Color.Transparent };
            gridLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });

            //BACK 
            gridLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            gridLayout.Children.Add(BackImageConstructor(), 0, 0);

            //Title
            gridLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(6, GridUnitType.Star) });
            gridLayout.Children.Add(TitleConstructor(titleText), 1, 0);

            //Home
            gridLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            gridLayout.Children.Add(HomeImageConstructor(), 2, 0);

            return gridLayout;
        }


        //construct navigation bar for HTML pages
        public Grid ConstructNavigationBarMain(string titleText)
        {
            Grid gridLayout = new Grid { VerticalOptions = LayoutOptions.FillAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand, ColumnSpacing = 0, BackgroundColor = Color.Transparent };
            gridLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });

            //BACK 
            gridLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            gridLayout.Children.Add(BackImageConstructor(), 0, 0);

            //Title
            gridLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(6, GridUnitType.Star) });
            gridLayout.Children.Add(TitleConstructor(titleText), 1, 0);

            //Home
            gridLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            gridLayout.Children.Add(HomeImageConstructor(), 2, 0);

            return gridLayout;
        }

        //On click action for About Button
        public async void ToAbout(object sender, EventArgs e)
        {
            //ChangeButtonColor(sender, e);
            await Navigation.PushAsync(new HTMLPage("About.html", "Introduction"));
        }

        // onclick action for References Button
        public async void ToReferences(object sender, EventArgs e)
        {
            //ChangeButtonColor(sender, e);
            await Navigation.PushAsync(new HTMLPage("References.html", "References"));
        }
        // onclick action for Contact Button
        public void ToContact(object sender, EventArgs e)
        {
            //ChangeButtonColor(sender, e);
            Device.OpenUri(new Uri("mailto:info@easterncoloradowildflowers.com"));
        }
        // onclick action for Contact Button
        public async void ToZones(object sender, EventArgs e)
        {
            GC.Collect();
            //ChangeButtonColor(sender, e);
            await Navigation.PushAsync(new ZonesPage());
        }
        // onclick action for Contact Button
        public async void ToFlowers(object sender, EventArgs e)
        {
            GC.Collect();
            //ChangeButtonColor(sender, e);
            await Navigation.PushAsync(new WildFlowersPage(downloadImages));
        }

        public async void ToPlains(object sender, EventArgs e)
        {
            //ChangeButtonColor(sender, e);
            await Navigation.PushAsync(new PlainsDetailsPage());
        }
        public async void ToFoothills(object sender, EventArgs e)
        {
            //ChangeButtonColor(sender, e);
            await Navigation.PushAsync(new FoothillsDetailsPage());
        }
        public async void ToMontane(object sender, EventArgs e)
        {
            //ChangeButtonColor(sender, e);
            await Navigation.PushAsync(new MontaneDetailsPage());
        }
        public async void ToSubalpine(object sender, EventArgs e)
        {
            //ChangeButtonColor(sender, e);
            await Navigation.PushAsync(new SubalpineDetailsPage());
        }
        public async void ToAlpine(object sender, EventArgs e)
        {
            //ChangeButtonColor(sender, e);
            await Navigation.PushAsync(new AlpineDetailsPage());
        }

        public WebView HTMLProcessor(string location)
        {
            // Generate WebView container
            var browser = new TransparentWebView();
            //var pdfBrowser = new CustomWebView { Uri = location, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand };
            string htmlText;

            // Get file locally unless the location is a web address
            if (location.Contains("http"))
            {
                htmlText = location;
                browser.Source = htmlText;
            }
            else if (!location.Contains(".pdf"))
            {
                // Get file from PCL--in order for HTML files to be automatically pulled from the PCL, they need to be in a Views/HTML folder
                var assembly = typeof(HTMLPage).GetTypeInfo().Assembly;
                Stream stream = assembly.GetManifestResourceStream("WildFlowers.Views.HTML." + location);
                htmlText = "";
                using (var reader = new System.IO.StreamReader(stream)) { htmlText = reader.ReadToEnd(); }
                var htmlSource = new HtmlWebViewSource();
                htmlSource.Html = htmlText;
                browser.Source = htmlSource;
            }
            return browser;
        }

        //protected async void ChangeButtonColor(object sender, EventArgs e)
        //{
        //    var button = (Button)sender;
        //    button.BackgroundColor = Color.FromHex("BBC9D845");
        //    await Task.Delay(100);
        //    button.BackgroundColor = Color.FromHex("CC1E4D2B");
        //}

        public Label TitleConstructor(String titleText)
        {
            return new Label { Text = titleText, TextColor = Color.Black, FontSize = 16, HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center };
        }

        public Image BackImageConstructor()
        {
            Image backImage = new Image
            {
                Source = ImageSource.FromResource("WildFlowers.Resources.Icons.back_arrow.png"),
                HeightRequest = 20,
                WidthRequest = 20,
                Margin = new Thickness(0, 15, 0, 15)
            };
            var backGestureRecognizer = new TapGestureRecognizer();
            backGestureRecognizer.Tapped += async (sender, e) =>
            {
                GC.Collect();
                await Navigation.PopAsync();
            };
            backImage.GestureRecognizers.Add(backGestureRecognizer);

            return backImage;
        }

        public Image HomeImageConstructor()
        {
            //headerIcon
            Image homeImage = new Image
            {
                Source = ImageSource.FromResource("WildFlowers.Resources.Icons.header-icon.png"),
                HeightRequest = 20,
                WidthRequest = 20,
                Margin = new Thickness(0, 15, 0, 15)
            };


            var homeImageGestureRecognizer = new TapGestureRecognizer();
            homeImageGestureRecognizer.Tapped += async (sender, e) =>
            {
                await Navigation.PopToRootAsync();
                await Navigation.PushAsync(new MainPage());
            };
            homeImage.GestureRecognizers.Add(homeImageGestureRecognizer);
            return homeImage;
        }

        public WebView htmlLineTagConstructor()
        {
            var browser = new WebView();
            var htmlSource = new HtmlWebViewSource();
            htmlSource.Html = @"
                <html><body>
                    <p style = 'text-indent: 2.0em; margin:0'> Plains</p>
                    <p style = 'text-indent: 2.0em; margin:0; color:#808080'> 3,500 to 6,500 feet</p>
                    <hr>
                </body></html>";
            browser.Source = htmlSource;
            return browser;
        }
    }

    static class Extensions
    {
        public static void Sort<TSource, TKey>(this Collection<TSource> source, Func<TSource, TKey> keySelector, string sortDirection)
        {
            List<TSource> sortedList;
            if (sortDirection == "\u25B2")
            {
                sortedList = source.OrderByDescending(keySelector).ToList();
            }
            else
            {
                sortedList = source.OrderBy(keySelector).ToList();
            }
            source.Clear();
            foreach (var sortedItem in sortedList)
                source.Add(sortedItem);
        }
    }
}
