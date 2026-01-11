using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.OpenApi;
using P72.Api.Common.Configuration;
using P72.Api.Trading.DataAccess.Repository;
using P72.Api.Trading.Extensions;
//using P72.Api.Trading.Filters;
using P72.Api.Trading.Middleware;
using P72.Api.Trading.Orchestrator;
//using Microsoft.OpenApi.Models;
using System.Diagnostics.CodeAnalysis;
using static P72.API.Trading.Filters.TradeRequestValidatorAttribute;

namespace P72.API.Trading
{
    public class Startup
    {
        private const string AllowAllCors = "AllowAll";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public IConfiguration? Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(AllowAllCors,
                                  builder =>
                                  {
                                      builder.AllowAnyHeader();
                                      builder.AllowAnyMethod();
                                      builder.SetIsOriginAllowed(origin => true);
                                  });
            });

            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "P72 Trading API",
                    Description = "Trading API endpoints"
                });
            });
            services.AddOptions();

            // Add services to the container.
            services.AddMemoryCache();
            //services.AddSingleton<TradeValidatorFilter>();
           
            services.AddTransient<IConfigManager, ConfigManager>();
            services.AddTransient<ITradeDetails, TradeDetails>();
            services.AddTransient<ITradeRepository, TradeRepository>();

            services.AddControllers();
            services.AddHttpContextAccessor();services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddHttpClient();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        [ExcludeFromCodeCoverage]
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //app.UseMiddleware<AuthenticationMiddleware>();
            app.UseHttpsRedirection();
            app.UseRequestResponseLogging();
            app.UseCustomExceptionMiddleware();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "P72 Trading API v1");
            });
            app.UseSwagger();
        }
    }
}
