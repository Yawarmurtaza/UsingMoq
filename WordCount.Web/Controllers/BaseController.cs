using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace WordCount.Web.Controllers
{
    public class BaseController : Controller
    {
        public ISession Session { get; set; }   
        public BaseController(IServiceProvider services)
        {
            this.Session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
        }
    }
}