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
    .UseStaticFiles() // For Dashboard.
    .UseRouting()
    .UseEndpoints(endpoints =>
    {
        // Elsa API Endpoints are implemented as regular ASP.NET Core API controllers.
        endpoints.MapControllers();

        // For Dashboard.
        endpoints.MapFallbackToPage("/_Host");
    });

app.Run();