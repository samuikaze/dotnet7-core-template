# DotNet 7 Template

�o���ɮױN�|�����p��q�o�ӼҪO���}�l�s����ݸѨM���

## Repository �M�׳]�w

1. ���N Repository �M�׳]�w���ҰʱM��
2. �ϥΥH�U���O�N���w��Ʈw������ƪ�i��ϦV�u�{�A�إߥX Model ����

	```Powershell
	Scaffold-DbContext "connection-string" Pomelo.EntityFrameworkCore.MySql -OutputDir Models -Context DBContexts -Tables <TABLE_NAME> Project DotNet7.Template.Repository -Force -NoOnConfiguration
	```

3. �N Api �M�׳]�w���ҰʱM��

## �ѦҸ��

- [Using the Repository Pattern with the Entity Framework](https://medium.com/@mlbors/using-the-repository-pattern-with-the-entity-framework-fa4679f2139)
- [Scaffolding (Reverse Engineering)](https://learn.microsoft.com/en-us/ef/core/managing-schemas/scaffolding/?tabs=vs)
- [[EF Core] �ϥ�.NET Core CLI�إ߸�Ʈw��������](https://dotblogs.com.tw/jerry809/2019/03/13/105934)
- [Creating a Model for an Existing Database in Entity Framework Core](https://www.entityframeworktutorial.net/efcore/create-model-for-existing-database-in-ef-core.aspx)
