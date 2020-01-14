//#define NotSQLite
//#define NotSQLite
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Data;
using System;

namespace MvcMovie
{
    public class Startup
    {
        private IWebHostEnvironment HostingEnvironment { get; }

        private IConfiguration Configuration { get; set; }
        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            Configuration = configuration;
            HostingEnvironment = env;
        }

#if NotSQLite
        #region snippet_ConfigureServices
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddDbContext<MvcMovieContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("MvcMovieContext")));
        }
        #endregion
#else
        #region snippet_UseSqlite
        public void ConfigureServices(IServiceCollection services)
        {
            Configuration = new ConfigurationBuilder()
              .SetBasePath(HostingEnvironment.ContentRootPath)
              .AddJsonFile("appsettings_SQLite.json", true, true)
              .AddEnvironmentVariables()
              .Build();
            services.AddControllersWithViews();

            services.AddDbContext<MvcMovieContext>(options =>
                    options.UseSqlite(Configuration.GetConnectionString("MvcMovieContext")));
        }
        #endregion
#endif

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            #region snippet_1
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            #endregion
#if NotSQLite
            Console.WriteLine("use NotSQLLite");
#else
            Console.WriteLine("Not use NotSQLLite");
#endif

        }
    }
}
