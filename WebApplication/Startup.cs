using System;
using System.Text;
using AutoMapper;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using WebApplication.Entities;
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
                    builder => builder.WithOrigins("http://localhost:4200")
                    .AllowAnyHeader().AllowAnyMethod());
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
            services.AddIdentity<User, UserRole>(options => 
            {
                // 密码设置
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
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
            services.AddControllers(config => 
            {
                config.Filters.Add<JsonExceptionFilter>();
            }).AddNewtonsoftJson(setup =>
            {
                setup.SerializerSettings.ContractResolver =
                    new CamelCasePropertyNamesContractResolver();
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

            app.UseRouting();

            app.UseCors("angular");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
