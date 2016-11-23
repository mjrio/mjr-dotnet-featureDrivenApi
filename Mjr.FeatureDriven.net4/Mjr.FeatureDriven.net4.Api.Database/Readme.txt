Documentation
https://msdn.microsoft.com/en-us/data/ee712907#codefirst
https://coding.abel.nu/2012/03/ef-migrations-command-reference/

Remark:
All commands are run from the package manager console and with the Api.Database project selected.

Enable migration command
Only once to reset migration
Enable-Migrations -ProjectName Mjr.FeatureDriven.net4.Api.Database -StartupProjectName Mjr.FeatureDriven.net4.Api -ContextProjectName Mjr.FeatureDriven.net4.Api -Force

Add a migration for each Version
(Ex:- Add-migration v1.0.0)
As long as you are in the same version (not released) use the -force parameter to rescaffold the migration.
(Ex:- Add-migration v1.0.0 -force)

Get Sql Script
To package a sql script you need to create a sql file :
Update-Database -Script
Then save it using the same name as the created migrations file (with .sql extension) in the script folder

Seeding:
If you want to add seeding, use the seeder class.
This will be used by the update-database command and the unit test createdatabase.


Delete migrations or reset migrations
		a. Delete the migrations folder in your project
		b. Delete the __MigrationHistory table in your database (may be under system tables)
Then run the following command in the Package Manager Console:
Enable-Migrations -ProjectName Mjr.FeatureDriven.net4.Api.Database -StartupProjectName Mjr.FeatureDriven.net4.Api -ContextProjectName Mjr.FeatureDriven.net4.Api -Force
