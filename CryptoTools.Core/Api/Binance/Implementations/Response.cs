using CryptoTools.Core.Api.Binance.Interfaces;
using System.IO;
using System.Net;

namespace CryptoTools.Core.Api.Binance.Implementations
{
    public class Response : IResponse
    {
        private readonly WebResponse response;

        public Response(WebResponse response)
        {
            this.response = response;
        }

        public Stream GetResponseStream()
        {
            return response.GetResponseStream();
        }
    }
}
