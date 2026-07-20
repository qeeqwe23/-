SET NOCOUNT ON;
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
SET ANSI_PADDING ON;
SET ANSI_WARNINGS ON;
SET CONCAT_NULL_YIELDS_NULL ON;
SET ARITHABORT ON;
SET NUMERIC_ROUNDABORT OFF;
GO

PRINT N'开始初始化 BookSalesDB 数据库...';
GO

IF DB_ID(N'BookSalesDB') IS NULL
BEGIN
    CREATE DATABASE BookSalesDB;
END
GO

USE BookSalesDB;
GO

IF OBJECT_ID(N'trg_BookPrices_Cascade', N'TR') IS NOT NULL DROP TRIGGER trg_BookPrices_Cascade;
IF OBJECT_ID(N'trg_BookShelf_Cascade', N'TR') IS NOT NULL DROP TRIGGER trg_BookShelf_Cascade;
IF OBJECT_ID(N'trg_BookInbound_Cascade', N'TR') IS NOT NULL DROP TRIGGER trg_BookInbound_Cascade;
IF OBJECT_ID(N'trg_BookInbound_NameSync', N'TR') IS NOT NULL DROP TRIGGER trg_BookInbound_NameSync;
IF OBJECT_ID(N'trg_BookOutbound_NameSync', N'TR') IS NOT NULL DROP TRIGGER trg_BookOutbound_NameSync;
GO

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
        BookCode NVARCHAR(30) NULL,
        ISBN NVARCHAR(30) NULL,
        BookName NVARCHAR(100) NOT NULL,
        CategoryName NVARCHAR(50) NULL,
        Author NVARCHAR(50) NOT NULL,
        Publisher NVARCHAR(100) NULL,
        PublishedDate DATE NULL,
        Description NVARCHAR(500) NULL,
        Price DECIMAL(10,2) NOT NULL,
        Stock INT NOT NULL,
        MinStock INT NOT NULL DEFAULT 5,
        IsOnSale BIT NOT NULL DEFAULT 1,
        CreatedAt DATETIME NOT NULL DEFAULT GETDATE()
    );
END
GO

IF COL_LENGTH('dbo.Books', 'BookCode') IS NULL
BEGIN
    ALTER TABLE dbo.Books ADD BookCode NVARCHAR(30) NULL;
END
GO

IF COL_LENGTH('dbo.Books', 'ISBN') IS NULL
BEGIN
    ALTER TABLE dbo.Books ADD ISBN NVARCHAR(30) NULL;
END
GO

IF COL_LENGTH('dbo.Books', 'CategoryName') IS NULL
BEGIN
    ALTER TABLE dbo.Books ADD CategoryName NVARCHAR(50) NULL;
END
GO

IF COL_LENGTH('dbo.Books', 'PublishedDate') IS NULL
BEGIN
    ALTER TABLE dbo.Books ADD PublishedDate DATE NULL;
END
GO

IF COL_LENGTH('dbo.Books', 'Description') IS NULL
BEGIN
    ALTER TABLE dbo.Books ADD Description NVARCHAR(500) NULL;
END
GO

IF COL_LENGTH('dbo.Books', 'MinStock') IS NULL
BEGIN
    ALTER TABLE dbo.Books ADD MinStock INT NOT NULL CONSTRAINT DF_Books_MinStock DEFAULT 5 WITH VALUES;
END
GO

IF COL_LENGTH('dbo.Books', 'IsOnSale') IS NULL
BEGIN
    ALTER TABLE dbo.Books ADD IsOnSale BIT NOT NULL CONSTRAINT DF_Books_IsOnSale DEFAULT 1 WITH VALUES;
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

IF OBJECT_ID(N'BookCategories', N'U') IS NULL
BEGIN
    CREATE TABLE BookCategories (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        CategoryCode NVARCHAR(30) NOT NULL UNIQUE,
        CategoryName NVARCHAR(50) NOT NULL,
        ParentCategory NVARCHAR(50) NULL,
        Description NVARCHAR(200) NULL,
        CreatedAt DATETIME NOT NULL DEFAULT GETDATE()
    );
END
GO

IF OBJECT_ID(N'AuthorPublishers', N'U') IS NULL
BEGIN
    CREATE TABLE AuthorPublishers (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        ProfileCode NVARCHAR(30) NOT NULL UNIQUE,
        ProfileType NVARCHAR(20) NOT NULL,
        Name NVARCHAR(100) NOT NULL,
        ContactPerson NVARCHAR(50) NULL,
        Phone NVARCHAR(30) NULL,
        Email NVARCHAR(100) NULL,
        Address NVARCHAR(200) NULL,
        Description NVARCHAR(500) NULL
    );
END
GO

IF OBJECT_ID(N'BookPrices', N'U') IS NULL
BEGIN
    CREATE TABLE BookPrices (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        BookCode NVARCHAR(30) NOT NULL,
        BookName NVARCHAR(100) NOT NULL,
        CostPrice DECIMAL(10,2) NOT NULL,
        SalePrice DECIMAL(10,2) NOT NULL,
        DiscountPrice DECIMAL(10,2) NULL,
        MemberPrice DECIMAL(10,2) NULL,
        EffectiveDate DATE NOT NULL
    );
END
GO

IF OBJECT_ID(N'BookShelfRecords', N'U') IS NULL
BEGIN
    CREATE TABLE BookShelfRecords (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        BookCode NVARCHAR(30) NOT NULL,
        BookName NVARCHAR(100) NOT NULL,
        SaleStatus NVARCHAR(20) NOT NULL,
        OnSaleTime DATETIME NULL,
        OffSaleTime DATETIME NULL,
        OffSaleReason NVARCHAR(200) NULL
    );
END
GO

IF OBJECT_ID(N'Suppliers', N'U') IS NULL
BEGIN
    CREATE TABLE Suppliers (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        SupplierName NVARCHAR(100) NOT NULL,
        ContactPerson NVARCHAR(50) NULL,
        Phone NVARCHAR(30) NULL,
        Address NVARCHAR(200) NULL
    );
END
GO

IF OBJECT_ID(N'PurchaseOrders', N'U') IS NULL
BEGIN
    CREATE TABLE PurchaseOrders (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        PurchaseNo NVARCHAR(30) NOT NULL UNIQUE,
        BookCode NVARCHAR(30) NOT NULL,
        BookName NVARCHAR(100) NOT NULL,
        Quantity INT NOT NULL,
        PurchasePrice DECIMAL(10,2) NOT NULL,
        Supplier NVARCHAR(100) NOT NULL,
        PurchaseDate DATE NOT NULL,
        PurchaseStatus NVARCHAR(20) NOT NULL DEFAULT N'待入库'
    );
END
GO

IF OBJECT_ID(N'BookInboundRecords', N'U') IS NULL
BEGIN
    CREATE TABLE BookInboundRecords (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        InboundNo NVARCHAR(30) NOT NULL UNIQUE,
        PurchaseNo NVARCHAR(30) NULL,
        BookCode NVARCHAR(30) NOT NULL,
        BookName NVARCHAR(100) NOT NULL,
        InboundQuantity INT NOT NULL,
        InboundTime DATETIME NOT NULL DEFAULT GETDATE(),
        OperatorName NVARCHAR(50) NULL
    );
END
GO

IF OBJECT_ID(N'BookOutboundRecords', N'U') IS NULL
BEGIN
    CREATE TABLE BookOutboundRecords (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        OutboundNo NVARCHAR(30) NOT NULL UNIQUE,
        BookCode NVARCHAR(30) NOT NULL,
        BookName NVARCHAR(100) NOT NULL,
        OutboundType NVARCHAR(20) NOT NULL,
        OutboundQuantity INT NOT NULL,
        OutboundTime DATETIME NOT NULL DEFAULT GETDATE(),
        Reason NVARCHAR(200) NULL
    );
END
GO

IF OBJECT_ID(N'InventoryChecks', N'U') IS NULL
BEGIN
    CREATE TABLE InventoryChecks (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        CheckNo NVARCHAR(30) NOT NULL UNIQUE,
        BookCode NVARCHAR(30) NOT NULL,
        BookName NVARCHAR(100) NOT NULL,
        SystemStock INT NOT NULL,
        ActualStock INT NOT NULL,
        DifferenceQuantity AS (ActualStock - SystemStock) PERSISTED,
        CheckResult NVARCHAR(20) NOT NULL,
        CheckDate DATE NOT NULL
    );
END
GO

IF OBJECT_ID(N'StockWarnings', N'U') IS NULL
BEGIN
    CREATE TABLE StockWarnings (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        WarningNo NVARCHAR(30) NOT NULL UNIQUE,
        BookCode NVARCHAR(30) NOT NULL,
        BookName NVARCHAR(100) NOT NULL,
        CurrentStock INT NOT NULL,
        MinStock INT NOT NULL,
        ReplenishmentAdvice NVARCHAR(200) NULL,
        WarningTime DATETIME NOT NULL DEFAULT GETDATE()
    );
END
GO

IF OBJECT_ID(N'Customers', N'U') IS NULL
BEGIN
    CREATE TABLE Customers (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        CustomerCode NVARCHAR(30) NOT NULL UNIQUE,
        CustomerName NVARCHAR(50) NOT NULL,
        Phone NVARCHAR(30) NULL,
        Address NVARCHAR(200) NULL,
        MemberLevel NVARCHAR(20) NOT NULL DEFAULT N'普通会员',
        RegisterDate DATE NOT NULL DEFAULT GETDATE(),
        Remark NVARCHAR(200) NULL
    );
END
GO

IF OBJECT_ID(N'ShoppingCartItems', N'U') IS NULL
BEGIN
    CREATE TABLE ShoppingCartItems (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        CartNo NVARCHAR(30) NOT NULL,
        CustomerCode NVARCHAR(30) NOT NULL,
        BookCode NVARCHAR(30) NOT NULL,
        BookName NVARCHAR(100) NOT NULL,
        Quantity INT NOT NULL,
        AddedTime DATETIME NOT NULL DEFAULT GETDATE()
    );
END
GO

IF OBJECT_ID(N'SaleOrders', N'U') IS NULL
BEGIN
    CREATE TABLE SaleOrders (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        OrderNo NVARCHAR(30) NOT NULL UNIQUE,
        CustomerCode NVARCHAR(30) NOT NULL,
        CustomerName NVARCHAR(50) NOT NULL,
        OrderTime DATETIME NOT NULL DEFAULT GETDATE(),
        OrderStatus NVARCHAR(20) NOT NULL DEFAULT N'待支付',
        OrderAmount DECIMAL(10,2) NOT NULL DEFAULT 0,
        CancelReason NVARCHAR(200) NULL
    );
END
GO

IF OBJECT_ID(N'OrderDetails', N'U') IS NULL
BEGIN
    CREATE TABLE OrderDetails (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        DetailNo NVARCHAR(30) NOT NULL UNIQUE,
        OrderNo NVARCHAR(30) NOT NULL,
        BookCode NVARCHAR(30) NOT NULL,
        BookName NVARCHAR(100) NOT NULL,
        Quantity INT NOT NULL,
        UnitPrice DECIMAL(10,2) NOT NULL,
        Discount DECIMAL(5,2) NOT NULL DEFAULT 1,
        Subtotal AS (Quantity * UnitPrice * Discount) PERSISTED
    );
END
GO

IF OBJECT_ID(N'Payments', N'U') IS NULL
BEGIN
    CREATE TABLE Payments (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        PaymentNo NVARCHAR(30) NOT NULL UNIQUE,
        OrderNo NVARCHAR(30) NOT NULL,
        PaymentMethod NVARCHAR(30) NOT NULL,
        PaymentAmount DECIMAL(10,2) NOT NULL,
        PaymentTime DATETIME NULL,
        PaymentStatus NVARCHAR(20) NOT NULL DEFAULT N'未支付',
        TransactionNo NVARCHAR(50) NULL
    );
END
GO

IF OBJECT_ID(N'OrderDeliveries', N'U') IS NULL
BEGIN
    CREATE TABLE OrderDeliveries (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        DeliveryNo NVARCHAR(30) NOT NULL UNIQUE,
        OrderNo NVARCHAR(30) NOT NULL,
        ReceiverAddress NVARCHAR(200) NOT NULL,
        LogisticsCompany NVARCHAR(100) NULL,
        LogisticsNo NVARCHAR(50) NULL,
        DeliveryTime DATETIME NULL,
        DeliveryStatus NVARCHAR(20) NOT NULL DEFAULT N'未发货'
    );
END
GO

IF OBJECT_ID(N'ReturnRefunds', N'U') IS NULL
BEGIN
    CREATE TABLE ReturnRefunds (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        ReturnNo NVARCHAR(30) NOT NULL UNIQUE,
        OrderNo NVARCHAR(30) NOT NULL,
        BookCode NVARCHAR(30) NOT NULL,
        ReturnQuantity INT NOT NULL,
        ApplyReason NVARCHAR(200) NULL,
        AuditStatus NVARCHAR(20) NOT NULL DEFAULT N'待审核',
        InboundStatus NVARCHAR(20) NOT NULL DEFAULT N'未入库',
        RefundAmount DECIMAL(10,2) NOT NULL DEFAULT 0
    );
END
GO

IF OBJECT_ID(N'CustomerPurchaseRecords', N'U') IS NULL
BEGIN
    CREATE TABLE CustomerPurchaseRecords (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        CustomerCode NVARCHAR(30) NOT NULL,
        CustomerName NVARCHAR(50) NOT NULL,
        OrderNo NVARCHAR(30) NOT NULL,
        BookName NVARCHAR(100) NOT NULL,
        Quantity INT NOT NULL,
        PurchaseAmount DECIMAL(10,2) NOT NULL,
        PurchaseTime DATETIME NOT NULL
    );
END
GO

IF OBJECT_ID(N'SaleStatistics', N'U') IS NULL
BEGIN
    CREATE TABLE SaleStatistics (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        StatisticType NVARCHAR(20) NOT NULL,
        StartDate DATE NOT NULL,
        EndDate DATE NOT NULL,
        SaleQuantity INT NOT NULL,
        SaleAmount DECIMAL(10,2) NOT NULL
    );
END
GO

IF OBJECT_ID(N'BestSellerAnalysis', N'U') IS NULL
BEGIN
    CREATE TABLE BestSellerAnalysis (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        BookCode NVARCHAR(30) NOT NULL,
        BookName NVARCHAR(100) NOT NULL,
        SaleQuantity INT NOT NULL,
        SaleAmount DECIMAL(10,2) NOT NULL,
        Ranking INT NOT NULL,
        AnalysisResult NVARCHAR(100) NULL
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM Users WHERE UserName = N'admin')
BEGIN
    INSERT INTO Users(UserName, Password, RoleName)
    VALUES (N'admin', N'123456', N'管理员');
END
GO

IF NOT EXISTS (SELECT 1 FROM BookCategories WHERE CategoryCode = N'CAT001')
BEGIN
    INSERT INTO BookCategories(CategoryCode, CategoryName, ParentCategory, Description)
    VALUES
    (N'CAT001', N'计算机', NULL, N'编程、数据库、网络等计算机类图书'),
    (N'CAT002', N'文学', NULL, N'小说、散文、诗歌等文学类图书'),
    (N'CAT003', N'教育', NULL, N'教材、考试、教学辅导类图书'),
    (N'CAT004', N'历史', NULL, N'历史、人物传记类图书');
END
GO

IF NOT EXISTS (SELECT 1 FROM Books WHERE BookName = N'C# 程序设计教程')
BEGIN
    INSERT INTO Books(BookCode, ISBN, BookName, Author, Publisher, PublishedDate, Description, Price, Stock, MinStock, IsOnSale)
    VALUES
    (N'B001', N'9787300000011', N'C# 程序设计教程', N'王小明', N'清华大学出版社', '2023-08-01', N'适合课程设计的 C# 入门教材', 58.00, 40, 8, 1),
    (N'B002', N'9787110000022', N'SQL Server 数据库应用', N'李华', N'人民邮电出版社', '2022-06-15', N'数据库设计与查询实践', 66.00, 35, 8, 1),
    (N'B003', N'9787110000033', N'ASP.NET Core Web 开发', N'赵强', N'机械工业出版社', '2024-01-20', N'Web 后端开发基础', 88.00, 20, 5, 1),
    (N'B004', N'9787040000044', N'数据结构与算法', N'陈晨', N'高等教育出版社', '2021-09-01', N'计算机专业基础课程教材', 49.80, 50, 10, 1),
    (N'B005', N'9787120000055', N'计算机网络基础', N'刘洋', N'电子工业出版社', '2023-03-12', N'网络原理与实践', 55.00, 28, 6, 1),
    (N'B006', N'9787300000066', N'软件工程导论', N'周敏', N'清华大学出版社', '2022-11-03', N'软件项目管理与开发过程', 45.00, 32, 6, 1),
    (N'B007', N'9787110000077', N'Java 程序设计', N'孙磊', N'人民邮电出版社', '2021-05-18', N'Java 语言基础教材', 62.00, 25, 5, 1),
    (N'B008', N'9787110000088', N'Python 编程基础', N'吴迪', N'机械工业出版社', '2024-04-10', N'Python 入门与项目实践', 72.00, 45, 9, 1);
END
GO

UPDATE Books SET BookCode = N'B001', ISBN = N'9787300000011', PublishedDate = '2023-08-01', Description = N'适合课程设计的 C# 入门教材', MinStock = 8, IsOnSale = 1 WHERE BookName = N'C# 程序设计教程';
UPDATE Books SET BookCode = N'B002', ISBN = N'9787110000022', PublishedDate = '2022-06-15', Description = N'数据库设计与查询实践', MinStock = 8, IsOnSale = 1 WHERE BookName = N'SQL Server 数据库应用';
UPDATE Books SET BookCode = N'B003', ISBN = N'9787110000033', PublishedDate = '2024-01-20', Description = N'Web 后端开发基础', MinStock = 5, IsOnSale = 1 WHERE BookName = N'ASP.NET Core Web 开发';
UPDATE Books SET BookCode = N'B004', ISBN = N'9787040000044', PublishedDate = '2021-09-01', Description = N'计算机专业基础课程教材', MinStock = 10, IsOnSale = 1 WHERE BookName = N'数据结构与算法';
UPDATE Books SET BookCode = N'B005', ISBN = N'9787120000055', PublishedDate = '2023-03-12', Description = N'网络原理与实践', MinStock = 6, IsOnSale = 1 WHERE BookName = N'计算机网络基础';
UPDATE Books SET BookCode = N'B006', ISBN = N'9787300000066', PublishedDate = '2022-11-03', Description = N'软件项目管理与开发过程', MinStock = 6, IsOnSale = 1 WHERE BookName = N'软件工程导论';
UPDATE Books SET BookCode = N'B007', ISBN = N'9787110000077', PublishedDate = '2021-05-18', Description = N'Java 语言基础教材', MinStock = 5, IsOnSale = 1 WHERE BookName = N'Java 程序设计';
UPDATE Books SET BookCode = N'B008', ISBN = N'9787110000088', PublishedDate = '2024-04-10', Description = N'Python 入门与项目实践', MinStock = 9, IsOnSale = 1 WHERE BookName = N'Python 编程基础';
UPDATE Books SET BookCode = N'B009', ISBN = N'9787040000099', PublishedDate = '2020-09-01', Description = N'操作系统基础教材', MinStock = 5, IsOnSale = 1 WHERE BookName = N'操作系统原理';
UPDATE Books SET BookCode = N'B010', ISBN = N'9787040000105', PublishedDate = '2019-08-01', Description = N'数据库理论基础教材', MinStock = 5, IsOnSale = 1 WHERE BookName = N'数据库系统概论';
GO

IF NOT EXISTS (SELECT 1 FROM AuthorPublishers WHERE ProfileCode = N'A001')
BEGIN
    INSERT INTO AuthorPublishers(ProfileCode, ProfileType, Name, ContactPerson, Phone, Email, Address, Description)
    VALUES
    (N'A001', N'作者', N'王小明', NULL, N'13800000001', N'author01@example.com', NULL, N'C# 与数据库课程作者'),
    (N'P001', N'出版社', N'清华大学出版社', N'张经理', N'010-88880001', N'press01@example.com', N'北京市海淀区', N'计算机教材出版社');
END
GO

IF NOT EXISTS (SELECT 1 FROM Customers WHERE CustomerCode = N'C001')
BEGIN
    INSERT INTO Customers(CustomerCode, CustomerName, Phone, Address, MemberLevel, RegisterDate, Remark)
    VALUES
    (N'C001', N'张三', N'13900000001', N'北京市海淀区', N'普通会员', GETDATE(), N'示例客户'),
    (N'C002', N'李四', N'13900000002', N'上海市浦东新区', N'银卡会员', GETDATE(), N'示例客户');
END
GO

IF NOT EXISTS (SELECT 1 FROM BookPrices)
BEGIN
    INSERT INTO BookPrices(BookCode, BookName, CostPrice, SalePrice, DiscountPrice, MemberPrice, EffectiveDate)
    VALUES
    (N'B001', N'C# 程序设计教程', 38.00, 58.00, 52.00, 49.00, GETDATE()),
    (N'B002', N'SQL Server 数据库应用', 44.00, 66.00, 59.00, 56.00, GETDATE()),
    (N'B008', N'Python 编程基础', 48.00, 72.00, 65.00, 61.00, GETDATE());
END
GO

IF NOT EXISTS (SELECT 1 FROM BookShelfRecords)
BEGIN
    INSERT INTO BookShelfRecords(BookCode, BookName, SaleStatus, OnSaleTime, OffSaleTime, OffSaleReason)
    VALUES
    (N'B001', N'C# 程序设计教程', N'正常销售', DATEADD(DAY, -30, GETDATE()), NULL, NULL),
    (N'B002', N'SQL Server 数据库应用', N'正常销售', DATEADD(DAY, -28, GETDATE()), NULL, NULL),
    (N'B003', N'ASP.NET Core Web 开发', N'暂不上架', DATEADD(DAY, -10, GETDATE()), NULL, N'等待活动定价确认');
END
GO

IF NOT EXISTS (SELECT 1 FROM Suppliers)
BEGIN
    INSERT INTO Suppliers(SupplierName, ContactPerson, Phone, Address)
    VALUES
    (N'北京文轩图书供应中心', N'赵经理', N'010-66660001', N'北京市朝阳区'),
    (N'上海智源教材批发部', N'钱经理', N'021-66660002', N'上海市杨浦区');
END
GO

IF NOT EXISTS (SELECT 1 FROM PurchaseOrders)
BEGIN
    INSERT INTO PurchaseOrders(PurchaseNo, BookCode, BookName, Quantity, PurchasePrice, Supplier, PurchaseDate, PurchaseStatus)
    VALUES
    (N'CG20260719001', N'B001', N'C# 程序设计教程', 30, 38.00, N'北京文轩图书供应中心', GETDATE(), N'已入库'),
    (N'CG20260719002', N'B002', N'SQL Server 数据库应用', 25, 44.00, N'上海智源教材批发部', GETDATE(), N'已入库'),
    (N'CG20260719003', N'B008', N'Python 编程基础', 40, 48.00, N'北京文轩图书供应中心', GETDATE(), N'待入库');
END
GO

IF NOT EXISTS (SELECT 1 FROM BookInboundRecords)
BEGIN
    INSERT INTO BookInboundRecords(InboundNo, PurchaseNo, BookCode, BookName, InboundQuantity, InboundTime, OperatorName)
    VALUES
    (N'RK20260719001', N'CG20260719001', N'B001', N'C# 程序设计教程', 30, GETDATE(), N'管理员'),
    (N'RK20260719002', N'CG20260719002', N'B002', N'SQL Server 数据库应用', 25, GETDATE(), N'管理员');
END
GO

IF NOT EXISTS (SELECT 1 FROM BookOutboundRecords)
BEGIN
    INSERT INTO BookOutboundRecords(OutboundNo, BookCode, BookName, OutboundType, OutboundQuantity, OutboundTime, Reason)
    VALUES
    (N'CK20260719001', N'B001', N'C# 程序设计教程', N'销售出库', 2, GETDATE(), N'客户订单发货'),
    (N'CK20260719002', N'B002', N'SQL Server 数据库应用', N'销售出库', 1, GETDATE(), N'客户订单发货'),
    (N'CK20260719003', N'B004', N'数据结构与算法', N'损坏出库', 1, GETDATE(), N'封面破损');
END
GO

IF NOT EXISTS (SELECT 1 FROM InventoryChecks)
BEGIN
    INSERT INTO InventoryChecks(CheckNo, BookCode, BookName, SystemStock, ActualStock, CheckResult, CheckDate)
    VALUES
    (N'PD20260719001', N'B001', N'C# 程序设计教程', 40, 40, N'实际数量和系统一致', GETDATE()),
    (N'PD20260719002', N'B002', N'SQL Server 数据库应用', 35, 34, N'实际库存少了 1 本', GETDATE()),
    (N'PD20260719003', N'B008', N'Python 编程基础', 45, 47, N'实际库存多了 2 本', GETDATE());
END
GO

IF NOT EXISTS (SELECT 1 FROM StockWarnings)
BEGIN
    INSERT INTO StockWarnings(WarningNo, BookCode, BookName, CurrentStock, MinStock, ReplenishmentAdvice, WarningTime)
    VALUES
    (N'YJ20260719001', N'B003', N'ASP.NET Core Web 开发', 20, 25, N'建议补货 20 本', GETDATE()),
    (N'YJ20260719002', N'B007', N'Java 程序设计', 25, 30, N'建议补货 15 本', GETDATE());
END
GO

IF NOT EXISTS (SELECT 1 FROM ShoppingCartItems)
BEGIN
    INSERT INTO ShoppingCartItems(CartNo, CustomerCode, BookCode, BookName, Quantity, AddedTime)
    VALUES
    (N'GW20260719001', N'C001', N'B001', N'C# 程序设计教程', 1, GETDATE()),
    (N'GW20260719001', N'C001', N'B002', N'SQL Server 数据库应用', 1, GETDATE()),
    (N'GW20260719002', N'C002', N'B008', N'Python 编程基础', 2, GETDATE());
END
GO

IF NOT EXISTS (SELECT 1 FROM SaleOrders)
BEGIN
    INSERT INTO SaleOrders(OrderNo, CustomerCode, CustomerName, OrderTime, OrderStatus, OrderAmount, CancelReason)
    VALUES
    (N'DD20260719001', N'C001', N'张三', GETDATE(), N'已完成', 124.00, NULL),
    (N'DD20260719002', N'C002', N'李四', GETDATE(), N'待发货', 144.00, NULL),
    (N'DD20260719003', N'C001', N'张三', DATEADD(DAY, -1, GETDATE()), N'已取消', 58.00, N'客户重复下单');
END
GO

IF NOT EXISTS (SELECT 1 FROM OrderDetails)
BEGIN
    INSERT INTO OrderDetails(DetailNo, OrderNo, BookCode, BookName, Quantity, UnitPrice, Discount)
    VALUES
    (N'MX20260719001', N'DD20260719001', N'B001', N'C# 程序设计教程', 1, 58.00, 1.00),
    (N'MX20260719002', N'DD20260719001', N'B002', N'SQL Server 数据库应用', 1, 66.00, 1.00),
    (N'MX20260719003', N'DD20260719002', N'B008', N'Python 编程基础', 2, 72.00, 1.00);
END
GO

IF NOT EXISTS (SELECT 1 FROM Payments)
BEGIN
    INSERT INTO Payments(PaymentNo, OrderNo, PaymentMethod, PaymentAmount, PaymentTime, PaymentStatus, TransactionNo)
    VALUES
    (N'ZF20260719001', N'DD20260719001', N'微信支付', 124.00, GETDATE(), N'已支付', N'WX202607190001'),
    (N'ZF20260719002', N'DD20260719002', N'支付宝', 144.00, GETDATE(), N'已支付', N'ALI202607190002'),
    (N'ZF20260719003', N'DD20260719003', N'微信支付', 58.00, NULL, N'已取消', NULL);
END
GO

IF NOT EXISTS (SELECT 1 FROM OrderDeliveries)
BEGIN
    INSERT INTO OrderDeliveries(DeliveryNo, OrderNo, ReceiverAddress, LogisticsCompany, LogisticsNo, DeliveryTime, DeliveryStatus)
    VALUES
    (N'FH20260719001', N'DD20260719001', N'北京市海淀区', N'顺丰速运', N'SF202607190001', GETDATE(), N'已签收'),
    (N'FH20260719002', N'DD20260719002', N'上海市浦东新区', N'中通快递', N'ZT202607190002', NULL, N'待发货');
END
GO

IF NOT EXISTS (SELECT 1 FROM ReturnRefunds)
BEGIN
    INSERT INTO ReturnRefunds(ReturnNo, OrderNo, BookCode, ReturnQuantity, ApplyReason, AuditStatus, InboundStatus, RefundAmount)
    VALUES
    (N'TH20260719001', N'DD20260719001', N'B002', 1, N'客户买错版本', N'待审核', N'未入库', 66.00),
    (N'TH20260719002', N'DD20260719003', N'B001', 1, N'订单取消', N'已完成', N'无需入库', 58.00);
END
GO

IF NOT EXISTS (SELECT 1 FROM CustomerPurchaseRecords)
BEGIN
    INSERT INTO CustomerPurchaseRecords(CustomerCode, CustomerName, OrderNo, BookName, Quantity, PurchaseAmount, PurchaseTime)
    VALUES
    (N'C001', N'张三', N'DD20260719001', N'C# 程序设计教程', 1, 58.00, GETDATE()),
    (N'C001', N'张三', N'DD20260719001', N'SQL Server 数据库应用', 1, 66.00, GETDATE()),
    (N'C002', N'李四', N'DD20260719002', N'Python 编程基础', 2, 144.00, GETDATE());
END
GO

IF NOT EXISTS (SELECT 1 FROM SaleStatistics)
BEGIN
    INSERT INTO SaleStatistics(StatisticType, StartDate, EndDate, SaleQuantity, SaleAmount)
    VALUES
    (N'日统计', CONVERT(DATE, GETDATE()), CONVERT(DATE, GETDATE()), 4, 268.00),
    (N'月统计', DATEFROMPARTS(YEAR(GETDATE()), MONTH(GETDATE()), 1), CONVERT(DATE, GETDATE()), 18, 1186.00),
    (N'年度统计', DATEFROMPARTS(YEAR(GETDATE()), 1, 1), CONVERT(DATE, GETDATE()), 96, 6520.00);
END
GO

IF NOT EXISTS (SELECT 1 FROM BestSellerAnalysis)
BEGIN
    INSERT INTO BestSellerAnalysis(BookCode, BookName, SaleQuantity, SaleAmount, Ranking, AnalysisResult)
    VALUES
    (N'B008', N'Python 编程基础', 32, 2304.00, 1, N'畅销图书，建议持续补货'),
    (N'B001', N'C# 程序设计教程', 28, 1624.00, 2, N'课程教材，销量稳定'),
    (N'B002', N'SQL Server 数据库应用', 20, 1320.00, 3, N'数据库方向常用教材');
END
GO

IF NOT EXISTS (SELECT 1 FROM Sales)
BEGIN
    INSERT INTO Sales(BookId, Quantity, UnitPrice)
    SELECT Id, 2, Price FROM Books WHERE BookName = N'C# 程序设计教程';

    INSERT INTO Sales(BookId, Quantity, UnitPrice)
    SELECT Id, 1, Price FROM Books WHERE BookName = N'SQL Server 数据库应用';

    INSERT INTO Sales(BookId, Quantity, UnitPrice)
    SELECT Id, 3, Price FROM Books WHERE BookName = N'Python 编程基础';
END
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.default_constraints dc
    INNER JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id
    WHERE dc.parent_object_id = OBJECT_ID(N'Books') AND c.name = N'Price'
)
BEGIN
    ALTER TABLE Books ADD CONSTRAINT DF_Books_Price DEFAULT 0 FOR Price;
END
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.default_constraints dc
    INNER JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id
    WHERE dc.parent_object_id = OBJECT_ID(N'Books') AND c.name = N'Stock'
)
BEGIN
    ALTER TABLE Books ADD CONSTRAINT DF_Books_Stock DEFAULT 0 FOR Stock;
END
GO

IF OBJECT_ID(N'trg_Books_Cascade', N'TR') IS NOT NULL
BEGIN
    DROP TRIGGER trg_Books_Cascade;
END
GO

CREATE TRIGGER trg_Books_Cascade
ON Books
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO AuthorPublishers(ProfileCode, ProfileType, Name, Description)
    SELECT DISTINCT
        N'AUTO-A-' + CONVERT(NVARCHAR(20), ABS(CHECKSUM(i.Author))),
        N'作者',
        i.Author,
        N'由图书档案自动维护'
    FROM inserted i
    WHERE ISNULL(i.Author, N'') <> N''
      AND NOT EXISTS (
          SELECT 1 FROM AuthorPublishers ap
          WHERE ap.ProfileType = N'作者' AND ap.Name = i.Author
      );

    INSERT INTO AuthorPublishers(ProfileCode, ProfileType, Name, Description)
    SELECT DISTINCT
        N'AUTO-P-' + CONVERT(NVARCHAR(20), ABS(CHECKSUM(i.Publisher))),
        N'出版社',
        i.Publisher,
        N'由图书档案自动维护'
    FROM inserted i
    WHERE ISNULL(i.Publisher, N'') <> N''
      AND NOT EXISTS (
          SELECT 1 FROM AuthorPublishers ap
          WHERE ap.ProfileType = N'出版社' AND ap.Name = i.Publisher
      );

    INSERT INTO BookPrices(BookCode, BookName, CostPrice, SalePrice, DiscountPrice, MemberPrice, EffectiveDate)
    SELECT
        i.BookCode,
        i.BookName,
        ROUND(i.Price * 0.70, 2),
        i.Price,
        ROUND(i.Price * 0.90, 2),
        ROUND(i.Price * 0.85, 2),
        CONVERT(DATE, GETDATE())
    FROM inserted i
    WHERE ISNULL(i.BookCode, N'') <> N''
      AND NOT EXISTS (SELECT 1 FROM BookPrices p WHERE p.BookCode = i.BookCode);

    INSERT INTO BookShelfRecords(BookCode, BookName, SaleStatus, OnSaleTime)
    SELECT
        i.BookCode,
        i.BookName,
        CASE WHEN i.IsOnSale = 1 THEN N'正常销售' ELSE N'已下架' END,
        CASE WHEN i.IsOnSale = 1 THEN GETDATE() ELSE NULL END
    FROM inserted i
    WHERE ISNULL(i.BookCode, N'') <> N''
      AND NOT EXISTS (SELECT 1 FROM BookShelfRecords s WHERE s.BookCode = i.BookCode);

    UPDATE target SET
        target.BookName = i.BookName,
        target.SalePrice = i.Price,
        target.DiscountPrice = ROUND(i.Price * 0.90, 2),
        target.MemberPrice = ROUND(i.Price * 0.85, 2)
    FROM BookPrices target
    INNER JOIN inserted i ON target.BookCode = i.BookCode;

    UPDATE target SET
        target.BookName = i.BookName,
        target.SaleStatus = CASE
            WHEN i.IsOnSale = 1 THEN N'正常销售'
            WHEN target.SaleStatus = N'暂不上架' THEN N'暂不上架'
            ELSE N'已下架'
        END,
        target.OffSaleTime = CASE WHEN i.IsOnSale = 0 AND target.OffSaleTime IS NULL THEN GETDATE() ELSE target.OffSaleTime END
    FROM BookShelfRecords target
    INNER JOIN inserted i ON target.BookCode = i.BookCode;

    UPDATE target SET target.BookName = i.BookName FROM PurchaseOrders target INNER JOIN inserted i ON target.BookCode = i.BookCode;
    UPDATE target SET target.BookName = i.BookName FROM BookInboundRecords target INNER JOIN inserted i ON target.BookCode = i.BookCode;
    UPDATE target SET target.BookName = i.BookName FROM BookOutboundRecords target INNER JOIN inserted i ON target.BookCode = i.BookCode;
    UPDATE target SET target.BookName = i.BookName FROM InventoryChecks target INNER JOIN inserted i ON target.BookCode = i.BookCode;
    UPDATE target SET target.BookName = i.BookName FROM ShoppingCartItems target INNER JOIN inserted i ON target.BookCode = i.BookCode;
    UPDATE target SET target.BookName = i.BookName FROM OrderDetails target INNER JOIN inserted i ON target.BookCode = i.BookCode;
    UPDATE target SET target.BookName = i.BookName FROM CustomerPurchaseRecords target INNER JOIN inserted i ON target.BookName = i.BookName;
    UPDATE target SET target.BookName = i.BookName FROM BestSellerAnalysis target INNER JOIN inserted i ON target.BookCode = i.BookCode;

    MERGE StockWarnings AS target
    USING (
        SELECT BookCode, BookName, Stock, MinStock
        FROM inserted
        WHERE ISNULL(BookCode, N'') <> N'' AND Stock < MinStock
    ) AS source
    ON target.BookCode = source.BookCode
    WHEN MATCHED THEN
        UPDATE SET
            BookName = source.BookName,
            CurrentStock = source.Stock,
            MinStock = source.MinStock,
            ReplenishmentAdvice = N'建议补货 ' + CONVERT(NVARCHAR(20), source.MinStock - source.Stock + 10) + N' 本',
            WarningTime = GETDATE()
    WHEN NOT MATCHED THEN
        INSERT(WarningNo, BookCode, BookName, CurrentStock, MinStock, ReplenishmentAdvice, WarningTime)
        VALUES(N'YJ' + REPLACE(REPLACE(REPLACE(CONVERT(NVARCHAR(19), GETDATE(), 120), N'-', N''), N':', N''), N' ', N'') + source.BookCode,
               source.BookCode, source.BookName, source.Stock, source.MinStock,
               N'建议补货 ' + CONVERT(NVARCHAR(20), source.MinStock - source.Stock + 10) + N' 本', GETDATE());

    DELETE target
    FROM StockWarnings target
    INNER JOIN inserted i ON target.BookCode = i.BookCode
    WHERE i.Stock >= i.MinStock;
END
GO

IF OBJECT_ID(N'trg_Customers_Cascade', N'TR') IS NOT NULL
BEGIN
    DROP TRIGGER trg_Customers_Cascade;
END
GO

CREATE TRIGGER trg_Customers_Cascade
ON Customers
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE target SET target.CustomerName = i.CustomerName
    FROM SaleOrders target
    INNER JOIN inserted i ON target.CustomerCode = i.CustomerCode;

    UPDATE target SET target.CustomerName = i.CustomerName
    FROM CustomerPurchaseRecords target
    INNER JOIN inserted i ON target.CustomerCode = i.CustomerCode;
END
GO

IF OBJECT_ID(N'trg_BookInbound_Cascade', N'TR') IS NOT NULL
BEGIN
    DROP TRIGGER trg_BookInbound_Cascade;
END
GO

CREATE TRIGGER trg_BookInbound_Cascade
ON BookInboundRecords
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE b
    SET b.Stock = b.Stock + i.InboundQuantity
    FROM Books b
    INNER JOIN inserted i ON b.BookCode = i.BookCode;

    UPDATE p
    SET p.PurchaseStatus = N'已入库'
    FROM PurchaseOrders p
    INNER JOIN inserted i ON p.PurchaseNo = i.PurchaseNo;
END
GO

IF OBJECT_ID(N'trg_BookOutbound_Cascade', N'TR') IS NOT NULL
BEGIN
    DROP TRIGGER trg_BookOutbound_Cascade;
END
GO

CREATE TRIGGER trg_BookOutbound_Cascade
ON BookOutboundRecords
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE b
    SET b.Stock = CASE WHEN b.Stock >= i.OutboundQuantity THEN b.Stock - i.OutboundQuantity ELSE 0 END
    FROM Books b
    INNER JOIN inserted i ON b.BookCode = i.BookCode;
END
GO

IF OBJECT_ID(N'trg_OrderDetails_Cascade', N'TR') IS NOT NULL
BEGIN
    DROP TRIGGER trg_OrderDetails_Cascade;
END
GO

CREATE TRIGGER trg_OrderDetails_Cascade
ON OrderDetails
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE d
    SET
        d.BookName = b.BookName,
        d.UnitPrice = CASE WHEN d.UnitPrice = 0 THEN b.Price ELSE d.UnitPrice END
    FROM OrderDetails d
    INNER JOIN inserted i ON d.Id = i.Id
    INNER JOIN Books b ON d.BookCode = b.BookCode;

    UPDATE o
    SET o.OrderAmount = ISNULL(total.Amount, 0)
    FROM SaleOrders o
    INNER JOIN (
        SELECT OrderNo FROM inserted
        UNION
        SELECT OrderNo FROM deleted
    ) changed ON o.OrderNo = changed.OrderNo
    OUTER APPLY (
        SELECT SUM(Subtotal) AS Amount
        FROM OrderDetails d
        WHERE d.OrderNo = o.OrderNo
    ) total;

    DELETE r
    FROM CustomerPurchaseRecords r
    INNER JOIN (
        SELECT OrderNo FROM inserted
        UNION
        SELECT OrderNo FROM deleted
    ) changed ON r.OrderNo = changed.OrderNo;

    INSERT INTO CustomerPurchaseRecords(CustomerCode, CustomerName, OrderNo, BookName, Quantity, PurchaseAmount, PurchaseTime)
    SELECT o.CustomerCode, o.CustomerName, d.OrderNo, d.BookName, d.Quantity, d.Subtotal, o.OrderTime
    FROM OrderDetails d
    INNER JOIN SaleOrders o ON d.OrderNo = o.OrderNo
    INNER JOIN (
        SELECT OrderNo FROM inserted
        UNION
        SELECT OrderNo FROM deleted
    ) changed ON d.OrderNo = changed.OrderNo
    WHERE o.OrderStatus <> N'已取消';
END
GO

IF OBJECT_ID(N'trg_Payments_Cascade', N'TR') IS NOT NULL
BEGIN
    DROP TRIGGER trg_Payments_Cascade;
END
GO

CREATE TRIGGER trg_Payments_Cascade
ON Payments
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE o
    SET o.OrderStatus = CASE WHEN i.PaymentStatus = N'已支付' THEN N'待发货' ELSE o.OrderStatus END
    FROM SaleOrders o
    INNER JOIN inserted i ON o.OrderNo = i.OrderNo;
END
GO

INSERT INTO AuthorPublishers(ProfileCode, ProfileType, Name, Description)
SELECT DISTINCT N'AUTO-A-' + CONVERT(NVARCHAR(20), ABS(CHECKSUM(Author))), N'作者', Author, N'由图书档案自动维护'
FROM Books b
WHERE ISNULL(Author, N'') <> N''
  AND NOT EXISTS (SELECT 1 FROM AuthorPublishers ap WHERE ap.ProfileType = N'作者' AND ap.Name = b.Author);
GO

INSERT INTO AuthorPublishers(ProfileCode, ProfileType, Name, Description)
SELECT DISTINCT N'AUTO-P-' + CONVERT(NVARCHAR(20), ABS(CHECKSUM(Publisher))), N'出版社', Publisher, N'由图书档案自动维护'
FROM Books b
WHERE ISNULL(Publisher, N'') <> N''
  AND NOT EXISTS (SELECT 1 FROM AuthorPublishers ap WHERE ap.ProfileType = N'出版社' AND ap.Name = b.Publisher);
GO

INSERT INTO BookPrices(BookCode, BookName, CostPrice, SalePrice, DiscountPrice, MemberPrice, EffectiveDate)
SELECT BookCode, BookName, ROUND(Price * 0.70, 2), Price, ROUND(Price * 0.90, 2), ROUND(Price * 0.85, 2), CONVERT(DATE, GETDATE())
FROM Books b
WHERE ISNULL(BookCode, N'') <> N''
  AND NOT EXISTS (SELECT 1 FROM BookPrices p WHERE p.BookCode = b.BookCode);
GO

INSERT INTO BookShelfRecords(BookCode, BookName, SaleStatus, OnSaleTime)
SELECT BookCode, BookName, CASE WHEN IsOnSale = 1 THEN N'正常销售' ELSE N'已下架' END, CASE WHEN IsOnSale = 1 THEN GETDATE() ELSE NULL END
FROM Books b
WHERE ISNULL(BookCode, N'') <> N''
  AND NOT EXISTS (SELECT 1 FROM BookShelfRecords s WHERE s.BookCode = b.BookCode);
GO

IF OBJECT_ID(N'trg_ShoppingCart_Cascade', N'TR') IS NOT NULL
BEGIN
    DROP TRIGGER trg_ShoppingCart_Cascade;
END
GO

CREATE TRIGGER trg_ShoppingCart_Cascade
ON ShoppingCartItems
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE cart
    SET cart.BookName = b.BookName
    FROM ShoppingCartItems cart
    INNER JOIN inserted i ON cart.Id = i.Id
    INNER JOIN Books b ON cart.BookCode = b.BookCode;

    DELETE cart
    FROM ShoppingCartItems cart
    INNER JOIN inserted i ON cart.Id = i.Id
    WHERE EXISTS (
        SELECT 1
        FROM SaleOrders o
        INNER JOIN OrderDetails d ON o.OrderNo = d.OrderNo
        WHERE o.CustomerCode = cart.CustomerCode
          AND d.BookCode = cart.BookCode
          AND o.OrderStatus <> N'已取消'
    );
END
GO

IF OBJECT_ID(N'trg_SaleOrders_Cascade', N'TR') IS NOT NULL
BEGIN
    DROP TRIGGER trg_SaleOrders_Cascade;
END
GO

CREATE TRIGGER trg_SaleOrders_Cascade
ON SaleOrders
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE o
    SET o.CustomerName = c.CustomerName
    FROM SaleOrders o
    INNER JOIN inserted i ON o.Id = i.Id
    INNER JOIN Customers c ON o.CustomerCode = c.CustomerCode;

    DELETE r
    FROM CustomerPurchaseRecords r
    INNER JOIN inserted i ON r.OrderNo = i.OrderNo
    WHERE i.OrderStatus = N'已取消';

    DELETE p
    FROM Payments p
    INNER JOIN inserted i ON p.OrderNo = i.OrderNo
    WHERE i.OrderStatus = N'已取消';

    DELETE d
    FROM OrderDeliveries d
    INNER JOIN inserted i ON d.OrderNo = i.OrderNo
    WHERE i.OrderStatus = N'已取消';
END
GO

IF OBJECT_ID(N'trg_Delivery_Cascade', N'TR') IS NOT NULL
BEGIN
    DROP TRIGGER trg_Delivery_Cascade;
END
GO

CREATE TRIGGER trg_Delivery_Cascade
ON OrderDeliveries
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE o
    SET o.OrderStatus =
        CASE
            WHEN i.DeliveryStatus IN (N'已签收', N'已完成') THEN N'已完成'
            WHEN i.DeliveryStatus IN (N'已发货', N'配送中') THEN N'已发货'
            ELSE o.OrderStatus
        END
    FROM SaleOrders o
    INNER JOIN inserted i ON o.OrderNo = i.OrderNo
    WHERE o.OrderStatus <> N'已取消';
END
GO

IF OBJECT_ID(N'trg_ReturnRefund_Cascade', N'TR') IS NOT NULL
BEGIN
    DROP TRIGGER trg_ReturnRefund_Cascade;
END
GO

CREATE TRIGGER trg_ReturnRefund_Cascade
ON ReturnRefunds
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    IF TRIGGER_NESTLEVEL() > 1 RETURN;

    UPDATE r
    SET r.RefundAmount =
        ISNULL(d.UnitPrice, 0) * r.ReturnQuantity
    FROM ReturnRefunds r
    INNER JOIN inserted i ON r.Id = i.Id
    LEFT JOIN OrderDetails d ON r.OrderNo = d.OrderNo AND r.BookCode = d.BookCode
    WHERE r.RefundAmount = 0;

    UPDATE o
    SET o.OrderStatus = N'已退货'
    FROM SaleOrders o
    INNER JOIN inserted i ON o.OrderNo = i.OrderNo
    WHERE i.AuditStatus IN (N'已完成', N'已通过', N'正常');

    INSERT INTO BookInboundRecords(InboundNo, PurchaseNo, BookCode, BookName, InboundQuantity, InboundTime, OperatorName)
    SELECT
        N'THRK' + CONVERT(NVARCHAR(20), i.Id),
        NULL,
        i.BookCode,
        ISNULL(b.BookName, i.BookCode),
        i.ReturnQuantity,
        GETDATE(),
        N'退货入库'
    FROM inserted i
    LEFT JOIN Books b ON i.BookCode = b.BookCode
    WHERE i.AuditStatus IN (N'已完成', N'已通过', N'正常')
      AND NOT EXISTS (
          SELECT 1 FROM BookInboundRecords r WHERE r.InboundNo = N'THRK' + CONVERT(NVARCHAR(20), i.Id)
      );
END
GO

IF OBJECT_ID(N'trg_OrderDetails_CartCleanup', N'TR') IS NOT NULL
BEGIN
    DROP TRIGGER trg_OrderDetails_CartCleanup;
END
GO

CREATE TRIGGER trg_OrderDetails_CartCleanup
ON OrderDetails
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    DELETE cart
    FROM ShoppingCartItems cart
    INNER JOIN SaleOrders o ON cart.CustomerCode = o.CustomerCode
    INNER JOIN inserted i ON o.OrderNo = i.OrderNo AND cart.BookCode = i.BookCode
    WHERE o.OrderStatus <> N'已取消';
END
GO

DELETE cart
FROM ShoppingCartItems cart
WHERE EXISTS (
    SELECT 1
    FROM SaleOrders o
    INNER JOIN OrderDetails d ON o.OrderNo = d.OrderNo
    WHERE o.CustomerCode = cart.CustomerCode
      AND d.BookCode = cart.BookCode
      AND o.OrderStatus <> N'已取消'
);
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.default_constraints dc
    INNER JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id
    INNER JOIN sys.tables t ON c.object_id = t.object_id
    WHERE t.name = N'OrderDetails' AND c.name = N'DetailNo'
)
BEGIN
    ALTER TABLE OrderDetails ADD CONSTRAINT DF_OrderDetails_DetailNo DEFAULT (N'MX' + LEFT(REPLACE(CONVERT(NVARCHAR(36), NEWID()), N'-', N''), 28)) FOR DetailNo;
END
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.default_constraints dc
    INNER JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id
    INNER JOIN sys.tables t ON c.object_id = t.object_id
    WHERE t.name = N'OrderDetails' AND c.name = N'Discount'
)
BEGIN
    ALTER TABLE OrderDetails ADD CONSTRAINT DF_OrderDetails_Discount DEFAULT (1) FOR Discount;
END
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.default_constraints dc
    INNER JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id
    INNER JOIN sys.tables t ON c.object_id = t.object_id
    WHERE t.name = N'ShoppingCartItems' AND c.name = N'CartNo'
)
BEGIN
    ALTER TABLE ShoppingCartItems ADD CONSTRAINT DF_ShoppingCartItems_CartNo DEFAULT (N'GW' + LEFT(REPLACE(CONVERT(NVARCHAR(36), NEWID()), N'-', N''), 28)) FOR CartNo;
END
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.default_constraints dc
    INNER JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id
    INNER JOIN sys.tables t ON c.object_id = t.object_id
    WHERE t.name = N'Payments' AND c.name = N'PaymentNo'
)
BEGIN
    ALTER TABLE Payments ADD CONSTRAINT DF_Payments_PaymentNo DEFAULT (N'FK' + LEFT(REPLACE(CONVERT(NVARCHAR(36), NEWID()), N'-', N''), 28)) FOR PaymentNo;
END
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.default_constraints dc
    INNER JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id
    INNER JOIN sys.tables t ON c.object_id = t.object_id
    WHERE t.name = N'OrderDeliveries' AND c.name = N'DeliveryNo'
)
BEGIN
    ALTER TABLE OrderDeliveries ADD CONSTRAINT DF_OrderDeliveries_DeliveryNo DEFAULT (N'FH' + LEFT(REPLACE(CONVERT(NVARCHAR(36), NEWID()), N'-', N''), 28)) FOR DeliveryNo;
END
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.default_constraints dc
    INNER JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id
    INNER JOIN sys.tables t ON c.object_id = t.object_id
    WHERE t.name = N'ReturnRefunds' AND c.name = N'ReturnNo'
)
BEGIN
    ALTER TABLE ReturnRefunds ADD CONSTRAINT DF_ReturnRefunds_ReturnNo DEFAULT (N'TH' + LEFT(REPLACE(CONVERT(NVARCHAR(36), NEWID()), N'-', N''), 28)) FOR ReturnNo;
END
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.default_constraints dc
    INNER JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id
    INNER JOIN sys.tables t ON c.object_id = t.object_id
    WHERE t.name = N'BookPrices' AND c.name = N'EffectiveDate'
)
BEGIN
    ALTER TABLE BookPrices ADD CONSTRAINT DF_BookPrices_EffectiveDate DEFAULT (CONVERT(DATE, GETDATE())) FOR EffectiveDate;
END
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.default_constraints dc
    INNER JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id
    INNER JOIN sys.tables t ON c.object_id = t.object_id
    WHERE t.name = N'PurchaseOrders' AND c.name = N'PurchaseDate'
)
BEGIN
    ALTER TABLE PurchaseOrders ADD CONSTRAINT DF_PurchaseOrders_PurchaseDate DEFAULT (CONVERT(DATE, GETDATE())) FOR PurchaseDate;
END
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.default_constraints dc
    INNER JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id
    INNER JOIN sys.tables t ON c.object_id = t.object_id
    WHERE t.name = N'InventoryChecks' AND c.name = N'CheckDate'
)
BEGIN
    ALTER TABLE InventoryChecks ADD CONSTRAINT DF_InventoryChecks_CheckDate DEFAULT (CONVERT(DATE, GETDATE())) FOR CheckDate;
END
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.default_constraints dc
    INNER JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id
    INNER JOIN sys.tables t ON c.object_id = t.object_id
    WHERE t.name = N'StockWarnings' AND c.name = N'WarningNo'
)
BEGIN
    ALTER TABLE StockWarnings ADD CONSTRAINT DF_StockWarnings_WarningNo DEFAULT (N'YJ' + LEFT(REPLACE(CONVERT(NVARCHAR(36), NEWID()), N'-', N''), 28)) FOR WarningNo;
END
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.default_constraints dc
    INNER JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id
    INNER JOIN sys.tables t ON c.object_id = t.object_id
    WHERE t.name = N'CustomerPurchaseRecords' AND c.name = N'PurchaseTime'
)
BEGIN
    ALTER TABLE CustomerPurchaseRecords ADD CONSTRAINT DF_CustomerPurchaseRecords_PurchaseTime DEFAULT (GETDATE()) FOR PurchaseTime;
END
GO

IF OBJECT_ID(N'DF_OrderDetails_DetailNo', N'D') IS NOT NULL
BEGIN
    ALTER TABLE OrderDetails DROP CONSTRAINT DF_OrderDetails_DetailNo;
END
GO
ALTER TABLE OrderDetails ADD CONSTRAINT DF_OrderDetails_DetailNo DEFAULT (N'MX' + LEFT(REPLACE(CONVERT(NVARCHAR(36), NEWID()), N'-', N''), 28)) FOR DetailNo;
GO

IF OBJECT_ID(N'DF_ShoppingCartItems_CartNo', N'D') IS NOT NULL
BEGIN
    ALTER TABLE ShoppingCartItems DROP CONSTRAINT DF_ShoppingCartItems_CartNo;
END
GO
ALTER TABLE ShoppingCartItems ADD CONSTRAINT DF_ShoppingCartItems_CartNo DEFAULT (N'GW' + LEFT(REPLACE(CONVERT(NVARCHAR(36), NEWID()), N'-', N''), 28)) FOR CartNo;
GO

IF OBJECT_ID(N'DF_Payments_PaymentNo', N'D') IS NOT NULL
BEGIN
    ALTER TABLE Payments DROP CONSTRAINT DF_Payments_PaymentNo;
END
GO
ALTER TABLE Payments ADD CONSTRAINT DF_Payments_PaymentNo DEFAULT (N'FK' + LEFT(REPLACE(CONVERT(NVARCHAR(36), NEWID()), N'-', N''), 28)) FOR PaymentNo;
GO

IF OBJECT_ID(N'DF_OrderDeliveries_DeliveryNo', N'D') IS NOT NULL
BEGIN
    ALTER TABLE OrderDeliveries DROP CONSTRAINT DF_OrderDeliveries_DeliveryNo;
END
GO
ALTER TABLE OrderDeliveries ADD CONSTRAINT DF_OrderDeliveries_DeliveryNo DEFAULT (N'FH' + LEFT(REPLACE(CONVERT(NVARCHAR(36), NEWID()), N'-', N''), 28)) FOR DeliveryNo;
GO

IF OBJECT_ID(N'DF_ReturnRefunds_ReturnNo', N'D') IS NOT NULL
BEGIN
    ALTER TABLE ReturnRefunds DROP CONSTRAINT DF_ReturnRefunds_ReturnNo;
END
GO
ALTER TABLE ReturnRefunds ADD CONSTRAINT DF_ReturnRefunds_ReturnNo DEFAULT (N'TH' + LEFT(REPLACE(CONVERT(NVARCHAR(36), NEWID()), N'-', N''), 28)) FOR ReturnNo;
GO

IF OBJECT_ID(N'DF_StockWarnings_WarningNo', N'D') IS NOT NULL
BEGIN
    ALTER TABLE StockWarnings DROP CONSTRAINT DF_StockWarnings_WarningNo;
END
GO
ALTER TABLE StockWarnings ADD CONSTRAINT DF_StockWarnings_WarningNo DEFAULT (N'YJ' + LEFT(REPLACE(CONVERT(NVARCHAR(36), NEWID()), N'-', N''), 28)) FOR WarningNo;
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.default_constraints dc
    INNER JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id
    INNER JOIN sys.tables t ON c.object_id = t.object_id
    WHERE t.name = N'BookPrices' AND c.name = N'BookName'
)
BEGIN
    ALTER TABLE BookPrices ADD CONSTRAINT DF_BookPrices_BookName DEFAULT (N'') FOR BookName;
END
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.default_constraints dc
    INNER JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id
    INNER JOIN sys.tables t ON c.object_id = t.object_id
    WHERE t.name = N'BookShelfRecords' AND c.name = N'BookName'
)
BEGIN
    ALTER TABLE BookShelfRecords ADD CONSTRAINT DF_BookShelfRecords_BookName DEFAULT (N'') FOR BookName;
END
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.default_constraints dc
    INNER JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id
    INNER JOIN sys.tables t ON c.object_id = t.object_id
    WHERE t.name = N'PurchaseOrders' AND c.name = N'BookName'
)
BEGIN
    ALTER TABLE PurchaseOrders ADD CONSTRAINT DF_PurchaseOrders_BookName DEFAULT (N'') FOR BookName;
END
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.default_constraints dc
    INNER JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id
    INNER JOIN sys.tables t ON c.object_id = t.object_id
    WHERE t.name = N'BookInboundRecords' AND c.name = N'BookName'
)
BEGIN
    ALTER TABLE BookInboundRecords ADD CONSTRAINT DF_BookInboundRecords_BookName DEFAULT (N'') FOR BookName;
END
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.default_constraints dc
    INNER JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id
    INNER JOIN sys.tables t ON c.object_id = t.object_id
    WHERE t.name = N'BookOutboundRecords' AND c.name = N'BookName'
)
BEGIN
    ALTER TABLE BookOutboundRecords ADD CONSTRAINT DF_BookOutboundRecords_BookName DEFAULT (N'') FOR BookName;
END
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.default_constraints dc
    INNER JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id
    INNER JOIN sys.tables t ON c.object_id = t.object_id
    WHERE t.name = N'InventoryChecks' AND c.name = N'BookName'
)
BEGIN
    ALTER TABLE InventoryChecks ADD CONSTRAINT DF_InventoryChecks_BookName DEFAULT (N'') FOR BookName;
END
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.default_constraints dc
    INNER JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id
    INNER JOIN sys.tables t ON c.object_id = t.object_id
    WHERE t.name = N'InventoryChecks' AND c.name = N'SystemStock'
)
BEGIN
    ALTER TABLE InventoryChecks ADD CONSTRAINT DF_InventoryChecks_SystemStock DEFAULT (0) FOR SystemStock;
END
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.default_constraints dc
    INNER JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id
    INNER JOIN sys.tables t ON c.object_id = t.object_id
    WHERE t.name = N'InventoryChecks' AND c.name = N'CheckResult'
)
BEGIN
    ALTER TABLE InventoryChecks ADD CONSTRAINT DF_InventoryChecks_CheckResult DEFAULT (N'实际数量和系统一致') FOR CheckResult;
END
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.default_constraints dc
    INNER JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id
    INNER JOIN sys.tables t ON c.object_id = t.object_id
    WHERE t.name = N'ShoppingCartItems' AND c.name = N'BookName'
)
BEGIN
    ALTER TABLE ShoppingCartItems ADD CONSTRAINT DF_ShoppingCartItems_BookName DEFAULT (N'') FOR BookName;
END
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.default_constraints dc
    INNER JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id
    INNER JOIN sys.tables t ON c.object_id = t.object_id
    WHERE t.name = N'SaleOrders' AND c.name = N'CustomerName'
)
BEGIN
    ALTER TABLE SaleOrders ADD CONSTRAINT DF_SaleOrders_CustomerName DEFAULT (N'') FOR CustomerName;
END
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.default_constraints dc
    INNER JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id
    INNER JOIN sys.tables t ON c.object_id = t.object_id
    WHERE t.name = N'OrderDetails' AND c.name = N'BookName'
)
BEGIN
    ALTER TABLE OrderDetails ADD CONSTRAINT DF_OrderDetails_BookName DEFAULT (N'') FOR BookName;
END
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.default_constraints dc
    INNER JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id
    INNER JOIN sys.tables t ON c.object_id = t.object_id
    WHERE t.name = N'OrderDetails' AND c.name = N'UnitPrice'
)
BEGIN
    ALTER TABLE OrderDetails ADD CONSTRAINT DF_OrderDetails_UnitPrice DEFAULT (0) FOR UnitPrice;
END
GO

IF OBJECT_ID(N'trg_BookPrices_Cascade', N'TR') IS NOT NULL
BEGIN
    DROP TRIGGER trg_BookPrices_Cascade;
END
GO

CREATE TRIGGER trg_BookPrices_Cascade
ON BookPrices
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    IF TRIGGER_NESTLEVEL() > 1 RETURN;

    UPDATE p
    SET p.BookName = b.BookName
    FROM BookPrices p
    INNER JOIN inserted i ON p.Id = i.Id
    INNER JOIN Books b ON p.BookCode = b.BookCode
    WHERE ISNULL(p.BookName, N'') <> ISNULL(b.BookName, N'');

    UPDATE b
    SET b.Price = i.SalePrice
    FROM Books b
    INNER JOIN inserted i ON b.BookCode = i.BookCode
    WHERE b.Price <> i.SalePrice;
END
GO

IF OBJECT_ID(N'trg_BookShelf_Cascade', N'TR') IS NOT NULL
BEGIN
    DROP TRIGGER trg_BookShelf_Cascade;
END
GO

CREATE TRIGGER trg_BookShelf_Cascade
ON BookShelfRecords
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    IF TRIGGER_NESTLEVEL() > 1 RETURN;

    UPDATE s
    SET
        s.BookName = b.BookName,
        s.OffSaleReason = CASE WHEN i.SaleStatus = N'正常销售' THEN NULL ELSE s.OffSaleReason END
    FROM BookShelfRecords s
    INNER JOIN inserted i ON s.Id = i.Id
    INNER JOIN Books b ON s.BookCode = b.BookCode
    WHERE ISNULL(s.BookName, N'') <> ISNULL(b.BookName, N'')
       OR (i.SaleStatus = N'正常销售' AND s.OffSaleReason IS NOT NULL);

    UPDATE b
    SET b.IsOnSale = CASE WHEN i.SaleStatus = N'正常销售' THEN 1 ELSE 0 END
    FROM Books b
    INNER JOIN inserted i ON b.BookCode = i.BookCode
    WHERE b.IsOnSale <> CASE WHEN i.SaleStatus = N'正常销售' THEN 1 ELSE 0 END;
END
GO

IF OBJECT_ID(N'trg_PurchaseOrders_NameSync', N'TR') IS NOT NULL
BEGIN
    DROP TRIGGER trg_PurchaseOrders_NameSync;
END
GO

CREATE TRIGGER trg_PurchaseOrders_NameSync
ON PurchaseOrders
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    IF TRIGGER_NESTLEVEL() > 1 RETURN;

    UPDATE p
    SET p.BookName = b.BookName
    FROM PurchaseOrders p
    INNER JOIN inserted i ON p.Id = i.Id
    INNER JOIN Books b ON p.BookCode = b.BookCode
    WHERE ISNULL(p.BookName, N'') <> ISNULL(b.BookName, N'');
END
GO

IF OBJECT_ID(N'trg_BookInbound_NameSync', N'TR') IS NOT NULL
BEGIN
    DROP TRIGGER trg_BookInbound_NameSync;
END
GO

CREATE TRIGGER trg_BookInbound_NameSync
ON BookInboundRecords
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE r
    SET r.BookName = b.BookName
    FROM BookInboundRecords r
    INNER JOIN inserted i ON r.Id = i.Id
    INNER JOIN Books b ON r.BookCode = b.BookCode;
END
GO

IF OBJECT_ID(N'trg_BookOutbound_NameSync', N'TR') IS NOT NULL
BEGIN
    DROP TRIGGER trg_BookOutbound_NameSync;
END
GO

CREATE TRIGGER trg_BookOutbound_NameSync
ON BookOutboundRecords
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE r
    SET r.BookName = b.BookName
    FROM BookOutboundRecords r
    INNER JOIN inserted i ON r.Id = i.Id
    INNER JOIN Books b ON r.BookCode = b.BookCode;
END
GO

IF OBJECT_ID(N'trg_BookInbound_NameSync', N'TR') IS NOT NULL DROP TRIGGER trg_BookInbound_NameSync;
IF OBJECT_ID(N'trg_BookOutbound_NameSync', N'TR') IS NOT NULL DROP TRIGGER trg_BookOutbound_NameSync;
GO

IF OBJECT_ID(N'trg_InventoryChecks_Cascade', N'TR') IS NOT NULL
BEGIN
    DROP TRIGGER trg_InventoryChecks_Cascade;
END
GO

CREATE TRIGGER trg_InventoryChecks_Cascade
ON InventoryChecks
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    IF TRIGGER_NESTLEVEL() > 1 RETURN;

    UPDATE c
    SET
        c.BookName = b.BookName,
        c.SystemStock = b.Stock,
        c.CheckResult = CASE
            WHEN c.ActualStock > b.Stock THEN N'实际库存多了 ' + CONVERT(NVARCHAR(20), c.ActualStock - b.Stock) + N' 本'
            WHEN c.ActualStock < b.Stock THEN N'实际库存少了 ' + CONVERT(NVARCHAR(20), b.Stock - c.ActualStock) + N' 本'
            ELSE N'实际数量和系统一致'
        END
    FROM InventoryChecks c
    INNER JOIN inserted i ON c.Id = i.Id
    INNER JOIN Books b ON c.BookCode = b.BookCode
    WHERE ISNULL(c.BookName, N'') <> ISNULL(b.BookName, N'')
       OR c.SystemStock <> b.Stock
       OR c.CheckResult <> CASE
            WHEN c.ActualStock > b.Stock THEN N'实际库存多了 ' + CONVERT(NVARCHAR(20), c.ActualStock - b.Stock) + N' 本'
            WHEN c.ActualStock < b.Stock THEN N'实际库存少了 ' + CONVERT(NVARCHAR(20), b.Stock - c.ActualStock) + N' 本'
            ELSE N'实际数量和系统一致'
        END;
END
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.default_constraints dc
    INNER JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id
    INNER JOIN sys.tables t ON c.object_id = t.object_id
    WHERE t.name = N'BookInboundRecords' AND c.name = N'BookCode'
)
BEGIN
    ALTER TABLE BookInboundRecords ADD CONSTRAINT DF_BookInboundRecords_BookCode DEFAULT (N'') FOR BookCode;
END
GO

IF OBJECT_ID(N'trg_BookInbound_Cascade', N'TR') IS NOT NULL
BEGIN
    DROP TRIGGER trg_BookInbound_Cascade;
END
GO

CREATE TRIGGER trg_BookInbound_Cascade
ON BookInboundRecords
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    IF TRIGGER_NESTLEVEL() > 1 RETURN;

    UPDATE r
    SET
        r.BookCode = COALESCE(NULLIF(i.BookCode, N''), p.BookCode, r.BookCode),
        r.BookName = COALESCE(NULLIF(i.BookName, N''), p.BookName, b.BookName, r.BookName)
    FROM BookInboundRecords r
    INNER JOIN inserted i ON r.Id = i.Id
    LEFT JOIN PurchaseOrders p ON i.PurchaseNo = p.PurchaseNo
    LEFT JOIN Books b ON COALESCE(NULLIF(i.BookCode, N''), p.BookCode) = b.BookCode
    WHERE ISNULL(r.BookCode, N'') <> ISNULL(COALESCE(NULLIF(i.BookCode, N''), p.BookCode, r.BookCode), N'')
       OR ISNULL(r.BookName, N'') <> ISNULL(COALESCE(NULLIF(i.BookName, N''), p.BookName, b.BookName, r.BookName), N'');

    WITH changes AS (
        SELECT COALESCE(NULLIF(i.BookCode, N''), p.BookCode) AS BookCode, SUM(i.InboundQuantity) AS Quantity
        FROM inserted i
        LEFT JOIN PurchaseOrders p ON i.PurchaseNo = p.PurchaseNo
        GROUP BY COALESCE(NULLIF(i.BookCode, N''), p.BookCode)
        UNION ALL
        SELECT COALESCE(NULLIF(d.BookCode, N''), p.BookCode) AS BookCode, -SUM(d.InboundQuantity)
        FROM deleted d
        LEFT JOIN PurchaseOrders p ON d.PurchaseNo = p.PurchaseNo
        GROUP BY COALESCE(NULLIF(d.BookCode, N''), p.BookCode)
    ),
    totalChanges AS (
        SELECT BookCode, SUM(Quantity) AS Quantity
        FROM changes
        WHERE ISNULL(BookCode, N'') <> N''
        GROUP BY BookCode
    )
    UPDATE b
    SET b.Stock = CASE WHEN b.Stock + c.Quantity < 0 THEN 0 ELSE b.Stock + c.Quantity END
    FROM Books b
    INNER JOIN totalChanges c ON b.BookCode = c.BookCode;

    WITH changedPurchases AS (
        SELECT PurchaseNo FROM inserted WHERE PurchaseNo IS NOT NULL
        UNION
        SELECT PurchaseNo FROM deleted WHERE PurchaseNo IS NOT NULL
    )
    UPDATE p
    SET p.PurchaseStatus = CASE
        WHEN ISNULL(total.InboundQuantity, 0) <= 0 THEN N'待入库'
        WHEN ISNULL(total.InboundQuantity, 0) < p.Quantity THEN N'部分入库'
        ELSE N'已入库'
    END
    FROM PurchaseOrders p
    INNER JOIN changedPurchases changed ON p.PurchaseNo = changed.PurchaseNo
    OUTER APPLY (
        SELECT SUM(r.InboundQuantity) AS InboundQuantity
        FROM BookInboundRecords r
        WHERE r.PurchaseNo = p.PurchaseNo
    ) total;
END
GO

IF OBJECT_ID(N'trg_Delivery_Cascade', N'TR') IS NOT NULL
BEGIN
    DROP TRIGGER trg_Delivery_Cascade;
END
GO

CREATE TRIGGER trg_Delivery_Cascade
ON OrderDeliveries
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE o
    SET o.OrderStatus =
        CASE
            WHEN i.DeliveryStatus IN (N'已签收', N'已完成') THEN N'已完成'
            WHEN i.DeliveryStatus IN (N'已发货', N'配送中') THEN N'已发货'
            ELSE o.OrderStatus
        END
    FROM SaleOrders o
    INNER JOIN inserted i ON o.OrderNo = i.OrderNo
    WHERE o.OrderStatus <> N'已取消';

    INSERT INTO BookOutboundRecords(OutboundNo, BookCode, BookName, OutboundType, OutboundQuantity, OutboundTime, Reason)
    SELECT
        N'XSCK' + CONVERT(NVARCHAR(12), i.Id) + N'-' + CONVERT(NVARCHAR(12), d.Id),
        d.BookCode,
        d.BookName,
        N'销售出库',
        d.Quantity,
        ISNULL(i.DeliveryTime, GETDATE()),
        N'订单 ' + i.OrderNo + N' 发货'
    FROM inserted i
    INNER JOIN OrderDetails d ON i.OrderNo = d.OrderNo
    WHERE i.DeliveryStatus IN (N'已发货', N'配送中', N'已签收', N'已完成')
      AND NOT EXISTS (
          SELECT 1
          FROM BookOutboundRecords r
          WHERE r.OutboundNo = N'XSCK' + CONVERT(NVARCHAR(12), i.Id) + N'-' + CONVERT(NVARCHAR(12), d.Id)
      );
END
GO

INSERT INTO BookCategories(CategoryCode, CategoryName, ParentCategory, Description)
SELECT v.CategoryCode, v.CategoryName, NULL, v.Description
FROM (VALUES
    (N'CAT005', N'经济管理', N'财务、管理、市场营销类图书'),
    (N'CAT006', N'少儿读物', N'儿童阅读、启蒙和科普类图书'),
    (N'CAT007', N'艺术设计', N'美术、设计、摄影类图书'),
    (N'CAT008', N'外语学习', N'英语、日语和语言考试类图书'),
    (N'CAT009', N'生活健康', N'生活方式、运动和健康类图书'),
    (N'CAT010', N'科技科普', N'科学技术和大众科普类图书')
) AS v(CategoryCode, CategoryName, Description)
WHERE NOT EXISTS (SELECT 1 FROM BookCategories c WHERE c.CategoryCode = v.CategoryCode);
GO

UPDATE Books SET CategoryName = N'计算机' WHERE BookCode IN (N'B001', N'B002', N'B003', N'B004', N'B005', N'B006', N'B007', N'B008', N'B009', N'B010');
GO

INSERT INTO Books(BookCode, ISBN, BookName, CategoryName, Author, Publisher, PublishedDate, Description, Price, Stock, MinStock, IsOnSale)
SELECT v.BookCode, v.ISBN, v.BookName, v.CategoryName, v.Author, v.Publisher, v.PublishedDate, v.Description, v.Price, v.Stock, v.MinStock, v.IsOnSale
FROM (VALUES
    (N'B011', N'9787020000111', N'现代文学精选', N'文学', N'林晓', N'人民文学出版社', CONVERT(DATE, '2021-04-18'), N'现代文学阅读选本', 39.00, 18, 6, 1),
    (N'B012', N'9787100000122', N'中国通史简明读本', N'历史', N'许明', N'中华书局', CONVERT(DATE, '2020-10-01'), N'适合入门阅读的中国史读本', 52.00, 12, 8, 1),
    (N'B013', N'9787300000133', N'教育心理学基础', N'教育', N'韩梅', N'北京师范大学出版社', CONVERT(DATE, '2022-09-10'), N'教育学与心理学基础教材', 48.00, 16, 10, 1),
    (N'B014', N'9787500000144', N'市场营销实务', N'经济管理', N'周航', N'中国财政经济出版社', CONVERT(DATE, '2023-02-15'), N'营销案例与实务操作', 68.00, 9, 12, 1),
    (N'B015', N'9787550000155', N'少儿科学故事', N'少儿读物', N'叶青青', N'接力出版社', CONVERT(DATE, '2024-05-01'), N'面向小学生的科学故事', 32.00, 7, 10, 1),
    (N'B016', N'9787530000166', N'平面设计入门', N'艺术设计', N'顾南', N'上海人民美术出版社', CONVERT(DATE, '2022-07-20'), N'设计基础与案例', 59.00, 11, 8, 1),
    (N'B017', N'9787560000177', N'大学英语阅读训练', N'外语学习', N'赵琳', N'外语教学与研究出版社', CONVERT(DATE, '2021-12-05'), N'大学英语阅读训练资料', 42.00, 22, 10, 1),
    (N'B018', N'9787110000188', N'人工智能科普导读', N'科技科普', N'沈越', N'机械工业出版社', CONVERT(DATE, '2024-03-01'), N'人工智能概念与应用科普', 76.00, 6, 12, 1)
) AS v(BookCode, ISBN, BookName, CategoryName, Author, Publisher, PublishedDate, Description, Price, Stock, MinStock, IsOnSale)
WHERE NOT EXISTS (SELECT 1 FROM Books b WHERE b.BookCode = v.BookCode);
GO

UPDATE b
SET
    b.CategoryName = v.CategoryName,
    b.Price = v.Price,
    b.MinStock = v.MinStock,
    b.IsOnSale = v.IsOnSale
FROM Books b
INNER JOIN (VALUES
    (N'B011', N'文学', 39.00, 6, 1),
    (N'B012', N'历史', 52.00, 8, 1),
    (N'B013', N'教育', 48.00, 10, 1),
    (N'B014', N'经济管理', 68.00, 12, 1),
    (N'B015', N'少儿读物', 32.00, 10, 1),
    (N'B016', N'艺术设计', 59.00, 8, 1),
    (N'B017', N'外语学习', 42.00, 10, 1),
    (N'B018', N'科技科普', 76.00, 12, 1)
) AS v(BookCode, CategoryName, Price, MinStock, IsOnSale) ON b.BookCode = v.BookCode;
GO

INSERT INTO Customers(CustomerCode, CustomerName, Phone, Address, MemberLevel, RegisterDate, Remark)
SELECT v.CustomerCode, v.CustomerName, v.Phone, v.Address, v.MemberLevel, GETDATE(), N'补充演示客户'
FROM (VALUES
    (N'C003', N'王芳', N'13900000003', N'广州市天河区', N'金卡会员'),
    (N'C004', N'赵强', N'13900000004', N'深圳市南山区', N'普通会员'),
    (N'C005', N'刘敏', N'13900000005', N'杭州市西湖区', N'银卡会员'),
    (N'C006', N'陈杰', N'13900000006', N'成都市武侯区', N'金卡会员'),
    (N'C007', N'孙丽', N'13900000007', N'南京市鼓楼区', N'普通会员'),
    (N'C008', N'周宁', N'13900000008', N'武汉市洪山区', N'银卡会员')
) AS v(CustomerCode, CustomerName, Phone, Address, MemberLevel)
WHERE NOT EXISTS (SELECT 1 FROM Customers c WHERE c.CustomerCode = v.CustomerCode);
GO

INSERT INTO PurchaseOrders(PurchaseNo, BookCode, BookName, Quantity, PurchasePrice, Supplier, PurchaseDate, PurchaseStatus)
SELECT v.PurchaseNo, v.BookCode, b.BookName, v.Quantity, v.PurchasePrice, v.Supplier, v.PurchaseDate, N'待入库'
FROM (VALUES
    (N'CG20260720001', N'B014', 20, 45.00, N'北京文轩图书供应中心', CONVERT(DATE, GETDATE())),
    (N'CG20260720002', N'B015', 30, 18.00, N'上海智源教材批发部', CONVERT(DATE, GETDATE())),
    (N'CG20260720003', N'B018', 25, 52.00, N'北京文轩图书供应中心', CONVERT(DATE, GETDATE())),
    (N'CG20260720004', N'B012', 15, 34.00, N'上海智源教材批发部', DATEADD(DAY, -2, CONVERT(DATE, GETDATE())))
) AS v(PurchaseNo, BookCode, Quantity, PurchasePrice, Supplier, PurchaseDate)
INNER JOIN Books b ON v.BookCode = b.BookCode
WHERE NOT EXISTS (SELECT 1 FROM PurchaseOrders p WHERE p.PurchaseNo = v.PurchaseNo);
GO

INSERT INTO BookInboundRecords(InboundNo, PurchaseNo, BookCode, BookName, InboundQuantity, InboundTime, OperatorName)
SELECT v.InboundNo, v.PurchaseNo, p.BookCode, p.BookName, v.InboundQuantity, GETDATE(), N'管理员'
FROM (VALUES
    (N'RK20260720001', N'CG20260720001', 20),
    (N'RK20260720002', N'CG20260720002', 12),
    (N'RK20260720003', N'CG20260720004', 15)
) AS v(InboundNo, PurchaseNo, InboundQuantity)
INNER JOIN PurchaseOrders p ON v.PurchaseNo = p.PurchaseNo
WHERE NOT EXISTS (SELECT 1 FROM BookInboundRecords r WHERE r.InboundNo = v.InboundNo);
GO

INSERT INTO SaleOrders(OrderNo, CustomerCode, CustomerName, OrderTime, OrderStatus, OrderAmount, CancelReason)
SELECT v.OrderNo, v.CustomerCode, c.CustomerName, v.OrderTime, v.OrderStatus, 0, v.CancelReason
FROM (VALUES
    (N'DD20260720004', N'C003', DATEADD(HOUR, -2, GETDATE()), N'已完成', CAST(NULL AS NVARCHAR(200))),
    (N'DD20260720005', N'C004', DATEADD(DAY, -2, GETDATE()), N'已完成', CAST(NULL AS NVARCHAR(200))),
    (N'DD20260720006', N'C005', DATEADD(DAY, -5, GETDATE()), N'已发货', CAST(NULL AS NVARCHAR(200))),
    (N'DD20260720007', N'C006', DATEADD(DAY, -8, GETDATE()), N'待发货', CAST(NULL AS NVARCHAR(200))),
    (N'DD20260720008', N'C003', DATEADD(DAY, -16, GETDATE()), N'已退货', CAST(NULL AS NVARCHAR(200))),
    (N'DD20260720009', N'C007', DATEADD(DAY, -35, GETDATE()), N'已完成', CAST(NULL AS NVARCHAR(200))),
    (N'DD20260720010', N'C008', DATEADD(DAY, -60, GETDATE()), N'已取消', N'客户修改采购计划')
) AS v(OrderNo, CustomerCode, OrderTime, OrderStatus, CancelReason)
INNER JOIN Customers c ON v.CustomerCode = c.CustomerCode
WHERE NOT EXISTS (SELECT 1 FROM SaleOrders o WHERE o.OrderNo = v.OrderNo);
GO

INSERT INTO OrderDetails(DetailNo, OrderNo, BookCode, BookName, Quantity, UnitPrice, Discount)
SELECT v.DetailNo, v.OrderNo, v.BookCode, b.BookName, v.Quantity, b.Price, v.Discount
FROM (VALUES
    (N'MX20260720004', N'DD20260720004', N'B008', 2, 1.00),
    (N'MX20260720005', N'DD20260720004', N'B018', 1, 0.95),
    (N'MX20260720006', N'DD20260720005', N'B011', 3, 1.00),
    (N'MX20260720007', N'DD20260720005', N'B012', 1, 1.00),
    (N'MX20260720008', N'DD20260720006', N'B014', 2, 0.90),
    (N'MX20260720009', N'DD20260720007', N'B015', 4, 1.00),
    (N'MX20260720010', N'DD20260720008', N'B002', 1, 1.00),
    (N'MX20260720011', N'DD20260720009', N'B017', 5, 0.95),
    (N'MX20260720012', N'DD20260720010', N'B013', 1, 1.00)
) AS v(DetailNo, OrderNo, BookCode, Quantity, Discount)
INNER JOIN Books b ON v.BookCode = b.BookCode
WHERE NOT EXISTS (SELECT 1 FROM OrderDetails d WHERE d.DetailNo = v.DetailNo);
GO

INSERT INTO Payments(PaymentNo, OrderNo, PaymentMethod, PaymentAmount, PaymentTime, PaymentStatus, TransactionNo)
SELECT v.PaymentNo, v.OrderNo, v.PaymentMethod, o.OrderAmount, v.PaymentTime, v.PaymentStatus, v.TransactionNo
FROM (VALUES
    (N'ZF20260720004', N'DD20260720004', N'微信支付', DATEADD(HOUR, -1, GETDATE()), N'已支付', N'WX202607200004'),
    (N'ZF20260720005', N'DD20260720005', N'支付宝', DATEADD(DAY, -2, GETDATE()), N'已支付', N'ALI202607200005'),
    (N'ZF20260720006', N'DD20260720006', N'银行卡', DATEADD(DAY, -5, GETDATE()), N'已支付', N'BANK202607200006'),
    (N'ZF20260720007', N'DD20260720007', N'微信支付', DATEADD(DAY, -8, GETDATE()), N'已支付', N'WX202607200007'),
    (N'ZF20260720008', N'DD20260720008', N'支付宝', DATEADD(DAY, -16, GETDATE()), N'已退款', N'ALI202607200008'),
    (N'ZF20260720009', N'DD20260720009', N'微信支付', DATEADD(DAY, -35, GETDATE()), N'已支付', N'WX202607200009')
) AS v(PaymentNo, OrderNo, PaymentMethod, PaymentTime, PaymentStatus, TransactionNo)
INNER JOIN SaleOrders o ON v.OrderNo = o.OrderNo
WHERE NOT EXISTS (SELECT 1 FROM Payments p WHERE p.PaymentNo = v.PaymentNo);
GO

INSERT INTO OrderDeliveries(DeliveryNo, OrderNo, ReceiverAddress, LogisticsCompany, LogisticsNo, DeliveryTime, DeliveryStatus)
SELECT v.DeliveryNo, v.OrderNo, c.Address, v.LogisticsCompany, v.LogisticsNo, v.DeliveryTime, v.DeliveryStatus
FROM (VALUES
    (N'FH20260720004', N'DD20260720004', N'顺丰速运', N'SF202607200004', GETDATE(), N'已签收'),
    (N'FH20260720005', N'DD20260720005', N'中通快递', N'ZT202607200005', DATEADD(DAY, -1, GETDATE()), N'已签收'),
    (N'FH20260720006', N'DD20260720006', N'顺丰速运', N'SF202607200006', DATEADD(DAY, -4, GETDATE()), N'已发货'),
    (N'FH20260720009', N'DD20260720009', N'圆通快递', N'YT202607200009', DATEADD(DAY, -34, GETDATE()), N'已签收')
) AS v(DeliveryNo, OrderNo, LogisticsCompany, LogisticsNo, DeliveryTime, DeliveryStatus)
INNER JOIN SaleOrders o ON v.OrderNo = o.OrderNo
INNER JOIN Customers c ON o.CustomerCode = c.CustomerCode
WHERE NOT EXISTS (SELECT 1 FROM OrderDeliveries d WHERE d.DeliveryNo = v.DeliveryNo);
GO

INSERT INTO ReturnRefunds(ReturnNo, OrderNo, BookCode, ReturnQuantity, ApplyReason, AuditStatus, InboundStatus, RefundAmount)
SELECT v.ReturnNo, v.OrderNo, v.BookCode, v.ReturnQuantity, v.ApplyReason, v.AuditStatus, N'已入库', 0
FROM (VALUES
    (N'TH20260720003', N'DD20260720008', N'B002', 1, N'客户退回教材', N'已完成')
) AS v(ReturnNo, OrderNo, BookCode, ReturnQuantity, ApplyReason, AuditStatus)
WHERE NOT EXISTS (SELECT 1 FROM ReturnRefunds r WHERE r.ReturnNo = v.ReturnNo);
GO

INSERT INTO InventoryChecks(CheckNo, BookCode, BookName, SystemStock, ActualStock, CheckResult, CheckDate)
SELECT v.CheckNo, b.BookCode, b.BookName, b.Stock, v.ActualStock, N'正常', v.CheckDate
FROM (VALUES
    (N'PD20260720004', N'B004', 49, CONVERT(DATE, GETDATE())),
    (N'PD20260720005', N'B014', 27, CONVERT(DATE, GETDATE())),
    (N'PD20260720006', N'B015', 16, CONVERT(DATE, GETDATE())),
    (N'PD20260720007', N'B018', 4, CONVERT(DATE, GETDATE()))
) AS v(CheckNo, BookCode, ActualStock, CheckDate)
INNER JOIN Books b ON v.BookCode = b.BookCode
WHERE NOT EXISTS (SELECT 1 FROM InventoryChecks c WHERE c.CheckNo = v.CheckNo);
GO

INSERT INTO BookPrices(BookCode, BookName, CostPrice, SalePrice, DiscountPrice, MemberPrice, EffectiveDate)
SELECT b.BookCode, b.BookName, ROUND(b.Price * 0.70, 2), b.Price, ROUND(b.Price * 0.90, 2), ROUND(b.Price * 0.85, 2), CONVERT(DATE, GETDATE())
FROM Books b
WHERE NOT EXISTS (SELECT 1 FROM BookPrices p WHERE p.BookCode = b.BookCode);
GO

INSERT INTO BookShelfRecords(BookCode, BookName, SaleStatus, OnSaleTime)
SELECT b.BookCode, b.BookName, CASE WHEN b.IsOnSale = 1 THEN N'正常销售' ELSE N'已下架' END, GETDATE()
FROM Books b
WHERE NOT EXISTS (SELECT 1 FROM BookShelfRecords s WHERE s.BookCode = b.BookCode);
GO

MERGE StockWarnings AS target
USING (
    SELECT
        BookCode,
        BookName,
        Stock AS CurrentStock,
        MinStock,
        CASE
            WHEN Stock < MinStock THEN N'建议补货 ' + CONVERT(NVARCHAR(20), MinStock - Stock + 10) + N' 本'
            WHEN Stock = MinStock THEN N'库存刚好达到最低线'
            ELSE N'库存正常'
        END AS ReplenishmentAdvice
    FROM Books
) AS source
ON target.BookCode = source.BookCode
WHEN MATCHED THEN
    UPDATE SET
        BookName = source.BookName,
        CurrentStock = source.CurrentStock,
        MinStock = source.MinStock,
        ReplenishmentAdvice = source.ReplenishmentAdvice,
        WarningTime = GETDATE()
WHEN NOT MATCHED THEN
    INSERT (WarningNo, BookCode, BookName, CurrentStock, MinStock, ReplenishmentAdvice, WarningTime)
    VALUES (N'YJ' + LEFT(REPLACE(CONVERT(NVARCHAR(36), NEWID()), N'-', N''), 28), source.BookCode, source.BookName, source.CurrentStock, source.MinStock, source.ReplenishmentAdvice, GETDATE());
GO

UPDATE p
SET p.PurchaseStatus = CASE
    WHEN ISNULL(total.InboundQuantity, 0) <= 0 THEN N'待入库'
    WHEN ISNULL(total.InboundQuantity, 0) < p.Quantity THEN N'部分入库'
    ELSE N'已入库'
END
FROM PurchaseOrders p
OUTER APPLY (
    SELECT SUM(r.InboundQuantity) AS InboundQuantity
    FROM BookInboundRecords r
    WHERE r.PurchaseNo = p.PurchaseNo
) total;
GO

UPDATE BookShelfRecords
SET
    SaleStatus = CASE
        WHEN SaleStatus IN (N'正常', N'正常销售') THEN N'正常销售'
        WHEN SaleStatus = N'待处理' THEN N'暂不上架'
        ELSE SaleStatus
    END,
    OffSaleReason = CASE WHEN SaleStatus IN (N'正常', N'正常销售') THEN NULL ELSE OffSaleReason END;
GO

UPDATE b
SET b.IsOnSale = CASE WHEN s.SaleStatus = N'正常销售' THEN 1 ELSE 0 END
FROM Books b
INNER JOIN BookShelfRecords s ON b.BookCode = s.BookCode;
GO

UPDATE InventoryChecks
SET CheckResult = CASE
    WHEN ActualStock > SystemStock THEN N'实际库存多了 ' + CONVERT(NVARCHAR(20), ActualStock - SystemStock) + N' 本'
    WHEN ActualStock < SystemStock THEN N'实际库存少了 ' + CONVERT(NVARCHAR(20), SystemStock - ActualStock) + N' 本'
    ELSE N'实际数量和系统一致'
END;
GO

IF OBJECT_ID(N'DF_InventoryChecks_CheckResult', N'D') IS NOT NULL
BEGIN
    ALTER TABLE InventoryChecks DROP CONSTRAINT DF_InventoryChecks_CheckResult;
END
GO
ALTER TABLE InventoryChecks ADD CONSTRAINT DF_InventoryChecks_CheckResult DEFAULT (N'实际数量和系统一致') FOR CheckResult;
GO

INSERT INTO ShoppingCartItems(CartNo, CustomerCode, BookCode, BookName, Quantity, AddedTime)
SELECT v.CartNo, v.CustomerCode, v.BookCode, b.BookName, v.Quantity, GETDATE()
FROM (VALUES
    (N'GW20260720003', N'C004', N'B016', 1),
    (N'GW20260720004', N'C005', N'B017', 2),
    (N'GW20260720005', N'C006', N'B011', 1),
    (N'GW20260720006', N'C008', N'B018', 1),
    (N'GW20260720007', N'C002', N'B014', 1)
) AS v(CartNo, CustomerCode, BookCode, Quantity)
INNER JOIN Books b ON v.BookCode = b.BookCode
WHERE NOT EXISTS (SELECT 1 FROM ShoppingCartItems cart WHERE cart.CartNo = v.CartNo);
GO

UPDATE BookShelfRecords
SET SaleStatus = N'暂不上架',
    OffSaleReason = N'等待活动定价确认'
WHERE BookCode = N'B003';
GO

PRINT N'BookSalesDB 初始化完成。';
SELECT
    (SELECT COUNT(*) FROM Books) AS BookCount,
    (SELECT COUNT(*) FROM BookCategories) AS CategoryCount,
    (SELECT COUNT(*) FROM Customers) AS CustomerCount,
    (SELECT COUNT(*) FROM SaleOrders) AS OrderCount,
    (SELECT COUNT(*) FROM ShoppingCartItems) AS CartItemCount;
GO
