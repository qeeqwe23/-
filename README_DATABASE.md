# 数据库一键初始化

本项目使用 SQL Server 数据库 `BookSalesDB`。

## 一键生成

双击仓库根目录下的 `init_database.bat`。

脚本会自动执行 `database_query_and_data.sql`，完成：

- 创建 `BookSalesDB`
- 创建项目需要的表
- 写入演示数据
- 创建默认值和级联触发器

## SQL Server 不是默认实例时

在命令行中指定服务器名：

```bat
init_database.bat .\SQLEXPRESS
```

程序默认连接：

```text
Data Source=.;Initial Catalog=BookSalesDB;Integrated Security=True
```

如果本机连接方式不同，可以设置环境变量 `BOOKSALES_CONN`。
