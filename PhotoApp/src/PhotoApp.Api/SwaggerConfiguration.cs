using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace PhotoApp.Api
{
    public static class SwaggerConfiguration
    {
        public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddVersionedApiExplorer(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
            });

            services.AddSwaggerGen(options =>
            {
                options.EnableAnnotations();

                IApiVersionDescriptionProvider apiVersionDescriptionProvider = services
                .BuildServiceProvider()
                .GetRequiredService<IApiVersionDescriptionProvider>();

                foreach(ApiVersionDescription description in apiVersionDescriptionProvider.ApiVersionDescriptions)
                {
                    options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
                }

               /* options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Photo App",
                    Description = "An ASP.NET Core Web API for managing or share your photos",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Example Contact",
                        Url = new Uri("https://example.com/contact")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Example License",
                        Url = new Uri("https://example.com/license")
                    }
                });*/

                // using System.Reflection;
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
                options.CustomOperationIds(e => $"{e.ActionDescriptor.RouteValues["controller"]}_{e.ActionDescriptor.RouteValues["action"]}");
            });
            return services;
        }

        public static IApplicationBuilder ConfigureSwagger(this IApplicationBuilder applicationBuilder, IApiVersionDescriptionProvider apiVersionDescriptionProvider)
        {
            if (applicationBuilder == null)
            {
                throw new ArgumentNullException(nameof(applicationBuilder));
            }

            applicationBuilder.UseSwagger(
                c => c.RouteTemplate = $"swagger/{ApiConstants.ServiceName}/{{documentName}}/swagger.json"
            );

            /*applicationBuilder.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1"));*/

            applicationBuilder.UseSwaggerUI(
                c =>
                {
                    c.RoutePrefix = $"swagger/{ApiConstants.ServiceName}";

                    foreach (ApiVersionDescription item in apiVersionDescriptionProvider.ApiVersionDescriptions)
                    {
                        c.SwaggerEndpoint($"{item.GroupName}/swagger.json", item.GroupName.ToUpperInvariant());
                    }
                }    
            );

            return applicationBuilder;
        }

        private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var info = new OpenApiInfo
            {
                Title = $"{ApiConstants.FriendlyServiceName} API {description.ApiVersion}",
                Version = description.ApiVersion.ToString(),
                Description = "###Thang###",
            }; 

            if (description.IsDeprecated)
            {
                info.Description += $"{Environment.NewLine} This API version has been deprecated.";
            }

            return info;
        }
    }
}
