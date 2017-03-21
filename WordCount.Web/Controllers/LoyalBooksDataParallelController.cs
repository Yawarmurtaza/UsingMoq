using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WordCount.Model;
using WordCount.ServiceManagers;
using WordCount.ServiceManagers.Interfaces;
using WordCount.Web.Infrastructure;

namespace WordCount.Web.Controllers
{
    public class LoyalBooksDataParallelController : BaseController
    {
        private readonly IWebApiManager bookManager;
        public LoyalBooksDataParallelController(IDependencyResolver serviceResolver, IServiceProvider services) : base(services)
        {
            this.bookManager = serviceResolver.GetWebApiManagerByName(typeof(LoyalBooksWebApiParallelManager));
        }

        [HttpGet]
        [Route("api/LoyalBooksDataParallel/{pageNumber}/{bookName}")]
        public async Task<IEnumerable<WordOccurance>> NextTenWords(int pageNumber = 1, string bookName = null)
        {
            if (string.IsNullOrEmpty(bookName))
            {
                bookName = this.Session.GetString("bookName");
                if (string.IsNullOrEmpty(bookName))
                {
                    return null;
                }
            }

            IEnumerable<WordOccurance> wordCount = await this.bookManager.GetIndivisualWordsCount(bookName);

            int total = wordCount.Count();

            int wordsToDisplayOnPage = 10; // set your page size, which is number of records per page

            int skip = wordsToDisplayOnPage * (pageNumber - 1);

            skip = skip < total ? skip : total - wordsToDisplayOnPage; // if skip is greater or equal to total then keep displaying the last page..


            wordCount = wordCount.Skip(skip).Take(wordsToDisplayOnPage);
            return wordCount;
        }

    }
}