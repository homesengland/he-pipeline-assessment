using Elsa.CustomActivities.Activities.CheckYourAnswersScreen;
using Elsa.CustomActivities.Activities.ConfirmationScreen;
using Elsa.CustomActivities.Activities.FinishWorkflow;
using Elsa.CustomActivities.Activities.HousingNeed;
using Elsa.CustomActivities.Activities.PCSProfileDataSource;
using Elsa.CustomActivities.Activities.QuestionScreen;
using Elsa.CustomActivities.Activities.Scoring;
using Elsa.CustomActivities.Activities.Shared;
using Elsa.CustomActivities.Activities.SinglePipelineDataSource;
using Elsa.CustomActivities.Activities.VFMDataSource;
using Elsa.CustomActivities.Describers;
using Elsa.CustomActivities.Handlers;
using Elsa.CustomActivities.Handlers.Syntax;
using Elsa.CustomActivities.OptionsProviders;
using Elsa.CustomInfrastructure.Data;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomWorkflow.Sdk.Extensions;
using Elsa.Expressions;
using Elsa.Persistence.EntityFramework.Core.Extensions;
using Elsa.Persistence.EntityFramework.SqlServer;
using Elsa.Runtime;
using Elsa.Server.Extensions;
using Elsa.Server.Helpers;
using Elsa.Server.Providers;
using Elsa.Server.Services;
using Elsa.Server.StartupTasks;
using He.PipelineAssessment.Data.Auth;
using MediatR;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;


var builder = WebApplication.CreateBuilder(args);
var elsaConnectionString = builder.Configuration.GetConnectionString("Elsa");
var elsaCustomConnectionString = builder.Configuration.GetConnectionString("ElsaCustom");




// Elsa services.
builder.Services
    .AddElsa(elsa => elsa
        .UseEntityFrameworkPersistence(ef => ef.UseSqlServer(elsaConnectionString, typeof(Elsa.Persistence.EntityFramework.SqlServer.Migrations.Initial)))
        .AddActivity<SinglePipelineDataSource>()
        .AddActivity<PCSProfileDataSource>()
        .AddActivity<VFMDataSource>()
        .AddActivity<HousingNeedDataSource>()
        .AddActivity<QuestionScreen>()
        .AddActivity<CheckYourAnswersScreen>()
        .AddActivity<ConfirmationScreen>()
        .AddActivity<FinishWorkflow>()
        .AddActivity<PotScore>()
        .NoCoreActivities()
        .AddConsoleActivities()
    );

builder.Services.AddScoped<ICustomPropertyDescriber, CustomPropertyDescriber>();

builder.Services.TryAddProvider<IExpressionHandler, InformationTextExpressionHandler>(ServiceLifetime.Singleton);
builder.Services.TryAddProvider<IExpressionHandler, QuestionListExpressionHandler>(ServiceLifetime.Singleton);
builder.Services.TryAddSingleton<INestedSyntaxExpressionHandler, NestedSyntaxExpressionHandler>();

builder.Services.AddDbContext<ElsaCustomContext>(config =>
    config.UseSqlServer(elsaCustomConnectionString,
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


builder.Services.AddMediatR(typeof(Program).Assembly);
builder.Services.AddApplicationInsightsTelemetry();
builder.Services.AddScoped<IDateTimeProvider, DateTimeProvider>();
builder.Services.AddScoped<IActivityDataProvider, ActivityDataProvider>();
builder.Services.AddScoped<IWorkflowRegistryProvider, WorkflowRegistryProvider>();
builder.Services.AddScoped<IWorkflowInstanceProvider, WorkflowInstanceProvider>();
builder.Services.AddScoped<IWorkflowPathProvider, WorkflowPathProvider>();
builder.Services.AddScoped<IWorkflowNextActivityProvider, WorkflowNextActivityProvider>();
builder.Services.AddScoped(typeof(PotScoreOptionsProvider));

builder.Services.AddScoped<IElsaCustomModelHelper, ElsaCustomModelHelper>();

builder.Services.AddScoped<IDeleteChangedWorkflowPathService, DeleteChangedWorkflowPathService>();
builder.Services.AddScoped<INextActivityNavigationService, NextActivityNavigationService>();


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

builder.Services.AddEsriHttpClients(builder.Configuration, builder.Environment.IsDevelopment());

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
    .UseEndpoints(endpoints =>
    {
        // Elsa API Endpoints are implemented as regular ASP.NET Core API controllers.
        endpoints.MapControllers();
        endpoints.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}");
    });


app.Run();