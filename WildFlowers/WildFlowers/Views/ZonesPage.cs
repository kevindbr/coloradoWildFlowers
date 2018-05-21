using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PortableApp
{
    public partial class ZonesPage : ViewHelpers
    {
        protected async override void OnAppearing()
        {

            base.OnAppearing();
        }

        public ZonesPage()
        {
            // Turn off navigation bar and initialize pageContainer
            NavigationPage.SetHasNavigationBar(this, false);
            AbsoluteLayout pageContainer = ConstructPageContainer();

            // Initialize grid for inner container
            Grid innerContainer = new Grid { Padding = new Thickness(0, 0, 0, 0), RowSpacing = 0, BackgroundColor = Color.FromHex("66000000") };
            innerContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            // Add navigationBar to inner container
            Grid navigationBar = ConstructNavigationBarMain("VEGETATION ZONES");
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });
            innerContainer.Children.Add(navigationBar, 0, 0);


            //Intro Text
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(60) });
            String intro = "Vegetation Zones follow those described by the Colorado Native Plant Society:\n";
            Label plainString = new Label { Text = intro,FontAttributes = FontAttributes.Bold, TextColor = Color.White, FontSize = 14, HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center, Margin = new Thickness(5, 5, 5, 5) };
            innerContainer.Children.Add(plainString, 0, 1);


            // Add navigation buttons
            Button plainsButton = new Button
            {
                Style = Application.Current.Resources["semiTransparentButton"] as Style,
                Text = "Plains (3,500 to 6,500 feet)",
                HorizontalOptions = LayoutOptions.FillAndExpand
                //Margin = new Thickness(0, 10, 0, 10)
            };
            plainsButton.Clicked += ToPlains;
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(60) });
            innerContainer.Children.Add(plainsButton, 0, 2);

            Button foothillsButton = new Button
            {
                Style = Application.Current.Resources["semiTransparentButton"] as Style,
                Text = "Foothills (6,500 to 8,000 feet)",
                HorizontalOptions = LayoutOptions.FillAndExpand
                //Margin = new Thickness(0, 10, 0, 10)
            };
            foothillsButton.Clicked += ToFoothills;
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(60) });
            innerContainer.Children.Add(foothillsButton, 0, 3);

            Button montaneButton = new Button
            {
                Style = Application.Current.Resources["semiTransparentButton"] as Style,
                Text = "Montane (8,000 to 10,000 feet)",
                HorizontalOptions = LayoutOptions.FillAndExpand
                //Margin = new Thickness(0, 10, 0, 10)
            };
            montaneButton.Clicked += ToMontane;
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(60) });
            innerContainer.Children.Add(montaneButton, 0, 4);

            Button subalpineButton = new Button
            {
                Style = Application.Current.Resources["semiTransparentButton"] as Style,
                Text = "Subalpine (10,000 to 11,500 feet)",
                HorizontalOptions = LayoutOptions.FillAndExpand
                //Margin = new Thickness(0, 10, 0, 10)
            };
            subalpineButton.Clicked += ToSubalpine;
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(60) });
            innerContainer.Children.Add(subalpineButton, 0, 5);
            
            Button alpineButton = new Button
            {
                Style = Application.Current.Resources["semiTransparentButton"] as Style,
                Text = "Alpine (tree line)",
                HorizontalOptions = LayoutOptions.FillAndExpand
                //Margin = new Thickness(0, 10, 0, 10)
            };
            alpineButton.Clicked += ToAlpine;
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(60) });
            innerContainer.Children.Add(alpineButton, 0, 6);
            
           
            // Add inner container to page container and set as page content
            pageContainer.Children.Add(innerContainer, new Rectangle(0, 0, 1, 1), AbsoluteLayoutFlags.All);
            Content = pageContainer;
            
        }
    }
}
