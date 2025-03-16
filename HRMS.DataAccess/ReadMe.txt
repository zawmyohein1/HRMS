
-Create Class Library Project

-Install Entity Framework

	dotnet add package Microsoft.EntityFrameworkCore.SqlServer
	dotnet add package Microsoft.EntityFrameworkCore.Design
	dotnet add package Microsoft.EntityFrameworkCore.Tools

-Creat SQL Query

-Run SQL Query



-To create Migration Script run  Command under project root foler
	dotnet ef migrations add InitialCreate --project HRMS.DataAccess --startup-project HRMS.API

-To update the database accroding to migration script

	dotnet ef database update --project HRMS.DataAccess --startup-project HRMS.API

