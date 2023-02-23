using Elsa.CustomInfrastructure.Data;
using Elsa.Dashboard;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.DataProtection;
using Elsa.CustomWorkflow.Sdk.HttpClients;

var builder = WebApplication.CreateBuilder(args);

// For Dashboard.
builder.Services.AddRazorPages();
builder.Services.AddScoped<NonceConfig>();
builder.Services.AddScoped<DbContext>(provider => provider.GetRequiredService<ElsaCustomContext>());
builder.Services.AddDataProtection().PersistKeysToDbContext<ElsaCustomContext>();

string serverURl = builder.Configuration["Urls:ElsaServer"];
builder.Services.AddHttpClient("ElsaServerClient", client =>
{
  client.BaseAddress = new Uri(serverURl);

});

builder.Services.AddControllers();

builder.Services.AddScoped<IElsaServerHttpClient, ElsaServerHttpClient>();

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
    .UseDirectoryBrowser()
    .UseEndpoints(endpoints =>
    {
        // Elsa API Endpoints are implemented as regular ASP.NET Core API controllers.
        endpoints.MapControllers();

        // For Dashboard.
        endpoints.MapFallbackToPage("/_Host");


    });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=home}/{action=index}/{id?}");

app.Run();
