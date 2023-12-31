﻿using long_term_care.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



namespace long_term_care
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


            services.AddDbContext<longtermcareContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("longtermcareContext")));

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = new PathString("/Main/Login");
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(600);
                    options.SlidingExpiration = true;
                    options.AccessDeniedPath = "/Main/Login";
                });

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("管理員", policy =>
                    policy.RequireRole("管理員"));

                options.AddPolicy("社工", policy =>
                    policy.RequireRole("社工"));

                options.AddPolicy("志工", policy =>
                    policy.RequireRole("志工"));

                options.AddPolicy("志工簽到負責人", policy =>
                    policy.RequireRole("志工簽到負責人"));

                options.AddPolicy("課表負責人", policy =>
                    policy.RequireRole("課表負責人"));

                options.AddPolicy("活動設計負責人", policy =>
                    policy.RequireRole("活動設計負責人"));
            });

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


            app.UsePathBase("/LongCare");

            app.UseAuthentication(); //驗證

            app.UseAuthorization();

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                 endpoints.MapControllerRoute(
                     name: "backend",
                     pattern: "backend/{controller}/{action}/{id?}",
                     defaults: new { controller = "Home", action = "Index" }
                );

                 endpoints.MapControllerRoute(
                        name: "frontend",
                        pattern: "{controller}/{action}/{id?}",
                        defaults: new { controller = "Home", action = "Index" }
                 );

                endpoints.MapFallbackToController("Index", "Home");
            });
        }
    }
}
