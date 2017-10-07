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

        public ConsoleRequestHandler()
        {
        }

        public async override Task<String> Get(string url)
        {
            var response = await client.GetAsync(url);

            return await response.Content.ReadAsStringAsync();
        }

        public async override void Post(String url)
        {
			var content = new FormUrlEncodedContent(values);

            try
            {
                var response = await client.PostAsync(url, content);

                Console.WriteLine(response.Headers);
            } catch(Exception e) {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.ToString());
            }
		}
    }
}
