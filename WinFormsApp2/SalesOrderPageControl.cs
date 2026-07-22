using System.Data;
using System.Data.SqlClient;

namespace WinFormsApp2
{
    internal sealed class SalesOrderPageControl : UserControl
    {
        private readonly DataGridView orderGrid = new DataGridView();
        private readonly DataGridView detailGrid = new DataGridView();
        private readonly TextBox txtOrderNo = new TextBox();
        private readonly TextBox txtCustomerCode = new TextBox();
        private readonly ComboBox cmbOrderStatus = new ComboBox();
        private readonly TextBox txtBookCode = new TextBox();
        private readonly NumericUpDown numQuantity = new NumericUpDown();
        private string? selectedOrderNo;

        public SalesOrderPageControl()
        {
            InitializeComponent();
            Reload();
            ClearOrderForm();
        }

        public void Reload()
        {
            LoadOrders();
            LoadDetails(selectedOrderNo);
        }

        private void InitializeComponent()
        {
            Dock = DockStyle.Fill;
            BackColor = UiTheme.Page;
            Font = UiTheme.Regular(10F);

            TableLayoutPanel root = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                BackColor = UiTheme.Page,
                Padding = new Padding(18)
            };
            root.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 340F));
            root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

            root.Controls.Add(CreateEditPanel(), 0, 0);
            root.Controls.Add(CreateListPanel(), 1, 0);
            Controls.Add(root);
        }

        private Control CreateEditPanel()
        {
            Panel panel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = UiTheme.Surface,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(18)
            };

            FlowLayoutPanel fields = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                BackColor = UiTheme.Surface
            };

            UiTheme.StyleInput(txtOrderNo);
            UiTheme.StyleInput(txtCustomerCode);
            UiTheme.StyleInput(txtBookCode);

            cmbOrderStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbOrderStatus.Items.AddRange(new object[] { "待支付", "待发货", "已发货", "已完成", "已取消", "已退货" });
            cmbOrderStatus.SelectedIndex = 0;
            cmbOrderStatus.Height = 32;
            cmbOrderStatus.BackColor = UiTheme.Surface;
            cmbOrderStatus.ForeColor = UiTheme.Text;
            cmbOrderStatus.Font = UiTheme.Regular(10.5F);

            numQuantity.Minimum = 1;
            numQuantity.Maximum = 9999;
            numQuantity.Value = 1;
            numQuantity.Height = 32;
            numQuantity.Font = UiTheme.Regular(10.5F);

            fields.Controls.Add(CreateField("订单编号", txtOrderNo));
            fields.Controls.Add(CreateField("客户编号", txtCustomerCode));
            fields.Controls.Add(CreateField("订单状态", cmbOrderStatus));
            fields.Controls.Add(CreateDivider());
            fields.Controls.Add(CreateField("图书编号", txtBookCode));
            fields.Controls.Add(CreateField("购买数量", numQuantity));

            TableLayoutPanel actions = new TableLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 152,
                ColumnCount = 2,
                RowCount = 3,
                Padding = new Padding(0, 10, 0, 0)
            };
            actions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            actions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            actions.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));
            actions.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));
            actions.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));

            actions.Controls.Add(CreateButton("创建订单", btnCreateOrder_Click, true), 0, 0);
            actions.Controls.Add(CreateButton("更新状态", btnUpdateStatus_Click), 1, 0);
            actions.Controls.Add(CreateButton("添加图书", btnAddDetail_Click, true), 0, 1);
            actions.Controls.Add(CreateButton("删除明细", btnDeleteDetail_Click), 1, 1);
            actions.Controls.Add(CreateButton("清空", btnClear_Click), 0, 2);
            actions.Controls.Add(CreateButton("刷新", (_, _) => Reload()), 1, 2);

            panel.Controls.Add(fields);
            panel.Controls.Add(actions);
            panel.Controls.Add(CreateHint("先创建或选择订单，再添加订单明细。"));
            panel.Controls.Add(CreateTitle("订单录入"));
            return panel;
        }

        private Control CreateListPanel()
        {
            TableLayoutPanel panel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 2,
                BackColor = UiTheme.Page,
                Padding = new Padding(18, 0, 0, 0)
            };
            panel.RowStyles.Add(new RowStyle(SizeType.Percent, 48F));
            panel.RowStyles.Add(new RowStyle(SizeType.Percent, 52F));

            panel.Controls.Add(CreateGridPanel("订单列表", orderGrid), 0, 0);
            panel.Controls.Add(CreateGridPanel("当前订单明细", detailGrid), 0, 1);
            return panel;
        }

        private Panel CreateGridPanel(string title, DataGridView grid)
        {
            Panel panel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = UiTheme.Surface,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(0, 0, 0, 14),
                Padding = new Padding(18, 44, 18, 18)
            };

            ConfigureGrid(grid);
            panel.Controls.Add(grid);
            panel.Controls.Add(CreateTitle(title));
            return panel;
        }

        private static Label CreateTitle(string text)
        {
            return new Label
            {
                Dock = DockStyle.Top,
                Height = 38,
                Text = text,
                Font = UiTheme.Bold(12F),
                ForeColor = UiTheme.Text,
                TextAlign = ContentAlignment.MiddleLeft
            };
        }

        private static Label CreateHint(string text)
        {
            return new Label
            {
                Dock = DockStyle.Top,
                Height = 46,
                Text = text,
                ForeColor = UiTheme.MutedText,
                TextAlign = ContentAlignment.TopLeft
            };
        }

        private static Control CreateDivider()
        {
            return new Panel
            {
                Width = 286,
                Height = 16,
                BackColor = UiTheme.Surface
            };
        }

        private static Panel CreateField(string label, Control input)
        {
            Panel panel = new Panel
            {
                Width = 286,
                Height = 68,
                Margin = new Padding(0, 0, 0, 10),
                BackColor = UiTheme.Surface
            };
            input.Dock = DockStyle.Top;
            panel.Controls.Add(input);
            panel.Controls.Add(new Label
            {
                Text = label,
                Dock = DockStyle.Top,
                Height = 24,
                ForeColor = UiTheme.MutedText
            });
            return panel;
        }

        private static Button CreateButton(string text, EventHandler handler, bool primary = false)
        {
            Button button = new Button
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(4),
                Text = text
            };
            if (primary)
            {
                UiTheme.StylePrimaryButton(button);
            }
            else
            {
                UiTheme.StyleSecondaryButton(button);
            }
            button.Click += handler;
            return button;
        }

        private void ConfigureGrid(DataGridView grid)
        {
            grid.Dock = DockStyle.Fill;
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.MultiSelect = false;
            grid.ReadOnly = true;
            grid.RowHeadersVisible = false;
            grid.BackgroundColor = UiTheme.Surface;
            grid.BorderStyle = BorderStyle.None;
            grid.EnableHeadersVisualStyles = false;
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 252);
            grid.ColumnHeadersDefaultCellStyle.ForeColor = UiTheme.MutedText;
            grid.ColumnHeadersDefaultCellStyle.Font = UiTheme.Bold(9F);
            grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(219, 234, 254);
            grid.DefaultCellStyle.SelectionForeColor = UiTheme.Text;
            grid.GridColor = UiTheme.Border;
            grid.RowTemplate.Height = 32;
            if (grid == orderGrid)
            {
                grid.CellClick += orderGrid_CellClick;
            }
        }

        private void LoadOrders()
        {
            orderGrid.DataSource = DbHelper.Query(@"
SELECT Id, OrderNo AS N'订单编号', CustomerCode AS N'客户编号', CustomerName AS N'客户姓名',
       CONVERT(NVARCHAR(16), OrderTime, 120) AS N'下单时间',
       OrderStatus AS N'订单状态', OrderAmount AS N'订单金额'
FROM SaleOrders
ORDER BY Id DESC");
            HideColumn(orderGrid, "Id");
            orderGrid.ClearSelection();
            if (!string.IsNullOrWhiteSpace(selectedOrderNo))
            {
                SelectOrder(selectedOrderNo);
            }
        }

        private void LoadDetails(string? orderNo)
        {
            if (string.IsNullOrWhiteSpace(orderNo))
            {
                detailGrid.DataSource = new DataTable();
                return;
            }

            detailGrid.DataSource = DbHelper.Query(@"
SELECT Id, DetailNo AS N'明细编号', BookCode AS N'图书编号', BookName AS N'书名',
       Quantity AS N'数量', UnitPrice AS N'单价', Discount AS N'折扣', Subtotal AS N'小计'
FROM OrderDetails
WHERE OrderNo=@OrderNo
ORDER BY Id", new SqlParameter("@OrderNo", orderNo));
            HideColumn(detailGrid, "Id");
        }

        private void orderGrid_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || orderGrid.CurrentRow?.DataBoundItem is not DataRowView row)
            {
                return;
            }

            selectedOrderNo = row.Row["订单编号"].ToString();
            txtOrderNo.Text = selectedOrderNo;
            txtCustomerCode.Text = row.Row["客户编号"].ToString();
            string status = row.Row["订单状态"].ToString() ?? string.Empty;
            int index = cmbOrderStatus.FindStringExact(status);
            cmbOrderStatus.SelectedIndex = index >= 0 ? index : 0;
            LoadDetails(selectedOrderNo);
        }

        private void btnCreateOrder_Click(object? sender, EventArgs e)
        {
            string orderNo = txtOrderNo.Text.Trim();
            string customerCode = txtCustomerCode.Text.Trim();
            if (string.IsNullOrWhiteSpace(orderNo) || string.IsNullOrWhiteSpace(customerCode))
            {
                MessageBox.Show("请填写订单编号和客户编号。");
                return;
            }

            DataTable customer = DbHelper.Query("SELECT CustomerName FROM Customers WHERE CustomerCode=@CustomerCode",
                new SqlParameter("@CustomerCode", customerCode));
            if (customer.Rows.Count == 0)
            {
                MessageBox.Show("客户编号不存在。");
                return;
            }

            try
            {
                DbHelper.Execute(@"
INSERT INTO SaleOrders(OrderNo, CustomerCode, CustomerName, OrderTime, OrderStatus, OrderAmount)
VALUES(@OrderNo, @CustomerCode, @CustomerName, GETDATE(), @OrderStatus, 0)",
                    new SqlParameter("@OrderNo", orderNo),
                    new SqlParameter("@CustomerCode", customerCode),
                    new SqlParameter("@CustomerName", customer.Rows[0]["CustomerName"]),
                    new SqlParameter("@OrderStatus", cmbOrderStatus.Text));
                EnsurePaymentRecord(orderNo);
                EnsureDeliveryRecord(orderNo);
                selectedOrderNo = orderNo;
                Reload();
                SelectOrder(orderNo);
            }
            catch (Exception ex)
            {
                MessageBox.Show("创建订单失败：" + ex.Message);
            }
        }

        private void btnUpdateStatus_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(selectedOrderNo))
            {
                MessageBox.Show("请先选择订单。");
                return;
            }

            DbHelper.Execute("UPDATE SaleOrders SET OrderStatus=@OrderStatus WHERE OrderNo=@OrderNo",
                new SqlParameter("@OrderStatus", cmbOrderStatus.Text),
                new SqlParameter("@OrderNo", selectedOrderNo));
            Reload();
            SelectOrder(selectedOrderNo);
        }

        private void btnAddDetail_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(selectedOrderNo) || !OrderExists(selectedOrderNo))
            {
                MessageBox.Show("请先创建或选择一个订单。");
                return;
            }

            string bookCode = txtBookCode.Text.Trim();
            int quantity = Convert.ToInt32(numQuantity.Value);
            if (string.IsNullOrWhiteSpace(bookCode))
            {
                MessageBox.Show("请填写图书编号。");
                return;
            }

            DataTable book = DbHelper.Query("SELECT BookName, Price, Stock FROM Books WHERE BookCode=@BookCode",
                new SqlParameter("@BookCode", bookCode));
            if (book.Rows.Count == 0)
            {
                MessageBox.Show("图书编号不存在。");
                return;
            }

            int stock = Convert.ToInt32(book.Rows[0]["Stock"]);
            if (quantity > stock)
            {
                MessageBox.Show("购买数量不能超过当前库存。");
                return;
            }

            try
            {
                DbHelper.Execute(@"
INSERT INTO OrderDetails(DetailNo, OrderNo, BookCode, BookName, Quantity, UnitPrice, Discount)
VALUES(@DetailNo, @OrderNo, @BookCode, @BookName, @Quantity, @UnitPrice, 1)",
                    new SqlParameter("@DetailNo", GenerateDetailNo()),
                    new SqlParameter("@OrderNo", selectedOrderNo),
                    new SqlParameter("@BookCode", bookCode),
                    new SqlParameter("@BookName", book.Rows[0]["BookName"]),
                    new SqlParameter("@Quantity", quantity),
                    new SqlParameter("@UnitPrice", book.Rows[0]["Price"]));
                EnsurePaymentRecord(selectedOrderNo);
                EnsureDeliveryRecord(selectedOrderNo);
                Reload();
                SelectOrder(selectedOrderNo);
                txtBookCode.Clear();
                numQuantity.Value = 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("添加明细失败：" + ex.Message);
            }
        }

        private void btnDeleteDetail_Click(object? sender, EventArgs e)
        {
            if (detailGrid.CurrentRow?.DataBoundItem is not DataRowView row || !row.Row.Table.Columns.Contains("Id"))
            {
                MessageBox.Show("请先选择一条订单明细。");
                return;
            }

            string? orderNo = selectedOrderNo;
            DbHelper.Execute("DELETE FROM OrderDetails WHERE Id=@Id", new SqlParameter("@Id", row.Row["Id"]));
            Reload();
            if (!string.IsNullOrWhiteSpace(orderNo))
            {
                SelectOrder(orderNo);
            }
        }

        private void btnClear_Click(object? sender, EventArgs e)
        {
            ClearOrderForm();
        }

        private void ClearOrderForm()
        {
            selectedOrderNo = null;
            txtOrderNo.Text = GenerateOrderNo();
            txtCustomerCode.Clear();
            txtBookCode.Clear();
            numQuantity.Value = 1;
            cmbOrderStatus.SelectedIndex = 0;
            orderGrid.ClearSelection();
            detailGrid.DataSource = new DataTable();
            try
            {
                orderGrid.CurrentCell = null;
            }
            catch
            {
            }
        }

        private bool OrderExists(string orderNo)
        {
            return Convert.ToInt32(DbHelper.Query("SELECT COUNT(*) FROM SaleOrders WHERE OrderNo=@OrderNo",
                new SqlParameter("@OrderNo", orderNo)).Rows[0][0]) > 0;
        }

        private static void EnsurePaymentRecord(string orderNo)
        {
            DbHelper.Execute(@"
IF NOT EXISTS (SELECT 1 FROM Payments WHERE OrderNo=@OrderNo)
BEGIN
    INSERT INTO Payments(PaymentNo, OrderNo, PaymentMethod, PaymentAmount, PaymentTime, PaymentStatus, TransactionNo)
    SELECT @PaymentNo, o.OrderNo, N'未选择', o.OrderAmount, NULL, N'未支付', NULL
    FROM SaleOrders o
    WHERE o.OrderNo=@OrderNo;
END
ELSE
BEGIN
    UPDATE p
    SET p.PaymentAmount = o.OrderAmount
    FROM Payments p
    INNER JOIN SaleOrders o ON p.OrderNo = o.OrderNo
    WHERE p.OrderNo=@OrderNo AND p.PaymentStatus <> N'已支付';
END",
                new SqlParameter("@OrderNo", orderNo),
                new SqlParameter("@PaymentNo", GeneratePaymentNo()));
        }

        private static void EnsureDeliveryRecord(string orderNo)
        {
            DbHelper.Execute(@"
IF NOT EXISTS (SELECT 1 FROM OrderDeliveries WHERE OrderNo=@OrderNo)
BEGIN
    INSERT INTO OrderDeliveries(DeliveryNo, OrderNo, ReceiverAddress, LogisticsCompany, LogisticsNo, DeliveryTime, DeliveryStatus)
    SELECT @DeliveryNo, o.OrderNo, ISNULL(c.Address, N'待补充'), NULL, NULL, NULL,
           CASE
               WHEN o.OrderStatus = N'已完成' THEN N'已签收'
               WHEN o.OrderStatus = N'已发货' THEN N'已发货'
               WHEN o.OrderStatus = N'待发货' THEN N'待发货'
               ELSE N'未发货'
           END
    FROM SaleOrders o
    LEFT JOIN Customers c ON o.CustomerCode = c.CustomerCode
    WHERE o.OrderNo=@OrderNo AND o.OrderStatus NOT IN (N'已取消', N'已退货');
END",
                new SqlParameter("@OrderNo", orderNo),
                new SqlParameter("@DeliveryNo", GenerateDeliveryNo()));
        }

        private void SelectOrder(string orderNo)
        {
            selectedOrderNo = orderNo;
            foreach (DataGridViewRow row in orderGrid.Rows)
            {
                if (row.DataBoundItem is DataRowView view && view.Row["订单编号"].ToString() == orderNo)
                {
                    row.Selected = true;
                    orderGrid.CurrentCell = row.Cells.Cast<DataGridViewCell>().First(cell => cell.Visible);
                    txtOrderNo.Text = orderNo;
                    txtCustomerCode.Text = view.Row["客户编号"].ToString();
                    string status = view.Row["订单状态"].ToString() ?? string.Empty;
                    int index = cmbOrderStatus.FindStringExact(status);
                    cmbOrderStatus.SelectedIndex = index >= 0 ? index : 0;
                    LoadDetails(orderNo);
                    break;
                }
            }
        }

        private static void HideColumn(DataGridView grid, string column)
        {
            if (grid.Columns.Contains(column))
            {
                grid.Columns[column].Visible = false;
            }
        }

        private static string GenerateOrderNo()
        {
            return "DD" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
        }

        private static string GenerateDetailNo()
        {
            return "MX" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
        }

        private static string GeneratePaymentNo()
        {
            return "FK" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
        }

        private static string GenerateDeliveryNo()
        {
            return "FH" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
        }
    }
}
