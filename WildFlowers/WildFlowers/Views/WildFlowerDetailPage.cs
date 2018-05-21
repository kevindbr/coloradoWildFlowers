using PortableApp.Models;
using PortableApp.Views;
using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace PortableApp.Views
{
    public partial class WildFlowerDetailPage : TabbedPage
    {
        public WildFlowerSetting selectedTabSetting = App.WildFlowerSettingsRepo.GetSetting("SelectedTab");

        public WildFlowerDetailPage(WildFlower plant, bool streaming)
        {
            GC.Collect();
            NavigationPage.SetHasNavigationBar(this, false);
            var helpers = new ViewHelpers();

            Children.Add(new WildFlowerImagesPage(plant,streaming) { Title = "IMAGES", Icon = "images.png" });
            Children.Add(new WildFlowerInfoPage(plant) { Title = "INFO", Icon = "info.png" });
            Children.Add(new WildFlowerEcologyPage(plant) { Title = "ECOLOGY", Icon = "ecology.png" });
            Children.Add(new WildFlowerRangePage(plant, streaming) { Title = "RANGE", Icon = "range.png" });
            BarBackgroundColor = Color.Black;
            BarTextColor = Color.White;
            BackgroundColor = Color.Black;

            if (selectedTabSetting != null)
                SelectedItem = Children[Convert.ToInt32(selectedTabSetting.valueint)];
            else
                SelectedItem = Children[0];

            this.CurrentPageChanged += RememberPageChange;
            System.GC.Collect();
        }

        private async void RememberPageChange(object sender, EventArgs e)
        {
            int index = this.Children.IndexOf(this.CurrentPage);
            await App.WildFlowerSettingsRepo.AddOrUpdateSettingAsync(new WildFlowerSetting { name = "SelectedTab", valueint = (long?)index });
        }
    }
}
