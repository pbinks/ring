using System;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using Kerv;
using System.Collections.Generic;

namespace ConsoleRunner
{
    public class ConsoleRequestHandler : RequestHandler
    {

        static CookieContainer cookies = new CookieContainer();
        public static readonly HttpClient client = new HttpClient(
            new HttpClientHandler {
                CookieContainer = cookies,
                AllowAutoRedirect = false,
                UseCookies = true
        });

        public async override Task<String> Get(Request request)
        {
            var response = await client.GetAsync(request.Url);

            return await response.Content.ReadAsStringAsync();
        }

        public async override Task<bool> Post(Request request)
        {
            var content = new FormUrlEncodedContent(request.Values);

            try {
                var response = await client.PostAsync(request.Url, content);
                IEnumerable<string> cookieValues;
                var hasCookies = response.Headers.TryGetValues("Set-Cookie", out cookieValues);
                Console.WriteLine(hasCookies);
                if (hasCookies) {
                    foreach (var cookie in cookieValues) {
                        if (cookie.StartsWith("kervplatform")) {
                            Console.WriteLine("found");
                        }
                    }
                }
            } catch(Exception e) {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.ToString());
                return false;
            }
            return true;
		}
    }
}
