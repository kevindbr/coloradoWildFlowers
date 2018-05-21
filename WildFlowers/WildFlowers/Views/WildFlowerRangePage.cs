using PortableApp.Models;
using System.Collections.ObjectModel;
using Xamarin.Forms;


namespace PortableApp
{
    public partial class WildFlowerRangePage : ViewHelpers
    {

        public WildFlowerRangePage(WildFlower plant, bool streaming)
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
                Padding = new Thickness(0, 20, 0, 20),
                Margin = new Thickness(0, 0, 0, 0)
            };
            StackLayout contentContainer = new StackLayout();

            Image rangeImage = new Image
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                WidthRequest = 600,
                HeightRequest = 300,
                Aspect = Aspect.AspectFit,
                Margin = new Thickness(10, 0, 10, 0),
            };

            rangeImage.BindingContext = plant;
            string imageBinding = streaming ? "RangePathStreamed" : "RangePathDownloaded";
            rangeImage.SetBinding(Image.SourceProperty, new Binding(imageBinding));
            contentContainer.Children.Add(rangeImage);


           /* Image mapKey = new Image
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                WidthRequest = 600,
                HeightRequest = 300,
                Aspect = Aspect.AspectFit,
                Margin = new Thickness(10, 0, 10, 0),
            };

            mapKey.Source = ImageSource.FromResource("WoodyPlants.Resources.Images.WoodyMapKey.png");

            contentContainer.Children.Add(mapKey);*/

            contentScrollView.Content = contentContainer;
            innerContainer.RowDefinitions.Add(new RowDefinition { });
            innerContainer.Children.Add(contentScrollView, 0, 1);

            // Add inner container to page container and set as page content
            pageContainer.Children.Add(innerContainer, new Rectangle(0, 0, 1, 1), AbsoluteLayoutFlags.All);
            Content = pageContainer;
            System.GC.Collect();
        }

    }
}
