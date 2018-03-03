
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
            webView.SetWebViewClient(new WebViewClient());

            foreach (var cookie in Session.Instance.Cookies) {
                CookieManager.Instance.SetCookie("https://kerv.com", cookie);
            }

            webView.Settings.JavaScriptEnabled = true;
            webView.LoadUrl("https://kerv.com/en/account/load/");
        }
    }
}
