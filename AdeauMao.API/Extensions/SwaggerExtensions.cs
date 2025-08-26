using Microsoft.OpenApi.Models;
using System.Reflection;
using Swashbuckle.AspNetCore.SwaggerGen;
using AdeauMao.Application.DTOs;
using System;


namespace AdeauMao.API.Extensions
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "AdeauMao GMAO API",
                    Version = "v1.0",
                    Description = "API complète pour le système de Gestion de Maintenance Assistée par Ordinateur (GMAO) AdeauMao",
                    Contact = new OpenApiContact
                    {
                        Name = "Équipe AdeauMao",
                        Email = "contact@adeaumao.com",
                        Url = new Uri("https://www.adeaumao.com")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "MIT License",
                        Url = new Uri("https://opensource.org/licenses/MIT")
                    }
                });

                // JWT Authentication configuration
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header utilisant le schéma Bearer. 
                                  Entrez 'Bearer' [espace] puis votre token dans le champ ci-dessous.
                                  Exemple: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });

                // Include XML comments
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    c.IncludeXmlComments(xmlPath);
                }

                // Custom schema IDs
                c.CustomSchemaIds(type => type.FullName);

                // Add operation filters
                c.OperationFilter<SwaggerDefaultValues>();
                c.DocumentFilter<SwaggerDocumentFilter>();

                // Group endpoints by tags
                c.TagActionsBy(api => new[] { api.GroupName ?? api.ActionDescriptor.RouteValues["controller"] });
                c.DocInclusionPredicate((name, api) => true);

                // Add examples
                c.SchemaFilter<SwaggerSchemaExampleFilter>();
            });

            return services;
        }

        public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "api-docs/{documentName}/swagger.json";
            });

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/api-docs/v1/swagger.json", "AdeauMao API V1");
                c.RoutePrefix = "api-docs";
                c.DocumentTitle = "AdeauMao GMAO API Documentation";
                
                // UI customization
                c.DefaultModelsExpandDepth(-1);
                c.DefaultModelRendering(Swashbuckle.AspNetCore.SwaggerUI.ModelRendering.Example);
                c.DisplayRequestDuration();
                c.EnableDeepLinking();
                c.EnableFilter();
                c.ShowExtensions();
                c.EnableValidator();
                
                // Custom CSS
                c.InjectStylesheet("/swagger-ui/custom.css");
                
                if (env.IsDevelopment())
                {
                    c.EnableTryItOutByDefault();
                }
            });

            return app;
        }
    }

    public class SwaggerDefaultValues : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var apiDescription = context.ApiDescription;

            operation.Deprecated |= apiDescription.ActionDescriptor.EndpointMetadata
                .OfType<ObsoleteAttribute>()
                .Any();

            foreach (var responseType in context.ApiDescription.SupportedResponseTypes)
            {
                var responseKey = responseType.IsDefaultResponse ? "default" : responseType.StatusCode.ToString();
                var response = operation.Responses[responseKey];

                foreach (var contentType in response.Content.Keys)
                {
                    if (responseType.ApiResponseFormats.All(x => x.MediaType != contentType))
                    {
                        response.Content.Remove(contentType);
                    }
                }
            }

            if (operation.Parameters == null)
                return;

            foreach (var parameter in operation.Parameters)
            {
                var description = apiDescription.ParameterDescriptions.First(p => p.Name == parameter.Name);
                parameter.Description ??= description.ModelMetadata?.Description;

                if (parameter.Schema.Default == null && description.DefaultValue != null)
                {
                    parameter.Schema.Default = new Microsoft.OpenApi.Any.OpenApiString(description.DefaultValue.ToString());
                }

                parameter.Required |= description.IsRequired;
            }
        }
    }

    public class SwaggerDocumentFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            // Add custom tags
            swaggerDoc.Tags = new List<OpenApiTag>
            {
                new OpenApiTag { Name = "Auth", Description = "Authentification et gestion des utilisateurs" },
                new OpenApiTag { Name = "Equipements", Description = "Gestion des équipements et organes" },
                new OpenApiTag { Name = "Employes", Description = "Gestion des employés, équipes et compétences" },
                new OpenApiTag { Name = "OrdresDeTravail", Description = "Gestion des ordres de travail" },
                new OpenApiTag { Name = "DemandesIntervention", Description = "Gestion des demandes d'intervention" },
                new OpenApiTag { Name = "Maintenance", Description = "Planification et suivi de la maintenance" },
                new OpenApiTag { Name = "Stock", Description = "Gestion des stocks et pièces de rechange" },
                new OpenApiTag { Name = "Rapports", Description = "Génération de rapports et statistiques" }
            };

            // Remove unwanted paths
            var pathsToRemove = swaggerDoc.Paths
                .Where(x => x.Key.ToLower().Contains("weatherforecast"))
                .ToList();

            foreach (var path in pathsToRemove)
            {
                swaggerDoc.Paths.Remove(path.Key);
            }
        }
    }

    public class SwaggerSchemaExampleFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type == typeof(CreateEquipementDto))
            {
                schema.Example = new Microsoft.OpenApi.Any.OpenApiObject
                {
                    ["reference"] = new Microsoft.OpenApi.Any.OpenApiString("EQ-001"),
                    ["nom"] = new Microsoft.OpenApi.Any.OpenApiString("Compresseur Principal"),
                    ["typeEquipement"] = new Microsoft.OpenApi.Any.OpenApiString("Compresseur"),
                    ["fabricant"] = new Microsoft.OpenApi.Any.OpenApiString("Atlas Copco"),
                    ["modele"] = new Microsoft.OpenApi.Any.OpenApiString("GA-75"),
                    ["dateMiseEnService"] = new Microsoft.OpenApi.Any.OpenApiString("2023-01-15"),
                    ["localisation"] = new Microsoft.OpenApi.Any.OpenApiString("Atelier Principal - Zone A"),
                    ["ligneProductionId"] = new Microsoft.OpenApi.Any.OpenApiInteger(1),
                    ["description"] = new Microsoft.OpenApi.Any.OpenApiString("Compresseur d'air principal pour l'alimentation pneumatique"),
                    ["etatOperationnel"] = new Microsoft.OpenApi.Any.OpenApiString("En service")
                };
            }
            else if (context.Type == typeof(CreateEmployeDto))
            {
                schema.Example = new Microsoft.OpenApi.Any.OpenApiObject
                {
                    ["nom"] = new Microsoft.OpenApi.Any.OpenApiString("Alami"),
                    ["prenom"] = new Microsoft.OpenApi.Any.OpenApiString("Ahmed"),
                    ["contact"] = new Microsoft.OpenApi.Any.OpenApiString("0661234567"),
                    ["roleInterne"] = new Microsoft.OpenApi.Any.OpenApiString("Technicien Électricien"),
                    ["utilisateurId"] = new Microsoft.OpenApi.Any.OpenApiInteger(1)
                };
            }
        }
    }
}

