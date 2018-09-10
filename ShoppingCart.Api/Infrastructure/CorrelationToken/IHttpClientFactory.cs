using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ShoppingCart.Api.Infrastructure.CorrelationToken
{
    public interface IHttpClientFactory
    {
        HttpClient Create(Uri uri);
    }
}
