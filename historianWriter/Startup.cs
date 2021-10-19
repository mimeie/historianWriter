using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using NLog;
using NLog.Targets.ElasticSearch;

namespace historianWriter
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

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "historianWriter", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "historianWriter v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //probeweise loggen
            var config = new NLog.Config.LoggingConfiguration();
            var logelastic = new ElasticSearchTarget
            {
                Name = "elastic",
                Uri = "http://jhistorian.mei.local:9200/",  //Uri = "http://192.168.2.41:32120", 
                Index = "historianwriter",  //"app-${level}-${date:format=yyyy.MM.dd}"
                Layout = "${logger} | ${threadid} | ${message}",
                IncludeAllProperties = true,
            };
            // Rules for mapping loggers to targets
            config.AddRule(NLog.LogLevel.Info, NLog.LogLevel.Fatal, logelastic);

            // Apply config
            NLog.LogManager.Configuration = config;
            var logger = NLog.LogManager.GetCurrentClassLogger();


            logger.Info($"historianWriter gestartet");
            //logger.WithProperty("LoggingIdxProp", 85).WithProperty("LogginTextProp", "helloProperty").Info($"start parallel mit Ifrit Funktion");
        }

        

    }
}
