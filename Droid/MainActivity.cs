using Android.App;
using Android.Widget;
using Android.OS;
using Kerv.Common;

namespace Kerv.Droid
{
    [Activity(Label = "Kerv")]
    public class MainActivity : Activity
    {
        TextView balanceView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Main);

            balanceView = FindViewById<TextView>(Resource.Id.balanceView);

            if (Account.IsBalanceSet)
            {
                balanceView.Text = Account.Balance.ToString();
            }

            UpdateStatement();
        }

        private async void UpdateStatement() {
            var handler = new StatementHandler();
            var success = await handler.RefreshStatement();
            if (success) {
                balanceView.Text = Account.Balance.ToString();
            }
        }
    }
}

