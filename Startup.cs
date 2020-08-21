using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StackExchange.Redis.Extensions.Core.Configuration;
using StackExchange.Redis.Extensions.Newtonsoft;

namespace RedisClient
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
            RedisConfiguration redisConfig = new RedisConfiguration()
            {
                ConnectTimeout = int.Parse(Environment.GetEnvironmentVariable("REDIS_TIMEOUT")),
                Ssl = bool.Parse(Environment.GetEnvironmentVariable("REDIS_SSL")),
                Database = int.Parse(Environment.GetEnvironmentVariable("REDIS_DB")),
                Hosts = new RedisHost[] { new RedisHost() { Host = Environment.GetEnvironmentVariable("REDIS_HOST"), Port = int.Parse(Environment.GetEnvironmentVariable("REDIS_PORT")) } }
            };
            var redisConfiguration = redisConfig;
            services.AddControllers();
            services.AddStackExchangeRedisExtensions<NewtonsoftSerializer>(redisConfiguration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
