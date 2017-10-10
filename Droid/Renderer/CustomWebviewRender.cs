using Android.OS;
using Xamarin.Forms.Platform.Android;
using oventy.Droid;
using Xamarin.Forms;
using oventy;
using System.Threading;
using System.Threading.Tasks;
using System;
using Android.Webkit;

[assembly: ExportRenderer(typeof(CustomWebview), typeof(CustomWebviewRender))]
namespace oventy.Droid
{
    public class CustomWebviewRender : WebViewRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.WebView> e)
        {
            base.OnElementChanged(e);

            if (Control != null && e.NewElement != null)
                SetupControlSettings();

            var webView = e.NewElement as CustomWebview;
            if (webView != null)
                webView.EvaluateJavascript = async (js) =>
                {
                    var reset = new ManualResetEvent(false);
                    var response = string.Empty;
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Control?.EvaluateJavascript(js, new JavascriptCallback((r) => { response = r; reset.Set(); }));
                    });
                    await Task.Run(() => { reset.WaitOne(); });
                    return response;
                };
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

        internal class JavascriptCallback : Java.Lang.Object, IValueCallback
        {
            public JavascriptCallback(Action<string> callback)
            {
                _callback = callback;
            }

            private Action<string> _callback;
            public void OnReceiveValue(Java.Lang.Object value)
            {
                _callback?.Invoke(Convert.ToString(value));
            }
        }
    }
}
