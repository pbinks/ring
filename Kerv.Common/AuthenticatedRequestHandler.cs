using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Kerv.Common
{
    public class NotLoggedInException : Exception {}

    public class AuthenticatedRequestHandler
    {
        protected async Task<HttpResponseMessage> Get(String url) {
            var response = await RequestHandler.Client.GetAsync(url);

            if (response.StatusCode == HttpStatusCode.OK) {
                return response;
            }

            if (String.IsNullOrEmpty(Credentials.Password)) {
                throw new NotLoggedInException();
            }

            var loginHandler = new LoginHandler();
            var success = await loginHandler.Login(Credentials.Username, Credentials.Password);

            if (success) {
                response = await RequestHandler.Client.GetAsync(url);
                return response;
            } else {
                throw new NotLoggedInException();
            }
        }
    }
}
