# Entites Scaffold
## Prerequisites
* 全域安裝 dotnet-ef
```
dotnet tool install --global dotnet-ef
``` 

*安裝基本相關套件
```
Microsoft.EntityFrameworkCore
Microsoft.EntityFrameworkCore.Design
Microsoft.EntityFrameworkCore.Tools(需要使用 Enables these commonly used commands:
Add-Migration 安裝)
URF.Core.EF.Trackable
```

*安裝使用 DB 對應的相關套件
```
Npgsql.EntityFrameworkCore.PostgreSQL
Npgsql.EntityFrameworkCore.PostgreSQL.Design
```

*安裝自動產生 Entity 文本用套件
```
EntityFrameworkCore.Scaffolding.Handlebars

1.安裝後新增 ScaffoldingDesignTimeServices 並重新執行 Entity 更新指令
2.修改 CodeTemplates 內容，並重新執行 Entity 更新指令
```

## Scaffolding
* cmd 移動至 Entities 資料夾並執行下列語法:
**-Context --LineBotContext--

https://docs.microsoft.com/zh-tw/ef/core/cli/dotnet#common-options

``` MSSQL
dotnet ef dbcontext scaffold "Data Source=.\SQLEXPRESS; Initial Catalog=LineBot;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -o Models --context-dir Contexts --context LineBotContext -f
```

``` MySQL
dotnet ef dbcontext scaffold "Server=127.0.0.1; Port=55750; Database=linebot; Uid=azure; Pwd=6#vWHD_$; Character Set=utf8" Pomelo.EntityFrameworkCore.MySql -o Models --context-dir Contexts --context LineBotContext -f
```

``` PostgreSQL
dotnet ef dbcontext scaffold "Host=ec2-52-45-238-24.compute-1.amazonaws.com;Database=d8eb2jm5fse32r;Username=oeibbdsxltadus;Password=150d3b1c00b6541644a8711bb8f06dfb24b971f2e689c6f5c2491a5e0ab63a75;Sslmode=Require;Trust Server Certificate=true" Npgsql.EntityFrameworkCore.PostgreSQL -o Models --context-dir Contexts --context LineBotContext -f
```

## vscode指令
```
 dotnet ef dbcontext scaffold "Data Source=.\SQLEXPRESS; Initial Catalog=LineBot;Trusted_Connection=True; Microsoft.EntityFrameworkCore.SqlServer --output-dir Models --context-dir Contexts --context LineBotContext -f
 ```

* 若無法成功更新 Entities 可刪除資料夾中的 Contexts 及 Models，重新執行上述語法
