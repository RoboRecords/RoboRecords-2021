/*
 * Startup.cs: the website main configuration
 * Copyright (C) 2021, Lemin <Leminn> and Refrag <R3FR4G>
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * See the 'LICENSE' file for more details.
 */

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using RoboRecords.DatabaseContexts;
using RoboRecords.Filters;
using RoboRecords.Models;
using RoboRecords.Services;

namespace RoboRecords
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            RoboRecordsDbContext.SetConnectionString(Configuration["TempSqlConnectionString"]);
            IdentityContext.SetConnectionString(Configuration["TempUserConnectionString"]);
            InitDatabase();
        }
        
        private static void InitDatabase()
        {
            // Context is the database reference. 'Using context' means the connection is temporary and will be closed at the end
            using (var context = new RoboRecordsDbContext())
            {
                if (context.Database.EnsureCreated()) // Checks to see if database exists and will create it if not.
                                                      // Returns true if database didn't exist.
                {
                    // Seed database here
                    Seed.InitialData();
                }
                
            }
            
            using (var context = new IdentityContext())
            {
                context.Database.EnsureCreated();
            }
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();

            services.AddDbContext<RoboRecordsDbContext>(ServiceLifetime.Scoped);
            services.AddDbContext<IdentityContext>(ServiceLifetime.Scoped);

            services.AddIdentity<IdentityRoboUser, IdentityRole>().AddEntityFrameworkStores<IdentityContext>();
            services.AddScoped<RoboUserManager>();
            services.AddSingleton<ApiKeyManager>();
            
            services.Configure<IdentityOptions>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters =
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+#";
                options.Password = new PasswordOptions()
                {
                    RequireDigit = true,
                    RequireLowercase = true,
                    RequireUppercase = true,
                    RequireNonAlphanumeric = true,
                    RequiredUniqueChars = 1,
                    RequiredLength = 8,
                };
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromDays(5);
            });
            services.AddHttpContextAccessor();

            services.AddControllers().AddJsonOptions(options =>
            { 
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                EnvVars.IsDevelopment = true;
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            
            EnvVars.ParseEnvironmentVariables(Configuration);
            FileManager.Initialize();
            
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(EnvVars.DataPath)
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapDefaultControllerRoute();
            });
            
            // NOTE: We are logging the initialization here because it needs to be after the EnvVars initialization
            Logger.Log("init");
        }
    }
}
