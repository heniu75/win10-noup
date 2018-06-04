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

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // bootstrap custom IOC and configure library services
            SetConfigurationBindings(services);

            // logging -- see https://stackoverflow.com/questions/45781873/is-net-core-2-0-logging-broken
            // also see https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/?view=aspnetcore-2.1&tabs=aspnetcore2x
            services.AddLogging(builder =>
            {
                builder.AddConfiguration(Configuration.GetSection("Logging"))
                    .AddConsole()
                    .AddDebug();
            });

            // Add controllers as services so they'll be resolved.
            services.AddMvc().AddControllersAsServices();


            // configure custom asp.net core 2 service bindings
            new ConfigureCoreCoreServices().RegisterTypes(services);

            // autofac takes over from here ...
            var containerBuilder = new ContainerBuilder();

            // see https://stackoverflow.com/questions/47777992/asp-net-core-2-0-inject-controller-with-autofac
            // see http://autofac.readthedocs.io/en/latest/integration/aspnetcore.html#quick-start-with-configurecontainer and
            // When you do service population, it will include your controller
            // types automatically
            // this MUST be done *after* you've configured your asp net core 2 services, else autofac
            // wont pick these up.
            containerBuilder.Populate(services);

            // configure autofac service bindings
            new ConfigureAutofacServices().RegisterTypes(containerBuilder);

            // If you want to set up a controller for, say, property injection
            // you can override the controller registration after populating services.
            //containerBuilder.RegisterType<MyController>().PropertiesAutowired();

            this.ActorSystemHost = new ActorSystemHost<StopServiceActor>(containerBuilder);
            this.ActorSystemHost.Start();

            // by the time we get get, the containerBuilder.Build() would have yielded a container
            this.ApplicationContainer = ActorSystemHost.Container;
            var serviceProvider = new AutofacServiceProvider(this.ApplicationContainer);
            StartupAssertions.DebugAssertIocCorrectlyConfigured(serviceProvider);
            return serviceProvider;
        }
        

        private void SetConfigurationBindings(IServiceCollection services)
        {
            // I cant find the bloody extension method for encapsulating into a class into the library cs project, so keep the configuration stuff here
            services.AddOptions();
            services.Configure<ApplicationConfig>(Configuration.GetSection("ApplicationConfig"));
            services.Configure<StopServiceActorConfiguration>(Configuration.GetSection("StopServiceActorConfiguration"));
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
