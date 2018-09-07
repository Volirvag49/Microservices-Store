using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ShoppingCart.Api.Infrastructure.CorrelationToken
{
    public class HttpClientFactory : IHttpClientFactory
    {
        private string correlationToken;

        public string CorrelationToken
        {
            get
            {
                return correlationToken;
            }

            internal set
            {
                correlationToken = value;
            }

        }        

        public async Task<HttpClient> Create(Uri uri)
        {
            var client = new HttpClient() { BaseAddress = uri };
            client.DefaultRequestHeaders.Add("Correlation-Token", this.correlationToken);

            return client;
        }
    }
}
