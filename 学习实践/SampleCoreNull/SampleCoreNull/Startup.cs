using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace SampleCoreNull
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }
        public Startup()
        {
            var builder = new ConfigurationBuilder().AddJsonFile("AppSettings.json");
            Configuration = builder.Build();
        }

        // ����ʱ�����ô˷����� ʹ�ô˷�����������ӵ�������
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(option => option.EnableEndpointRouting = false);
            services.AddEntityFrameworkSqlite()
                .AddDbContext<CoreDbContext>(options => options.UseSqlite(Configuration["database:sqlite"]));
        }

        // ����ʱ�����ô˷�����ʹ�ø÷��������� HTTP ����ܵ���
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseDefaultFiles();
            //app.UseStaticFiles();

            app.UseFileServer();
            //app.UseMvcWithDefaultRoute();

            app.UseMvc(ConfigureRoutes);

            //app.UseRouting();

            //app.UseEndpoints(endpoints =>
            //{
            //    //endpoints.MapGet("/", async context =>
            //    //{
            //    //    var req = context.Request;
            //    //    await context.Response.WriteAsync("Endpoints Context:" + $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}{context.Request.QueryString.ToUriComponent()}");
            //    //});
            //});

            app.Run(async (context) =>
            {
                //throw new System.Exception("Throw Exception");

                var req = context.Request;
                await context.Response.WriteAsync("Run Context:" + $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}{context.Request.QueryString.ToUriComponent()}");
            });

            //app.UseWelcomePage();
        }

        private void ConfigureRoutes(IRouteBuilder routeBuilder)
        {
            routeBuilder.MapRoute("Default", "{controller=Home}/{action=Index}/{id?}");
            //routeBuilder.MapRoute("Default", "{controller}/{action}/{id?}", new { controller = "Home", action = "Index" });

            AppSettings.SetAppSetting(Configuration.GetSection("database"));
        }
    }

    public class AppSettings
    {
        private static IConfigurationSection appSections = null;

        public static string AppSetting(string key)
        {
            string str = "";
            if (appSections.GetSection(key) != null)
            {
                str = appSections.GetSection(key).Value;
            }
            return str;
        }

        public static void SetAppSetting(IConfigurationSection section)
        {
            appSections = section;
        }
    }
}
