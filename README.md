# DotNet 7 Template

這份檔案將會說明如何從這個模板中開始新的後端解決方案

## 使用前

1. 請先關閉 Visual Studio 後，將資料夾名稱重新命名
2. 以 Visual Studio 重新開啟專案，並針對 Api 專案中 `ProjectReference` 的路徑做修改
	> 若找不到此宣告，則可跳過這步驟
3. 將 API、Repository 專案名稱重新命名
4. 針對專案點選右鍵 -> 同步命名空間
5. 確認各類別與介面的命名空間是否正確

## appSettings.json

如需使用資料庫，請打開 `ServiceProviders/DatabaseServiceProvider.cs` 將註解全部打開，並修改 DBContext 名稱為正確的名稱，同時請將資料庫連線字串、資料庫帳號與密碼設定完成，否則專案將無法啟動

## Service 與 Repository 類別與介面綁定

Service 與 Repository 的類別與介面需進行綁定，否則 DI 將無法正常注入

1. 打開 `ServiceProviders/ServiceMapperProvider.cs`，依據範例將類別與介面綁定起來

## Repository 專案設定

1. 先將 Repository 專案設定為啟動專案
2. 使用以下指令將指定資料庫中的資料表進行反向工程，建立出 Model 物件

	```Powershell
	Scaffold-DbContext "Server=<SERVER_URI>; Port=<SERVER_PORT>; Database=<DATABASE_NAME>; User ID=<DATABASE_USERNAME>; Password=<DATABASE_PASSWORD>" Pomelo.EntityFrameworkCore.MySql -OutputDir Models -ContextDir DBContexts -Tables <TABLE_NAME> Project <REPOSITORY_PROJECT_NAME> -Force -NoOnConfiguring
	```

3. 打開 `ServiceProviders/DatabaseServiceProvider.cs`，將最下方的註解打開，並將 DBContext 修改為正確的類別
	> 若有多個 DBContext 也請在這邊一並宣告
4. 將 Api 專案設定為啟動專案

## AutoMapper Profile 宣告

1. 在 `AutoMapperProfiles` 資料夾下新增一個 任意名稱的 Profile 類別
2. 在類別中宣告需要進行 Mapper 的類別綁定

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
