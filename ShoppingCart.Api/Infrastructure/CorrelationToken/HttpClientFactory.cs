using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ShoppingCart.Api.Infrastructure.CorrelationToken
{
    public class HttpClientFactory : IHttpClientFactory
    {
        public static string CorrelationToken {get; set; }

        public HttpClient Create(Uri uri)
        {
            var client = new HttpClient() { BaseAddress = uri };
            client.DefaultRequestHeaders.Add("Correlation-Token", CorrelationToken);

            return client;
        }
    }
}
