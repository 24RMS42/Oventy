using Android.OS;
using Xamarin.Forms.Platform.Android;
using oventy.Droid;
using Xamarin.Forms;
using oventy;

[assembly: ExportRenderer(typeof(CustomWebview), typeof(CustomWebviewRender))]
namespace oventy.Droid
{
    public class CustomWebviewRender : WebViewRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<WebView> e)
        {
            base.OnElementChanged(e);

            if (Control != null && e.NewElement != null)
                SetupControlSettings();
        }

        private void SetupControlSettings()
        {
            Control.Settings.JavaScriptEnabled = true;

            // Handy Hint: PDF JS will show massive fonts unless the minimum font size is defined as 0. I found this doesn't affect anything else I came across.
            Control.Settings.MinimumFontSize = 0;

            // Android 4.4 and below doesn't respect the ViewPort in HTML
            if (Build.VERSION.SdkInt < BuildVersionCodes.Lollipop)
                Control.Settings.UseWideViewPort = true;
        }
    }
}
