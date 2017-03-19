using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WordCount.ServiceManagers;
using WordCount.ServiceManagers.Interfaces;
using WordCount.Web.ViewModels;

namespace WordCount.Web.Controllers
{
    public class LoyalBooksParallelController : BaseController
    {
        private readonly IWebApiManager bookManager;
        
        public LoyalBooksParallelController(IDependencyResolver serviceResolver, IServiceProvider services) : base(services)
        {
            this.bookManager = serviceResolver.GetWebApiManagerByName(typeof(LoyalBooksWebApiParallelManager));
        }

        public async Task<ActionResult> ShowBookContent(string bookName)
        {
            this.Session.SetString("bookName", bookName);
            LoyalBooksTextViewModel model = new LoyalBooksTextViewModel();

            model.WordCount = await this.bookManager.GetIndivisualWordsCount(bookName);

            return this.View(model);
        }
    }
}