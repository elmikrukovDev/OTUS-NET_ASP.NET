using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PromoCodeFactory.Core.Abstractions;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.Administration;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.DataAccess.Contexts;
using PromoCodeFactory.DataAccess.Data;
using PromoCodeFactory.DataAccess.Repositories;

namespace PromoCodeFactory.WebHost
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<SqliteContext>(options =>
            {
                options.UseSqlite(_configuration.GetConnectionString("sqlite"))
                    .ConfigureWarnings(w => 
                        w.Ignore(RelationalEventId.PendingModelChangesWarning))
                    .UseLazyLoadingProxies()
                    .EnableSensitiveDataLogging();
            });

            services.AddScoped<IDatabaseInitializer, DatabaseInitializer>();
            services.AddHostedService<DatabaseInitializationService>();

            services.AddScoped<IRepository<PromoCode>, EfRepository<PromoCode>>();
            services.AddScoped<IRepository<Preference>, EfRepository<Preference>>();
            services.AddScoped<IRepository<Employee>, EfRepository<Employee>>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IPreferenceRepository, PreferenceRepository>();

            services.AddControllers();

            services.AddOpenApiDocument(options =>
            {
                options.Title = "PromoCode Factory API Doc";
                options.Version = "1.0";
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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
            });
        }
    }
}