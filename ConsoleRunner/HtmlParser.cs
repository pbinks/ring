using System;
using HtmlAgilityPack;
using Kerv;
using System.Collections.Generic;

namespace ConsoleRunner
{
    public class HtmlParser : Kerv.HtmlParser
    {
        public override String GetBalance(String html)
        {
            var document = new HtmlDocument();
            document.LoadHtml(html);
            var spans = document.DocumentNode.Descendants("span");
            foreach (var span in spans) {
                if (span.Attributes.Contains("class") &&
                    span.Attributes["class"].Value
                        .Contains("accountstatus__amount")) {
                    return span.InnerText;
                }
            }
            return "";
        }

        public override void GetTransactions(string html, out TransactionStore transactions)
        {
            var document = new HtmlDocument();
            document.LoadHtml(html);
            var table = document.GetElementbyId("transactions");
            var rows = table.Descendants("tr");
            transactions = new TransactionStore();
            foreach (var row in rows) {
                if (row.ChildNodes[1].Name == "th") {
                    continue;
                }
                var cells = row.ChildNodes;
                var dateString = cells[1].InnerText.Trim();
                var deviceString = cells[3].InnerText.Trim();
                var description = cells[5].InnerText.Trim();
                var amountString = cells[7].InnerText.Trim();
                var balanceString = cells[9].InnerText.Trim();
                var date = DateTime.Parse(dateString);
                var amount = new Money(amountString);
                var balance = new Money(balanceString);
            }
        }

        public override String GetVerificationToken(String html)
        {
            var document = new HtmlDocument();
            document.LoadHtml(html);
            var form = document.GetElementbyId("form0");
            var inputs = form.Elements("input");

            foreach (var input in inputs)
            {
                var name = input.GetAttributeValue("name", "");
                if (name == "__RequestVerificationToken")
                {
                    return input.GetAttributeValue("value", "");
                }
            }
            return "";
        }
    }
}
