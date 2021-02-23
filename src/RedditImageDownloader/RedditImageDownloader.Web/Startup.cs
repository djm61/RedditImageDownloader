using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using RedditImageDownloader.ApiClient;
using RedditImageDownloader.ApiClient.WebRequest;
using RedditImageDownloader.Web.Helpers;

using System;

namespace RedditImageDownloader.Web
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
            var apiConfiguration = Configuration.GetSection(ConfigLiterals.ApiConnectionKey);
            var host = apiConfiguration[ConfigLiterals.ApiConnectionHostKey] ?? throw new ArgumentNullException("API Host");
            var username = apiConfiguration[ConfigLiterals.ApiConnectionUsernameKey];
            var password = apiConfiguration[ConfigLiterals.ApiConnectionPasswordKey];
            var version = apiConfiguration[ConfigLiterals.ApiConnectionVersionKey] ?? throw new ArgumentNullException("API Version");

            host = $"{host}{version}";
            var httpRequestCriteria = new HttpRequestCriteria
            {
                BaseUri = new Uri(host)
            };

            if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
            {
                httpRequestCriteria.Credentials = new System.Net.NetworkCredential(username, password);
            }

            services.AddScoped<IHttpRequestFactory>(x => new HttpRequestFactory(httpRequestCriteria));
            services.AddScoped<IRedditImageDownloaderService, RedditImageDownloaderService>();

            services.AddControllersWithViews();
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
