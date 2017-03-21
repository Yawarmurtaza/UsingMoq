using System;
using WordCount.ServiceManagers.Interfaces;

namespace WordCount.Web.Infrastructure
{
    public interface IDependencyResolver
    {
        IWebApiManager GetWebApiManagerByName(Type implementingType = null);
    }
}