using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using MaineCoonApi.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace MaineCoonApi {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddRazorPages();
            
            services.AddRazorPages().AddRazorPagesOptions(options => {
                //options.Conventions.AuthorizeFolder("/");
                foreach (var role in Enum.GetValues(typeof(Models.User.role))) {
                    //Set Auth Role for every folder
                    options.Conventions.AuthorizeFolder("/" + role.ToString(), "Is" + role.ToString());
                }
                options.Conventions.AllowAnonymousToPage("/Index");
                options.Conventions.AllowAnonymousToPage("/Signin");
                options.Conventions.AllowAnonymousToPage("/Signup");
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => {
                    options.LoginPath = "/Signin";
                    options.LogoutPath = "/Account/LogOff";
                    options.ExpireTimeSpan = TimeSpan.FromSeconds(60);
                    options.SlidingExpiration = true;
                    options.AccessDeniedPath = "/Signin";
                });

            //Add role Requirements
            services.AddAuthorization(options => {
                foreach (var role in Enum.GetValues(typeof(Models.User.role))) {
                    options.AddPolicy("Is" + role.ToString(), policy =>
                    policy.RequireClaim(ClaimTypes.Role, role.ToString()));

                }
            });

            services.AddCors();

            services.AddDbContext<MaineCoonApiContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("DatabaseConnectionString")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }
            else {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseCors(options => {
                options.WithOrigins("http://localhost:11117").SetIsOriginAllowedToAllowWildcardSubdomains().AllowCredentials();
                options.AllowAnyMethod();
                options.AllowAnyHeader();
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            //app.UseAuthentication();
            //app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}
