using Android.Widget;
using Android.Text;
using PortableApp;
using PortableApp.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using G = Android.Graphics;

[assembly: ExportRenderer(typeof(CustomSearchBar), typeof(CustomSearchBarRenderer))]

namespace PortableApp.Droid
{
    public class CustomSearchBarRenderer : SearchBarRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<SearchBar> e)
        {
            base.OnElementChanged(e);
            var searchBar = (CustomSearchBar)Element;

            // Get native control (background set in shared code, but can use SetBackgroundColor here)
            SearchView searchView = (base.Control as SearchView);
            searchView.SetInputType(InputTypes.ClassText | InputTypes.TextVariationNormal);

            // Access search textview within control
            int textViewId = searchView.Context.Resources.GetIdentifier("android:id/search_src_text", null, null);
            EditText textView = (searchView.FindViewById(textViewId) as EditText);
            textView.SetHintTextColor(G.Color.Rgb(200, 200, 200));
            textView.SetTextColor(global::Android.Graphics.Color.White);

            // Customize frame color
            int frameId = searchView.Context.Resources.GetIdentifier("android:id/search_plate", null, null);
            Android.Views.View frameView = (searchView.FindViewById(frameId) as Android.Views.View);
            frameView.SetBackgroundColor(G.Color.Rgb(100, 100, 100));
        }
    }
}