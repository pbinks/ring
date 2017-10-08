using System;
using System.Threading.Tasks;
namespace Kerv
{
    public class BalanceHandler
    {
        static readonly String balanceUrl = 
            "https://kerv.com/en/account/statement/";

        public async Task<float> GetBalance(RequestHandler handler, 
                                            HtmlParser parser) 
        {
            var balanceRequest = new Request(balanceUrl);
            var result = await handler.Get(balanceRequest);
            var balanceString = parser.GetBalance(result);
            var transactions = new TransactionStore();
            parser.GetTransactions(result, out transactions);
            return float.Parse(balanceString.Trim().Substring(6));
        }
    }
}
