using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Blog.Api.Entities;
using Blog.Api.Filters;
using Blog.Api.Infrastructure.PropertyMapping;
using Blog.Api.Infrastructure.Repository;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;

namespace Blog.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(config =>
            {
                config.Filters.Add<JsonExceptionFilter>();
            }).AddNewtonsoftJson(setup =>
            {
                setup.SerializerSettings.ContractResolver =
                    new CamelCasePropertyNamesContractResolver();
                setup.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Local;
            });
            services.AddAuthorization();
            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme,
                options =>
                {
                    options.Authority = "https://localhost:5001";
                    options.ApiName = "api";
                    options.ApiSecret = "0ed8ca6e-6dcf-4d46-b38f-dfc784e362cd";
                });

            services.AddCors(options =>
            {
                options.AddPolicy(
                    "Angular",
                    builder =>
                    {
                        builder.WithOrigins(new string[]
                        {
                            "http://localhost:4200",
                        });
                        builder.AllowAnyHeader();
                        builder.AllowAnyMethod();
                        builder.AllowCredentials();
                    });
            });
            services.AddDistributedRedisCache(options =>
            {
                options.Configuration = Configuration["ConnectionRedis:Connection"];
                options.InstanceName = Configuration["ConnectionRedis:InstanceName"];
            });
            services.AddSwaggerGen(config => {
                config.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Blog API",
                    Version = "v1"
                });
            });
            services.AddDbContext<BlogDbContext>(options =>
            {
                options.UseMySql(Configuration.GetConnectionString("MySQLConnection"));
            });
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

            app.UseAuthentication();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("Angular");

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
