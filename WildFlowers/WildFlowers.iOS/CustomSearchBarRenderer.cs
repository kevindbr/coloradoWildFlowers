using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using UIKit;

[assembly: ExportRenderer(typeof(PortableApp.CustomSearchBar), typeof(PortableApp.iOS.CustomSearchBarRenderer))]
namespace PortableApp.iOS
{
    public class CustomSearchBarRenderer : SearchBarRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<SearchBar> args)
        {
            base.OnElementChanged(args);

            UISearchBar bar = (UISearchBar)this.Control;
            bar.SetPositionAdjustmentforSearchBarIcon(new UIOffset(10, 0), 0);
            bar.BarTintColor = new UIColor(red: 0.4f, green: 0.4f, blue: 0.4f, alpha: .0f);
        }
    }
}