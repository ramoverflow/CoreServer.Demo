using System.Reflection;
using CoreServer.Api.Extensions;
using CoreServer.Common.Configuration;
using CoreServer.Common.Core;
using CoreServer.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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

            services.AddCap(config =>
            {
                config.UseRabbitMQ(rabbitMqOptions =>
                {
                    rabbitMqOptions.UserName = "test";
                    rabbitMqOptions.Password = "test";
                    rabbitMqOptions.HostName = "192.168.253.130";
                });
                config.UseMySql("server=192.168.253.130;port=3306;user=rabbitmq;database=cap;password=123;");
            });

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