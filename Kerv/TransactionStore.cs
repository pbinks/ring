using System;
using System.Collections.Generic;

namespace Kerv
{
    public class TransactionStore
    {
        private List<Transaction> transactions;
        public IEnumerator<Transaction> Transactions {
            get => transactions.GetEnumerator();
        }

        public TransactionStore()
        {
            transactions = new List<Transaction>();
        }

        public void AddTransaction(Transaction transaction) {
            transactions.Add(transaction);
        }
    }
}
