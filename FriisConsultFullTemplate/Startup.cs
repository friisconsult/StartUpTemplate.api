using System;
using System.IO;
using FriisConsultFullTemplate.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Swagger;

namespace FriisConsultFullTemplate
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // connect to the database, for staging and development, make sure to update the appsettings.json, 
            // the MS_TableConnectionString is the default name for deploying to Azure, 
            // otherwise this can also be updated in the application.json
            services.AddDbContextPool<DatabaseContext>(options =>
            {
                if (Environment.IsStaging())
                {
                    options.UseInMemoryDatabase(Configuration.GetConnectionString("stating_database"));
                }
                else if (Environment.IsDevelopment())
                {
                    options.UseSqlite(Configuration.GetConnectionString("development_database"));
                }
                else if (Environment.IsProduction())
                {
                    options.UseSqlServer(Configuration.GetConnectionString("MS_TableConnectionString"));
                }
            });

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            // update your application insights key or delete this if you don't want application insight
            // using Azure hosting
            services.AddApplicationInsightsTelemetry();

            services.AddSwaggerGen(options =>
            {
                options.DescribeAllEnumsAsStrings();
                options.AddSecurityDefinition("Bearer", new ApiKeyScheme()
                {
                    In = "header",
                    Description = "Insert JWT api token, into Authorization field",
                    Name = "Authorization",
                    Type = "apiKey"
                });

                options.SwaggerDoc("v1", new Info
                {
                    Title = " - Api",
                    Version = "1.0",
                    Description = "This is a private API, and may not be use by other" +
                        "Than the owner ",
                    TermsOfService = "This is aprivate API",
                    Contact = new Contact
                    {
                        Name = "company name",
                        Email = "support@company.com",
                        Url = "https://company.com"
                    },
                    License = new License
                    {
                        Name = "No rights to use by anyone else than me",
                        Url = "https://api.company.com"
                    }
                });

                var basePath = AppContext.BaseDirectory;
                var xmlPath = Path.Combine(basePath, "FriisConsultFullTemplate.xml");
                options.IncludeXmlComments(xmlPath);
            });

            // 1. Add Authentication Services
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = Configuration.GetSection("auth0").GetValue<string>("authority");
                options.Audience = Configuration.GetSection("auth0").GetValue<string>("audience");
            });

            services.AddMvc()
                    .AddJsonOptions(options =>
                    {
                        options.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc;
                        options.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    })
                    .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "project name api v1");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
