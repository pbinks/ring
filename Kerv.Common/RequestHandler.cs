using System;
using System.Net;
using System.Net.Http;

namespace Kerv.Common
{
    public class RequestHandler
    {
        static CookieContainer cookies = new CookieContainer();
        private static HttpClient client = 
            new HttpClient(new HttpClientHandler {
                CookieContainer = cookies,
                AllowAutoRedirect = false,
                UseCookies = true
        });
        public static HttpClient Client { get => client; }

        public static void Reset() {
            cookies = new CookieContainer();
            Client.CancelPendingRequests();
            Client.Dispose();
            client = new HttpClient(new HttpClientHandler
            {
                CookieContainer = cookies,
                AllowAutoRedirect = false,
                UseCookies = true
            });
        }
    }
}
