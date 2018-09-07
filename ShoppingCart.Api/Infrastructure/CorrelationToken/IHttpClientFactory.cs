using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ShoppingCart.Api.Infrastructure.CorrelationToken
{
    public interface IHttpClientFactory
    {
        Task<HttpClient> Create(Uri uri);
    }
}
