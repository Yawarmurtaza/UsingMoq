using System.Net.Http;
using System.Threading.Tasks;

namespace WordCount.ServiceManagers.Interfaces
{
    public interface IHttpClientWrapper
    {
        Task<HttpResponseMessage> GetAsync(string controllerUri);
    }
}