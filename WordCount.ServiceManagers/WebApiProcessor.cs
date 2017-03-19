using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using WordCount.ServiceManagers.Interfaces;

namespace WordCount.ServiceManagers
{
    public class WebApiProcessor : IWebApiProcessor
    {
        private readonly IHttpClientWrapper httpClient;
        
        public string ApiPath { get; set; }
        
        public string WebLocation { get; set; }

        public WebApiProcessor(IHttpClientWrapper httpClient)
        {
            this.httpClient = httpClient;
        }
        
        public async Task<string> GetStringAsync()
        {
            this.CheckProperties();
            
            try
            {
                string webPath = string.Format("{0}{1}", this.WebLocation, this.ApiPath);
                // Logger.InfoFormat("Calling link = {0}", controllerUri);
                HttpResponseMessage httpResponseMessage = await this.httpClient.GetAsync(webPath);
                string stringContent = await httpResponseMessage.Content.ReadAsStringAsync();
                return stringContent;
            }
            catch (Exception ex)
            {
                // Logger.Error("GET: Failed with error :- " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Check the properties have all been populated.
        /// </summary>
        private void CheckProperties()
        {
            if (string.IsNullOrEmpty(this.WebLocation))
            {
                throw new InvalidOperationException("WebLocation");
            }

            if (string.IsNullOrEmpty(this.ApiPath))
            {
                throw new InvalidOperationException("ApiPath");
            }
        }
    }
}