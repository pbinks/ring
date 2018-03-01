using Android.App;
using Android.Widget;
using Android.Views;
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
        DeviceAdaptor deviceAdaptor;
        StatementHandler statementHandler;
        Device device;

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

            deviceAdaptor = new DeviceAdaptor(this);
            deviceSpinner.Adapter = deviceAdaptor;
            deviceSpinner.Enabled = false;
            deviceSpinner.ItemSelected += DeviceSelected;

            if (Account.IsBalanceSet)
            {
                balanceView.Text = Account.Balance.ToString();
            }

            statementHandler = new StatementHandler(this);
            UpdateStatement();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId) {
                case Resource.Id.top_up:
                    var intent = new Intent(this, typeof(TopupActivity));
                    StartActivity(intent);
                    return true;
            }
            return false;
        }

        private async void UpdateStatement() {
            transactionsRefreshLayout.Refreshing = true;
            var success = await statementHandler.RefreshStatement();
            if (success) {
                balanceView.Text = Account.Balance.ToString();
                deviceAdaptor.Devices = statementHandler.Devices;
                if (device == null) {
                    device = statementHandler.Devices[0];
                }
                if (device != statementHandler.Devices[0]) {
                    await statementHandler.TransactionsForDevice(device);
                }
                transactionAdaptor.Transactions = statementHandler.Transactions;
                if (deviceSpinner.Count > 1) {
                    deviceSpinner.Enabled = true;
                }
            }
            transactionsRefreshLayout.Refreshing = false;
        }

        async void DeviceSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            transactionsRefreshLayout.Refreshing = true;
            device = statementHandler.Devices[e.Position];
            await statementHandler.TransactionsForDevice(device);
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

