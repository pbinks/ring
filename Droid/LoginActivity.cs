
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
    [Activity(Label = "Kerv", MainLauncher = true, Icon = "@mipmap/icon")]
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
            SetEnabled(true);

            submitButton.Click += delegate {
                Login();
            };;
        }

        void SetEnabled(bool enabled) {
            usernameField.Activated = enabled;
            passwordField.Activated = enabled;
            submitButton.Activated = enabled;
            loginProgress.Visibility = enabled ? ViewStates.Gone : ViewStates.Visible;
        }

        async void Login() {
            SetEnabled(false);
            LoginHandler handler = new LoginHandler();
            var loggedIn = 
                await handler.Login(usernameField.Text, passwordField.Text);
            SetEnabled(true);
            if (loggedIn) {
                Toast.MakeText(this, "Success!", ToastLength.Long).Show();
            } else {
                Toast.MakeText(this, "Fail :(", ToastLength.Long).Show();
            }
        }
    }
}
