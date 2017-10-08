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
        ListView transactionsView;
        Spinner deviceSpinner;
        TransactionAdaptor transactionAdaptor;
        StatementHandler statementHandler;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Main);

            balanceView = FindViewById<TextView>(Resource.Id.balanceView);
            transactionsView = 
                FindViewById<ListView>(Resource.Id.transactionListView);
            deviceSpinner = FindViewById<Spinner>(Resource.Id.deviceSpinner);

            transactionAdaptor = new TransactionAdaptor(this);
            transactionsView.Adapter = transactionAdaptor;

            deviceSpinner.Adapter = 
                new ArrayAdapter(this, 
                                 Android.Resource.Layout.SimpleSpinnerDropDownItem, 
                                 Resources.GetStringArray(Resource.Array.device_list));
            deviceSpinner.Enabled = false;
            deviceSpinner.ItemSelected += DeviceSelected;

            if (Account.IsBalanceSet)
            {
                balanceView.Text = Account.Balance.ToString();
            }

            statementHandler = new StatementHandler();
            UpdateStatement();
        }

        private async void UpdateStatement() {
            var success = await statementHandler.RefreshStatement();
            if (success) {
                balanceView.Text = Account.Balance.ToString();
                transactionAdaptor.Transactions = statementHandler.Transactions;
                if (!string.IsNullOrEmpty(statementHandler.RingID)) {
                    deviceSpinner.Enabled = true;
                }
            }
        }

        async void DeviceSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            if (e.Position == 0) {
                await statementHandler.TransactionsForDevice(statementHandler.CardID);
            } else {
                await statementHandler.TransactionsForDevice(statementHandler.RingID);
            }
            transactionAdaptor.Transactions = statementHandler.Transactions;
        }
    }
}

