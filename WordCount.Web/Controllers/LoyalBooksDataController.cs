using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WordCount.Model;
using WordCount.ServiceManagers.Interfaces;
using WordCount.Web.Infrastructure;

namespace WordCount.Web.Controllers
{
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
<<<<<<< HEAD
        public async Task<IEnumerable<WordOccurance>> NextTenWords(int pageNumber = 1, string bookName = null)
=======
        public async Task<IEnumerable<WordOccurance>> NextTenWords(int pageNumber = 10, string bookName = null)
>>>>>>> 9f91daef4043959a18bbc3ff0929df4697ace9a0
        {
            if (string.IsNullOrEmpty(bookName))
            {
                bookName = this.Session.GetString("bookName");
                if (string.IsNullOrEmpty(bookName))
                {
                    return null;
                }
            }

<<<<<<< HEAD
            IEnumerable<WordOccurance> wordCount = await this.bookManager.GetIndivisualWordsCount(bookName);

            int total = wordCount.Count();

            int wordsToDisplayOnPage = 10; // set your page size, which is number of records per page

            int skip = wordsToDisplayOnPage * (pageNumber - 1);

            skip = skip < total ? skip : total - wordsToDisplayOnPage; // if skip is greater or equal to total then keep displaying the last page..
            

            wordCount = wordCount.Skip(skip).Take(wordsToDisplayOnPage);
=======
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
>>>>>>> 9f91daef4043959a18bbc3ff0929df4697ace9a0
            return wordCount;
        }
    }
}
