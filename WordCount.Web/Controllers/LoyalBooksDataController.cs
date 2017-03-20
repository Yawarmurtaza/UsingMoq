using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WordCount.Model;
using WordCount.ServiceManagers.Interfaces;

namespace WordCount.Web.Controllers
{
    [Route("api/[controller]")]
    public class LoyalBooksDataController : BaseController
    {
        private readonly IWebApiManager bookManager;
        public LoyalBooksDataController(IDependencyResolver serviceResolver, IServiceProvider services) : base(services)
        {
            this.bookManager = serviceResolver.GetWebApiManagerByName();
        }

/*
http://localhost:9761/api/LoyalBooksData/5/bookName
*/
        [HttpGet]
        [Route("api/LoyalBooksData/{pageNumber}/{bookName}")]
        public async Task<IEnumerable<WordOccurance>> NextTenWords(int pageNumber = 10, string bookName = null)
        {
            if (string.IsNullOrEmpty(bookName))
            {
                bookName = this.Session.GetString("bookName");
                if (string.IsNullOrEmpty(bookName))
                {
                    return null;
                }
            }

            int? recordsToSkip = this.Session.GetInt32("recordsToSkip");
            if (!recordsToSkip.HasValue)
            {
                recordsToSkip = 0;
                this.Session.SetInt32("recordsToSkip", recordsToSkip.Value);
            }
            else
            {
                recordsToSkip += 10;
                this.Session.SetInt32("recordsToSkip", recordsToSkip.Value);
            }
            int total = (await this.bookManager.GetIndivisualWordsCount(bookName)).Count();
            
            int pageSize = 10; // set your page size, which is number of records per page

            int skip = pageSize * (pageNumber - 1);

            bool canPage = skip < totalItems;
            
            IEnumerable<WordOccurance> wordCount =
                (await this.bookManager.GetIndivisualWordsCount(bookName)).Skip(skip).Take(pageSize);
            return wordCount;
        }

    }
}
