using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CoreServer.Api.Extensions;
using CoreServer.Common;
using CoreServer.Common.Configuration;
using CoreServer.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Savorboard.CAP.InMemoryMessageQueue;

namespace CoreServer.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            AppConfig.Configuration = configuration;
        }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            foreach (var serviceType in typeof(BaseService).Assembly.GetTypes())
            {
                if (serviceType.GetCustomAttribute(typeof(AutoServiceAttribute)) is AutoServiceAttribute
                    serviceAttribute)
                {
                    services.AddSingleton(serviceAttribute.Interface, serviceType);
                }
            }
            
            services.AddCap(config => { config.UseInMemoryMessageQueue(); });
            
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.ConfigurationExceptionHanlder();
            
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}