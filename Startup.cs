using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoDB.Driver.Core.Extensions.DiagnosticSources;
using MongoDB.Driver.Core.Extensions.OpenTelemetry;
using OpenTelemetry.Trace;

namespace sample
{
    public class Startup
    {
        public static MongoClient MongoClient;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var runner = Mongo2Go.MongoDbRunner.Start();
            var settings = MongoClientSettings.FromConnectionString(runner.ConnectionString);
            settings.ClusterConfigurator = cb => cb.Subscribe(new DiagnosticsActivityEventSubscriber());
            
            MongoClient = new MongoClient(settings);

            services.AddControllers();
            services.AddOpenTelemetry(builder => {
                    builder
                        .AddAspNetCoreInstrumentation()
                        .AddMongoDBInstrumentation()
                        .UseConsoleExporter();
            });
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
