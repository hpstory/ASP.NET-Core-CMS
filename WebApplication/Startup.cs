using System;
using System.IO;
using System.Text;
using AutoMapper;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using WebApplication.Entities;
using WebApplication.Entities.Identity.Entities;
using WebApplication.Filters;
using WebApplication.Infrastructure;
using WebApplication.Infrastructure.PropertyMapping;

namespace WebApplication
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
            var tokenSection = Configuration.GetSection("Security:Token");
            services.AddHsts(options => 
            {
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(120);
                options.Preload = true;
            });
            services.AddCors(options =>
            {
                options.AddPolicy(
                    "angular",
                    builder =>
                    {
                        builder.WithOrigins(new string[] { "http://localhost:4200" });
                        builder.AllowAnyHeader().WithExposedHeaders("X-Pagination");
                        builder.WithMethods("GET", "POST", "PUT", "DELETE");
                        builder.AllowCredentials();
                        builder.SetPreflightMaxAge(TimeSpan.FromSeconds(60));
                    });
            });
            services.AddDistributedRedisCache(options =>
            {
                options.Configuration = Configuration["ConnectionRedis:Connection"];
                options.InstanceName = Configuration["ConnectionRedis:InstanceName"];
            });
            services.AddHttpCacheHeaders(
                expires =>
                {
                    expires.MaxAge = 60;
                    expires.CacheLocation = CacheLocation.Public;
                },
                validate =>
                {
                    validate.MustRevalidate = true;
                });
            services.AddResponseCaching();
            services.AddDataProtection();
            services.AddIdentity<User, IdentityRole>(options => 
            {
                // 密码设置
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;

                // 锁定设置
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // 用户设置
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_@+";
                options.User.RequireUniqueEmail = false;
            }).AddEntityFrameworkStores<CMSDbContext>();
            services.AddAuthentication(options => 
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options => 
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = tokenSection["Issuer"],
                    ValidAudience = tokenSection["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSection["key"])),
                    ClockSkew = TimeSpan.Zero
                };
            });
            services.AddSwaggerGen(config => {
                config.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "CMS API",
                    Version = "v1"
                });
            });
            services.AddControllers(config => 
            {
                config.Filters.Add<JsonExceptionFilter>();
            }).AddNewtonsoftJson(setup =>
            {
                setup.SerializerSettings.ContractResolver =
                    new CamelCasePropertyNamesContractResolver();
                setup.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Local;
            });
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(Convert.ToDouble(Configuration["ConnectionRedis:SessionTimeOut"]));
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            services.AddDbContext<CMSDbContext>(options =>
            {
                options.UseMySql(Configuration.GetConnectionString("MySQLConnection"));
            });
            // In production, the Angular files will be served from this directory
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
            services.AddTransient<IPropertyMappingService, PropertyMappingService>();
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
                app.UseHsts();
            }

            app.UseResponseCaching();

            app.UseHttpCacheHeaders();

            app.UseHttpsRedirection();

            app.UseSession();

            app.UseRouting();

            app.UseCors("angular");

            app.UseAuthorization();

            app.UseSwagger();

            app.UseSwaggerUI(config => {
                config.SwaggerEndpoint("v1/swagger.json", "CMS API v1");
            });

            app.UseStaticFiles();

            app.UseFileServer(new FileServerOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "UploadFile")),
                RequestPath = "/staticFiles",
                EnableDirectoryBrowsing = true
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
