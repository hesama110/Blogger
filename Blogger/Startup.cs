using Blogger.Context;
using Blogger.Model;
using Blogger.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using ResponseMessageWrapper.Core.Extensions;
using Services;
using System;
using ExtensionsAttributes.Middleware.LogRequest;
using ExtensionsAttributes.Middleware.LogResponse;
using ExtensionsAttributes.Middleware.Logging;

namespace Blogger
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
           

            services.AddControllersWithViews();

            services.AddDbContext<IBloggerContext, BloggerContext>(options =>
                 options.UseSqlServer(Configuration.GetConnectionString("BloggerContextConnection")));
            /* services.AddDbContext<LogDbContext>(options =>
                 options.UseSqlServer(Configuration["database:logConnectionString"]));*/

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder/*.AllowAnyOrigin()*/
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            services.AddScoped(typeof(IBaseServices<>), typeof(BaseServices<>));

            //services.AddScoped<IBloggerContext, BloggerContext>();
            //services.AddScoped<ILogDbContext, LogDbContext>(
            services.AddScoped<ICategoryServices, CategoryServices>(
                _ => new CategoryServices(
                    new BaseServices<Category>(_.GetService<IBloggerContext>())
                )
            );
            services.AddScoped<ITagServices, TagServices>(
               _ => new TagServices(
                   new BaseServices<Tag>(_.GetService<IBloggerContext>())
               )
           );
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "BloggerWorkspace/dist/BloggerApp";
            });

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc(Configuration["SwaggerDocSettings:Version"], new OpenApiInfo
                {
                    Version = Configuration["SwaggerDocSettings:Version"],
                    Title = Configuration["SwaggerDocSettings:Title"],
                    Description = Configuration["SwaggerDocSettings:Description"],
                    TermsOfService = new Uri(Configuration["SwaggerDocSettings:TermsOfService"]),
                    Contact = new OpenApiContact
                    {
                        Name = Configuration["SwaggerDocSettings:ContactInfo:Name"],
                        Email = Configuration["SwaggerDocSettings:ContactInfo:Email"],
                        Url = new Uri(Configuration["SwaggerDocSettings:ContactInfo:Url"]),
                    },
                    License = new OpenApiLicense
                    {
                        Name = Configuration["SwaggerDocSettings:License:Name"],
                        Url = new Uri(Configuration["SwaggerDocSettings:License:Url"])
                    }
                });

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                var ver = Configuration["SwaggerDocSettings:Version"] ?? "v0";
                var title = Configuration["SwaggerDocSettings:Title"] ?? "Blogger API";
                c.SwaggerEndpoint($"/swagger/{ver}/swagger.json", $"{ver} {title}");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseLogRequest();
                app.UseLogResponse();
                
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAPIResponseWrapperMiddleware();

            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "BloggerApi",
                    pattern: "blog-api/{*article}",
                    defaults: new { controller = "Blogger", action = "Article" });

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });
            app.UseCors("CorsPolicy");

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "BlogWorkspace";
                spa.Options.StartupTimeout = new TimeSpan(0, 0, 180);
                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "serve:Blogger-app-net");
                   //spa.UseAngularCliServer(npmScript: "start-all");
                }
            });
        }
    }
}
