using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BlazorUI.Data;
using Infrastructure.EFCore.MSI;
using Infrastructure.EFCore.Data;
using Infrastructure.EFCore;
using ApplicationCore.SharedKernel.Abstractions;
using Microsoft.Extensions.Caching.Memory;

namespace BlazorUI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddMemoryCache();
            services.AddSingleton<IAzureSqlTokenProvider, AzureSqlTokenProviderCacheWrapper>(sp =>
            {
                var tokenProvider = new AzureIdentityAzureSqlTokenProvider();
                var cache = sp.GetService<IMemoryCache>();
                return new AzureSqlTokenProviderCacheWrapper(tokenProvider, cache);
            });
            //services.AddSingleton<IAzureSqlTokenProvider, AzureIdentityAzureSqlTokenProvider>();
            //services.Decorate<IAzureSqlTokenProvider, AzureSqlTokenProviderCacheWrapper>();
            //services.AddSingleton<AzureAdAuthenticationDbConnectionInterceptor>();

            services//.AddSingleton(sp => Configuration.GetSection("DbContextOptions").Get<DbContextOptions>())
                  .AddSingleton<IDbContext, AzureAdDbContextOptions>(sp=> 
                  {
                      var connectionString = Configuration["DbContextOptions:ConnectionString"];
                      var tokenProvider = sp.GetService<IAzureSqlTokenProvider>();
                      var interceptor = new AzureAdAuthenticationDbConnectionInterceptor(tokenProvider);
                      //var interceptor = sp.GetService<AzureAdAuthenticationDbConnectionInterceptor>();

                      return new AzureAdDbContextOptions(connectionString, interceptor);
                  })
                  .AddSingleton<DbContext>();


            services.AddSingleton(typeof(IAsyncReadRepository<>), typeof(EfReadRepository<>)); //or scoped? Check plz
            services.AddSingleton(typeof(IAsyncRepository<>), typeof(EfRepository<>));





            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddSingleton<WeatherForecastService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
