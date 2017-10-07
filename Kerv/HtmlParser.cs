using System;
namespace Kerv
{
    public abstract class HtmlParser
    {
        public HtmlParser()
        {
        }

        public abstract String GetVerificationToken(String html);
    }
}
