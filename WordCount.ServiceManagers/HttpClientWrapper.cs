using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WordCount.ServiceManagers.Interfaces;

namespace WordCount.ServiceManagers
{
    public class HttpClientWrapper : IHttpClientWrapper
    {
        private readonly HttpClient asyncClient;
        
        public HttpClientWrapper()
        {
            this.asyncClient = new HttpClient(new HttpClientHandler { UseDefaultCredentials = true });
            // this.asyncClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/text"));
        }

        public async Task<HttpResponseMessage> GetAsync(string controllerUri)
        {
            return await this.asyncClient.GetAsync(controllerUri);
        }
    }
}