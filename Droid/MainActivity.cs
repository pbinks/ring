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
        TransactionAdaptor transactionAdaptor;
        StatementHandler statementHandler;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Main);

            balanceView = FindViewById<TextView>(Resource.Id.balanceView);
            transactionsView = 
                FindViewById<ListView>(Resource.Id.transactionListView);
            transactionAdaptor = new TransactionAdaptor(this);
            transactionsView.Adapter = transactionAdaptor;

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
            }
        }
    }
}

