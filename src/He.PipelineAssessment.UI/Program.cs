using Elsa.CustomWorkflow.Sdk.Extensions;
using Elsa.CustomWorkflow.Sdk.HttpClients;
using FluentValidation;
using He.PipelineAssessment.Data.Auth;
using He.PipelineAssessment.Data.SinglePipeline;
using He.PipelineAssessment.Infrastructure;
using He.PipelineAssessment.Infrastructure.Data;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Infrastructure.Repository.StoredProcedure;
using He.PipelineAssessment.UI;
using He.PipelineAssessment.UI.Extensions;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.CreateAssessmentTool;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.CreateAssessmentToolWorkflow;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.UpdateAssessmentTool;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.UpdateAssessmentToolWorkflowCommand;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Mappers;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Validators;
using He.PipelineAssessment.UI.Features.SinglePipeline.Sync;
using He.PipelineAssessment.UI.Features.Workflow.QuestionScreenSaveAndContinue;
using MediatR;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


var pipelineAssessmentConnectionString = builder.Configuration.GetConnectionString("SqlDatabase");



// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.Configure<RazorViewEngineOptions>(options =>
{
    options.ViewLocationFormats.Add($"/Features/{{1}}/Views/{{0}}{RazorViewEngine.ViewExtension}");
    options.ViewLocationFormats.Add($"/Features/{{1}}/Views/Shared/{{0}}{RazorViewEngine.ViewExtension}");
    options.ViewLocationFormats.Add($"/Views/Shared/{{0}}{RazorViewEngine.ViewExtension}");
});

string serverURl = builder.Configuration["Urls:ElsaServer"];

//TODO: make this an extension in the SDK
builder.Services.AddHttpClient("ElsaServerClient", client =>
{
    client.BaseAddress = new Uri(serverURl);

});

builder.Services.AddScoped<IElsaServerHttpClient, ElsaServerHttpClient>();
builder.Services.AddScoped<IQuestionScreenSaveAndContinueMapper, QuestionScreenSaveAndContinueMapper>();
builder.Services.AddScoped<IAssessmentToolMapper, AssessmentToolMapper>();
builder.Services.AddScoped<IAssessmentToolWorkflowMapper, AssessmentToolWorkflowMapper>();
builder.Services.AddScoped<NonceConfig>();


builder.Services.AddMediatR(typeof(Program).Assembly);
builder.Services.AddApplicationInsightsTelemetry();

builder.Services.AddDbContext<PipelineAssessmentContext>(config =>
    config.UseSqlServer(pipelineAssessmentConnectionString,
        x => x.MigrationsAssembly("He.PipelineAssessment.Infrastructure")));

builder.Services.AddDbContext<PipelineAssessmentStoreProcContext>(config =>
    config.UseSqlServer(pipelineAssessmentConnectionString));

builder.Services.AddScoped<DbContext>(provider => provider.GetRequiredService<PipelineAssessmentContext>());
builder.Services.AddScoped<DbContext>(provider => provider.GetRequiredService<PipelineAssessmentStoreProcContext>());

//Validators
builder.Services.AddScoped<IValidator<QuestionScreenSaveAndContinueCommand>, SaveAndContinueCommandValidator>();
builder.Services.AddScoped<IValidator<CreateAssessmentToolCommand>, CreateAssessmentToolCommandValidator>();
builder.Services.AddScoped<IValidator<CreateAssessmentToolWorkflowCommand>, CreateAssessmentToolWorkflowCommandValidator>();
builder.Services.AddScoped<IValidator<UpdateAssessmentToolCommand>, UpdateAssessmentToolCommandValidator>();
builder.Services.AddScoped<IValidator<UpdateAssessmentToolWorkflowCommand>, UpdateAssessmentToolWorkflowCommandValidator>();
builder.Services.AddScoped<ISinglePipelineProvider, SinglePipelineProvider>();

builder.Services.AddDataProtection().PersistKeysToDbContext<PipelineAssessmentContext>();
builder.Services.AddScoped<IAssessmentRepository, AssessmentRepository>();
builder.Services.AddScoped<IStoredProcedureRepository, StoredProcedureRepository>();
builder.Services.AddScoped<IAdminAssessmentToolRepository, AdminAssessmentToolRepository>();
builder.Services.AddScoped<IAdminAssessmentToolWorkflowRepository, AdminAssessmentToolWorkflowRepository>();
builder.Services.AddScoped<ISyncCommandHandlerHelper, SyncCommandHandlerHelper>();
builder.Services.AddScoped<IUserProvider, UserProvider>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IIdentityClient, IdentityClient>();
builder.Services.AddTransient<BearerTokenHandler>();

builder.Services.AddOptions<IdentityClientConfig>()
.Configure<IConfiguration>((settings, configuration) =>
{
    configuration.GetSection("IdentityClientConfig").Bind(settings);
});

builder.Services.AddSinglePipelineClient(builder.Configuration, builder.Environment.IsDevelopment());

builder.AddCustomAuth0Configuration();
builder.Services.AddCustomAuthentication();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<PipelineAssessmentContext>();
    context.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error/Index");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
}

app.Use((context, next) =>
{
    context.Request.Scheme = "https";
    return next();
});

app.UseMiddleware<SecurityHeaderMiddleware>();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Assessment}/{action=Index}/{id?}");

app.Run();
