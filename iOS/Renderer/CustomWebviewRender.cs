using System;
using oventy;
using oventy.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomWebview), typeof(CustomWebviewRender))]
namespace oventy.iOS
{
    public class CustomWebviewRender : WebViewRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            if (NativeView != null && e.NewElement != null)
                SetupControlSettings();

            Delegate = new CustomWebviewDelegate(this);
        }

        private void SetupControlSettings()
        {
            var webView = ((UIWebView)NativeView);
            webView.ScalesPageToFit = true;
        }

        public class CustomWebviewDelegate : UIWebViewDelegate
        {
            CustomWebviewRender _webViewRenderer;

            public CustomWebviewDelegate(CustomWebviewRender webViewRenderer = null)
            {
                _webViewRenderer = webViewRenderer ?? new CustomWebviewRender();
            }

            public override void LoadingFinished(UIWebView webView)
            {
                var wv = _webViewRenderer.Element as CustomWebview;
                Console.WriteLine("webview load finished");

                var jsAccessTokenString = $"localStorage.setItem('ls.accessToken', '{Settings.AccessToken}')";
                var jsRefreshTokenString = $"localStorage.setItem('ls.refreshToken', '{Settings.RefreshToken}')";
                webView.EvaluateJavascript(jsAccessTokenString);
                webView.EvaluateJavascript(jsRefreshTokenString);
            }
        }
    }
}
