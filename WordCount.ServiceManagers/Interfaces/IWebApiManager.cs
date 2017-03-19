using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using WordCount.Model;

namespace WordCount.ServiceManagers.Interfaces
{
    public interface IWebApiManager
    {
        Task<IEnumerable<WordOccurance>> GetIndivisualWordsCount(string bookName);
    }
}