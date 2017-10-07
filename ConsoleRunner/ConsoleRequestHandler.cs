using System;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using Kerv;

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
            foreach (var header in response.Headers)
            {
                if (header.Key == "Set-Cookie")
                {
                    foreach (var val in header.Value)
                    {
                        cookies.SetCookies(new Uri(request.Url), val);
                    }
                }
            }

            return await response.Content.ReadAsStringAsync();
        }

        public async override Task<bool> Post(Request request)
        {
            var content = new FormUrlEncodedContent(request.Values);

            try {
                var response = await client.PostAsync(request.Url, content);
                var responseContent = await response.Content.ReadAsStringAsync();
                foreach (var header in response.Headers) {
                    if (header.Key == "Set-Cookie") {
                        foreach (var val in header.Value) {
                            cookies.SetCookies(new Uri(request.Url), val);
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
