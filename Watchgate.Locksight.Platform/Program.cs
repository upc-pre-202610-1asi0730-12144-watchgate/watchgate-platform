using Cortex.Mediator.Commands;
using Cortex.Mediator.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.OpenApi;
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
using Watchgate.Locksight.Platform.Iam.Resources;
using Watchgate.Locksight.Platform.Resources.Errors;
using Watchgate.Locksight.Platform.Resources.Shared;
using Watchgate.Locksight.Platform.SecurityAlerts.Application.CommandServices;
using Watchgate.Locksight.Platform.SecurityAlerts.Application.Internal.CommandServices;
using Watchgate.Locksight.Platform.SecurityAlerts.Application.Internal.QueryServices;
using Watchgate.Locksight.Platform.SecurityAlerts.Application.QueryServices;
using Watchgate.Locksight.Platform.SecurityAlerts.Domain.Repositories;
using Watchgate.Locksight.Platform.SecurityAlerts.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Watchgate.Locksight.Platform.SecurityAlerts.Resources;
using Watchgate.Locksight.Platform.SensorIntegration.Application.CommandServices;
using Watchgate.Locksight.Platform.SensorIntegration.Application.Internal.CommandServices;
using Watchgate.Locksight.Platform.SensorIntegration.Application.Internal.QueryServices;
using Watchgate.Locksight.Platform.SensorIntegration.Application.QueryServices;
using Watchgate.Locksight.Platform.SensorIntegration.Domain.Repositories;
using Watchgate.Locksight.Platform.SensorIntegration.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Watchgate.Locksight.Platform.SensorIntegration.Resources;
using Watchgate.Locksight.Platform.Shared.Domain.Repositories;
using Watchgate.Locksight.Platform.Shared.Infrastructure.Interfaces.AspNetCore.Configuration;
using Watchgate.Locksight.Platform.Shared.Infrastructure.Mediator.Cortex.Configuration;
using Watchgate.Locksight.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Watchgate.Locksight.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Watchgate.Locksight.Platform.Shared.Infrastructure.Pipeline.Middleware.Extensions;
using Watchgate.Locksight.Platform.WarehouseManagement.Application.CommandServices;
using Watchgate.Locksight.Platform.WarehouseManagement.Application.Internal.CommandServices;
using Watchgate.Locksight.Platform.WarehouseManagement.Application.Internal.QueryServices;
using Watchgate.Locksight.Platform.WarehouseManagement.Application.QueryServices;
using Watchgate.Locksight.Platform.WarehouseManagement.Domain.Repositories;
using Watchgate.Locksight.Platform.WarehouseManagement.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
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
        { [new OpenApiSecuritySchemeReference("bearer", document)] = [] });
    options.EnableAnnotations();
});

// Shared Bounded Context
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// IAM Bounded Context
builder.Services.Configure<TokenSettings>(builder.Configuration.GetSection("TokenSettings"));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<IUserCommandService, UserCommandService>();
builder.Services.AddScoped<IUserQueryService, UserQueryService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IHashingService, HashingService>();

// Warehouse Management Bounded Context
builder.Services.AddScoped<IWarehouseRepository, WarehouseRepository>();
builder.Services.AddScoped<IWarehouseZoneRepository, WarehouseZoneRepository>();
builder.Services.AddScoped<IWarehouseCommandService, WarehouseCommandService>();
builder.Services.AddScoped<IWarehouseQueryService, WarehouseQueryService>();

// Sensor Integration Bounded Context
builder.Services.AddScoped<ISensorRepository, SensorRepository>();
builder.Services.AddScoped<ISensorCommandService, SensorCommandService>();
builder.Services.AddScoped<ISensorQueryService, SensorQueryService>();

// Security Alerts Bounded Context
builder.Services.AddScoped<ISecurityAlertRepository, SecurityAlertRepository>();
builder.Services.AddScoped<IAlertIncidentRepository, AlertIncidentRepository>();
builder.Services.AddScoped<ISecurityAlertCommandService, SecurityAlertCommandService>();
builder.Services.AddScoped<ISecurityAlertQueryService, SecurityAlertQueryService>();

// Mediator Configuration
builder.Services.AddScoped(typeof(ICommandPipelineBehavior<>), typeof(LoggingCommandBehavior<>));
builder.Services.AddCortexMediator([typeof(Program)]);

var app = builder.Build();

// using (var scope = app.Services.CreateScope())
// {
//     var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
//     context.Database.EnsureCreated();
// }

app.UseGlobalExceptionHandler();

var supportedCultures = new[] { "en", "es" };
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);
app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapGet("/", () => Results.Redirect("/swagger"))
        .WithMetadata(new AllowAnonymousAttribute());}

app.UseCors("AllowAllPolicy");
app.UseRequestAuthorization();
app.MapControllers();

app.Run();
