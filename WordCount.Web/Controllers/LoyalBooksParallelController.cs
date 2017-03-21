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
        public LoyalBooksParallelController(IServiceProvider services) : base(services)
        {
        }

        public ActionResult ShowBookContent(string bookName)
        {
            this.Session.SetString("bookName", bookName);
            this.ViewBag.OperationName = "Parallel";

            LoyalBooksTextViewModel model = new LoyalBooksTextViewModel();
            model.OperationName = "| Parallel";
            model.BookName = bookName;
            return this.View(model);
            
        }
    }
}