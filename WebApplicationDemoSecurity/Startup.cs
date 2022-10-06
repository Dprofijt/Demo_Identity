using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using WebApplicationDemoSecurity.Authorization;

namespace WebApplicationDemoSecurity
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
            // 1. Add Authentication Cookie
            services.AddAuthentication("MyCookieAuth").AddCookie("MyCookieAuth", options =>
            {
                options.Cookie.Name = "MyCookieAuth";
                
                // Set the path to Login & AccessDenied pages
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
            });

            // 2. Add the Authorization Policies 
            services.AddAuthorization(options =>
            {
                options.AddPolicy("MustBelongToHRDepartment",
                    policy => policy.RequireClaim("Department", "HR"));
                
                options.AddPolicy("AdminOnly", 
                    policy => policy.RequireClaim("Admin"));
                options.AddPolicy("HRManagerOnly",
                    policy => policy
                        .RequireClaim("Department", "HR")
                        .RequireClaim("Manager")
                        //8b. add requirment to policy
                        .Requirements.Add(new HRManagerProbationRequirement(3))
                    );

            });
            services.AddSingleton<IAuthorizationHandler, HRManagerProbationRequirementHandler>();

            services.AddRazorPages();
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

            // 3. Add Authentication middleware (So that AddAuthentication in ConfigureServices will be used)
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
