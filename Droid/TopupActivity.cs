
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Webkit;
using Kerv.Common;
using System.IO;
using Android.Util;
using System.Text.RegularExpressions;

namespace Kerv.Droid
{
    [Activity(Label = "Top Up")]
    public class TopupActivity : Activity
    {
        WebView webView;
        ProgressBar progressBar;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.TopUp);

            progressBar = FindViewById<ProgressBar>(Resource.Id.loadProgress);

            webView = FindViewById<WebView>(Resource.Id.topupWebView);
            webView.Visibility = ViewStates.Invisible;
            webView.SetWebViewClient(new TopupClient(this, progressBar));

            foreach (var cookie in Session.Instance.Cookies) {
                CookieManager.Instance.SetCookie("https://kerv.com", cookie);
            }

            webView.Settings.JavaScriptEnabled = true;
            webView.LoadUrl("https://kerv.com/en/account/load/");
        }

        private class TopupClient : WebViewClient {
            private Context context;
            private ProgressBar loader;

            public TopupClient(Context context, ProgressBar loader) {
                this.context = context;
                this.loader = loader;
            }

            public override void OnPageFinished(WebView view, string url)
            {
                if (url.StartsWith("javascript")) {
                    base.OnPageFinished(view, url);
                    return;
                }

                var stream = context.Assets.Open("topup.css");
                StreamReader sr = new StreamReader(stream);
                string cssText = sr.ReadToEnd();
                cssText = Regex.Replace(cssText, @"\t|\n|\r", "");
                sr.Close();

                view.LoadUrl("javascript:(function() {" +
                    "var css = '" + cssText + "';" +
                    "var style = document.createElement('style');" +
                    "style.type = 'text/css';" +
                    "style.appendChild(document.createTextNode(css));" +
                    "document.head.appendChild(style);" +
                "})()");
                
                view.Visibility = ViewStates.Visible;

                base.OnPageFinished(view, url);
            }

        }
    }
}
