using Elsa.CustomInfrastructure.Data;
using Elsa.Dashboard;
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

// For Authentication
//builder.AddCustomAuth0Configuration();
//builder.Services.AddCustomAuthentication();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Error"); // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
}

app.Use((context, next) =>
{
  context.Request.Scheme = "https";
  return next();
});

app.UseMiddleware<SecurityHeaderMiddleware>();

app.UseStaticFiles()
    .UseStaticFiles(new StaticFileOptions
    {
      FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"www")),
      ServeUnknownFileTypes = true,
      RequestPath = "/static"
    }) // For Dashboard.
    .UseRouting()
    .UseDirectoryBrowser()
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
