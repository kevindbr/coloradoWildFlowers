using CarouselView.FormsPlugin.Abstractions;
using FFImageLoading.Forms;
using PortableApp.Models;
using PortableApp.Helpers;
using System;
using Xamarin.Forms;

namespace PortableApp
{
    public partial class WildFlowerImagesPage : ViewHelpers
    {

        public WildFlowerImagesPage(WildFlower plant, bool streaming)
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

            CarouselViewControl carouselControl = new CarouselViewControl();
            try
            {
                carouselControl.ItemsSource = plant.Images;
            }
            catch (NullReferenceException e)
            {
                carouselControl.ItemsSource = null;
            }

            carouselControl.ShowIndicators = true;

            DataTemplate imageTemplate = new DataTemplate(() =>
            {
                Grid cell = new Grid { BackgroundColor = Color.FromHex("88000000") };
                cell.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                cell.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

                // Add image
                var image = new ZoomImage { Margin = new Thickness(10, 0, 10, 0) };

                string imageBinding = streaming ? "ImagePathStreamed" : "ImagePathDownloaded";

                var cachedImage = new CachedImage()
                {
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    WidthRequest = 300,
                    HeightRequest = 300,
                    Aspect = Aspect.AspectFill,
                    Margin = new Thickness(10, 0, 10, 0),
                    CacheDuration = System.TimeSpan.FromDays(30),
                    DownsampleToViewSize = true,
                    RetryCount = 0,

                    RetryDelay = 250,
                    TransparencyEnabled = false,
                    LoadingPlaceholder = "loading.png",
                    ErrorPlaceholder = "error.png",
                };



                image.SetBinding(Image.SourceProperty, new Binding(imageBinding));
                cachedImage.SetBinding(CachedImage.SourceProperty, new Binding(imageBinding));

                cell.Children.Add(image, 0, 0);

                return cell;
            });

            carouselControl.ItemTemplate = imageTemplate;
            carouselControl.Position = 0;
            carouselControl.InterPageSpacing = 10;
            carouselControl.Orientation = CarouselViewOrientation.Horizontal;

            innerContainer.RowDefinitions.Add(new RowDefinition { });
            innerContainer.Children.Add(carouselControl, 0, 1);

            // Add inner container to page container and set as page content
            pageContainer.Children.Add(innerContainer, new Rectangle(0, 0, 1, 1), AbsoluteLayoutFlags.All);

            Content = pageContainer;
            System.GC.Collect();

        }

    }

}
