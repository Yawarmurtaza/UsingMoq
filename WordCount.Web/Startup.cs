using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WordCount.ServiceManagers;
using WordCount.ServiceManagers.Interfaces;

namespace WordCount.Web
{

    public interface IDependencyResolver
    {
        IWebApiManager GetWebApiManagerByName(Type implementingType = null);
    }

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

    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IDependencyResolver, LoyalBookApiManagerResolver>();
            services.AddTransient<IHttpClientWrapper, HttpClientWrapper>();

            services.AddTransient<LoyalBooksWebApiManager>();
            services.AddTransient<LoyalBooksWebApiParallelManager>();

            services.AddTransient<IWebApiProcessor, WebApiProcessor>();

            // Add framework services.
            services.AddSession();

            services.AddMemoryCache();
            services.AddMvc();
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            app.UseSession();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
