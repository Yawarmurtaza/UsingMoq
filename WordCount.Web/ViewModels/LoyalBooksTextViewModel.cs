using System.Collections.Concurrent;
using System.Collections.Generic;
using WordCount.Model;

namespace WordCount.Web.ViewModels
{
    public class LoyalBooksTextViewModel
    {
        public IEnumerable<WordOccurance> WordCount { get; set; }

    }
}

  