
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

namespace Kerv.Droid
{
    [Activity(Label = "Kerv", MainLauncher = true, Icon = "@mipmap/ic_launcher")]
    public class LoginActivity : Activity
    {
        private EditText usernameField, passwordField;
        private Button submitButton;
        private ProgressBar loginProgress;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Login);

            usernameField = FindViewById<EditText>(Resource.Id.usernameField);
            passwordField = FindViewById<EditText>(Resource.Id.passwordField);
            submitButton = FindViewById<Button>(Resource.Id.submitButton);
            loginProgress = FindViewById<ProgressBar>(Resource.Id.loginProgress);

            submitButton.Click += delegate {
                Login(usernameField.Text, passwordField.Text);
            };

            if (!String.IsNullOrEmpty(Credentials.Username)) {
                usernameField.Text = Credentials.Username;
            }

            if (String.IsNullOrEmpty(Credentials.Password)) {
                SetEnabled(true);
            } else {
                SetEnabled(false);
                Login(Credentials.Username, Credentials.Password);
            }
        }

        void SetEnabled(bool enabled) {
            usernameField.Visibility = enabled ? ViewStates.Visible : ViewStates.Gone;
            passwordField.Visibility = enabled ? ViewStates.Visible : ViewStates.Gone;
            submitButton.Visibility = enabled ? ViewStates.Visible : ViewStates.Gone;
            loginProgress.Visibility = enabled ? ViewStates.Gone : ViewStates.Visible;
        }

        async void Login(string username, string password) {
            SetEnabled(false);
            LoginHandler handler = new LoginHandler();
            var loggedIn = 
                await handler.Login(username, password);
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
