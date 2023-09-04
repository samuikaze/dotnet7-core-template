# DotNet 7 Template

這份檔案將會說明如何從這個模板中開始新的後端解決方案

## 使用前

1. 請先關閉 Visual Studio 後，將資料夾名稱重新命名
2. 以 Visual Studio 重新開啟專案，並針對 Api 專案中 `ProjectReference` 的路徑做修改
	> 若找不到此宣告，則可跳過這步驟
3. 將 API、Repository 專案名稱重新命名
4. 針對專案點選右鍵 -> 同步命名空間
5. 修改 `build-push-deploy.yml` 檔中的 `DotNet7.Template.Api/Dockerfile` 將 `DotNet7.Template.Api` 修改為正確的專案名稱
6. 確認各類別與介面的命名空間是否正確
7. 若有需要，可以修改 `appsettings.Development.json` 中 `Swagger.RoutePrefix` 設定，此用途為設定 API 路徑前綴

另外，若 Visual Studio 將檔案儲存成 UTF-8 With BOM 會造成問題，請依據以下步驟設定：

1. 安裝 `Fix File Encoding` 延伸模組
2. 打開 `工具` > `選項` > 左側選單選擇 `Fix File Encoding`
3. 在右側 `UTF8 without signature files regex` 輸入框中輸入 `\.(.+)$` 全部套用
4. 按下確定完成

## appSettings.json

如需使用資料庫，請打開 `ServiceProviders/DatabaseServiceProvider.cs` 將註解全部打開，並修改 DBContext 名稱為正確的名稱，同時請將資料庫連線字串、資料庫帳號與密碼設定完成，否則專案將無法啟動

## Service 與 Repository 類別與介面綁定

Service 與 Repository 的類別與介面需進行綁定，否則 DI 將無法正常注入

1. 打開 `ServiceProviders/ServiceMapperProvider.cs`，依據範例將類別與介面綁定起來

## Repository 專案設定

1. 先將 Repository 專案設定為啟動專案
2. 使用以下指令將指定資料庫中的資料表進行反向工程，建立出 Model 物件

	```Powershell
	Scaffold-DbContext "Server=<SERVER_URI>; Port=<SERVER_PORT>; Database=<DATABASE_NAME>; User ID=<DATABASE_USERNAME>; Password=<DATABASE_PASSWORD>" Pomelo.EntityFrameworkCore.MySql -OutputDir Models -ContextDir DBContexts -Tables <TABLE_NAME> -Project <REPOSITORY_PROJECT_NAME> -Force -NoOnConfiguring
	```

3. 打開 `ServiceProviders/DatabaseServiceProvider.cs`，將最下方的註解打開，並將 DBContext 修改為正確的類別
	> 若有多個 DBContext 也請在這邊一並宣告
4. 將 Api 專案設定為啟動專案

## AutoMapper Profile 宣告

1. 在 `AutoMapperProfiles` 資料夾下新增一個 任意名稱的 Profile 類別
2. 在類別中宣告需要進行 Mapper 的類別綁定

## 環境變數

ASP.Net 取用設定名稱常使用 `:` 作為階層的分隔，但許多系統並不支援於環境變數名稱中包含 `:` 字符，因此可以 `__` (雙底線) 取代 `:`，其內部會自動做轉換。

## 正式機也可使用 Swagger

若需要於正式機器上使用 Swagger，請打開 `Program.cs`，將

```csharp
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(config =>
    {
        string? path = app.Configuration.GetValue<string>("Swagger:RoutePrefix");
        if (!string.IsNullOrEmpty(path))
        {
            config.PreSerializeFilters.Add((swaggerDoc, httpRequest) =>
            {
                string httpScheme = (app.Environment.IsDevelopment()) ? httpRequest.Scheme : "https";
                swaggerDoc.Servers = new List<OpenApiServer> {
                    new OpenApiServer { Url = $"{httpScheme}://{httpRequest.Host.Value}{path}" }
                };
            });
        }
    });
    app.UseSwaggerUI();
}
```

修改為

```csharp
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger(config =>
    {
        string? path = app.Configuration.GetValue<string>("Swagger:RoutePrefix");
        if (!string.IsNullOrEmpty(path))
        {
            config.PreSerializeFilters.Add((swaggerDoc, httpRequest) =>
            {
                string httpScheme = (app.Environment.IsDevelopment()) ? httpRequest.Scheme : "https";
                swaggerDoc.Servers = new List<OpenApiServer> {
                    new OpenApiServer { Url = $"{httpScheme}://{httpRequest.Host.Value}{path}" }
                };
            });
        }
    });
    app.UseSwaggerUI();
//}
```

另外，若正式機使用的網址包含有自訂的路徑 (例如: ingress 設定)，請將該路徑加入 `Swagger__RoutePrefix` 環境變數中

若正式機未使用 TLS，請打開 `Program.cs` 並找到

```csharp
string httpScheme = (app.Environment.IsDevelopment()) ? httpRequest.Scheme : "https";
```

將之修改為

```csharp
string httpScheme = (app.Environment.IsDevelopment()) ? httpRequest.Scheme : "http";
```

## 參考資料

- [Using the Repository Pattern with the Entity Framework](https://medium.com/@mlbors/using-the-repository-pattern-with-the-entity-framework-fa4679f2139)
- [Scaffolding (Reverse Engineering)](https://learn.microsoft.com/en-us/ef/core/managing-schemas/scaffolding/?tabs=vs)
- [Get ConnectionString from appsettings.json](https://stackoverflow.com/a/45845041)
- [Setting connection string with username and password in ASP.Core](https://stackoverflow.com/a/41624833)
- [[EF Core] 使用.NET Core CLI建立資料庫實體類型](https://dotblogs.com.tw/jerry809/2019/03/13/105934)
- [Creating a Model for an Existing Database in Entity Framework Core](https://www.entityframeworktutorial.net/efcore/create-model-for-existing-database-in-ef-core.aspx)
- [Pomelo EntityFrameworkCore Mysql - Getting Started](https://github.com/PomeloFoundation/Pomelo.EntityFrameworkCore.MySql/blob/master/README.md#getting-started)
- [AutoMapper —— 類別轉換超省力](https://igouist.github.io/post/2020/07/automapper/)
- [Dependency Injection - Automapper documentation](https://docs.automapper.org/en/stable/Dependency-injection.html)
- [How can I rename a project folder from within Visual Studio?](https://stackoverflow.com/questions/211241/how-can-i-rename-a-project-folder-from-within-visual-studio)
- [Does Swagger (Asp.Net Core) have a controller description?](https://stackoverflow.com/a/56395820)
- [Datetime utc issue after migrating to .NET Core 7](https://stackoverflow.com/a/75580112)
- [How to add method description in Swagger UI in WebAPI Application](https://stackoverflow.com/a/52958904)
- [Swashbuckle.AspNetCore Include Descriptions From XML Comments](https://github.com/domaindrivendev/Swashbuckle.AspNetCore#include-descriptions-from-xml-comments)
- [How to config visual studio to use UTF-8 as the default encoding for all projects?](https://stackoverflow.com/a/65945041)
- [Visual Studio: Add existing folder(s) to project](https://stackoverflow.com/a/40491760)
- [[Docker] .NET Core 的 Dockerfile 指令詳解](https://www.dotblogs.com.tw/fire/2022/10/27/225738)
- [dotnet publish](https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-publish)
- [使用 dotnet 命令列工具發行 .NET 6 專案](https://blog.darkthread.net/blog/dotnet6-publish-notes/)
- [教學課程：容器化 .NET 應用程式](https://learn.microsoft.com/zh-tw/dotnet/core/docker/build-container?tabs=windows)
- [Override ASP.NET Core appsettings key name that as dots with environment variable in a container](https://stackoverflow.com/a/74596837)
- [Non-prefixed environment variables](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-7.0&WT.mc_id=DT-MVP-5002040#non-prefixed-environment-variables)
- [ASP.NET Core Configuration values sometimes returns empty in Kubernetes](https://stackoverflow.com/a/63736378)
- [ASP.NET Core Environment variable colon in Linux](https://stackoverflow.com/a/40094999)
- [How to set base path property in swagger for .Net Core Web API](https://stackoverflow.com/a/61966213)
- [What's the difference between HttpRequest.Path and HttpRequest.PathBase in ASP.NET Core?](https://stackoverflow.com/a/58615034)
