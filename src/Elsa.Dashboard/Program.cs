using Elsa.CustomInfrastructure.Data;
using Elsa.CustomInfrastructure.Extensions;
using Elsa.Dashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// For Authentication
builder.AddCustomAuth0Configuration();
//builder.Services.AddCustomAuthentication();

builder.Services.AddAuthorization(options =>
options.FallbackPolicy = options.DefaultPolicy
); ;

// For Dashboard.
builder.Services.AddRazorPages();
builder.Services.AddScoped<NonceConfig>();
builder.Services.AddScoped<DbContext>(provider => provider.GetRequiredService<ElsaCustomContext>());
builder.Services.AddDataProtection().PersistKeysToDbContext<ElsaCustomContext>();



var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseMiddleware<SecurityHeaderMiddleware>();

app
    .UseStaticFiles()
        .UseStaticFiles(new StaticFileOptions
        {
          FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"www")),
          ServeUnknownFileTypes = true,
          RequestPath = "/static"
        })// For Dashboard.

    .UseRouting()
    .UseAuthentication()
    .UseAuthorization()
    .UseDirectoryBrowser()

  //app.MapControllerRoute(
  //    name: "default",
  //    pattern: "{controller=Home}/{action=Index}/{id?}");
  .UseEndpoints(endpoints =>
  {
    // Elsa API Endpoints are implemented as regular ASP.NET Core API controllers.
    endpoints
      .MapControllers();

    // For Dashboard.
    endpoints.MapFallbackToPage("/_Host");
  });





app.Run();

//public static class AppRole
//{

//  public const string PipelineAdmin = "Pipeline.Admin";
//  public const string PipelineAssessor = "Pipeline.Assessor";
//}

//public static class AuthorizationPolicies
//{
//  public const string AssignmentToPipelineAssessorRoleRequired = "AssignmentToPipelineAssessorRoleRequired";
//  public const string AssignmentToPipelineAdminRoleRequired = "AssignmentToPipelineAdminRoleRequired";
//}
