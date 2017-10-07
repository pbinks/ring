using System;
using HtmlAgilityPack;
namespace ConsoleRunner
{
    public class HtmlParser : Kerv.HtmlParser
    {
        public HtmlParser()
        {
        }

        public override String GetVerificationToken(String html) {
            var document = new HtmlDocument();
            document.LoadHtml(html);
            var form = document.GetElementbyId("form0");
            var inputs = form.Elements("input");

            foreach(var input in inputs) {
                var name = input.GetAttributeValue("name", "");
                if (name == "__RequestVerificationToken") {
                    return input.GetAttributeValue("value", "");
                }
            }

            return "";
        }
    }
}
