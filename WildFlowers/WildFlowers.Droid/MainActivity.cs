using System;

using Android.App;
using Android.Content.PM;
using Android.OS;
using CarouselView.FormsPlugin.Android;
using FFImageLoading.Forms.Droid;

namespace PortableApp.Droid
{
    [Activity(Label = "WildFlowers", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            //TabLayoutResource = Resource.Layout.Tabbar;
            //ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            CarouselViewRenderer.Init();
            CachedImageRenderer.Init();


            string dbPath = FileAccessHelper.GetLocalFilePath("db.db3");
            var platform = new SQLite.Net.Platform.XamarinAndroid.SQLitePlatformAndroid();
            LoadApplication(new PortableApp.App(platform, dbPath));


        }
    }
}

