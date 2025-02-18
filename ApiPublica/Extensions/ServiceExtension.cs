using Entities.Context.RunPayDb;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using Interfaces.Bussines;
using Entities.General;
using Bussines;
using Interfaces.DataAccess.Repository;
using DataAccess.Repository;
using Interfaces.DataAccess.Utilities;
using DataAccess.Utilities;
using Entities.Identity;
using Microsoft.Extensions.DependencyInjection.Extensions;


namespace ApiPublica.Extensions
{
    public static class ServiceExtension
    {
        public static void ConfigureContext(this IServiceCollection Services, IConfiguration Configuration)
        {
            //DbContextOptions           
            Services.AddDbContextFactory<RunPayDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("RunPayDbConnection"));
            });

        }

        public static void ConfigureServices(this IServiceCollection Services)
        {
            Services.AddHttpClient();
            Services.AddMemoryCache();

            Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<RunPayDbContext>().AddDefaultTokenProviders();


            //Servicios
            Services.AddScoped<IHelper, Helper>();
            Services.AddScoped<ILogService, LogService>();
            Services.AddScoped<IIdentityService, IdentityService>();
            Services.AddScoped<ITransactionServices, TransactionServices>();



            //Repositorios
            Services.AddScoped<ILogRepository, LogRepository>();
            Services.AddScoped<IIdentityRepository, IdentityRepository>();
            Services.AddScoped<ITransactionRepository, TransactionRepository>();



            Services.AddResponseCaching();
        }

        public static void CongifureAuthentication(this IServiceCollection Services, IConfiguration Configuration)
        {
            var jwtConfig = Configuration.GetSection("jwtConfig");
            Services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtConfig["validIssuer"],
                    ValidAudience = jwtConfig["validAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig["secret"])),
                };

            });

            Services.AddCors(options =>
            {
                options.AddPolicy("AllowOrigin",
                    builder => builder
                        .AllowAnyOrigin()  // You can also use .WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod());
            });
        }

        public static void CongifureSwagger(this IServiceCollection Services)
        {
            Services.AddEndpointsApiExplorer();
            Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "RunnPay Publica (v1.0)",
                    Version = "v1",
                    Description = "Api Publica ",
                    Contact = new OpenApiContact
                    {
                        Name = "RunnPay"
                    },
                    //TermsOfService = new Uri("https://example.com/terms"),
                });

                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer"
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
                            }
                        },
                        new List<string>()
                    }
                });

                c.CustomSchemaIds(x => (x.GetCustomAttributes<DisplayNameAttribute>().FirstOrDefault() ?? new DisplayNameAttribute(x.Name)).DisplayName);

                c.OperationFilter<CustomSwaggerOperationFilter>();

            });
        }

        public class CustomSwaggerOperationFilter : IOperationFilter
        {
            public void Apply(OpenApiOperation operation, OperationFilterContext context)
            {
                if (context.MethodInfo.GetCustomAttributes(typeof(DescriptionAttribute), false)?.FirstOrDefault() is DescriptionAttribute descriptionAttribute)
                {
                    operation.Description = descriptionAttribute.Description;
                }
            }
        }

        public class CustomAuthorizationFilter : IAsyncAuthorizationFilter
        {
            private readonly ILogService _logService;
            public AuthorizationPolicy Policy { get; }

            public CustomAuthorizationFilter(ILogService logService)
            {
                Policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                _logService = logService;
            }

            public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
            {
                if (context == null)
                {
                    throw new ArgumentNullException(nameof(context));
                }

                // Allow Anonymous skips all authorization
                if (context.Filters.Any(item => item is IAllowAnonymousFilter))
                {
                    return;
                }

                var policyEvaluator = context.HttpContext.RequestServices.GetRequiredService<IPolicyEvaluator>();
                var authenticateResult = await policyEvaluator.AuthenticateAsync(Policy, context.HttpContext);
                var authorizeResult = await policyEvaluator.AuthorizeAsync(Policy, authenticateResult, context.HttpContext, context);

                if (authorizeResult.Challenged)
                {
                    string errorMsg = context.HttpContext.Request.Headers["Authorization"].Count > 0 ? "El token a expirado o no es valido, por favor realice nuevamente la generación del token." : "Realice autenticación para seguir con el proceso";
                    context.Result = new JsonResult(new BaseResponse(errorMsg))
                    {
                        StatusCode = StatusCodes.Status401Unauthorized
                    };
                    await LogAccess(context);
                }
            }

            private async Task LogAccess(AuthorizationFilterContext context)
            {
                try
                {
                    var headers = context.HttpContext.Request.Headers.ToList();
                    var info = headers.Select(x => string.Format("Cabecera [ {0} => {1} ]", x.Key, string.Join('|', x.Value.ToList())));
                    await _logService.Logger(new MethodsParameters.Input.LogIn($"Acceso no autorizado.", string.Join(Environment.NewLine, info), "Error"));
                }
                catch (Exception ex)
                {
                    await _logService.Logger(new MethodsParameters.Input.LogIn(ex));
                }
            }
        }


    }
}
