using Cortex.Mediator.Commands;
using Cortex.Mediator.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.OpenApi;
using Watchgate.Locksight.Platform.Iam.Application.Acl;
using Watchgate.Locksight.Platform.CompanyRegistration.Application.CommandServices;
using Watchgate.Locksight.Platform.CompanyRegistration.Application.Internal.CommandServices;
using Watchgate.Locksight.Platform.CompanyRegistration.Application.Internal.QueryServices;
using Watchgate.Locksight.Platform.CompanyRegistration.Application.QueryServices;
using Watchgate.Locksight.Platform.CompanyRegistration.Domain.Repositories;
using Watchgate.Locksight.Platform.CompanyRegistration.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Watchgate.Locksight.Platform.Reporting.Application.CommandServices;
using Watchgate.Locksight.Platform.Reporting.Application.Internal.CommandServices;
using Watchgate.Locksight.Platform.Reporting.Application.Internal.QueryServices;
using Watchgate.Locksight.Platform.Reporting.Application.QueryServices;
using Watchgate.Locksight.Platform.Reporting.Domain.Repositories;
using Watchgate.Locksight.Platform.Reporting.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Watchgate.Locksight.Platform.Iam.Application.CommandServices;
using Watchgate.Locksight.Platform.Iam.Application.Internal.CommandServices;
using Watchgate.Locksight.Platform.Iam.Application.Internal.OutboundServices;
using Watchgate.Locksight.Platform.Iam.Application.Internal.QueryServices;
using Watchgate.Locksight.Platform.Iam.Application.QueryServices;
using Watchgate.Locksight.Platform.Iam.Domain.Repositories;
using Watchgate.Locksight.Platform.Iam.Infrastructure.Hashing.BCrypt.Services;
using Watchgate.Locksight.Platform.Iam.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Watchgate.Locksight.Platform.Iam.Infrastructure.Pipeline.Middleware.Extensions;
using Watchgate.Locksight.Platform.Iam.Infrastructure.Tokens.Jwt.Configuration;
using Watchgate.Locksight.Platform.Iam.Infrastructure.Tokens.Jwt.Services;
using Watchgate.Locksight.Platform.Iam.Interfaces.Acl;
using Watchgate.Locksight.Platform.Iam.Resources;
using Watchgate.Locksight.Platform.Resources.Errors;
using Watchgate.Locksight.Platform.Resources.Shared;
using Watchgate.Locksight.Platform.SecurityAlerts.Application.CommandServices;
using Watchgate.Locksight.Platform.SecurityAlerts.Application.Internal.CommandServices;
using Watchgate.Locksight.Platform.SecurityAlerts.Application.Internal.EventHandlers;
using Watchgate.Locksight.Platform.SecurityAlerts.Application.Internal.QueryServices;
using Watchgate.Locksight.Platform.SecurityAlerts.Application.QueryServices;
using Watchgate.Locksight.Platform.SecurityAlerts.Domain.Model.Events;
using Watchgate.Locksight.Platform.SecurityAlerts.Domain.Repositories;
using Watchgate.Locksight.Platform.SecurityAlerts.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Watchgate.Locksight.Platform.SecurityAlerts.Resources;
using Watchgate.Locksight.Platform.SensorIntegration.Application.Acl;
using Watchgate.Locksight.Platform.SensorIntegration.Application.CommandServices;
using Watchgate.Locksight.Platform.SensorIntegration.Application.Internal.CommandServices;
using Watchgate.Locksight.Platform.SensorIntegration.Application.Internal.QueryServices;
using Watchgate.Locksight.Platform.SensorIntegration.Application.QueryServices;
using Watchgate.Locksight.Platform.SensorIntegration.Domain.Repositories;
using Watchgate.Locksight.Platform.SensorIntegration.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Watchgate.Locksight.Platform.SensorIntegration.Interfaces.Acl;
using Watchgate.Locksight.Platform.SensorIntegration.Resources;
using Watchgate.Locksight.Platform.Shared.Domain.Model.Events;
using Watchgate.Locksight.Platform.Shared.Domain.Repositories;
using Watchgate.Locksight.Platform.Shared.Infrastructure.Events;
using Watchgate.Locksight.Platform.Shared.Infrastructure.Interfaces.AspNetCore.Configuration;
using Watchgate.Locksight.Platform.Shared.Infrastructure.Mediator.Cortex.Configuration;
using Watchgate.Locksight.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Watchgate.Locksight.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Watchgate.Locksight.Platform.Shared.Infrastructure.Pipeline.Middleware.Extensions;
using Watchgate.Locksight.Platform.SubscriptionManagement.Application.CommandServices;
using Watchgate.Locksight.Platform.SubscriptionManagement.Application.Internal.CommandServices;
using Watchgate.Locksight.Platform.SubscriptionManagement.Application.Internal.QueryServices;
using Watchgate.Locksight.Platform.SubscriptionManagement.Application.QueryServices;
using Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Repositories;
using Watchgate.Locksight.Platform.SubscriptionManagement.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Watchgate.Locksight.Platform.WarehouseManagement.Application.Acl;
using Watchgate.Locksight.Platform.WarehouseManagement.Application.CommandServices;
using Watchgate.Locksight.Platform.WarehouseManagement.Application.Internal.CommandServices;
using Watchgate.Locksight.Platform.WarehouseManagement.Application.Internal.QueryServices;
using Watchgate.Locksight.Platform.WarehouseManagement.Application.QueryServices;
using Watchgate.Locksight.Platform.WarehouseManagement.Domain.Repositories;
using Watchgate.Locksight.Platform.WarehouseManagement.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Watchgate.Locksight.Platform.WarehouseManagement.Interfaces.Acl;
using Watchgate.Locksight.Platform.WarehouseManagement.Resources;
using Watchgate.Locksight.Platform.Iam.Infrastructure.Pipeline.Middleware.Attributes;
using ProblemDetailsFactory = Watchgate.Locksight.Platform.Shared.Interfaces.Rest.ProblemDetails.ProblemDetailsFactory;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddControllers(options => options.Conventions.Add(new KebabCaseRouteNamingConvention()))
    .AddDataAnnotationsLocalization();

builder.Services.AddProblemDetails();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllPolicy",
        policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

builder.Services.AddDbContext<AppDbContext>((serviceProvider, options) =>
{
    var connectionStringTemplate = builder.Configuration.GetConnectionString("DefaultConnection");
    if (string.IsNullOrWhiteSpace(connectionStringTemplate))
        throw new InvalidOperationException("Database connection string is not set in the configuration.");

    var connectionString = Environment.ExpandEnvironmentVariables(connectionStringTemplate);
    options.UseMySQL(connectionString)
        .UseLoggerFactory(serviceProvider.GetRequiredService<ILoggerFactory>())
        .EnableDetailedErrors();

    if (builder.Environment.IsDevelopment())
        options.EnableSensitiveDataLogging();
});

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.AddSingleton<IStringLocalizer<ErrorMessages>, StringLocalizer<ErrorMessages>>();
builder.Services.AddSingleton<IStringLocalizer<CommonMessages>, StringLocalizer<CommonMessages>>();
builder.Services.AddSingleton<IStringLocalizer<IamMessages>, StringLocalizer<IamMessages>>();
builder.Services.AddSingleton<IStringLocalizer<WarehouseManagementMessages>, StringLocalizer<WarehouseManagementMessages>>();
builder.Services.AddSingleton<IStringLocalizer<SensorIntegrationMessages>, StringLocalizer<SensorIntegrationMessages>>();
builder.Services.AddSingleton<IStringLocalizer<SecurityAlertsMessages>, StringLocalizer<SecurityAlertsMessages>>();

builder.Services.AddSingleton<ProblemDetailsFactory>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Watchgate.Locksight.Platform",
        Version = "v1",
        Description = "Watchgate Locksight Platform API"
    });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
        { [new OpenApiSecuritySchemeReference("Bearer", document)] = [] });
    options.EnableAnnotations();
});

// Shared Bounded Context
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IEventDispatcher, EventDispatcher>();

// Company Registration Bounded Context
builder.Services.AddScoped<ICompanyAccountRepository, CompanyAccountRepository>();
builder.Services.AddScoped<ICompanyRegistrationCommandService, CompanyRegistrationCommandService>();
builder.Services.AddScoped<ICompanyRegistrationQueryService, CompanyRegistrationQueryService>();

// IAM Bounded Context
var tokenSecret = builder.Configuration["TOKEN_SECRET"];
if (!string.IsNullOrWhiteSpace(tokenSecret))
    builder.Configuration["TokenSettings:Secret"] = tokenSecret;
builder.Services.Configure<TokenSettings>(builder.Configuration.GetSection("TokenSettings"));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<IUserInvitationRepository, UserInvitationRepository>();
builder.Services.AddScoped<IUserAccessProfileRepository, UserAccessProfileRepository>();
builder.Services.AddScoped<IUserCommandService, UserCommandService>();
builder.Services.AddScoped<IUserQueryService, UserQueryService>();
builder.Services.AddScoped<IUserAccessCommandService, UserAccessCommandService>();
builder.Services.AddScoped<IUserAccessQueryService, UserAccessQueryService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IHashingService, HashingService>();
builder.Services.AddScoped<IIamContextFacade, IamContextFacade>();

// Warehouse Management Bounded Context
builder.Services.AddScoped<IWarehouseRepository, WarehouseRepository>();
builder.Services.AddScoped<IWarehouseZoneRepository, WarehouseZoneRepository>();
builder.Services.AddScoped<IWarehouseCommandService, WarehouseCommandService>();
builder.Services.AddScoped<IWarehouseQueryService, WarehouseQueryService>();
builder.Services.AddScoped<IWarehouseContextFacade, WarehouseContextFacade>();

// Sensor Integration Bounded Context
builder.Services.AddScoped<ISensorRepository, SensorRepository>();
builder.Services.AddScoped<ISensorCommandService, SensorCommandService>();
builder.Services.AddScoped<ISensorQueryService, SensorQueryService>();
builder.Services.AddScoped<ISensorContextFacade, SensorContextFacade>();

// Reporting Bounded Context
builder.Services.AddScoped<ISecurityReportRepository, SecurityReportRepository>();
builder.Services.AddScoped<IScheduledReportRepository, ScheduledReportRepository>();
builder.Services.AddScoped<IEventLogRepository, EventLogRepository>();
builder.Services.AddScoped<IReportingCommandService, ReportingCommandService>();
builder.Services.AddScoped<IReportingQueryService, ReportingQueryService>();

// Subscription Management Bounded Context
builder.Services.AddScoped<ISubscriptionPlanRepository, SubscriptionPlanRepository>();
builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();
builder.Services.AddScoped<ISubscriptionCommandService, SubscriptionCommandService>();
builder.Services.AddScoped<ISubscriptionQueryService, SubscriptionQueryService>();
builder.Services.AddScoped<IBillingCommandService, BillingCommandService>();
builder.Services.AddScoped<IBillingQueryService, BillingQueryService>();

// Security Alerts Bounded Context
builder.Services.AddScoped<ISecurityAlertRepository, SecurityAlertRepository>();
builder.Services.AddScoped<IAlertIncidentRepository, AlertIncidentRepository>();
builder.Services.AddScoped<ISecurityAlertCommandService, SecurityAlertCommandService>();
builder.Services.AddScoped<ISecurityAlertQueryService, SecurityAlertQueryService>();
builder.Services.AddScoped<IEventHandler<SecurityAlertTriggeredEvent>, SecurityAlertTriggeredEventHandler>();

// Mediator Configuration
builder.Services.AddScoped(typeof(ICommandPipelineBehavior<>), typeof(LoggingCommandBehavior<>));
builder.Services.AddCortexMediator([typeof(Program)]);

var app = builder.Build();

// using (var scope = app.Services.CreateScope())
// {
//     var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
//     context.Database.EnsureCreated();
// }

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    context.Database.Migrate();
}

app.UseGlobalExceptionHandler();

var supportedCultures = new[] { "en", "es" };
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);
app.UseHttpsRedirection();

app.UseSwagger();
app.UseSwaggerUI();
app.MapGet("/", () => Results.Redirect("/swagger")).WithMetadata(new AllowAnonymousAttribute());
app.MapGet("/api/v1/health", (IConfiguration configuration, IWebHostEnvironment environment) => Results.Ok(new
{
    status = "Healthy",
    service = "Watchgate LockSight Platform API",
    version = "3.0.0",
    environment = environment.EnvironmentName,
    databaseProvider = "Azure Database for MySQL",
    deployedAtUtc = DateTime.UtcNow,
    apiBasePath = "/api/v1",
    swagger = "/swagger",
    databaseSchema = configuration["DATABASE_SCHEMA"] ?? "watchgate_locksight_db"
})).WithMetadata(new AllowAnonymousAttribute());

app.UseCors("AllowAllPolicy");
app.UseRequestAuthorization();
app.MapControllers();

app.Run();
