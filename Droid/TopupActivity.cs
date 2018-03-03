
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

namespace Kerv.Droid
{
    [Activity(Label = "Top Up")]
    public class TopupActivity : Activity
    {
        WebView webView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.TopUp);

            webView = FindViewById<WebView>(Resource.Id.topupWebView);
            webView.Visibility = ViewStates.Invisible;
            webView.SetWebViewClient(new TopupClient());

            foreach (var cookie in Session.Instance.Cookies) {
                CookieManager.Instance.SetCookie("https://kerv.com", cookie);
            }

            webView.Settings.JavaScriptEnabled = true;
            webView.LoadUrl("https://kerv.com/en/account/load/");
        }

        private class TopupClient : WebViewClient {
            public override void OnPageFinished(WebView view, string url)
            {
                view.LoadUrl("javascript:(function() { " +
                             "document.getElementById('navbar').style.display = 'none'; " +
                             "})()");
                view.LoadUrl("javascript:(function() { " +
                             "document.getElementsByTagName('header')[0].style.display = 'none'; " +
                             "})()");
                view.LoadUrl("javascript:(function() { " +
                             "document.getElementById('mainContent').children[1].style.display = 'none'; " +
                             "})()");
                view.Visibility = ViewStates.Visible;
                base.OnPageFinished(view, url);
            }

        }
    }
}
