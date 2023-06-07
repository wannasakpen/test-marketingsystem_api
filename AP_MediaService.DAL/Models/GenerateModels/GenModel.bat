set mypath=%cd%

dotnet script %mypath%\PocosGenerator.csx -- output:DBModel.cs namespace:AP_MediaService.DAL.Models.GenerateModels config:..\..\appsettings.json connectionstring:ConnectionStrings:DefaultConnection dapper:true
