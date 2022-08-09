using Elsa;
using Elsa.CustomActivities.Activities.MultipleChoice;
using Elsa.Persistence.EntityFramework.Core.Extensions;
using Elsa.Persistence.EntityFramework.Sqlite;
using MyActivityLibrary.JavaScript;

var builder = WebApplication.CreateBuilder(args);

// Elsa services.
builder.Services
    .AddElsa(elsa => elsa
        .UseEntityFrameworkPersistence(ef => ef.UseSqlite())
        .AddActivity<MultipleChoiceQuestion>()
        .AddConsoleActivities()
    );

// Elsa API endpoints.
builder.Services.AddElsaApiEndpoints();

builder.Services.AddNotificationHandlers(typeof(GetMultipleChoiceQuestionScriptHandler));

builder.Services.AddBookmarkProvider<MultipleChoiceQuestionBookmarkProvider>();
builder.Services.AddScoped<IMultipleChoiceQuestionInvoker, MultipleChoiceQuestionInvoker>();
builder.Services.AddJavaScriptTypeDefinitionProvider<CustomTypeDefinitionProvider>();

// Allow arbitrary client browser apps to access the API.
// In a production environment, make sure to allow only origins you trust.
builder.Services.AddCors(cors => cors.AddDefaultPolicy(policy => policy
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowAnyOrigin()
    .WithExposedHeaders("Content-Disposition"))
);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app
    .UseCors()
    .UseHttpsRedirection()
    .UseStaticFiles() // For Dashboard.
    .UseRouting()
    .UseEndpoints(endpoints =>
    {
        // Elsa API Endpoints are implemented as regular ASP.NET Core API controllers.
        endpoints.MapControllers();
        endpoints.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}");
    });


app.Run();