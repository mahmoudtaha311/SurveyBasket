
using Asp.Versioning;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SurveyBasket.Api.Authentication;
using SurveyBasket.Api.Health;
using SurveyBasket.Api.Settings;
using SurveyBasket.Api.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Threading.RateLimiting;
namespace SurveyBasket.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
    {

        var connectionstring = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(connectionstring));


        services.AddControllers();

        services.AddCors(options =>
               options.AddDefaultPolicy(builder =>
                             builder
                                .AllowAnyOrigin()
                                .AllowAnyMethod()
                                .AllowAnyHeader()
                                )
               );

        //add swagger
        services.AddSwaggerServicesConfig();

        //add Mapster
        services.AddMapSterServicesConfig();

        //add fluent validation
        services.AddFluentValidationConfig();

        //add authentication configuration

        services.AddAuthConfig(configuration);

        services.AddScoped<IPollService, PollService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IEmailSender, EmailService>();
        services.AddScoped<IQuestionService, QuestionService>();
        services.AddScoped<IVoteService, VoteService>();
        services.AddScoped<IResultService, ResultService>();
        services.AddScoped<ICacheService, CacheService>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IRoleService, RoleService>();


        services.AddOptions<MailSettings>()
            .BindConfiguration(nameof(MailSettings))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
        services.AddBackgroundJobsConfig(configuration);

        services.AddHttpContextAccessor();

        //health checks
        services.AddHealthChecks()
            .AddSqlServer(name: "database", connectionString: connectionstring!)
            .AddHangfire(option => { option.MinimumAvailableServers = 1; })
            .AddCheck<MailProviderHealthCheck>(name: "Mail Provider");

        // rate limiting
        services.AddRateLimitConfig();

        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
            options.ApiVersionReader = new HeaderApiVersionReader("x-api-version");
        }).AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'V";
            options.SubstituteApiVersionInUrl = true;

        });


        return services;
    }

    private static IServiceCollection AddSwaggerServicesConfig(this IServiceCollection services)
    {
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.OperationFilter<SwaggerDefaultValues>();

        });

        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

        services.ConfigureSwaggerGen(options =>
        {
            options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description = "please add your token",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = JwtBearerDefaults.AuthenticationScheme
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
               {
                   new OpenApiSecurityScheme
                   {
                        Reference = new OpenApiReference
                        {
                          Id = JwtBearerDefaults.AuthenticationScheme ,
                          Type = ReferenceType.SecurityScheme
                        }
                   },
                   Array.Empty<string>()
               }
            });


        });
        return services;
    }

    private static IServiceCollection AddMapSterServicesConfig(this IServiceCollection services)
    {

        var mappingconfig = TypeAdapterConfig.GlobalSettings;
        mappingconfig.Scan(Assembly.GetExecutingAssembly());

        services.AddSingleton<IMapper>(new Mapper(mappingconfig));

        return services;
    }
    private static IServiceCollection AddFluentValidationConfig(this IServiceCollection services)
    {
        services.
              AddFluentValidationAutoValidation()
              .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;

    }
    private static IServiceCollection AddAuthConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.
            AddIdentity<ApplicationUser, ApplicationRole>().
            AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        services.AddTransient<IAuthorizationHandler, PermissionAuthorizationHandler>();
        services.AddTransient<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();
        services.AddSingleton<IJwtProvider, JwtProvider>();

        // option pattern configuration

        services.AddOptions<JwtOptions>().BindConfiguration(JwtOptions.SectionName).ValidateDataAnnotations().ValidateOnStart();

        var settings = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>();



        services.AddAuthentication(options =>
        {


            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }
        )
            .AddJwtBearer(o =>
            {
                o.SaveToken = true;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings!.key)),
                    ValidAudience = settings.audience,
                    ValidIssuer = settings.issuer,

                };

            });

        services.Configure<IdentityOptions>(options =>
        {

            options.Password.RequiredLength = 8;
            options.User.RequireUniqueEmail = true;
            options.SignIn.RequireConfirmedEmail = true;

        });


        return services;

    }

    private static IServiceCollection AddBackgroundJobsConfig(this IServiceCollection services, IConfiguration configuration)
    {
        // Add Hangfire services.
        services.AddHangfire(config => config
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(configuration.GetConnectionString("HangfireConnection")));

        // Add the processing server as IHostedService
        services.AddHangfireServer();


        return services;
    }

    private static IServiceCollection AddRateLimitConfig(this IServiceCollection services)
    {
        services.AddRateLimiter(rateLimiterOptions =>
        {
            rateLimiterOptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            rateLimiterOptions.AddPolicy(OptionsRateLimiting.IpLimitPolicy, HttpContext =>

                RateLimitPartition.GetFixedWindowLimiter
                (
                    partitionKey: HttpContext.Connection.RemoteIpAddress?.ToString(),
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = OptionsRateLimiting.IpLimitPermitLimit,
                        Window = TimeSpan.FromSeconds(OptionsRateLimiting.IpLimitWindow)
                    }
                )

            );


            rateLimiterOptions.AddPolicy(OptionsRateLimiting.UserLimitPolicy, HttpContext =>

               RateLimitPartition.GetFixedWindowLimiter
               (
                   partitionKey: HttpContext.User.GetUserId(),
                   factory: _ => new FixedWindowRateLimiterOptions
                   {
                       PermitLimit = OptionsRateLimiting.UserLimitPermitLimit,
                       Window = TimeSpan.FromSeconds(OptionsRateLimiting.UserLimitWindow)
                   }
               )

           );

            rateLimiterOptions.AddConcurrencyLimiter(OptionsRateLimiting.ConcurrencyPolicy, options =>
            {
                options.PermitLimit = OptionsRateLimiting.ConcurrencyPermitLimit;
                options.QueueLimit = OptionsRateLimiting.ConcurrencyQueueLimit;
                options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            });            

        });


        return services;
    }
}
