using Elsa.CustomInfrastructure.Data;
using Elsa.CustomInfrastructure.Extensions;
using Elsa.CustomWorkflow.Sdk.HttpClients;
using Elsa.Dashboard;
using Elsa.Dashboard.Models;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var elsaCustomConnectionString = builder.Configuration.GetConnectionString("ElsaCustom");

// For Dashboard.
builder.Services.AddRazorPages();

builder.Services.AddScoped<NonceConfig>();

builder.Services.AddDbContext<ElsaCustomContext>(config =>
    config.UseSqlServer(
        elsaCustomConnectionString,
        x => x.MigrationsAssembly("Elsa.CustomInfrastructure")));

builder.Services.AddScoped<DbContext>(provider => provider.GetRequiredService<ElsaCustomContext>());
builder.Services.AddDataProtection().PersistKeysToDbContext<ElsaCustomContext>();

builder.Services.Configure<Urls>(
            builder.Configuration.GetSection("Urls"));

string serverURl = builder.Configuration["Urls:ElsaServer"];
builder.Services.AddHttpClient("ElsaServerClient", client =>
{
  client.BaseAddress = new Uri(serverURl);
});

builder.Services.AddScoped<IElsaServerHttpClient, ElsaServerHttpClient>();

// For Authentication
builder.AddCustomAuth0Configuration();
builder.Services.AddCustomAuthentication();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Error");

}

app.Use((context, next) =>
{
  context.Request.Scheme = "https";
  return next();
});


app.Use((context, next) =>
{
  context.Request.Scheme = "https";
  return next();
});

app.UseMiddleware<SecurityHeaderMiddleware>();

app.UseStaticFiles().UseStaticFiles(new StaticFileOptions
{
  FileProvider =
      new Microsoft.Extensions.FileProviders.PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(),
        @"www")),
  ServeUnknownFileTypes = true,
  RequestPath = "/static"
})
  // For Dashboard.
  .UseRouting()
  .UseAuthentication()
  .UseAuthorization()
  .UseEndpoints(endpoints =>
  {
    // Elsa API Endpoints are implemented as regular ASP.NET Core API controllers.
    endpoints
      .MapControllers();
    // For Dashboard.
    endpoints.MapFallbackToPage("/_Host");
  });

app.Run();
