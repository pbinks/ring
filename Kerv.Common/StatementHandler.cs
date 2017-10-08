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
        static readonly String transactionsUrl = 
            "https://kerv.com/umbraco/Surface/KervAccount/CardStatementGet?page=1&duration=0&deviceid={0}&culture=en-GB";

        private List<Transaction> transactions;
        public List<Transaction> Transactions { get => transactions; }
        public String RingID { get; private set; }
        public String CardID { get; private set; }

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
                ParseDevices(html);
                Account.Balance = balance;
            } catch(InvalidFormatException ex) {
                return false;
            }
            return true;
        }

        public async Task<bool> TransactionsForDevice(string deviceID) {
            var html = await RequestHandler.Client.GetStringAsync(
                String.Format(transactionsUrl, deviceID));
            ParseTransactions(html);
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

        private void ParseDevices(String html) {
            var document = new HtmlDocument();
            document.LoadHtml(html);
            var select = document.GetElementbyId("deviceID");
            var options = select.Descendants("option");
            foreach (var option in options) {
                if (option.NextSibling.InnerText.Trim().StartsWith("Ring")) {
                    RingID = option.Attributes["value"].Value;
                } else if (option.NextSibling.InnerText.Trim().StartsWith("VCard")) {
                    CardID = option.Attributes["value"].Value;
                }
            }
        }
    }
}
