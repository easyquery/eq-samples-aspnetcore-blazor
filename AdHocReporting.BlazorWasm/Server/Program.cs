using System.Security.Claims;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using EasyData.Export;
using Korzh.EasyQuery.Services;

using EqDemo.Data;
using EqDemo.Models;
using EqDemo.Services;
using EqDemo.Shared;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("EqDemoSqLite"))
    //options.UseSqlServer(builder.Configuration.GetConnectionString("EqDemoDb"))
);
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(opts => {
    opts.Password.RequiredLength = 4;
    opts.Password.RequireNonAlphanumeric = false;
    opts.Password.RequireLowercase = false;
    opts.Password.RequireUppercase = false;
    opts.Password.RequireDigit = false;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<AppDbContext>();

// The Blazor WebAssembly client is served by this same app, so it signs in with the ordinary
// ASP.NET Core Identity cookie: there are no tokens to obtain, store or attach to requests.
// The browser sends the cookie with every same-origin call, the EasyQuery widgets' calls included.
builder.Services.ConfigureApplicationCookie(options => {
    options.LoginPath = "/Identity/Account/Login";
    options.LogoutPath = "/Identity/Account/Logout";

    // API calls must get a status code, not a redirect to the HTML login page
    options.Events.OnRedirectToLogin = context => RespondToChallenge(context, StatusCodes.Status401Unauthorized);
    options.Events.OnRedirectToAccessDenied = context => RespondToChallenge(context, StatusCodes.Status403Forbidden);

    static Task RespondToChallenge(RedirectContext<CookieAuthenticationOptions> context, int apiStatusCode)
    {
        if (context.Request.Path.StartsWithSegments("/api"))
            context.Response.StatusCode = apiStatusCode;
        else
            context.Response.Redirect(context.RedirectUri);
        return Task.CompletedTask;
    }
});

//EasyQuery services
builder.Services.AddEasyQuery()
        .UseSqlManager()
        .AddDefaultExporters()
        .AddDataExporter<PdfDataExporter>("pdf")
        .AddDataExporter<ExcelDataExporter>("excel");

// add the service that generators default reports
// it's used for demonstration purposes only and can't be deleted in production
builder.Services.AddScoped<DefaultReportGenerator>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

//to support non-Unicode code pages in PDF Exporter
System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseMigrationsEndPoint();
    app.UseWebAssemblyDebugging();
}
else {
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapEasyQuery(options => {
    options.DefaultModelId = "adhoc-reporting";
    options.SaveNewQuery = true;
    options.SaveQueryOnSync = true;
    options.Endpoint = "/api/adhoc-reporting";

    options.UseDbContextWithoutIdentity<AppDbContext>(loaderOptions => {
        //Ignore the "Reports" DbSet as well
        loaderOptions.AddFilter(entity => {
            return entity.ClrType != typeof(Report);
        });
    });

    // here we add our custom query store
    options.UseQueryStore((manager) => new ReportStore(manager.Services));

    options.UseDefaultAuthProvider((provider) => {
        //by default NewQuery, SaveQuery and RemoveQuery actions are accessible by the users with 'eq-manager' role 
        //here you can remove that requirement and make those actions available for all authorized users
        //provider.RequireAuthorization(EqAction.NewQuery, EqAction.SaveQuery, EqAction.RemoveQuery);

        //here is an example how you can make some actions accessible only by users with a particular role.
        //provider.RequireRole(DefaultEqAuthProvider.EqManagerRole, EqAction.NewQuery, EqAction.SaveQuery, EqAction.RemoveQuery);
    });

    options.AddPreFetchTunerWithHttpContext((manager, context) => {
        //the next two lines demonstrate how to add to each generated query a condition that filters data by the current user
        //string userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //manager.Query.ExtraConditions.AddSimpleCondition("Employees.EmployeeID", "Equal", userId);
    });
});

// The client's AuthenticationStateProvider asks here who the current user is
app.MapGet("/api/account/user", (ClaimsPrincipal user) =>
    user.Identity?.IsAuthenticated == true
        ? new UserInfo {
            IsAuthenticated = true,
            Name = user.Identity.Name,
            Roles = user.FindAll(ClaimTypes.Role).Select(c => c.Value).ToArray()
        }
        : new UserInfo());

// Logs the user out without leaving the Blazor app
app.MapPost("/api/account/logout", async (SignInManager<ApplicationUser> signInManager) => {
    await signInManager.SignOutAsync();
    return Results.NoContent();
}).RequireAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

//Init demo database (if necessary)
app.EnsureDbInitializedAsync(builder.Configuration, app.Environment).GetAwaiter().GetResult();

app.Run();
