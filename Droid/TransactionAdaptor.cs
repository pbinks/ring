using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;

namespace Kerv.Droid
{
    public class TransactionAdaptor : BaseAdapter<Transaction>
    {
        private List<Transaction> transactions;
        public List<Transaction> Transactions { get => transactions;
            set {
                transactions = value;
                NotifyDataSetChanged();
            }}
        private readonly Context context;

        public TransactionAdaptor(Context context) {
            this.context = context;
            transactions = new List<Transaction>(0);
        }

        public override Transaction this[int position] => transactions[position];

        public override int Count => transactions.Count;

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View view, ViewGroup parent) 
        {
            if (view == null) {
                var inflater = 
                    context.GetSystemService(Activity.LayoutInflaterService) 
                           as LayoutInflater;
                view = 
                    inflater.Inflate(Resource.Layout.TransactionListItem, null);
            }

            TextView dateView = 
                view.FindViewById<TextView>(Resource.Id.dateView);
            TextView descriptionView = 
                view.FindViewById<TextView>(Resource.Id.descriptionView);
            TextView amountView = 
                view.FindViewById<TextView>(Resource.Id.amountView);

            var transaction = this[position];
            dateView.Text = String.Format("{0:dd/MM/yy}", transaction.Date);
            descriptionView.Text = transaction.Description;
            amountView.Text = transaction.Amount.ToString();
            if (transaction.Type == Transaction.TransactionType.Credit) {
                amountView.Text = "+" + amountView.Text;
            }
            return view;
        }
    }
}
