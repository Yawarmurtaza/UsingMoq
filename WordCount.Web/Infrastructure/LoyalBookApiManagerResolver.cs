using System;
using Microsoft.Extensions.DependencyInjection;
using WordCount.ServiceManagers;
using WordCount.ServiceManagers.Interfaces;

namespace WordCount.Web.Infrastructure
{
    public class LoyalBookApiManagerResolver : IDependencyResolver
    {
        private readonly IServiceProvider services;

        public LoyalBookApiManagerResolver(IServiceProvider services)
        {
            this.services = services;
        }

        public IWebApiManager GetWebApiManagerByName(Type implementingType = null)
        {
            if (implementingType == null)
            {
                return this.services.GetService<LoyalBooksWebApiManager>();
            }

            if (string.Compare(implementingType.Name, "LoyalBooksWebApiParallelManager", StringComparison.CurrentCultureIgnoreCase) == 0)
            {
                return services.GetService<LoyalBooksWebApiParallelManager>();
            }

            return null;
        }
    }
}