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

        [HttpGet]
        public async Task<IEnumerable<WordOccurance>> NextTenWords(string bookName = null, int numberOfRecords = 10)
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
            
            IEnumerable<WordOccurance> wordCount =
                (await this.bookManager.GetIndivisualWordsCount(bookName)).Skip(recordsToSkip.Value).Take(numberOfRecords);
            return wordCount;
        }

    }
}