using Elsa;
using Elsa.CustomActivities.Activities.Currency;
using Elsa.CustomActivities.Activities.Date;
using Elsa.CustomActivities.Activities.MultipleChoice;
using Elsa.CustomActivities.Activities.Shared;
using Elsa.CustomActivities.Activities.SingleChoice;
using Elsa.CustomActivities.Activities.Text;
using Elsa.CustomInfrastructure.Data;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.Persistence.EntityFramework.Core.Extensions;
using Elsa.Persistence.EntityFramework.SqlServer;
using Elsa.Runtime;
using Elsa.Server.Extensions;
using Elsa.Server.Features.Workflow.LoadWorkflowActivity;
using Elsa.Server.Features.Workflow.SaveAndContinue;
using Elsa.Server.Features.Workflow.StartWorkflow;
using Elsa.Server.Providers;
using Elsa.Server.StartupTasks;
using MediatR;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using MyActivityLibrary.JavaScript;

var builder = WebApplication.CreateBuilder(args);
var elsaConnectionString = builder.Configuration.GetConnectionString("Elsa");
var elsaCustomConnectionString = builder.Configuration.GetConnectionString("ElsaCustom");




// Elsa services.
builder.Services
    .AddElsa(elsa => elsa
        .UseEntityFrameworkPersistence(ef => ef.UseSqlServer(elsaConnectionString, typeof(Elsa.Persistence.EntityFramework.SqlServer.Migrations.Initial)))
        .AddActivity<MultipleChoiceQuestion>()
        .AddActivity<SingleChoiceQuestion>()
        .AddActivity<CurrencyQuestion>()
        .AddActivity<TextQuestion>()
        .AddActivity<DateQuestion>()
        .AddConsoleActivities()
    );

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

builder.Services.AddScoped<IPipelineAssessmentRepository, PipelineAssessmentRepository>();
builder.Services.AddJavaScriptTypeDefinitionProvider<CustomTypeDefinitionProvider>();

builder.Services.AddMediatR(typeof(Program).Assembly);
builder.Services.AddApplicationInsightsTelemetry();
builder.Services.AddScoped<IDateTimeProvider, DateTimeProvider>();

//builder.Services.AddScoped<ISaveAndContinueHandler, SaveAndContinueHandler>();

builder.Services.AddScoped<IStartWorkflowMapper, StartWorkflowMapper>();
builder.Services.AddScoped<ISaveAndContinueMapper, SaveAndContinueMapper>();

builder.Services.AddScoped<ILoadWorkflowActivityJsonHelper, LoadWorkflowActivityJsonHelper>();
//builder.Services.AddScoped<ILoadWorkflowActivityMapper, LoadWorkflowActivityMapper>();

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