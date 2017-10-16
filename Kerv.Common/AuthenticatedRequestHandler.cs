using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Kerv.Common
{
    public interface LoggedOutListener {
        void OnLoggedOut();
    }

    public class AuthenticatedRequestHandler
    {
        private LoggedOutListener loggedOutListener;

        public AuthenticatedRequestHandler(LoggedOutListener listener) 
                => loggedOutListener = listener;

        protected async Task<HttpResponseMessage> Get(String url) {
            var response = await RequestHandler.Client.GetAsync(url);

            if (response.StatusCode == HttpStatusCode.OK) {
                return response;
            }

            if (String.IsNullOrEmpty(Credentials.Password)) {
                loggedOutListener.OnLoggedOut();
                return new HttpResponseMessage(HttpStatusCode.Forbidden);
            }

            var loginHandler = new LoginHandler();
            var success = await loginHandler.Login(Credentials.Username, 
                                                   Credentials.Password, true);

            if (success) {
                response = await RequestHandler.Client.GetAsync(url);
                return response;
            } else {
                loggedOutListener.OnLoggedOut();
                return new HttpResponseMessage(HttpStatusCode.Forbidden);
            }
        }
    }
}
