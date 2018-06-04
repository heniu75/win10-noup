using System;
using System.Diagnostics;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Win10NoUp.Library;
using Win10NoUp.Library.Hosts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Win10NoUp.Library.Config;

namespace Win10NoUp.Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IHostingEnvironment Env { get; }
        public IActorSystemHost ActorSystemHost { get; private set; }
        public IContainer ApplicationContainer { get; private set; }

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }

        public IServiceProvider ConfigureServices(IServiceCollection coreServices)
        {
            // configure custom asp.net core 2 service bindings such as mvc, logging and configuration (e.g. options)
            new ConfigureCoreCoreServices().RegisterTypes(coreServices, Configuration);

            // autofac takes over from here ...
            var containerBuilder = new ContainerBuilder();

            // see https://stackoverflow.com/questions/47777992/asp-net-core-2-0-inject-controller-with-autofac
            // see http://autofac.readthedocs.io/en/latest/integration/aspnetcore.html#quick-start-with-configurecontainer and
            // When you do service population, it will include your controller
            // types automatically
            // this MUST be done *after* you've configured your asp net core 2 services, else autofac
            // wont pick these up.
            containerBuilder.Populate(coreServices);

            // configure autofac service bindings
            new ConfigureAutofacServices().RegisterTypes(containerBuilder);

            // If you want to set up a controller for, say, property injection
            // you can override the controller registration after populating services.
            //containerBuilder.RegisterType<MyController>().PropertiesAutowired();

            // ----
            // put all new service configuration here
            // ----

            // ---- 

            // setup the actor system 
            this.ActorSystemHost = new ActorSystemHost<ApplicationManagerActor>(containerBuilder);
            this.ActorSystemHost.Start();

            // by the time we get here, the containerBuilder.Build() would have yielded a container
            this.ApplicationContainer = ActorSystemHost.Container;
            var serviceProvider = new AutofacServiceProvider(this.ApplicationContainer);
            StartupAssertions.DebugAssertIocCorrectlyConfigured(serviceProvider);
            return serviceProvider;
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
