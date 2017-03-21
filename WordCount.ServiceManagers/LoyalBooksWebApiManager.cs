using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using WordCount.Model;
using WordCount.ServiceManagers.Interfaces;

namespace WordCount.ServiceManagers
{
    public class LoyalBooksWebApiManager : BaseLoyalBooksWebApiManager
    {
        private readonly ITextProcessor textProcessor;
        private IEnumerable<WordOccurance> wordCount;

        public LoyalBooksWebApiManager(IWebApiProcessor apiProcessor, IMemoryCache cache, ITextProcessor textProcessor) : base(apiProcessor, cache)
        {
            this.textProcessor = textProcessor;
        }

        public override async Task<IEnumerable<WordOccurance>> GetIndivisualWordsCount(string bookName)
        {
            if (!this.cache.TryGetValue(bookName, out this.wordCount))
            {
                string text = await base.GetBookText(bookName);
                this.wordCount = this.textProcessor.CountWords(text).ConvertToWordOccurenceModel();
                this.cache.Set(bookName, this.wordCount, this.cacheEntryOptions);
            }

            return this.wordCount.OrderByDescending(item => item.Word);
        }
       
    }
}