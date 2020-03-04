using AutoMapper;
using Crossroads.Microservice.Logging;
using Crossroads.Web.Common.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.IO;
using System.Reflection;
using Crossroads.Service.Contact.Services.Contacts;
using MinistryPlatform.Contacts;
using ExternalSync.Hubspot;
using ExternalSync.TokenService;

namespace Crossroads.Service.Contact
{
    public class Startup
    {
        public Startup(IHostEnvironment env)
        {
            // load environment variables from .env for local development
            try
            {
                DotNetEnv.Env.Load("../.env");
            }
            catch
            {
                // no .env file present but since not required, just write
                Console.WriteLine("no .env file found, reading environment variables from machine");
            }

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("./appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"./appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options => options.EnableEndpointRouting = false)
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddNewtonsoftJson(options => options.UseMemberCasing());
            services.AddAutoMapper(typeof(Startup));
            services.AddDistributedMemoryCache();
            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddCors();

            // set up logging
            Logger.SetUpLogging(Environment.GetEnvironmentVariable("LOGZ_IO_KEY"),
                Environment.GetEnvironmentVariable("CRDS_ENV"));

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Contacts",
                    Description = "Microservice for Contacts"
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                // JWT-token authentication by bearer token
                var securityScheme = new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "bearer",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                };
                c.AddSecurityDefinition("Bearer", securityScheme);
            });

            // Dependency Injection
            CrossroadsWebCommonConfig.Register(services);

            // Service Layer
            services.AddSingleton<IContactService, ContactService>();

            // Repo Layer
            services.AddSingleton<IContactRepository, ContactRepository>();

            // External Sync Layer
            services.AddSingleton<IHubspotClient, HubspotClient>();
            services.AddSingleton<ITokenService, TokenService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseRouting();
            
            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Event Check-In API V1");
                c.RoutePrefix = string.Empty;
                c.DocExpansion(DocExpansion.None);
            });

            app.UseMvc();
        }
    }
}
