using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kerv
{
    public class OldLoginHandler 
    {
        static String LoginPageUrl = "https://kerv.com/en/loginsignup#login";
        static String LoginUrl = "https://kerv.com/umbraco/Surface/LoginSignup/Login";

        String username;
        String password;
        RequestHandler handler;

        public OldLoginHandler(String username, String password, RequestHandler handler)
        {
            this.username = username;
            this.password = password;
            this.handler = handler;
        }

        public async Task<bool> Login(HtmlParser parser) {
            var pageRequest = new Request(LoginPageUrl);
            var html = await handler.Get(pageRequest);
            var token = parser.GetVerificationToken(html);

            var loginRequest = new Request(LoginUrl);
            loginRequest.AddValue("LoginEmail", username);
            loginRequest.AddValue("UserPassword", password);
            loginRequest.AddValue("__RequestVerificationToken", token);
            return await handler.Post(loginRequest);
        }
    }
}
