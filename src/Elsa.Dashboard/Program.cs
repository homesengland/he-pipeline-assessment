var builder = WebApplication.CreateBuilder(args);

// For Dashboard.
builder.Services.AddRazorPages();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app
    //.UseHttpsRedirection()
    .UseStaticFiles()
    .UseStaticFiles(new StaticFileOptions
    {
      FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"dist")),
      ServeUnknownFileTypes = true,
      RequestPath = "/static"
    })
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

app.Run();
