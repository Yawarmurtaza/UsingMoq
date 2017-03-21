using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using WordCount.Model;
using WordCount.ServiceManagers.Interfaces;

namespace WordCount.ServiceManagers
{
    public class LoyalBooksWebApiParallelManager : BaseLoyalBooksWebApiManager
    {
        private readonly ITextProcessor textProcessor;
        private IEnumerable<WordOccurance> wordCount;

        public LoyalBooksWebApiParallelManager(IWebApiProcessor apiProcessor, IMemoryCache cache, ITextProcessor textProcessor)
            : base(apiProcessor, cache)
        {
            this.textProcessor = textProcessor;
        }
/*
ublic ActionResult CountWords(string text)
        {
            ViewBag.Title = "Home Page";


            // string text = "The/string123 \"customers /{ customerId}/ orders\" is the URI template for the route. Web API tries to match the request URI to the template.";

            List<char> chars = new List<char>();

            List<string> words = new List<string>();

            for (int index = 0; index < text.Length; index++)
            {
                while (index < text.Length && (char.IsLetter(text[index]) || char.IsNumber(text[index])))
                {
                    chars.Add(text[index]);
                    index++;
                }

                if (chars.Any())
                {
                    words.Add(string.Join(string.Empty, chars));
                    chars.Clear();
                }
                

            }

            return View(words);
        }
*/

        public override async Task<IEnumerable<WordOccurance>> GetIndivisualWordsCount(string bookName)
        {
            
            if (!base.cache.TryGetValue(bookName, out this.wordCount))
            {
                string text = await base.GetBookText(bookName);

                IList<string> sectionStrings = this.textProcessor.BreakIntoChunks(text);

                IList<Dictionary<string, int>> listOfWordCounts = new List<Dictionary<string, int>>();
                Parallel.ForEach(sectionStrings, stringSection =>
                {
                    listOfWordCounts.Add(this.textProcessor.CountWords(stringSection));

                });

                IDictionary<string, int> allWordCounts = this.MergeWordCountResults(listOfWordCounts);
                this.wordCount = allWordCounts.ConvertToWordOccurenceModel();
                this.cache.Set(bookName, this.wordCount, this.cacheEntryOptions);

            }

            return wordCount.OrderByDescending(item => item.Word);
        }

        private IDictionary<string, int> MergeWordCountResults(IList<Dictionary<string, int>> listOfWordCounts)
        {
            IDictionary<string, int> wc = new Dictionary<string, int>();

            foreach (Dictionary<string, int> nextItem in listOfWordCounts)
            {
                foreach (KeyValuePair<string, int> wordCountItem in nextItem)
                {
                    if (wc.ContainsKey(wordCountItem.Key))
                    {
                        wc[wordCountItem.Key] += wordCountItem.Value;
                    }
                    else
                    {
                        wc.Add(wordCountItem.Key, wordCountItem.Value);
                    }
                }
            }
            return wc;
        }


       
    }
}
