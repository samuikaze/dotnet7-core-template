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

如需使用資料庫，請打開 `Extensions/DatabaseExtension.cs` 將註解全部打開，並修改 DBContext 名稱為正確的名稱，同時請將資料庫連線字串、資料庫帳號與密碼設定完成，否則專案將無法啟動

## Service 與 Repository 類別與介面綁定

Service 與 Repository 的類別與介面需進行綁定，否則 DI 將無法正常注入

1. 打開 `Extensions/ServiceMapperExtension.cs`，依據範例將類別與介面綁定起來

## Repository 專案設定

1. 先將 Repository 專案設定為啟動專案
2. 使用以下指令將指定資料庫中的資料表進行反向工程，建立出 Model 物件

	```Powershell
	Scaffold-DbContext "Server=<SERVER_URI>; Port=<SERVER_PORT>; Database=<DATABASE_NAME>; User ID=<DATABASE_USERNAME>; Password=<DATABASE_PASSWORD>" Pomelo.EntityFrameworkCore.MySql -OutputDir Models -ContextDir DBContexts -Tables <TABLE_NAME> -Project <REPOSITORY_PROJECT_NAME> -Force -NoOnConfiguring
	```

3. 打開 `Extensions/DatabaseExtension.cs`，將最下方的註解打開，並將 DBContext 修改為正確的類別
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

## Middlewares

自行撰寫的 Middleware 的以參考[這篇文章](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/middleware/write?view=aspnetcore-7.0)進行撰寫，並存於 `Middlewares` 資料夾中，
再打開 `Extensions/MiddlewareExtension.cs` 檔，加入 `builder.UseMiddleware<YourMiddlewareClassName>()` 即可。

> 如果需要將資料傳遞到 Controller 中，可以利用 `context.HttpContext.Items` 當作傳遞方法。

## Filter

自行撰寫的 Filter 請放置於 `Filters` 資料夾中，其中若 Filter 中的建構式包含有依賴注入的部份，Controller 使用時請以 `[TypeFilter(typeof(YourFilterName))]` 撰寫。

> 如果需要將資料傳遞到 Controller 中，可以利用 `context.HttpContext.Items` 當作傳遞方法。

## HttpClient

自行撰寫的 HttpClient 請放置於 `HttpClients` 資料夾中，撰寫完後，請打開 `Extensions/HttpClientExtension.cs` 檔，增加 `serviceCollection.AddHttpClient<YourCustomClient>();` 即可。

## 參考資料

### IDE 設定

- [How to config visual studio to use UTF-8 as the default encoding for all projects?](https://stackoverflow.com/a/65945041)

### 基礎建置

- [How can I rename a project folder from within Visual Studio?](https://stackoverflow.com/questions/211241/how-can-i-rename-a-project-folder-from-within-visual-studio)
- [Datetime utc issue after migrating to .NET Core 7](https://stackoverflow.com/a/75580112)
- [Visual Studio: Add existing folder(s) to project](https://stackoverflow.com/a/40491760)
- [! (null-forgiving) operator (C# reference)](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/null-forgiving)
- [在 ASP.NET 核心中啟用跨原始來源要求 （CORS）](https://learn.microsoft.com/zh-tw/aspnet/core/security/cors?view=aspnetcore-7.0)
- [How to enable CORS in ASP.NET Core](https://stackoverflow.com/a/31942128)

### Repository 建置與資料庫連線

- [Using the Repository Pattern with the Entity Framework](https://medium.com/@mlbors/using-the-repository-pattern-with-the-entity-framework-fa4679f2139)
- [Scaffolding (Reverse Engineering)](https://learn.microsoft.com/en-us/ef/core/managing-schemas/scaffolding/?tabs=vs)
- [Get ConnectionString from appsettings.json](https://stackoverflow.com/a/45845041)
- [Setting connection string with username and password in ASP.Core](https://stackoverflow.com/a/41624833)
- [[EF Core] 使用.NET Core CLI建立資料庫實體類型](https://dotblogs.com.tw/jerry809/2019/03/13/105934)
- [Creating a Model for an Existing Database in Entity Framework Core](https://www.entityframeworktutorial.net/efcore/create-model-for-existing-database-in-ef-core.aspx)
- [Pomelo EntityFrameworkCore Mysql - Getting Started](https://github.com/PomeloFoundation/Pomelo.EntityFrameworkCore.MySql/blob/master/README.md#getting-started)

### AutoMapper

- [AutoMapper —— 類別轉換超省力](https://igouist.github.io/post/2020/07/automapper/)
- [Dependency Injection - Automapper documentation](https://docs.automapper.org/en/stable/Dependency-injection.html)

### Configuration

- [Configuration in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-7.0)
- [Non-prefixed environment variables](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-7.0&WT.mc_id=DT-MVP-5002040#non-prefixed-environment-variables)
- [ASP.NET Core Configuration values sometimes returns empty in Kubernetes](https://stackoverflow.com/a/63736378)
- [ASP.NET Core Environment variable colon in Linux](https://stackoverflow.com/a/40094999)

### Docker 相關

- [[Docker] .NET Core 的 Dockerfile 指令詳解](https://www.dotblogs.com.tw/fire/2022/10/27/225738)
- [dotnet publish](https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-publish)
- [使用 dotnet 命令列工具發行 .NET 6 專案](https://blog.darkthread.net/blog/dotnet6-publish-notes/)
- [教學課程：容器化 .NET 應用程式](https://learn.microsoft.com/zh-tw/dotnet/core/docker/build-container?tabs=windows)
- [Override ASP.NET Core appsettings key name that as dots with environment variable in a container](https://stackoverflow.com/a/74596837)

### 驗證與 Swagger 相關

- [Does Swagger (Asp.Net Core) have a controller description?](https://stackoverflow.com/a/56395820)
- [How to add method description in Swagger UI in WebAPI Application](https://stackoverflow.com/a/52958904)
- [Swashbuckle.AspNetCore Include Descriptions From XML Comments](https://github.com/domaindrivendev/Swashbuckle.AspNetCore#include-descriptions-from-xml-comments)
- [ASP.NET Core 驗證的概觀](https://learn.microsoft.com/zh-tw/aspnet/core/security/authentication/?view=aspnetcore-7.0)
- [How to set base path property in swagger for .Net Core Web API](https://stackoverflow.com/a/61966213)
- [What's the difference between HttpRequest.Path and HttpRequest.PathBase in ASP.NET Core?](https://stackoverflow.com/a/58615034)
- [Enabling authentication in swagger](https://stackoverflow.com/questions/71622325/enabling-authentication-in-swagger)
- [在 Swagger UI 加上驗證按鈕，讓 Request Header 傳遞 Authorize Token](https://igouist.github.io/post/2021/10/swagger-enable-authorize/)
- [Using Authorization with Swagger in ASP.NET Core](https://code-maze.com/swagger-authorization-aspnet-core/)
- [是誰在敲打我窗？什麼是 JWT ？](https://5xruby.tw/posts/what-is-jwt)
- [Write custom ASP.NET Core middleware](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/middleware/write?view=aspnetcore-7.0)
- [A web app that calls web APIs: Acquire a token for the app](https://learn.microsoft.com/en-us/azure/active-directory/develop/scenario-web-app-call-api-acquire-token?tabs=aspnetcore)
- [How to Add a BearerToken to an HttpClient Request](https://code-maze.com/add-bearertoken-httpclient-request/)
- [How to get access token from HttpContext in .Net core 2.0](https://stackoverflow.com/a/52887430)
- [Fetch access token from authorization header without bearer prefix](https://stackoverflow.com/a/63311105)
- [Why is StringValues used for Request.Query values?](https://stackoverflow.com/a/48189292)

### HttpClient 相關

- [在 ASP.NET Core 中使用 IHttpClientFactory 發出 HTTP 要求](https://learn.microsoft.com/zh-tw/aspnet/core/fundamentals/http-requests?view=aspnetcore-7.0)
- [.NET Core 中正確使用 HttpClient 的姿勢](https://www.cnblogs.com/willick/p/net-core-httpclient.html)
- [[C#] Web API - HttpClient 入門](https://marcus116.blogspot.com/2018/02/c-web-api-httpclient.html)
- [.net services.AddHttpClient Automatic Access Token Handling](https://stackoverflow.com/a/68306750)
- [HTTP 有效內容回應 - 使用 HttpClient 類別提出 HTTP 要求](https://learn.microsoft.com/zh-tw/dotnet/fundamentals/networking/http/httpclient#http-valid-content-responses)

### Middleware 相關

- [ASP.NET Core 中介軟體](https://learn.microsoft.com/zh-tw/aspnet/core/fundamentals/middleware/?view=aspnetcore-7.0)
- [ASP.NET Core 基礎 - Middleware](https://blog.darkthread.net/blog/aspnetcore-middleware-lab/)
- [Setting middleware for specific controller in ASP.NET Core](https://stackoverflow.com/a/76029185)
- [Custom middleware (or authorize) for specific route in ASP .NET Core 3.1 MVC](https://stackoverflow.com/a/61215529)
- [Use Middleware for some controllers](https://stackoverflow.com/a/66776177)
- [.net-core middleware return blank result](https://stackoverflow.com/a/46139237)
- [Write custom ASP.NET Core middleware](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/middleware/write?view=aspnetcore-7.0)
- [Is it possible to send values to controller from middleware in aspnet core api?](https://stackoverflow.com/a/39022636)
- [ASP.NET Core Middleware Passing Parameters to Controllers](https://stackoverflow.com/a/42789732)

### Filter 相關

- [ASP.NET Core 中的篩選條件](https://learn.microsoft.com/zh-tw/aspnet/core/mvc/controllers/filters?view=aspnetcore-7.0)
- [Custom Authorization Attribute in .Net Core 5](https://stackoverflow.com/a/67259993)
- [How do you create a custom AuthorizeAttribute in ASP.NET Core?](https://stackoverflow.com/a/48228330)
- [ASP.NET Core Web API custom AuthorizeAttribute issue](https://stackoverflow.com/a/66538677)
- [How can I use Dependency Injection in a .Net Core ActionFilterAttribute?](https://stackoverflow.com/a/52725674)
- [async action filter: Async & AuthorizeAttribute in ASP.NET WEB API](https://stackoverflow.com/a/52880347)
- [HttpContext.Items with ASP.NET MVC](https://stackoverflow.com/a/1135703)
- [ASP.NET Core 中的存取 HttpContext](https://learn.microsoft.com/zh-tw/aspnet/core/fundamentals/http-context?view=aspnetcore-7.0)
