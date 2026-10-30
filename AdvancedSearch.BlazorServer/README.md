# Advanced search (Blazor Server) — EasyQuery on Blazor Server

**Advanced search.** A full-page query builder: users pick the columns, combine any conditions they need, run the query and work with the result — sorting, paging and export.

Part of [EasyQuery samples for ASP.NET Core + Blazor](../README.md).

## Tech stack

| | |
|---|---|
| Host | ASP.NET Core 6 |
| UI | Blazor Server |
| Language | C# |
| EasyQuery | 7.5.0 — `Korzh.EasyQuery.SqLiteGate`, `Korzh.EasyQuery.AspNetCore`, `Korzh.EasyQuery.DataExport`, `Korzh.EasyQuery.EntityFrameworkCore.Relational`, `Korzh.EasyQuery.SqlServerGate` |
| EasyData | 1.6.0 — `EasyData.Exporters.ClosedXML`, `EasyData.Exporters.PdfSharp` |
| Database | SQLite or SQL Server Express LocalDB |

## Prerequisites

* [.NET 6 SDK](https://dotnet.microsoft.com/download)
* Visual Studio 2022 17.2 or later, VS Code or any other editor (optional)
* SQL Server Express LocalDB (installed with Visual Studio)

## Run it

### Visual Studio

1. Open `Samples.Blazor.sln` from the repository root.
2. Set `EqDemo.BlazorServer.AdvancedSearch` as the startup project.
3. Press F5.

### Command line

```bash
cd AdvancedSearch.BlazorServer
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

Ready to buy? See the [licensing options on korzh.com](https://korzh.com/easyquery?utm_source=github&utm_medium=sample&utm_campaign=eq-samples-aspnetcore-blazor&utm_content=AdvancedSearch.BlazorServer#licensing).

## Sample database

The demo database is created and filled automatically on the first start (about a minute or
two). It uses SQLite or SQL Server Express LocalDB; the connection string is in the app's configuration file.

## Related samples

* [Advanced search — ASP.NET Core Razor Pages and MVC](https://github.com/easyquery/eq-samples-aspnetcore-razor-mvc/tree/main/Razor.AdvancedSearch)
* [Advanced search — ASP.NET Core + Angular](https://github.com/easyquery/eq-samples-aspnetcore-angular/tree/main/AdvancedSearch)
* [Advanced search — ASP.NET Core + React](https://github.com/easyquery/eq-samples-aspnetcore-react/tree/main/AdvancedSearch)
* [Advanced search (Vue 3) — ASP.NET Core + Vue](https://github.com/easyquery/eq-samples-aspnetcore-vue/tree/main/Vue3)
* [Advanced search (Vue 2) — ASP.NET Core + Vue](https://github.com/easyquery/eq-samples-aspnetcore-vue/tree/main/Vue2)
* [Advanced search — ASP.NET Core + Stencil](https://github.com/easyquery/eq-samples-aspnetcore-stencil/tree/main/AdvancedSearch)
* [Advanced search — ASP.NET MVC 5 (.NET Framework)](https://github.com/easyquery/eq-samples-aspnet4-mvc/tree/main/AdvancedSearch)
* [Advanced search (separate front-end and service apps) — ASP.NET MVC 5 (.NET Framework)](https://github.com/easyquery/eq-samples-aspnet4-mvc/tree/main/AdvancedSearch-Multilayered)
* [Advanced search (C#) — ASP.NET Web Forms (.NET Framework)](https://github.com/easyquery/eq-samples-aspnet4-webforms/tree/main/AdvancedSearch)
* [Advanced search (Visual Basic) — ASP.NET Web Forms (.NET Framework)](https://github.com/easyquery/eq-samples-aspnet4-webforms/tree/main/VB.AdvancedSearch)
