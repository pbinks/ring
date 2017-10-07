using System;
namespace Kerv
{
    public abstract class HtmlParser
    {
        public abstract String GetVerificationToken(String html);

        public abstract String GetBalance(String html);
    }
}
