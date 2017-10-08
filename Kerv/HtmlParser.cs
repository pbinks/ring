using System;
namespace Kerv
{
    public abstract class HtmlParser
    {
        public abstract String GetVerificationToken(String html);

        public abstract String GetBalance(String html);

        public abstract void GetTransactions(String html, out TransactionStore transactions);
    }
}
