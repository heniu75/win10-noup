using System;
using Win10NoUp.Library;
using Win10NoUp.Library.Hosts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Win10NoUp.Library.Config;

namespace Win10NoUp.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }

       // public IServiceProvider ServiceProvider { get; }
        public IConfiguration Configuration { get; }

        public IHostingEnvironment Env { get; }

        //public IServiceProviderFactory Factory { get; set;  }
        public WebApplicationHost Host { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // bootstrap custom IOC and configure library services
            services.AddOptions();
            services.Configure<ApplicationConfig>(Configuration);

            var factory = new CoreServiceProviderFactory(services, new [] { new ConfigureCoreServices() });
            var host = new WebApplicationHost(factory);
            host.Run();

            var hostingEnv = factory.ServiceProvider.GetService<IHostingEnvironment>();
            var actorSystemHost = factory.ServiceProvider.GetService<IActorSystemHost>();
            //var actorSystemHost1 = services.GetService<IActorSystemHost>();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true
                });
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

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });
        }
    }
}
