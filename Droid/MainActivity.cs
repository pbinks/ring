using Android.App;
using Android.Widget;
using Android.OS;
using Kerv.Common;
using Android.Content;
using Android.Support.V4.Widget;

namespace Kerv.Droid
{
    [Activity(Label = "Ring")]
    public class MainActivity 
        : Activity, LoggedOutListener, SwipeRefreshLayout.IOnRefreshListener
    {


        TextView balanceView;
        ListView transactionsView;
        SwipeRefreshLayout transactionsRefreshLayout;
        Spinner deviceSpinner;
        TransactionAdaptor transactionAdaptor;
        StatementHandler statementHandler;
        string deviceId;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Main);

            balanceView = FindViewById<TextView>(Resource.Id.balanceView);
            transactionsView = 
                FindViewById<ListView>(Resource.Id.transactionListView);
            deviceSpinner = FindViewById<Spinner>(Resource.Id.deviceSpinner);
            transactionsRefreshLayout = 
                FindViewById<SwipeRefreshLayout>(
                    Resource.Id.transactionRefreshLayout);

            transactionsRefreshLayout.SetOnRefreshListener(this);

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

            statementHandler = new StatementHandler(this);
            UpdateStatement();
        }

        private async void UpdateStatement() {
            transactionsRefreshLayout.Refreshing = true;
            var success = await statementHandler.RefreshStatement();
            if (success) {
                balanceView.Text = Account.Balance.ToString();
                if (string.IsNullOrEmpty(deviceId))
                {
                    deviceId = statementHandler.CardID;
                }
                if (deviceId != statementHandler.CardID) {
                    await statementHandler.TransactionsForDevice(deviceId);
                }
                transactionAdaptor.Transactions = statementHandler.Transactions;
                if (!string.IsNullOrEmpty(statementHandler.RingID)) {
                    deviceSpinner.Enabled = true;
                }
            }
            transactionsRefreshLayout.Refreshing = false;
        }

        async void DeviceSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            transactionsRefreshLayout.Refreshing = true;
            if (e.Position == 0) {
                deviceId = statementHandler.CardID;
            } else {
                deviceId = statementHandler.RingID;
            }
            await statementHandler.TransactionsForDevice(deviceId);
            transactionAdaptor.Transactions = statementHandler.Transactions;
            transactionsRefreshLayout.Refreshing = false;
        }

        public void OnLoggedOut()
        {
            var intent = new Intent(this, typeof(LoginActivity));
            StartActivity(intent);
            Finish();
        }

        public void OnRefresh()
        {
            UpdateStatement();
        }
    }
}

