USE BookSalesDB;
GO

-- 如果表不存在，先手动创建。已有表时可以跳过这一段。
IF OBJECT_ID(N'Users', N'U') IS NULL
BEGIN
    CREATE TABLE Users (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        UserName NVARCHAR(50) NOT NULL UNIQUE,
        Password NVARCHAR(50) NOT NULL,
        RoleName NVARCHAR(20) NOT NULL DEFAULT N'管理员'
    );
END
GO

IF OBJECT_ID(N'Books', N'U') IS NULL
BEGIN
    CREATE TABLE Books (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        BookName NVARCHAR(100) NOT NULL,
        Author NVARCHAR(50) NOT NULL,
        Publisher NVARCHAR(100) NULL,
        Price DECIMAL(10,2) NOT NULL,
        Stock INT NOT NULL,
        CreatedAt DATETIME NOT NULL DEFAULT GETDATE()
    );
END
GO

IF OBJECT_ID(N'Sales', N'U') IS NULL
BEGIN
    CREATE TABLE Sales (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        BookId INT NOT NULL,
        Quantity INT NOT NULL,
        UnitPrice DECIMAL(10,2) NOT NULL,
        TotalAmount AS (Quantity * UnitPrice) PERSISTED,
        SaleTime DATETIME NOT NULL DEFAULT GETDATE(),
        CONSTRAINT FK_Sales_Books FOREIGN KEY (BookId) REFERENCES Books(Id)
    );
END
GO

-- 添加管理员账号
IF NOT EXISTS (SELECT 1 FROM Users WHERE UserName = N'admin')
BEGIN
    INSERT INTO Users(UserName, Password, RoleName)
    VALUES (N'admin', N'123456', N'管理员');
END
GO

-- 批量添加图书记录
INSERT INTO Books(BookName, Author, Publisher, Price, Stock)
VALUES
(N'C# 程序设计教程', N'王小明', N'清华大学出版社', 58.00, 40),
(N'SQL Server 数据库应用', N'李华', N'人民邮电出版社', 66.00, 35),
(N'ASP.NET Core Web 开发', N'赵强', N'机械工业出版社', 88.00, 20),
(N'数据结构与算法', N'陈晨', N'高等教育出版社', 49.80, 50),
(N'计算机网络基础', N'刘洋', N'电子工业出版社', 55.00, 28),
(N'软件工程导论', N'周敏', N'清华大学出版社', 45.00, 32),
(N'Java 程序设计', N'孙磊', N'人民邮电出版社', 62.00, 25),
(N'Python 编程基础', N'吴迪', N'机械工业出版社', 72.00, 45),
(N'操作系统原理', N'郑伟', N'高等教育出版社', 59.50, 22),
(N'数据库系统概论', N'王珊', N'高等教育出版社', 69.00, 30);
GO

-- 添加销售记录。这里使用书名查 BookId，避免手动写错 Id。
INSERT INTO Sales(BookId, Quantity, UnitPrice)
SELECT Id, 2, Price FROM Books WHERE BookName = N'C# 程序设计教程';

INSERT INTO Sales(BookId, Quantity, UnitPrice)
SELECT Id, 1, Price FROM Books WHERE BookName = N'SQL Server 数据库应用';

INSERT INTO Sales(BookId, Quantity, UnitPrice)
SELECT Id, 3, Price FROM Books WHERE BookName = N'Python 编程基础';

INSERT INTO Sales(BookId, Quantity, UnitPrice)
SELECT Id, 1, Price FROM Books WHERE BookName = N'数据结构与算法';
GO

-- 查询全部图书
SELECT Id, BookName, Author, Publisher, Price, Stock, CreatedAt
FROM Books
ORDER BY Id DESC;

-- 按书名或作者模糊查询图书，把 C# 换成你要搜的关键词
SELECT Id, BookName, Author, Publisher, Price, Stock, CreatedAt
FROM Books
WHERE BookName LIKE N'%C#%' OR Author LIKE N'%C#%'
ORDER BY Id DESC;

-- 查询销售明细
SELECT
    s.Id,
    b.BookName,
    b.Author,
    s.Quantity,
    s.UnitPrice,
    s.TotalAmount,
    s.SaleTime
FROM Sales s
INNER JOIN Books b ON s.BookId = b.Id
ORDER BY s.SaleTime DESC;

-- 查询销售总额
SELECT SUM(TotalAmount) AS SaleTotalAmount
FROM Sales;

-- 按图书统计销量和销售额
SELECT
    b.BookName,
    SUM(s.Quantity) AS SaleQuantity,
    SUM(s.TotalAmount) AS SaleAmount
FROM Sales s
INNER JOIN Books b ON s.BookId = b.Id
GROUP BY b.BookName
ORDER BY SaleAmount DESC;
