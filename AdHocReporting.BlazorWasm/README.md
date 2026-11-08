# Ad-hoc reporting (Blazor WebAssembly) — EasyQuery on Blazor WebAssembly

**Ad-hoc reporting.** A basic ad-hoc reporting tool: users keep a list of reports, set up the columns and conditions of each one, and see the result as a table (with grouping and totals) or a chart.

Part of [EasyQuery samples for ASP.NET Core + Blazor](../README.md).

## Tech stack

| | |
|---|---|
| Host | ASP.NET Core 10 |
| UI | Blazor WebAssembly |
| Language | C# |
| EasyQuery | 7.5.0 — `Korzh.EasyQuery.AspNetCore`, `Korzh.EasyQuery.DataExport`, `Korzh.EasyQuery.EntityFrameworkCore.Relational`, `Korzh.EasyQuery.EntityFrameworkCore.Identity` |
| EasyData | 1.6.0 — `EasyData.Exporters.ClosedXML`, `EasyData.Exporters.PdfSharp` |
| Database | SQLite or SQL Server Express LocalDB |
| Authentication | ASP.NET Core Identity, cookie sign-in |

## Prerequisites

* [.NET 10 SDK](https://dotnet.microsoft.com/download)
* Visual Studio 2022 17.2 or later, VS Code or any other editor (optional)
* SQL Server Express LocalDB (installed with Visual Studio)

## Run it

### Visual Studio

1. Open `Samples.Blazor.sln` from the repository root.
2. Set `EqDemo.BlazorWasm.AdhocReporting.Server` as the startup project.
3. Press F5.

### Command line

```bash
cd AdHocReporting.BlazorWasm/Server
dotnet run
```

Then open the URL printed in the console.

## License key

You don't need one to start. On the first run the sample shows a **No license** dialog: enter your
email, then the code we send to it, and you get a free 30-day trial key on the spot. The key is saved to `EasyQuery.lck`; restart the sample to apply it.

Already have a key? Put it into `appsettings.json` of the app that serves the EasyQuery API and the dialog won't appear:

```json
"EasyQuery": {
  "LicenseKey": "your key goes here"
}
```

Ready to buy? See the [licensing options on korzh.com](https://korzh.com/easyquery?utm_source=github&utm_medium=sample&utm_campaign=eq-samples-aspnetcore-blazor&utm_content=AdHocReporting.BlazorWasm#licensing).

## Sign-in

The Reports page is for signed-in users only: each user has their own list of reports, and only
users in the `eq-manager` role may create, save or remove them. Log in with the demo account
(`demo@korzh.com` / `demo`, created on the first start), or register a new account — it gets the
`eq-manager` role and a few starter reports.

Authentication is plain ASP.NET Core Identity with its cookie, which is how Microsoft recommends securing a
hosted Blazor WebAssembly app whose client and API share one origin:

* **Server** — `AddDefaultIdentity` stores users and roles in the sample's database (`Data/AppDbContext.cs`, an
  `IdentityDbContext`). Log in, registration and account management are Identity's Razor Pages under
  `Areas/Identity`. Two small endpoints in `Program.cs` serve the client: `api/account/user` tells it who is
  signed in and with which roles, and `api/account/logout` signs out. API requests without a valid cookie get
  `401`/`403` rather than a redirect to the login page.
* **Client** — `Services/ServerAuthenticationStateProvider.cs`, a custom `AuthenticationStateProvider`, builds the
  user from `api/account/user`, so `[Authorize]`, `<AuthorizeView>` and role checks work as usual.
  `Pages/Authentication.razor` sends Log in and Register to the server's Identity pages and performs Log out.
* **EasyQuery** — the widgets call the EasyQuery API from the same origin, so the browser sends the cookie with
  them; there are no access tokens to obtain or pass to the widgets. On the server, `UseDefaultAuthProvider` in
  `Program.cs` decides which EasyQuery actions each user may perform.

The sample used to sign in through Duende IdentityServer (`Microsoft.AspNetCore.ApiAuthorization.IdentityServer`),
which Microsoft no longer develops; it needs neither now.

## Sample database

The demo database is created and filled automatically on the first start (about a minute or
two). It uses SQLite or SQL Server Express LocalDB; the connection string is in the app's configuration file.

## Related samples

* [Ad-hoc reporting — ASP.NET Core Razor Pages and MVC](https://github.com/easyquery/eq-samples-aspnetcore-razor-mvc/tree/main/Razor.AdHocReporting)
* [Ad-hoc reporting (TypeScript) — ASP.NET Core Razor Pages and MVC](https://github.com/easyquery/eq-samples-aspnetcore-razor-mvc/tree/main/RazorTypeScript.AdHocReporting)
* [Ad-hoc reporting — ASP.NET Core + Angular](https://github.com/easyquery/eq-samples-aspnetcore-angular/tree/main/AdHocReporting)
* [Ad-hoc reporting — ASP.NET MVC 5 (.NET Framework)](https://github.com/easyquery/eq-samples-aspnet4-mvc/tree/main/AdhocReporting)
* [Ad-hoc reporting (database metadata) — ASP.NET MVC 5 (.NET Framework)](https://github.com/easyquery/eq-samples-aspnet4-mvc/tree/main/AdhocReportingMetaData)
