using Flurl.Http;

namespace Finsight.Client.Http
{
    public abstract class BaseService
    {
        protected IFlurlClient Client { get; }
        protected void SetAuthHeader(string token) => Client.WithOAuthBearerToken(token);

        protected BaseService()
        {
            Client = FlurlHttp.Clients.Get("Client");
        }
    }
}