
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
using Kerv.Common;

using Microsoft.Azure.Mobile;
using Microsoft.Azure.Mobile.Analytics;
using Microsoft.Azure.Mobile.Crashes;

namespace Kerv.Droid
{
    [Activity(Label = "Ring", MainLauncher = true, Icon = "@mipmap/ic_launcher")]
    public class LoginActivity : Activity
    {
        private EditText usernameField, passwordField;
        private Button submitButton;
        private ProgressBar loginProgress;
        private CheckBox rememberCheckbox;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            MobileCenter.Start(GetString(Resource.String.azure_mobile_key),
                   typeof(Analytics), typeof(Crashes));

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Login);

            usernameField = FindViewById<EditText>(Resource.Id.usernameField);
            passwordField = FindViewById<EditText>(Resource.Id.passwordField);
            submitButton = FindViewById<Button>(Resource.Id.submitButton);
            loginProgress = 
                FindViewById<ProgressBar>(Resource.Id.loginProgress);
            rememberCheckbox = 
                FindViewById<CheckBox>(Resource.Id.rememberCheckbox);

            submitButton.Click += delegate {
                Login(usernameField.Text, passwordField.Text, 
                      rememberCheckbox.Checked);
            };

            if (!String.IsNullOrEmpty(Credentials.Username)) {
                usernameField.Text = Credentials.Username;
            }

            if (String.IsNullOrEmpty(Credentials.Password)) {
                SetEnabled(true);
            } else {
                SetEnabled(false);
                Login(Credentials.Username, Credentials.Password, true);
            }
        }

        void SetEnabled(bool enabled) {
            usernameField.Visibility = enabled ? ViewStates.Visible : ViewStates.Gone;
            passwordField.Visibility = enabled ? ViewStates.Visible : ViewStates.Gone;
            submitButton.Visibility = enabled ? ViewStates.Visible : ViewStates.Gone;
            rememberCheckbox.Visibility = enabled ? ViewStates.Visible : ViewStates.Gone;
            loginProgress.Visibility = enabled ? ViewStates.Gone : ViewStates.Visible;
        }

        async void Login(string username, string password, bool remember) {
            SetEnabled(false);
            LoginHandler handler = new LoginHandler();
            var loggedIn = 
                await handler.Login(username, password, remember);
            if (loggedIn) {
                var intent = new Intent(this, typeof(MainActivity));
                StartActivity(intent);
                Finish();
            } else {
                Toast.MakeText(this, "Incorrect login details", 
                               ToastLength.Long).Show();
                SetEnabled(true);
            }
        }
    }
}
