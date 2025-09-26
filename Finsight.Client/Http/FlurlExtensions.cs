using Flurl.Http;

namespace Finsight.Client.Http
{
    public static class FlurlExtensions
    {
        public static IFlurlRequest Request(this IFlurlRequest request)
        {
            return request;
        }
    }
}
