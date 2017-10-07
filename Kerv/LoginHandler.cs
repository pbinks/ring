using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kerv
{
    public class LoginHandler 
    {
        static String LoginPageUrl = "https://kerv.com/en/loginsignup#login";
        static String LoginUrl = "https://kerv.com/umbraco/Surface/LoginSignup/Login";

        String username;
        String password;
        RequestHandler handler;

        public LoginHandler(String username, String password, RequestHandler handler)
        {
            this.username = username;
            this.password = password;
            this.handler = handler;
        }

        public async Task<bool> Login(HtmlParser parser) {
            var html = await handler.Get(LoginPageUrl);
            var token = parser.GetVerificationToken(html);

            handler.AddValue("LoginEmail", username);
            handler.AddValue("UserPassword", password);
            handler.AddValue("__RequestVerificationToken", token);
            handler.Post(LoginUrl);

            return true;
        }
    }
}
