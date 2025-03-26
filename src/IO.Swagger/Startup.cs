﻿/*
 * Math Calculator API
 *
 * REST API to perform a mathematical operation on two numbers.
 *
 * OpenAPI spec version: 1.0.0
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */
using System;
using System.IO;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using IO.Swagger.Filters;
using IO.Swagger.Security;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Collections.Generic;

namespace IO.Swagger
{
    /// <summary>
    /// Startup
    /// </summary>
    public class Startup
    {
        private readonly IWebHostEnvironment _hostingEnv;
        private IConfiguration Configuration { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="env"></param>
        /// <param name="configuration"></param>
        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            _hostingEnv = env;
            Configuration = configuration;
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)

        {
         

            // Add framework services.
            services
                .AddMvc(options =>
                {
                    options.InputFormatters.RemoveType<Microsoft.AspNetCore.Mvc.Formatters.SystemTextJsonInputFormatter>();
                    options.OutputFormatters.RemoveType<Microsoft.AspNetCore.Mvc.Formatters.SystemTextJsonOutputFormatter>();
                })
                // Add JSON formatters
                .AddNewtonsoftJson(opts =>
                {
                    opts.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    opts.SerializerSettings.Converters.Add(new StringEnumConverter(new CamelCaseNamingStrategy()));
                })
                // Add XML support
                .AddXmlSerializerFormatters();

            // add SecretKey from settings    
            var jwtKey = Encoding.ASCII.GetBytes(Configuration["JwtSettings:SecretKey"]);

            // Add Authentication services and set default authentication schema to Bearer
            services.AddAuthentication("Bearer")
              .AddJwtBearer("Bearer", options =>
              {
                  options.RequireHttpsMetadata = false;
                  options.SaveToken = true;
                  options.TokenValidationParameters = new TokenValidationParameters
                  {
                      ValidateIssuerSigningKey = true,
                      IssuerSigningKey = new SymmetricSecurityKey(jwtKey),
                      ValidateIssuer = false,
                      ValidateAudience = false,
                      ValidateLifetime = true
                  };
              });


            // swagger setup
            services
                .AddSwaggerGen(c =>
                {
                    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer"
                    });

                    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                },
                                Scheme = "Bearer",
                                Name = "Bearer",
                                In = ParameterLocation.Header
                            },
                            new List<string>()
                        }
                    });
    
                    c.SwaggerDoc("1.0.0", new OpenApiInfo
                    {
                        Version = "1.0.0",
                        Title = "Math Calculator API",
                        Description = "Math Calculator API (ASP.NET Core 8.0)",
                        Contact = new OpenApiContact()
                        {
                           Name = "Swagger Codegen Contributors",
                           Url = new Uri("https://github.com/swagger-api/swagger-codegen"),
                           //Email = ""
                        },
                        //TermsOfService = new Uri("")
                    });

                    // Add local server to Swagger document
                    c.AddServer(new OpenApiServer
                    {
                        Url = "http://localhost:50352" 
                    });

                    // Set the comments path for the Swagger JSON and UI.
                    c.CustomSchemaIds(type => type.FullName);
                    // Include XML comments in Swagger
                    c.IncludeXmlComments($"{AppContext.BaseDirectory}{Path.DirectorySeparatorChar}{_hostingEnv.ApplicationName}.xml");
                    // Sets the basePath property in the Swagger document generated
                    c.DocumentFilter<BasePathFilter>("/boazcompany/CalculatorAPI/1.0.0");

                    // Include DataAnnotation attributes on Controller Action parameters as Swagger validation rules (e.g required, pattern, ..)
                    // Use [ValidateModelState] on Actions to actually validate it in C# as well!
                    c.OperationFilter<GeneratePathParamsValidationFilter>();
                });
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="loggerFactory"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            //
            app.UseRouting();

            //TODO: Uncomment this if you need wwwroot folder
            // app.UseStaticFiles();

            // activate authentication and authorization
            app.UseAuthentication();
            app.UseAuthorization();

            // activate swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                //TODO: Either use the SwaggerGen generated Swagger contract (generated from C# classes)
                c.SwaggerEndpoint("/swagger/1.0.0/swagger.json", "Math Calculator API");

                //TODO: Or alternatively use the original Swagger contract that's included in the static files
                // c.SwaggerEndpoint("/swagger-original.json", "Math Calculator API Original");
            });

            //TODO: Use Https Redirection
            // app.UseHttpsRedirection();

            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // handle exceptions according to environment
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //TODO: Enable production exception handling (https://docs.microsoft.com/en-us/aspnet/core/fundamentals/error-handling)
                app.UseExceptionHandler("/Error");

                app.UseHsts();
            }
        }
    }
}
