using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using LearnerOpsLms.Data;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

var appInsightsConnectionString =
    builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"];

if (!string.IsNullOrWhiteSpace(appInsightsConnectionString))
{
    builder.Services.AddApplicationInsightsTelemetry();
}

// Add services to the container.

builder.Services.AddDbContext<LearnerOpsDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("LearnerOpsDb"),
        sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(10),
                errorNumbersToAdd: null);

            sqlOptions.CommandTimeout(60);    
        }));

builder.Services
    .AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));

    options.AddPolicy("AdminOrAssessor", policy =>
        policy.RequireRole("Admin", "Assessor"));

    options.AddPolicy("AdminAssessorOrLearner", policy =>
        policy.RequireRole("Admin", "Assessor", "Learner"));
});

builder.Services
    .AddControllersWithViews(options =>
    {
        options.Filters.Add(new AuthorizeFilter());
    })
    .AddMicrosoftIdentityUI();

builder.Services.AddRazorPages();

var app = builder.Build();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();