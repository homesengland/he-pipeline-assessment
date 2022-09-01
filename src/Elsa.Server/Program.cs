using Elsa;
using Elsa.CustomActivities.Activities.Currency;
using Elsa.CustomActivities.Activities.Date;
using Elsa.CustomActivities.Activities.MultipleChoice;
using Elsa.CustomActivities.Activities.Shared;
using Elsa.CustomInfrastructure.Data;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.Persistence.EntityFramework.Core.Extensions;
using Elsa.Persistence.EntityFramework.Sqlite;
using Elsa.Runtime;
using Elsa.Server.Features.Shared;
using Elsa.Server.Features.Shared.SaveAndContinue;
using Elsa.Server.Features.Workflow.LoadWorkflowActivity;
using Elsa.Server.Features.Workflow.StartWorkflow;
using Elsa.Server.Providers;
using Elsa.Server.StartupTasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyActivityLibrary.JavaScript;

var builder = WebApplication.CreateBuilder(args);
var elsaConnectionString = builder.Configuration.GetConnectionString("Elsa");
var pipelineAssessmentConnectionString = builder.Configuration.GetConnectionString("PipelineAssessment");

// Elsa services.
builder.Services
    .AddElsa(elsa => elsa
        .UseEntityFrameworkPersistence(ef => ef.UseSqlite(elsaConnectionString))
        .AddActivity<MultipleChoiceQuestion>()
        .AddActivity<CurrencyQuestion>()
        .AddActivity<DateQuestion>()
        .AddConsoleActivities()
    );

builder.Services.AddDbContext<PipelineAssessmentContext>(config =>
    config.UseSqlite(pipelineAssessmentConnectionString,
        x => x.MigrationsAssembly("Elsa.CustomInfrastructure")));
builder.Services.AddScoped<DbContext>(provider => provider.GetRequiredService<PipelineAssessmentContext>());

//Commenting out for now, as I don't think this is the right approach
//builder.Services.AddWorkflowContextProvider<PipelineAssessmentWorkflowContextProvider>();
builder.Services.AddStartupTask<RunPipelineAssessmentMigrations>();

// Elsa API endpoints.
builder.Services.AddElsaApiEndpoints();

builder.Services.AddNotificationHandlers(typeof(GetMultipleChoiceQuestionScriptHandler));
builder.Services.AddNotificationHandlers(typeof(GetCurrencyQuestionScriptHandler));
builder.Services.AddNotificationHandlers(typeof(GetDateQuestionScriptHandler));

builder.Services.AddBookmarkProvider<QuestionBookmarkProvider>();
builder.Services.AddScoped<IQuestionInvoker, QuestionInvoker>();

builder.Services.AddScoped<IPipelineAssessmentRepository, PipelineAssessmentRepository>();
builder.Services.AddJavaScriptTypeDefinitionProvider<CustomTypeDefinitionProvider>();

builder.Services.AddMediatR(typeof(Program).Assembly);
builder.Services.AddScoped<IDateTimeProvider, DateTimeProvider>();

builder.Services.AddScoped<ISaveAndContinueHandler, SaveAndContinueHandler>();

builder.Services.AddScoped<IStartWorkflowMapper, StartWorkflowMapper>();
builder.Services.AddScoped<ISaveAndContinueMapper, SaveAndContinueMapper>();

builder.Services.AddScoped<ILoadWorkflowActivityJsonHelper, LoadWorkflowActivityJsonHelper>();
builder.Services.AddScoped<ILoadWorkflowActivityMapper, LoadWorkflowActivityMapper>();

// Allow arbitrary client browser apps to access the API.
// In a production environment, make sure to allow only origins you trust.
builder.Services.AddCors(cors => cors.AddDefaultPolicy(policy => policy
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowAnyOrigin()
    .WithExposedHeaders("Content-Disposition"))
);

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