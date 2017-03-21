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
    public class LoyalBooksController : BaseController
    {
        public LoyalBooksController(IServiceProvider services) : base(services)
        {
        }

        public async Task<ActionResult> ShowBookContent(string bookName)
        {
            this.Session.SetString("bookName", bookName);
            LoyalBooksTextViewModel model = new LoyalBooksTextViewModel();

            model.BookName = bookName;

            return this.View(model);
            
        }
    }
}