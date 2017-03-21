using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using WordCount.Model;
using WordCount.ServiceManagers.Interfaces;

namespace WordCount.ServiceManagers
{
    public abstract class BaseLoyalBooksWebApiManager : IWebApiManager
    {
        private readonly IWebApiProcessor apiProcessor;
        protected readonly IMemoryCache cache;
        protected readonly MemoryCacheEntryOptions cacheEntryOptions;
        private string bookText;

        protected BaseLoyalBooksWebApiManager(IWebApiProcessor apiProcessor, IMemoryCache cache)
        {
            this.apiProcessor = apiProcessor;
            this.apiProcessor.ApiPath = "download/text/";
            this.apiProcessor.WebLocation = "http://www.loyalbooks.com/";
            this.cache = cache;
            this.cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromHours(1));
        }

        public abstract Task<IEnumerable<WordOccurance>> GetIndivisualWordsCount(string bookName);

        protected async Task<string> GetBookText(string bookName)
        {
            this.apiProcessor.ApiPath += bookName;

            if (!this.cache.TryGetValue(this.apiProcessor.ApiPath, out bookText))
            {
                this.bookText = await this.apiProcessor.GetStringAsync();
                this.cache.Set(this.apiProcessor.ApiPath, this.bookText, this.cacheEntryOptions);
            }

            return bookText;
        }
    }
}