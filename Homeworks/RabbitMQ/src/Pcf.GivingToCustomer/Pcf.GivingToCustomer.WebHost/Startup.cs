using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pcf.GivingToCustomer.Core;
using Pcf.GivingToCustomer.Core.Abstractions.Consumers;
using Pcf.GivingToCustomer.Core.Abstractions.Gateways;
using Pcf.GivingToCustomer.Core.Abstractions.Repositories;
using Pcf.GivingToCustomer.Core.Abstractions.Services;
using Pcf.GivingToCustomer.DataAccess;
using Pcf.GivingToCustomer.DataAccess.Data;
using Pcf.GivingToCustomer.DataAccess.Repositories;
using Pcf.GivingToCustomer.Integration;
using Pcf.GivingToCustomer.Integration.grpc;
using Pcf.GivingToCustomer.Integration.Messaging;
using RabbitMQ.Client;
using SurveyManageService.Infrastructure.Messaging;
using System;
using System.Threading.Tasks;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace Pcf.GivingToCustomer.WebHost
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddMvcOptions(x =>
                x.SuppressAsyncSuffixInActionNames = false);
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<INotificationGateway, NotificationGateway>();
            services.AddScoped<IDbInitializer, EfDbInitializer>();
            services.AddScoped<IPromoCodeService, PromoCodeService>();
            services.AddDbContext<DataContext>(x =>
            {
                //x.UseSqlite("Filename=PromocodeFactoryGivingToCustomerDb.sqlite");
                x.UseNpgsql(Configuration.GetConnectionString("PromocodeFactoryGivingToCustomerDb"));
                x.UseSnakeCaseNamingConvention();
                x.UseLazyLoadingProxies();
            });

            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            services.AddGrpc();
            services.AddOpenApiDocument(options =>
            {
                options.Title = "PromoCode Factory Giving To Customer API Doc";
                options.Version = "1.0";
            });

            var rabbitConfig = Configuration.GetSection("RabbitMQ");

            services.AddSingleton(async sp =>
            {
                var factory = new ConnectionFactory
                {
                    HostName = rabbitConfig["HostName"],
                    Port = int.Parse(rabbitConfig["Port"]!),
                    UserName = rabbitConfig["UserName"],
                    Password = rabbitConfig["Password"]
                };
                return await factory.CreateConnectionAsync();
            });

            services.AddSingleton(async sp =>
            {
                var connection = await sp.GetRequiredService<Task<IConnection>>();
                var channel = await connection.CreateChannelAsync();
                return channel;
            });
            services.AddSingleton<IEventConsumer, RabbitMqEventConsumer>();
            services.AddScoped<IGivingPromoCodeToCustomerEventConsumer, RabbitMqGivingPromoCodeToCustomerEventConsumer>();
            services.AddHostedService<PromoCodeEventsBackgroundService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IDbInitializer dbInitializer)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseOpenApi();
            app.UseSwaggerUi(x =>
            {
                x.DocExpansion = "list";
            });

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGrpcService<CustomerGrpcService>();
            });

            dbInitializer.InitializeDb();
        }
    }
}