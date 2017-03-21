using System.Collections.Generic;

namespace WordCount.ServiceManagers
{
    public interface ITextProcessor
    {
        Dictionary<string, int> CountWords(string text);
        IList<string> BreakIntoChunks(string text);
    }
}