using Elsa.CustomInfrastructure.Data;
using Elsa.CustomWorkflow.Sdk.HttpClients;
using Elsa.Dashboard;
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

string serverURl = builder.Configuration["Urls:ElsaServer"];
builder.Services.AddHttpClient("ElsaServerClient", client =>
{
  client.BaseAddress = new Uri(serverURl);
});

builder.Services.AddScoped<IElsaServerHttpClient, ElsaServerHttpClient>();

var app = builder.Build();
app.UseExceptionHandler("/Error");
//app.UseStatusCodePagesWithReExecute("/Error");

if (!app.Environment.IsDevelopment())
{
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
    .UseEndpoints(endpoints =>
    {
      // Elsa API Endpoints are implemented as regular ASP.NET Core API controllers.
      endpoints.MapControllers();

      // For Dashboard.
      endpoints.MapFallbackToPage("/_Host");
    });

app.Run();
