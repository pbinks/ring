using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace Kerv.Common
{
    public class StatementHandler
    {
        public class InvalidFormatException : Exception {}

        static readonly String statementUrl =
            "https://kerv.com/en/account/statement/";

        private List<Transaction> transactions;
        public List<Transaction> Transactions { get => transactions; }

        public StatementHandler() {
            transactions = new List<Transaction>();
        }

        public async Task<bool> RefreshStatement()
        {
            var html = await RequestHandler.Client.GetStringAsync(statementUrl);
            try
            {
                var balance = ParseBalance(html);
                ParseTransactions(html);
                Account.Balance = balance;
            } catch(InvalidFormatException ex) {
                return false;
            }
            return true;
        }

        private Money ParseBalance(String html)
        {
            var document = new HtmlDocument();
            document.LoadHtml(html);
            var spans = document.DocumentNode.Descendants("span");
            foreach (var span in spans)
            {
                if (span.Attributes.Contains("class") &&
                    span.Attributes["class"].Value
                        .Contains("accountstatus__amount"))
                {
                    return new Money(span.InnerText);
                }
            }
            throw new InvalidFormatException();
        }

        private void ParseTransactions(String html)
        {
            var document = new HtmlDocument();
            document.LoadHtml(html);
            var table = document.GetElementbyId("transactions");
            var rows = table.Descendants("tr");
            transactions = new List<Transaction>();
            foreach (var row in rows)
            {
                if (row.ChildNodes[1].Name == "th")
                {
                    continue;
                }
                var cells = row.ChildNodes;
                var dateString = cells[1].InnerText.Trim();
                var deviceString = cells[3].InnerText.Trim();
                var description = cells[5].InnerText.Trim();
                var amountString = cells[7].InnerText.Trim();
                var date = DateTime.Parse(dateString);
                var amount = new Money(amountString);
                Transactions.Add(new Transaction { Date = date, 
                    Amount = amount, 
                    Description = description, 
                    Device = deviceString 
                });
            }
        }
    }
}
