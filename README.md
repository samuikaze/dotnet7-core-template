# DotNet 7 Template

這份檔案將會說明如何從這個模板中開始新的後端解決方案

## Repository 專案設定

1. 先將 Repository 專案設定為啟動專案
2. 使用以下指令將指定資料庫中的資料表進行反向工程，建立出 Model 物件

	```Powershell
	Scaffold-DbContext "connection-string" Pomelo.EntityFrameworkCore.MySql -OutputDir Models -Context DBContexts -Tables <TABLE_NAME> Project DotNet7.Template.Repository -Force -NoOnConfiguration
	```

3. 將 Api 專案設定為啟動專案

## 參考資料

- [Using the Repository Pattern with the Entity Framework](https://medium.com/@mlbors/using-the-repository-pattern-with-the-entity-framework-fa4679f2139)
- [Scaffolding (Reverse Engineering)](https://learn.microsoft.com/en-us/ef/core/managing-schemas/scaffolding/?tabs=vs)
- [[EF Core] 使用.NET Core CLI建立資料庫實體類型](https://dotblogs.com.tw/jerry809/2019/03/13/105934)
- [Creating a Model for an Existing Database in Entity Framework Core](https://www.entityframeworktutorial.net/efcore/create-model-for-existing-database-in-ef-core.aspx)
