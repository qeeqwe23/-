namespace WinFormsApp2
{
    internal static class ModuleCatalog
    {
        public static readonly ModuleDefinition BookBasic = new ModuleDefinition(
            "图书基础信息管理",
            "维护图书的核心档案信息。",
            "图书编号", "ISBN", "书名", "分类", "作者", "出版社", "出版日期", "简介");

        public static readonly ModuleDefinition BookSearch = new ModuleDefinition(
            "图书检索",
            "按分类、书名、作者或出版社查询可销售图书。",
            "分类", "图书编号", "ISBN", "书名", "作者", "出版社", "销售价", "当前库存", "销售状态");

        public static readonly ModuleDefinition BookCategory = new ModuleDefinition(
            "图书分类管理",
            "维护图书分类名称和说明。",
            "分类编号", "分类名称", "分类说明");

        public static readonly ModuleDefinition AuthorPublisher = new ModuleDefinition(
            "作者与出版社管理",
            "维护作者和出版社的基础资料。",
            "资料编号", "类型", "名称", "简介");

        public static readonly ModuleDefinition BookPrice = new ModuleDefinition(
            "图书价格管理",
            "维护图书进货价、销售价和会员价。",
            "图书编号", "书名", "进货价", "销售价", "会员价");

        public static readonly ModuleDefinition BookShelf = new ModuleDefinition(
            "图书上架与下架管理",
            "维护图书销售状态和下架原因。",
            "图书编号", "书名", "销售状态", "下架原因");

        public static readonly ModuleDefinition Purchase = new ModuleDefinition(
            "图书采购管理",
            "维护采购单和供应商信息，并查看对应入库进度。",
            "采购单号", "图书编号", "书名", "采购数量", "采购价格", "供应商", "入库状态");

        public static readonly ModuleDefinition Inbound = new ModuleDefinition(
            "图书入库管理",
            "按采购单记录到货入库数量，并联动采购状态和库存。",
            "入库单号", "采购单号", "图书编号", "书名", "入库数量", "经办人");

        public static readonly ModuleDefinition Outbound = new ModuleDefinition(
            "图书出库管理",
            "记录销售或损坏出库并联动库存。",
            "出库单号", "图书编号", "书名", "出库类型", "出库数量", "原因");

        public static readonly ModuleDefinition StockCheck = new ModuleDefinition(
            "库存盘点管理",
            "比较系统库存和实际库存。",
            "图书编号", "书名", "系统库存", "实际库存", "差异数量", "库存说明");

        public static readonly ModuleDefinition StockWarning = new ModuleDefinition(
            "库存预警管理",
            "显示库存低于最低数量的图书。",
            "图书编号", "书名", "当前库存", "最低库存", "补货建议");

        public static readonly ModuleDefinition Customer = new ModuleDefinition(
            "客户信息管理",
            "维护客户基础信息。",
            "客户编号", "姓名", "电话", "会员等级");

        public static readonly ModuleDefinition Cart = new ModuleDefinition(
            "购物车管理",
            "维护客户购物车中的图书和数量。",
            "客户编号", "图书编号", "书名", "购买数量");

        public static readonly ModuleDefinition SaleOrder = new ModuleDefinition(
            "销售订单管理",
            "维护订单状态和金额。",
            "订单编号", "客户编号", "客户姓名", "订单状态", "订单金额");

        public static readonly ModuleDefinition OrderDetail = new ModuleDefinition(
            "订单明细管理",
            "维护订单中每本书的数量和单价。",
            "订单编号", "图书编号", "书名", "数量", "单价", "小计");

        public static readonly ModuleDefinition Payment = new ModuleDefinition(
            "收款与支付管理",
            "维护订单支付情况。",
            "订单编号", "支付方式", "支付金额", "支付状态");

        public static readonly ModuleDefinition Delivery = new ModuleDefinition(
            "订单发货管理",
            "维护订单收货和物流状态。",
            "订单编号", "收货地址", "物流单号", "发货状态");

        public static readonly ModuleDefinition ReturnRefund = new ModuleDefinition(
            "退货退款管理",
            "维护退货审核和退款金额。",
            "订单编号", "图书编号", "退货数量", "申请原因", "审核状态", "退款金额");

        public static readonly ModuleDefinition CustomerHistory = new ModuleDefinition(
            "客户购买记录管理",
            "查看客户历史购买记录。",
            "客户编号", "客户姓名", "订单编号", "图书名称", "购买数量", "购买金额");

        public static readonly ModuleDefinition SaleStatistics = new ModuleDefinition(
            "销售数据统计",
            "查看销售数量和销售金额。",
            "统计类型", "销售数量", "销售金额");

        public static readonly ModuleDefinition BestSeller = new ModuleDefinition(
            "畅销图书分析",
            "查看图书销售排行。",
            "图书编号", "书名", "销量", "销售金额", "排行");

        public static readonly ModuleGroupDefinition BookProfileGroup = new ModuleGroupDefinition(
            "图书档案",
            "维护图书资料、定价和上下架状态，并按分类、作者、出版社检索图书。",
            "维护图书资料",
            BookBasic, BookSearch, BookPrice, BookShelf);

        public static readonly ModuleGroupDefinition InventoryFlowGroup = new ModuleGroupDefinition(
            "库存流转",
            "围绕采购、入库、出库、盘点和预警处理库存变化。",
            "处理库存业务",
            Purchase, Inbound, Outbound, StockCheck, StockWarning);

        public static readonly ModuleGroupDefinition CustomerOrderGroup = new ModuleGroupDefinition(
            "客户订单",
            "客户先维护购物车，再生成订单、明细、支付、发货和退货，购买记录由系统自动生成。",
            "进入订单流程",
            Customer, Cart, SaleOrder, OrderDetail, Payment, Delivery, ReturnRefund);

        public static readonly ModuleGroupDefinition BusinessAnalysisGroup = new ModuleGroupDefinition(
            "经营分析",
            "查看销售统计和畅销图书分析，辅助判断补货和经营重点。",
            "查看经营数据",
            SaleStatistics, BestSeller);

        public static readonly ModuleGroupDefinition[] MainGroups =
        {
            BookProfileGroup, InventoryFlowGroup, CustomerOrderGroup, BusinessAnalysisGroup
        };

        static ModuleCatalog()
        {
            ConfigureTitles();
            BindTables();
            BindQueries();
        }

        private static void ConfigureTitles()
        {
            BookBasic.WithTitles("图书资料", "图书档案列表");
            BookSearch.WithTitles("检索条件", "图书检索结果");
            BookCategory.WithTitles("分类资料", "分类列表");
            AuthorPublisher.WithTitles("作者出版社资料", "作者出版社列表");
            BookPrice.WithTitles("价格设置", "图书价格列表");
            BookShelf.WithTitles("上下架设置", "上下架状态列表");

            Purchase.WithTitles("采购单信息", "采购单列表");
            Inbound.WithTitles("入库信息", "入库记录列表");
            Outbound.WithTitles("出库信息", "出库记录列表");
            StockCheck.WithTitles("盘点条件", "库存核对结果");
            StockWarning.WithTitles("预警条件", "库存预警列表");

            Customer.WithTitles("客户资料", "客户列表");
            Cart.WithTitles("购物车内容", "当前购物车");
            SaleOrder.WithTitles("订单信息", "订单列表");
            OrderDetail.WithTitles("订单明细", "订单商品列表");
            Payment.WithTitles("收款信息", "支付记录列表");
            Delivery.WithTitles("发货信息", "发货记录列表");
            ReturnRefund.WithTitles("退货申请", "退货退款列表");
            CustomerHistory.WithTitles("购买记录条件", "客户购买记录");

            SaleStatistics.WithTitles("统计条件", "销售统计结果");
            BestSeller.WithTitles("排行条件", "畅销图书排行");
        }

        private static void BindTables()
        {
            BookBasic.BindTable("Books",
                Field("图书编号", "BookCode"), Field("ISBN", "ISBN"), Field("书名", "BookName"),
                Field("分类", "CategoryName"), Field("作者", "Author"), Field("出版社", "Publisher"), Field("出版日期", "PublishedDate"),
                Field("简介", "Description"));

            BookCategory.BindTable("BookCategories",
                Field("分类编号", "CategoryCode"), Field("分类名称", "CategoryName"),
                Field("分类说明", "Description"));

            AuthorPublisher.BindTable("AuthorPublishers",
                Field("资料编号", "ProfileCode"), Field("类型", "ProfileType"), Field("名称", "Name"),
                Field("简介", "Description"));

            BookPrice.BindTable("BookPrices",
                Field("图书编号", "BookCode"), Field("书名", "BookName", true), Field("进货价", "CostPrice"),
                Field("销售价", "SalePrice"), Field("会员价", "MemberPrice"));

            BookShelf.BindTable("BookShelfRecords",
                Field("图书编号", "BookCode"), Field("书名", "BookName", true), Field("销售状态", "SaleStatus"),
                Field("下架原因", "OffSaleReason"));

            Purchase.BindTable("PurchaseOrders",
                Field("采购单号", "PurchaseNo"), Field("图书编号", "BookCode"), Field("书名", "BookName", true),
                Field("采购数量", "Quantity"), Field("采购价格", "PurchasePrice"), Field("供应商", "Supplier"),
                Field("入库状态", "PurchaseStatus", true));

            Inbound.BindTable("BookInboundRecords",
                Field("入库单号", "InboundNo"), Field("采购单号", "PurchaseNo"), Field("图书编号", "BookCode", true),
                Field("书名", "BookName", true), Field("入库数量", "InboundQuantity"), Field("经办人", "OperatorName"));

            Outbound.BindTable("BookOutboundRecords",
                Field("出库单号", "OutboundNo"), Field("图书编号", "BookCode"), Field("书名", "BookName", true),
                Field("出库类型", "OutboundType"), Field("出库数量", "OutboundQuantity"), Field("原因", "Reason"));

            StockCheck.BindTable("InventoryChecks",
                Field("图书编号", "BookCode"), Field("书名", "BookName", true),
                Field("系统库存", "SystemStock", true), Field("实际库存", "ActualStock"),
                Field("差异数量", "DifferenceQuantity", true), Field("库存说明", "CheckResult", true));

            StockWarning.BindTable("StockWarnings",
                Field("图书编号", "BookCode"), Field("书名", "BookName"), Field("当前库存", "CurrentStock"),
                Field("最低库存", "MinStock"), Field("补货建议", "ReplenishmentAdvice"));

            Customer.BindTable("Customers",
                Field("客户编号", "CustomerCode"), Field("姓名", "CustomerName"), Field("电话", "Phone"),
                Field("会员等级", "MemberLevel"));

            Cart.BindTable("ShoppingCartItems",
                Field("客户编号", "CustomerCode"), Field("图书编号", "BookCode"),
                Field("书名", "BookName", true), Field("购买数量", "Quantity"));

            SaleOrder.BindTable("SaleOrders",
                Field("订单编号", "OrderNo"), Field("客户编号", "CustomerCode"), Field("客户姓名", "CustomerName", true),
                Field("订单状态", "OrderStatus"), Field("订单金额", "OrderAmount", true));

            OrderDetail.BindTable("OrderDetails",
                Field("订单编号", "OrderNo"), Field("图书编号", "BookCode"), Field("书名", "BookName", true),
                Field("数量", "Quantity"), Field("单价", "UnitPrice", true), Field("小计", "Subtotal", true));

            Payment.BindTable("Payments",
                Field("订单编号", "OrderNo"), Field("支付方式", "PaymentMethod"),
                Field("支付金额", "PaymentAmount"), Field("支付状态", "PaymentStatus"));

            Delivery.BindTable("OrderDeliveries",
                Field("订单编号", "OrderNo"), Field("收货地址", "ReceiverAddress"),
                Field("物流单号", "LogisticsNo"), Field("发货状态", "DeliveryStatus"));

            ReturnRefund.BindTable("ReturnRefunds",
                Field("订单编号", "OrderNo"), Field("图书编号", "BookCode"), Field("退货数量", "ReturnQuantity"),
                Field("申请原因", "ApplyReason"), Field("审核状态", "AuditStatus"), Field("退款金额", "RefundAmount", true));

            CustomerHistory.BindTable("CustomerPurchaseRecords",
                Field("客户编号", "CustomerCode"), Field("客户姓名", "CustomerName"), Field("订单编号", "OrderNo"),
                Field("图书名称", "BookName"), Field("购买数量", "Quantity"), Field("购买金额", "PurchaseAmount"));

            BookSearch.AsReadOnly();
            AuthorPublisher.AsReadOnly();
            StockCheck.AsReadOnly();
            StockWarning.AsReadOnly();
            CustomerHistory.AsReadOnly();
            SaleStatistics.AsReadOnly();
            BestSeller.AsReadOnly();

            // 经营分析由订单明细实时汇总，不允许手动维护。
        }

        private static void BindQueries()
        {
            BookBasic.QuerySql = "SELECT Id, BookCode AS N'图书编号', ISBN AS N'ISBN', BookName AS N'书名', CategoryName AS N'分类', Author AS N'作者', Publisher AS N'出版社', CONVERT(NVARCHAR(10), PublishedDate, 120) AS N'出版日期', Description AS N'简介' FROM Books ORDER BY Id DESC";
            BookSearch.QuerySql = "SELECT b.Id, ISNULL(b.CategoryName, N'未分类') AS N'分类', b.BookCode AS N'图书编号', b.ISBN AS N'ISBN', b.BookName AS N'书名', b.Author AS N'作者', b.Publisher AS N'出版社', b.Price AS N'销售价', b.Stock AS N'当前库存', ISNULL(s.SaleStatus, CASE WHEN b.IsOnSale = 1 THEN N'正常销售' ELSE N'已下架' END) AS N'销售状态' FROM Books b LEFT JOIN BookShelfRecords s ON b.BookCode = s.BookCode ORDER BY b.CategoryName, b.BookName";
            BookCategory.QuerySql = "SELECT Id, CategoryCode AS N'分类编号', CategoryName AS N'分类名称', Description AS N'分类说明' FROM BookCategories ORDER BY Id";
            AuthorPublisher.QuerySql = "SELECT Id, ProfileCode AS N'资料编号', ProfileType AS N'类型', Name AS N'名称', Description AS N'简介' FROM AuthorPublishers ORDER BY Id";
            BookPrice.QuerySql = "SELECT Id, BookCode AS N'图书编号', BookName AS N'书名', CostPrice AS N'进货价', SalePrice AS N'销售价', MemberPrice AS N'会员价' FROM BookPrices ORDER BY Id DESC";
            BookShelf.QuerySql = "SELECT Id, BookCode AS N'图书编号', BookName AS N'书名', SaleStatus AS N'销售状态', CASE WHEN SaleStatus = N'正常销售' THEN N'' ELSE ISNULL(OffSaleReason, N'未填写') END AS N'下架原因' FROM BookShelfRecords ORDER BY Id DESC";
            Purchase.QuerySql = "SELECT Id, PurchaseNo AS N'采购单号', BookCode AS N'图书编号', BookName AS N'书名', Quantity AS N'采购数量', PurchasePrice AS N'采购价格', Supplier AS N'供应商', PurchaseStatus AS N'入库状态' FROM PurchaseOrders ORDER BY Id DESC";
            Inbound.QuerySql = "SELECT Id, InboundNo AS N'入库单号', PurchaseNo AS N'采购单号', BookCode AS N'图书编号', BookName AS N'书名', InboundQuantity AS N'入库数量', OperatorName AS N'经办人' FROM BookInboundRecords ORDER BY Id DESC";
            Outbound.QuerySql = "SELECT Id, OutboundNo AS N'出库单号', BookCode AS N'图书编号', BookName AS N'书名', OutboundType AS N'出库类型', OutboundQuantity AS N'出库数量', Reason AS N'原因' FROM BookOutboundRecords ORDER BY Id DESC";
            StockCheck.QuerySql = @"SELECT
    b.Id,
    b.BookCode AS N'图书编号',
    b.BookName AS N'书名',
    b.Stock AS N'系统库存',
    ISNULL(lastCheck.ActualStock, b.Stock) AS N'实际库存',
    ISNULL(lastCheck.ActualStock, b.Stock) - b.Stock AS N'差异数量',
    CASE
        WHEN lastCheck.ActualStock IS NULL THEN N'还没有做过盘点'
        WHEN lastCheck.ActualStock > b.Stock THEN N'实际库存多了 ' + CONVERT(NVARCHAR(20), lastCheck.ActualStock - b.Stock) + N' 本'
        WHEN lastCheck.ActualStock < b.Stock THEN N'实际库存少了 ' + CONVERT(NVARCHAR(20), b.Stock - lastCheck.ActualStock) + N' 本'
        ELSE N'实际数量和系统一致'
    END AS N'库存说明'
FROM Books b
OUTER APPLY (
    SELECT TOP 1 CheckNo, ActualStock
    FROM InventoryChecks c
    WHERE c.BookCode = b.BookCode
    ORDER BY c.CheckDate DESC, c.Id DESC
) lastCheck
ORDER BY b.BookCode";
            StockWarning.QuerySql = @"SELECT
    b.Id,
    b.BookCode AS N'图书编号',
    b.BookName AS N'书名',
    b.Stock AS N'当前库存',
    b.MinStock AS N'最低库存',
    CASE
        WHEN b.Stock < b.MinStock THEN N'建议补货 ' + CONVERT(NVARCHAR(20), b.MinStock - b.Stock + 10) + N' 本'
        WHEN b.Stock = b.MinStock THEN N'库存刚好达到最低线'
        ELSE N'库存正常'
    END AS N'补货建议'
FROM Books b
ORDER BY CASE WHEN b.Stock < b.MinStock THEN 0 ELSE 1 END, b.BookCode";
            Customer.QuerySql = "SELECT Id, CustomerCode AS N'客户编号', CustomerName AS N'姓名', Phone AS N'电话', MemberLevel AS N'会员等级' FROM Customers ORDER BY Id DESC";
            Cart.QuerySql = "SELECT Id, CustomerCode AS N'客户编号', BookCode AS N'图书编号', BookName AS N'书名', Quantity AS N'购买数量' FROM ShoppingCartItems ORDER BY Id DESC";
            SaleOrder.QuerySql = "SELECT Id, OrderNo AS N'订单编号', CustomerCode AS N'客户编号', CustomerName AS N'客户姓名', OrderStatus AS N'订单状态', OrderAmount AS N'订单金额' FROM SaleOrders ORDER BY Id DESC";
            OrderDetail.QuerySql = "SELECT Id, OrderNo AS N'订单编号', BookCode AS N'图书编号', BookName AS N'书名', Quantity AS N'数量', UnitPrice AS N'单价', Subtotal AS N'小计' FROM OrderDetails ORDER BY Id DESC";
            Payment.QuerySql = "SELECT Id, OrderNo AS N'订单编号', PaymentMethod AS N'支付方式', PaymentAmount AS N'支付金额', PaymentStatus AS N'支付状态' FROM Payments ORDER BY Id DESC";
            Delivery.QuerySql = "SELECT Id, OrderNo AS N'订单编号', ReceiverAddress AS N'收货地址', LogisticsNo AS N'物流单号', DeliveryStatus AS N'发货状态' FROM OrderDeliveries ORDER BY Id DESC";
            ReturnRefund.QuerySql = "SELECT Id, OrderNo AS N'订单编号', BookCode AS N'图书编号', ReturnQuantity AS N'退货数量', ApplyReason AS N'申请原因', AuditStatus AS N'审核状态', RefundAmount AS N'退款金额' FROM ReturnRefunds ORDER BY Id DESC";
            CustomerHistory.QuerySql = "SELECT Id, CustomerCode AS N'客户编号', CustomerName AS N'客户姓名', OrderNo AS N'订单编号', BookName AS N'图书名称', Quantity AS N'购买数量', PurchaseAmount AS N'购买金额' FROM CustomerPurchaseRecords ORDER BY Id DESC";
            SaleStatistics.QuerySql = @"
SELECT
    1 AS Id,
    N'今日' AS N'统计类型',
    ISNULL(SUM(d.Quantity), 0) AS N'销售数量',
    ISNULL(SUM(d.Subtotal), 0) AS N'销售金额'
FROM OrderDetails d
INNER JOIN SaleOrders o ON d.OrderNo = o.OrderNo
WHERE CONVERT(DATE, o.OrderTime) = CONVERT(DATE, GETDATE())
  AND o.OrderStatus <> N'已取消'
UNION ALL
SELECT
    2,
    N'本周',
    ISNULL(SUM(d.Quantity), 0),
    ISNULL(SUM(d.Subtotal), 0)
FROM OrderDetails d
INNER JOIN SaleOrders o ON d.OrderNo = o.OrderNo
WHERE DATEDIFF(WEEK, 0, o.OrderTime) = DATEDIFF(WEEK, 0, GETDATE())
  AND o.OrderStatus <> N'已取消'
UNION ALL
SELECT
    3,
    N'本月',
    ISNULL(SUM(d.Quantity), 0),
    ISNULL(SUM(d.Subtotal), 0)
FROM OrderDetails d
INNER JOIN SaleOrders o ON d.OrderNo = o.OrderNo
WHERE YEAR(o.OrderTime) = YEAR(GETDATE())
  AND MONTH(o.OrderTime) = MONTH(GETDATE())
  AND o.OrderStatus <> N'已取消'
UNION ALL
SELECT
    4,
    N'全年',
    ISNULL(SUM(d.Quantity), 0),
    ISNULL(SUM(d.Subtotal), 0)
FROM OrderDetails d
INNER JOIN SaleOrders o ON d.OrderNo = o.OrderNo
WHERE YEAR(o.OrderTime) = YEAR(GETDATE())
  AND o.OrderStatus <> N'已取消'";

            BestSeller.QuerySql = @"
SELECT
    ROW_NUMBER() OVER (ORDER BY SUM(d.Quantity) DESC, SUM(d.Subtotal) DESC) AS Id,
    d.BookCode AS N'图书编号',
    d.BookName AS N'书名',
    SUM(d.Quantity) AS N'销量',
    SUM(d.Subtotal) AS N'销售金额',
    ROW_NUMBER() OVER (ORDER BY SUM(d.Quantity) DESC, SUM(d.Subtotal) DESC) AS N'排行'
FROM OrderDetails d
INNER JOIN SaleOrders o ON d.OrderNo = o.OrderNo
WHERE o.OrderStatus <> N'已取消'
GROUP BY d.BookCode, d.BookName
ORDER BY N'排行'";
        }

        private static ModuleFieldDefinition Field(string displayName, string columnName, bool readOnly = false)
        {
            return new ModuleFieldDefinition(displayName, columnName, readOnly);
        }
    }
}
