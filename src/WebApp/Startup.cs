using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApp.Code;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;

namespace WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();

            services.AddSingleton<MosaicStoreManager>();

            services.AddSignalR()
                    .AddAzureSignalR(options =>
                    {
                        options.ConnectionCount = 15;
                        options.AccessTokenLifetime = TimeSpan.FromDays(1);
                        //options.ClaimsProvider = context => context.User.Claims;
                    });
                    //.AddMessagePackProtocol();
                    

            

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            //app.UseSignalR(routes =>
            app.UseAzureSignalR(routes =>
            {
                routes.MapHub<WebApp.Code.Action>("/connector/action");
            });

        }
    }
}
