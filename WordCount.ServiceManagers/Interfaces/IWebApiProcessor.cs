using System.Collections.Generic;
using System.Threading.Tasks;

namespace WordCount.ServiceManagers.Interfaces
{
    public interface IWebApiProcessor
    {
        string ApiPath { get; set; }

        string WebLocation { get; set; }

        Task<string> GetStringAsync();
    }
}