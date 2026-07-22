using System.Data;
using System.Data.SqlClient;

namespace WinFormsApp2
{
    public partial class MainForm : Form
    {
        private readonly Panel contentPanel = new Panel();
        private readonly Label breadcrumbLabel = new Label();
        private Button? activeNav;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            BuildShell();
            ShowDashboard();
        }

        private void BuildShell()
        {
            Controls.Clear();

            TableLayoutPanel root = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = UiTheme.Page
            };
            root.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 236F));
            root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

            root.Controls.Add(CreateSidebar(), 0, 0);
            root.Controls.Add(CreateMainArea(), 1, 0);
            Controls.Add(root);
        }

        private Control CreateSidebar()
        {
            Panel sidebar = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(12, 31, 56),
                Padding = new Padding(0)
            };

            Label brand = new Label
            {
                Dock = DockStyle.Top,
                Height = 78,
                Text = "  图书销售管理系统",
                Font = UiTheme.Bold(13F),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleLeft
            };

            FlowLayoutPanel nav = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Padding = new Padding(10, 16, 10, 0),
                BackColor = Color.FromArgb(12, 31, 56)
            };

            nav.Controls.Add(CreateNavButton("首页", null, true));
            nav.Controls.Add(CreateNavButton("图书管理", ModuleCatalog.BookProfileGroup));
            nav.Controls.Add(CreateNavButton("库存管理", ModuleCatalog.InventoryFlowGroup));
            nav.Controls.Add(CreateNavButton("销售订单", ModuleCatalog.CustomerOrderGroup));
            nav.Controls.Add(CreateNavButton("数据统计", ModuleCatalog.BusinessAnalysisGroup));

            Label footer = new Label
            {
                Dock = DockStyle.Bottom,
                Height = 54,
                Text = "  v1.0.0",
                Font = UiTheme.Regular(9F),
                ForeColor = Color.FromArgb(148, 163, 184),
                TextAlign = ContentAlignment.MiddleLeft
            };

            sidebar.Controls.Add(nav);
            sidebar.Controls.Add(footer);
            sidebar.Controls.Add(brand);
            return sidebar;
        }

        private Button CreateNavButton(string text, ModuleGroupDefinition? group, bool selected = false)
        {
            Button button = new Button
            {
                Width = 216,
                Height = 52,
                Margin = new Padding(0, 0, 0, 8),
                Padding = new Padding(16, 0, 14, 0),
                Text = text,
                Tag = group,
                TextAlign = ContentAlignment.MiddleLeft,
                FlatStyle = FlatStyle.Flat,
                Font = UiTheme.Bold(10.5F),
                Cursor = Cursors.Hand,
                UseVisualStyleBackColor = false
            };
            button.FlatAppearance.BorderSize = 0;
            button.Click += NavButton_Click;
            StyleNavButton(button, selected);
            if (selected)
            {
                activeNav = button;
            }
            return button;
        }

        private void NavButton_Click(object? sender, EventArgs e)
        {
            if (sender is not Button button)
            {
                return;
            }

            if (activeNav != null)
            {
                StyleNavButton(activeNav, false);
            }
            activeNav = button;
            StyleNavButton(button, true);

            if (button.Tag is ModuleGroupDefinition group)
            {
                ShowWorkspace(group);
            }
            else
            {
                ShowDashboard();
            }
        }

        private static void StyleNavButton(Button button, bool selected)
        {
            button.BackColor = selected ? Color.FromArgb(37, 99, 235) : Color.FromArgb(12, 31, 56);
            button.ForeColor = selected ? Color.White : Color.FromArgb(203, 213, 225);
            button.FlatAppearance.MouseOverBackColor = selected ? Color.FromArgb(37, 99, 235) : Color.FromArgb(22, 45, 78);
            button.FlatAppearance.MouseDownBackColor = Color.FromArgb(29, 78, 216);
        }

        private Control CreateMainArea()
        {
            TableLayoutPanel main = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                BackColor = UiTheme.Page
            };
            main.RowStyles.Add(new RowStyle(SizeType.Absolute, 76F));
            main.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            main.Controls.Add(CreateTopbar(), 0, 0);
            contentPanel.Dock = DockStyle.Fill;
            contentPanel.BackColor = UiTheme.Page;
            main.Controls.Add(contentPanel, 0, 1);
            return main;
        }

        private Control CreateTopbar()
        {
            Panel topbar = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = UiTheme.Surface,
                Padding = new Padding(22, 0, 24, 0)
            };

            breadcrumbLabel.Dock = DockStyle.Left;
            breadcrumbLabel.Width = 320;
            breadcrumbLabel.Text = "首页 / 数据看板";
            breadcrumbLabel.Font = UiTheme.Regular(10F);
            breadcrumbLabel.ForeColor = UiTheme.MutedText;
            breadcrumbLabel.TextAlign = ContentAlignment.MiddleLeft;

            topbar.Controls.Add(breadcrumbLabel);
            return topbar;
        }

        private void ShowDashboard()
        {
            contentPanel.Controls.Clear();
            breadcrumbLabel.Text = "首页 / 数据看板";

            TableLayoutPanel dashboard = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = UiTheme.Page,
                Padding = new Padding(20),
                ColumnCount = 4,
                RowCount = 4
            };
            for (int i = 0; i < 4; i++)
            {
                dashboard.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            }
            dashboard.RowStyles.Add(new RowStyle(SizeType.Absolute, 132F));
            dashboard.RowStyles.Add(new RowStyle(SizeType.Percent, 42F));
            dashboard.RowStyles.Add(new RowStyle(SizeType.Percent, 34F));
            dashboard.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));

            DashboardData data = LoadDashboardData();
            dashboard.Controls.Add(CreateMetricCard("今日销售额", data.TodayAmount.ToString("C"), "订单收入合计", Color.FromArgb(37, 99, 235)), 0, 0);
            dashboard.Controls.Add(CreateMetricCard("今日订单数", data.TodayOrders.ToString(), "今日新增订单", Color.FromArgb(22, 163, 74)), 1, 0);
            dashboard.Controls.Add(CreateMetricCard("库存图书总数", data.BookStock.ToString("N0"), "当前库存总量", Color.FromArgb(245, 158, 11)), 2, 0);
            dashboard.Controls.Add(CreateMetricCard("客户数量", data.CustomerCount.ToString("N0"), "会员资料总数", Color.FromArgb(124, 58, 237)), 3, 0);

            Panel trendPanel = CreateSectionPanel("销售趋势");
            AddSectionContent(trendPanel, new DashboardChartPanel { Dock = DockStyle.Fill, ChartKind = DashboardChartKind.Line, Values = data.TrendValues, Labels = data.TrendLabels, AccentColor = UiTheme.Accent });
            dashboard.Controls.Add(trendPanel, 0, 1);
            dashboard.SetColumnSpan(trendPanel, 2);

            Panel rankPanel = CreateSectionPanel("热门图书销量 TOP10");
            AddSectionContent(rankPanel, new DashboardChartPanel { Dock = DockStyle.Fill, ChartKind = DashboardChartKind.Bar, Values = data.RankValues, Labels = data.RankLabels, AccentColor = Color.FromArgb(37, 99, 235) });
            dashboard.Controls.Add(rankPanel, 2, 1);
            dashboard.SetColumnSpan(rankPanel, 2);

            Panel warningPanel = CreateSectionPanel("库存预警");
            AddSectionContent(warningPanel, CreateDashboardGrid(data.StockWarnings));
            dashboard.Controls.Add(warningPanel, 0, 2);
            dashboard.SetColumnSpan(warningPanel, 2);

            Panel orderPanel = CreateSectionPanel("最近订单");
            AddSectionContent(orderPanel, CreateDashboardGrid(data.RecentOrders));
            dashboard.Controls.Add(orderPanel, 2, 2);
            dashboard.SetColumnSpan(orderPanel, 2);

            Label copyright = new Label
            {
                Dock = DockStyle.Fill,
                Text = "© 2026 图书销售管理系统",
                ForeColor = UiTheme.MutedText,
                TextAlign = ContentAlignment.MiddleLeft
            };
            dashboard.Controls.Add(copyright, 0, 3);
            dashboard.SetColumnSpan(copyright, 4);

            contentPanel.Controls.Add(dashboard);
        }

        private void ShowWorkspace(ModuleGroupDefinition group)
        {
            contentPanel.Controls.Clear();
            breadcrumbLabel.Text = $"{group.Title} / 业务管理";
            ManagementWorkspaceForm form = new ManagementWorkspaceForm(group)
            {
                TopLevel = false,
                FormBorderStyle = FormBorderStyle.None,
                Dock = DockStyle.Fill
            };
            contentPanel.Controls.Add(form);
            form.Show();
        }

        private static Panel CreateMetricCard(string title, string value, string hint, Color accent)
        {
            Panel card = CreateCardPanel();
            card.Padding = new Padding(22, 18, 22, 18);

            Panel icon = new Panel
            {
                BackColor = accent,
                Width = 64,
                Height = 64,
                Location = new Point(22, 28)
            };

            Label iconText = new Label
            {
                Dock = DockStyle.Fill,
                Text = "■",
                ForeColor = Color.White,
                Font = UiTheme.Bold(16F),
                TextAlign = ContentAlignment.MiddleCenter
            };
            icon.Controls.Add(iconText);

            Label titleLabel = new Label
            {
                Location = new Point(106, 26),
                Size = new Size(190, 24),
                Text = title,
                ForeColor = UiTheme.MutedText,
                Font = UiTheme.Regular(9.5F)
            };

            Label valueLabel = new Label
            {
                Location = new Point(106, 53),
                Size = new Size(220, 34),
                Text = value,
                ForeColor = UiTheme.Text,
                Font = UiTheme.Bold(17F)
            };

            Label hintLabel = new Label
            {
                Location = new Point(106, 92),
                Size = new Size(220, 22),
                Text = hint,
                ForeColor = UiTheme.MutedText,
                Font = UiTheme.Regular(9F)
            };

            card.Controls.Add(hintLabel);
            card.Controls.Add(valueLabel);
            card.Controls.Add(titleLabel);
            card.Controls.Add(icon);
            return card;
        }

        private static Panel CreateSectionPanel(string title)
        {
            Panel panel = CreateCardPanel();
            panel.Padding = new Padding(0);

            Label titleLabel = new Label
            {
                Dock = DockStyle.Top,
                Height = 44,
                Text = title,
                Font = UiTheme.Bold(11.5F),
                ForeColor = UiTheme.Text,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(18, 0, 0, 0)
            };

            Panel body = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(18, 4, 18, 18),
                BackColor = UiTheme.Surface
            };

            panel.Controls.Add(body);
            panel.Controls.Add(titleLabel);
            panel.Tag = body;
            return panel;
        }

        private static void AddSectionContent(Panel section, Control content)
        {
            if (section.Tag is not Panel body)
            {
                section.Controls.Add(content);
                return;
            }

            body.Controls.Clear();
            body.Controls.Add(content);
        }

        private static Panel CreateCardPanel()
        {
            return new Panel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(8),
                BackColor = UiTheme.Surface,
                BorderStyle = BorderStyle.FixedSingle
            };
        }

        private static DataGridView CreateDashboardGrid(DataTable table)
        {
            DataGridView grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                DataSource = table,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BorderStyle = BorderStyle.None,
                BackgroundColor = UiTheme.Surface,
                EnableHeadersVisualStyles = false
            };
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 252);
            grid.ColumnHeadersDefaultCellStyle.ForeColor = UiTheme.MutedText;
            grid.ColumnHeadersDefaultCellStyle.Font = UiTheme.Bold(9F);
            grid.DefaultCellStyle.Font = UiTheme.Regular(9F);
            grid.DefaultCellStyle.ForeColor = UiTheme.Text;
            grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(219, 234, 254);
            grid.DefaultCellStyle.SelectionForeColor = UiTheme.Text;
            grid.GridColor = UiTheme.Border;
            grid.RowTemplate.Height = 30;
            return grid;
        }

        private static DashboardData LoadDashboardData()
        {
            DashboardData data = new DashboardData();
            try
            {
                data.TodayAmount = Convert.ToDecimal(DbHelper.Query(@"
SELECT ISNULL(SUM(OrderAmount), 0)
FROM SaleOrders
WHERE CONVERT(DATE, OrderTime) = CONVERT(DATE, GETDATE())
  AND OrderStatus <> N'已取消'").Rows[0][0]);

                data.TodayOrders = Convert.ToInt32(DbHelper.Query(@"
SELECT COUNT(*)
FROM SaleOrders
WHERE CONVERT(DATE, OrderTime) = CONVERT(DATE, GETDATE())").Rows[0][0]);

                data.BookStock = Convert.ToInt32(DbHelper.Query("SELECT ISNULL(SUM(Stock), 0) FROM Books").Rows[0][0]);
                data.CustomerCount = Convert.ToInt32(DbHelper.Query("SELECT COUNT(*) FROM Customers").Rows[0][0]);

                DataTable trend = DbHelper.Query(@"
SELECT TOP 7 CONVERT(NVARCHAR(5), OrderTime, 110) AS Label, SUM(OrderAmount) AS Amount
FROM SaleOrders
WHERE OrderStatus <> N'已取消'
GROUP BY CONVERT(DATE, OrderTime), CONVERT(NVARCHAR(5), OrderTime, 110)
ORDER BY CONVERT(DATE, OrderTime)");
                data.TrendLabels = trend.Rows.Cast<DataRow>().Select(row => row["Label"].ToString() ?? string.Empty).ToArray();
                data.TrendValues = trend.Rows.Cast<DataRow>().Select(row => Convert.ToDecimal(row["Amount"])).ToArray();

                DataTable rank = DbHelper.Query(@"
SELECT TOP 10 BookName, SUM(Quantity) AS Quantity
FROM OrderDetails
GROUP BY BookName
ORDER BY SUM(Quantity) DESC");
                data.RankLabels = rank.Rows.Cast<DataRow>().Select(row => row["BookName"].ToString() ?? string.Empty).ToArray();
                data.RankValues = rank.Rows.Cast<DataRow>().Select(row => Convert.ToDecimal(row["Quantity"])).ToArray();

                data.StockWarnings = DbHelper.Query(@"
SELECT TOP 5 BookName AS N'图书', Stock AS N'库存', MinStock AS N'预警值'
FROM Books
WHERE Stock <= MinStock
ORDER BY Stock");

                data.RecentOrders = DbHelper.Query(@"
SELECT TOP 5 OrderNo AS N'订单编号', CustomerName AS N'客户', OrderAmount AS N'金额', OrderStatus AS N'状态'
FROM SaleOrders
ORDER BY OrderTime DESC");
            }
            catch
            {
                data.StockWarnings = new DataTable();
                data.RecentOrders = new DataTable();
            }

            return data;
        }

        private sealed class DashboardData
        {
            public decimal TodayAmount { get; set; }
            public int TodayOrders { get; set; }
            public int BookStock { get; set; }
            public int CustomerCount { get; set; }
            public string[] TrendLabels { get; set; } = Array.Empty<string>();
            public decimal[] TrendValues { get; set; } = Array.Empty<decimal>();
            public string[] RankLabels { get; set; } = Array.Empty<string>();
            public decimal[] RankValues { get; set; } = Array.Empty<decimal>();
            public DataTable StockWarnings { get; set; } = new DataTable();
            public DataTable RecentOrders { get; set; } = new DataTable();
        }
    }
}
