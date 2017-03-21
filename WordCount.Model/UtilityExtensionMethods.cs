using System.Collections.Generic;
using System.Linq;

namespace WordCount.Model
{
    public static class UtilityExtensionMethods
    {
        public static IEnumerable<WordOccurance> ConvertToWordOccurenceModel(this IDictionary<string, int> allWordCounts)
        {
            IEnumerable < WordOccurance > 
                wordCount = allWordCounts.Where(x => x.Key != string.Empty && x.Key != "*").Select(word => new WordOccurance()
                {
                    Word = word.Key,
                    Count = word.Value,
                    PrimeNumberStatus = word.Value == 1 ? "UNIT" : word.Value.IsPrime() ? "YES" : "NO"
                });

            return wordCount;
        }

        public static bool IsPrime(this int candidate)
        {
            if ((candidate & 1) == 0)
            {
                return candidate == 2;
            }
           
            for (int i = 3; (i * i) <= candidate; i += 2)
            {
                if ((candidate % i) == 0)
                {
                    return false;
                }
            }

            return candidate != 1;
        }
    }
}