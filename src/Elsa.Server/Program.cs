using Elsa.Activities.Primitives;
using Elsa.CustomActivities.Activities.CheckYourAnswersScreen;
using Elsa.CustomActivities.Activities.ConfirmationScreen;
using Elsa.CustomActivities.Activities.FinishWorkflow;
using Elsa.CustomActivities.Activities.HousingNeed;
using Elsa.CustomActivities.Activities.PCSProfileDataSource;
using Elsa.CustomActivities.Activities.QuestionScreen;
using Elsa.CustomActivities.Activities.RunEconomicCalculations;
using Elsa.CustomActivities.Activities.Scoring;
using Elsa.CustomActivities.Activities.Shared;
using Elsa.CustomActivities.Activities.SinglePipelineDataSource;
using Elsa.CustomActivities.Activities.VFMDataSource;
using Elsa.CustomActivities.Describers;
using Elsa.CustomActivities.Handlers;
using Elsa.CustomActivities.Handlers.Scoring;
using Elsa.CustomActivities.Handlers.Syntax;
using Elsa.CustomActivities.Providers;
using Elsa.CustomInfrastructure.Data;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomWorkflow.Sdk.Extensions;
using Elsa.CustomWorkflow.Sdk.Providers;
using Elsa.Expressions;
using Elsa.Persistence.EntityFramework.SqlServer;
using Elsa.Providers.Workflows;
using Elsa.Runtime;
using Elsa.Server.Extensions;
using Elsa.Server.Helpers;
using Elsa.Server.Middleware;
using Elsa.Server.Providers;
using Elsa.Server.Services;
using Elsa.Server.StartupTasks;
using Elsa.Services;
using He.PipelineAssessment.Data.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Elsa.Services.Workflows;
using Elsa.Server.Mappers;
using Elsa.CustomActivities.Activities.RegionalIPUDataSource;
using Elsa.CustomActivities.Activities.RegionalFigsDataSource;
using FluentValidation;
using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using Elsa.Server.Features.Workflow.QuestionScreenValidateAndSave;

var builder = WebApplication.CreateBuilder(args);
var elsaConnectionString = builder.Configuration.GetConnectionString("Elsa");
var elsaCustomConnectionString = builder.Configuration.GetConnectionString("ElsaCustom");

using var loggerFactory = LoggerFactory.Create(loggingBuilder => loggingBuilder
    .SetMinimumLevel(LogLevel.Trace)
    .AddConsole());

ILogger logger = loggerFactory.CreateLogger<Program>(); ;

var redisConnectionString = builder.Configuration.GetConnectionString("Redis");
var clearCache = Convert.ToBoolean(builder.Configuration["Redis:ClearCache"]);
if (!builder.Environment.IsDevelopment())
{
    logger.LogInformation("Attempting to set up Redis Connection.  Environment is not Development");
    await builder.Services.AddRedisWithSelfSignedSslCertificate(redisConnectionString!, builder.Configuration["Redis:SslCertificatePath"], builder.Configuration["Redis:SslCertificateKeyPath"], logger, clearCache );
}
else
{
    var redisConfiguration = builder.Configuration["Redis:Configuration"];
    if (!string.IsNullOrEmpty(redisConfiguration))
    {
        await builder.Services.AddRedisLocal(redisConfiguration, logger, clearCache);
    }
}

bool useCache = (Convert.ToBoolean(builder.Configuration["Redis:UseCache"]) && !builder.Environment.IsDevelopment()) || !string.IsNullOrWhiteSpace(builder.Configuration["Redis:Configuration"]);

logger.LogInformation($"Using Cache: {useCache}");
// Elsa services.
builder.Services
    .AddElsa(elsa => elsa
        .UseEntityFrameworkPersistenceWithCache(ef => ef.UseSqlServer(elsaConnectionString!, typeof(Elsa.Persistence.EntityFramework.SqlServer.Migrations.Initial)), true, useCache)
        .NoCoreActivities()
        .AddActivity<SinglePipelineDataSource>()
        .AddActivity<PCSProfileDataSource>()
        .AddActivity<VFMDataSource>()
        .AddActivity<HousingNeedDataSource>()
        .AddActivity<QuestionScreen>()
        .AddActivity<CheckYourAnswersScreen>()
        .AddActivity<ConfirmationScreen>()
        .AddActivity<FinishWorkflow>()
        .AddActivity<ScoringCalculation>()
        .AddActivity<RunEconomicCalculations>()
        .AddActivity<SetVariable>()
        .AddActivity<RegionalIPUDataSource>()
        .AddActivity<RegionalFigsDataSource>()
        .AddConsoleActivities()
    );

builder.Services.AddScoped<IWorkflowRegistry, WorkflowRegistry>();


builder.Services.AddScoped<ICustomPropertyDescriber, CustomPropertyDescriber>();

builder.Services.AddScoped<IWorkflowPublisher, Elsa.Server.Publisher.WorkflowPublisher>();

builder.Services.TryAddProvider<IExpressionHandler, InformationTextExpressionHandler>(ServiceLifetime.Singleton);
builder.Services.TryAddProvider<IExpressionHandler, InformationTextGroupExpressionHandler>(ServiceLifetime.Singleton);
builder.Services.TryAddProvider<IExpressionHandler, QuestionListExpressionHandler>(ServiceLifetime.Singleton);
builder.Services.TryAddProvider<IExpressionHandler, ScoringCalculationExpressionHandler>(ServiceLifetime.Singleton);
builder.Services.TryAddSingleton<INestedSyntaxExpressionHandler, NestedSyntaxExpressionHandler>();

builder.Services.AddDbContext<ElsaCustomContext>(config =>
    config.UseSqlServer(elsaCustomConnectionString!,
        x => x.MigrationsAssembly("Elsa.CustomInfrastructure")));

builder.Services.AddScoped<DbContext>(provider => provider.GetRequiredService<ElsaCustomContext>());
builder.Services.AddDataProtection().PersistKeysToDbContext<ElsaCustomContext>();

// Elsa API endpoints.
builder.Services.AddElsaApiEndpoints();

//Custom method.  Register new Script Handlers here.
builder.Services.AddCustomElsaScriptHandlers();

builder.Services.AddBookmarkProvider<QuestionBookmarkProvider>();
builder.Services.AddScoped<IQuestionInvoker, QuestionInvoker>();

builder.Services.AddScoped<IElsaCustomRepository, ElsaCustomRepository>();


builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
});
builder.Services.AddApplicationInsightsTelemetry();
builder.Services.AddScoped<IDateTimeProvider, DateTimeProvider>();
builder.Services.AddScoped<IActivityDataProvider, ActivityDataProvider>();
builder.Services.AddScoped<IEnumerable<IWorkflowProvider>>(x =>
{
    var context = x.GetRequiredService<IHttpContextAccessor>();
    if (context.HttpContext != null)
    {
        var path = context.HttpContext.Request.Path;
        if (path.HasValue && path.Value == "/v1/workflow-registry")
        {
            return new List<IWorkflowProvider>();
        }
    }
    var service = x.GetRequiredService<IWorkflowProvider>();
    return new List<IWorkflowProvider>()
    {
        service
    };
});
builder.Services.AddScoped<IWorkflowRegistryProvider, WorkflowRegistryProvider>();
builder.Services.AddScoped<IWorkflowInstanceProvider, WorkflowInstanceProvider>();
builder.Services.AddScoped<IWorkflowPathProvider, WorkflowPathProvider>();
builder.Services.AddScoped<IWorkflowNextActivityProvider, WorkflowNextActivityProvider>();
builder.Services.AddScoped(typeof(PotScoreOptionsProvider));
builder.Services.AddScoped<IScoreProvider, ScoreProvider>();

builder.Services.AddScoped<IElsaCustomModelHelper, ElsaCustomModelHelper>();

builder.Services.AddScoped<IDeleteChangedWorkflowPathService, DeleteChangedWorkflowPathService>();
builder.Services.AddScoped<INextActivityNavigationService, NextActivityNavigationService>();

builder.Services.AddScoped<ITextGroupMapper, TextGroupMapper>();


builder.Services.AddScoped<IValidator<WorkflowActivityDataDto>, QuestionScreenValidator>();

// Allow arbitrary client browser apps to access the API.
// In a production environment, make sure to allow only origins you trust.
builder.Services.AddCors(cors => cors.AddDefaultPolicy(policy => policy
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowAnyOrigin()
    .WithExposedHeaders("Content-Disposition"))
);

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddStartupTask<RunElsaCustomMigrations>();
}

builder.Services.AddScoped<IIdentityClient, IdentityClient>();
builder.Services.AddTransient<BearerTokenHandler>();

builder.Services.AddOptions<IdentityClientConfig>()
.Configure<IConfiguration>((settings, configuration) =>
{
    configuration.GetSection("IdentityClientConfig").Bind(settings);
});


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Audience = builder.Configuration["Auth0Config:Audience"];
        options.Authority = builder.Configuration["Auth0Config:Authority"];
    });

builder.Services.AddEsriHttpClients(builder.Configuration, builder.Environment.IsDevelopment());


//HibernatingRhinos.Profiler.Appender.EntityFramework.EntityFrameworkProfiler.Initialize();
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();


}

app
    .UseCors()
    .UseHttpsRedirection()
    .UseStaticFiles() // For Dashboard.
    .UseRouting()
    .UseAuthentication()
    .UseAuthorization()
    .UseCustomControllerOverrides(builder.Configuration["Elsa:ServerPrefix"])
    .UseEndpoints(endpoints =>
    {
    // Elsa API Endpoints are implemented as regular ASP.NET Core API controllers.
    endpoints.MapControllers().RequireAuthorization(); // locks down elsa server end points
        endpoints.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}");

    });


app.Run();