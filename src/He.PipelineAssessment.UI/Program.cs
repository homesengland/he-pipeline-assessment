using Elsa.CustomWorkflow.Sdk.HttpClients;
using He.PipelineAssessment.Infrastructure.Data;
using He.PipelineAssessment.UI;
using He.PipelineAssessment.UI.Features.Workflow.SaveAndContinue;
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
    options.ViewLocationFormats.Add($"/Views/Shared/{{0}}{RazorViewEngine.ViewExtension}");
});

string serverURl = builder.Configuration["Urls:ElsaServer"];

//TODO: make this an extension in the SDK
builder.Services.AddHttpClient("ElsaServerClient", client =>
{
    client.BaseAddress = new Uri(serverURl);

});

builder.Services.AddScoped<IElsaServerHttpClient, ElsaServerHttpClient>();
builder.Services.AddScoped<ISaveAndContinueMapper, SaveAndContinueMapper>();
builder.Services.AddScoped<NonceConfig>();


builder.Services.AddMediatR(typeof(Program).Assembly);
builder.Services.AddApplicationInsightsTelemetry();

builder.Services.AddDbContext<PipelineAssessmentContext>(config =>
    config.UseSqlServer(pipelineAssessmentConnectionString,
        x => x.MigrationsAssembly("He.PipelineAssessment.Infrastructure")));

builder.Services.AddScoped<DbContext>(provider => provider.GetRequiredService<PipelineAssessmentContext>());
builder.Services.AddDataProtection().PersistKeysToDbContext<PipelineAssessmentContext>();

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
    app.UseHsts();
}

app.UseMiddleware<SecurityHeaderMiddleware>();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Workflow}/{action=Index}/{id?}");

app.Run();
