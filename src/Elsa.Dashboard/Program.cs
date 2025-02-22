using Elsa.CustomInfrastructure.Data;
using Elsa.CustomInfrastructure.Extensions;
using Elsa.CustomWorkflow.Sdk;
using Elsa.CustomWorkflow.Sdk.HttpClients;
using Elsa.Dashboard;
using Elsa.Dashboard.Models;
using Microsoft.AspNetCore.Builder;
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
builder.Services.AddApplicationInsightsTelemetry();

builder.Services.Configure<Urls>(
            builder.Configuration.GetSection("Urls"));

builder.Services.Configure<Auth0Config>(
  builder.Configuration.GetSection("Auth0Config"));

var domain = builder.Configuration["Auth0Config:Domain"]!;
var clientId = builder.Configuration["Auth0Config:MachineToMachineClientId"]!;
var clientSecret = builder.Configuration["Auth0Config:MachineToMachineClientSecret"]!;
var audience = builder.Configuration["Auth0Config:Audience"]!;
var tokenService = new TokenProvider(domain, clientId, clientSecret, audience);
builder.Services.AddSingleton<ITokenProvider>(tokenService);


string serverURl = builder.Configuration["Urls:ElsaServer"]!;
builder.Services.AddHttpClient("ElsaServerClient", client =>
{
  client.BaseAddress = new Uri(serverURl);
});

builder.Services.AddHttpClient("DataDictionaryClient", client =>
{
  client.BaseAddress = new Uri(serverURl);
});


builder.Services.AddScoped<IElsaServerHttpClient, ElsaServerHttpClient>();
builder.Services.AddScoped<IDataDictionaryHttpClient, DataDictionaryHttpClient>();

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
      .MapControllers().RequireAuthorization();
    // For Dashboard.
    endpoints.MapFallbackToPage("/_Host");
  });
app.Run();
