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
        private IEnumerable<WordOccurance> wordCount;

        public LoyalBooksWebApiParallelManager(IWebApiProcessor apiProcessor, IMemoryCache cache)
            : base(apiProcessor, cache)
        {
        }


        public override async Task<IEnumerable<WordOccurance>> GetIndivisualWordsCount(string bookName)
        {
            
            if (!base.cache.TryGetValue(bookName, out this.wordCount))
            {
                string text = await base.GetBookText(bookName);

                IList<string> sectionStrings = this.BreakIntoChunks(text);

                IList<Dictionary<string, int>> listOfWordCounts = new List<Dictionary<string, int>>();
                Parallel.ForEach(sectionStrings, stringSection =>
                {
                    listOfWordCounts.Add(base.CountWords(stringSection));

                });

                IDictionary<string, int> allWordCounts = this.MergeWordCountResults(listOfWordCounts);
                this.wordCount = allWordCounts.ConvertToWordOccurenceModel();
                this.cache.Set(bookName, this.wordCount, this.cacheEntryOptions);

            }

            return wordCount.OrderBy(item => item.Word);
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


        private IList<string> BreakIntoChunks(string text)
        {
            int originalSectionLength = text.Length / 8;
            int sectionLength = originalSectionLength;
            int startIndex = 0;

            IList<string> sectionStrings = new List<string>();

            while (string.Join(string.Empty, sectionStrings).Length + 1 < text.Length)
            {
                sectionLength = startIndex + sectionLength < text.Length
                    ? sectionLength
                    : text.Length - startIndex - 1;

                char criticalLetter = text[startIndex + sectionLength];

                while (char.IsLetter(criticalLetter) || char.IsDigit(criticalLetter))
                {
                    sectionLength++;
                    criticalLetter = text[startIndex + sectionLength];
                }

                string sectionString = text.Substring(startIndex, sectionLength);
                sectionStrings.Add(sectionString);
                startIndex += sectionString.Length;
                sectionLength = originalSectionLength;
            }

            return sectionStrings;
        }
    }
}