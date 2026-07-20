using System.Data;
using System.Data.SqlClient;

namespace WinFormsApp2
{
    internal sealed class ModulePageControl : UserControl
    {
        private readonly ModuleDefinition module;
        private readonly Dictionary<string, Control> inputs = new Dictionary<string, Control>();
        private readonly DataGridView grid = new DataGridView();
        private readonly FlowLayoutPanel fieldsPanel = new FlowLayoutPanel();
        private readonly TextBox searchBox = new TextBox();
        private DataTable table = new DataTable();

        public ModulePageControl(ModuleDefinition module)
        {
            this.module = module;
            InitializeComponent();
            if (module.CanWrite)
            {
                BuildInputs();
            }
            Reload();
        }

        public void Reload()
        {
            table = LoadTable();
            grid.DataSource = table.DefaultView;
            ApplySearchFilter();
            HideSystemColumns();
        }

        private void InitializeComponent()
        {
            Dock = DockStyle.Fill;
            BackColor = Color.White;
            Font = new Font("Microsoft YaHei UI", 10F);

            TableLayoutPanel layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = module.CanWrite ? 2 : 1,
                Padding = new Padding(18),
                BackColor = Color.White
            };
            if (module.CanWrite)
            {
                layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 360F));
                layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            }
            else
            {
                layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            }

            if (module.CanWrite)
            {
                Panel editPanel = new Panel
                {
                    Dock = DockStyle.Fill,
                    Padding = new Padding(0, 0, 18, 0),
                    BackColor = Color.White
                };
                editPanel.Controls.Add(fieldsPanel);
                editPanel.Controls.Add(CreateActionPanel());
                editPanel.Controls.Add(CreateDescriptionLabel());
                editPanel.Controls.Add(CreateTitleLabel(module.EditTitle));
                layout.Controls.Add(editPanel, 0, 0);
            }

            Panel listPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = module.CanWrite ? new Padding(18, 0, 0, 0) : Padding.Empty,
                BackColor = Color.White
            };
            ConfigureGrid();
            listPanel.Controls.Add(grid);
            listPanel.Controls.Add(CreateSearchPanel());
            listPanel.Controls.Add(CreateTitleLabel(module.ListTitle));

            fieldsPanel.Dock = DockStyle.Fill;
            fieldsPanel.FlowDirection = FlowDirection.TopDown;
            fieldsPanel.WrapContents = false;
            fieldsPanel.AutoScroll = true;
            fieldsPanel.BackColor = Color.White;

            layout.Controls.Add(listPanel, module.CanWrite ? 1 : 0, 0);
            Controls.Add(layout);
        }

        private Label CreateDescriptionLabel()
        {
            return new Label
            {
                Dock = DockStyle.Top,
                Height = 58,
                Text = module.Description,
                ForeColor = Color.FromArgb(80, 80, 80),
                TextAlign = ContentAlignment.TopLeft
            };
        }

        private static Label CreateTitleLabel(string text)
        {
            return new Label
            {
                Dock = DockStyle.Top,
                Height = 34,
                Text = text,
                Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft
            };
        }

        private TableLayoutPanel CreateActionPanel()
        {
            TableLayoutPanel panel = new TableLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 98,
                ColumnCount = 2,
                RowCount = 2,
                Padding = new Padding(0, 8, 0, 0)
            };
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            panel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            panel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));

            Button btnAdd = CreateActionButton("添加", btnAdd_Click);
            Button btnUpdate = CreateActionButton("修改", btnUpdate_Click);
            Button btnDelete = CreateActionButton("删除", btnDelete_Click);
            Button btnClear = CreateActionButton("清空", btnClear_Click);

            panel.Controls.Add(btnAdd, 0, 0);
            panel.Controls.Add(btnUpdate, 1, 0);
            panel.Controls.Add(btnDelete, 0, 1);
            panel.Controls.Add(btnClear, 1, 1);
            return panel;
        }

        private Panel CreateSearchPanel()
        {
            Panel panel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 46,
                Padding = new Padding(0, 4, 0, 8),
                BackColor = Color.White
            };

            FlowLayoutPanel row = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                BackColor = Color.White
            };

            searchBox.Width = 280;
            searchBox.Height = 32;
            searchBox.Margin = new Padding(0, 4, 8, 0);
            searchBox.BorderStyle = BorderStyle.FixedSingle;
            searchBox.KeyDown += searchBox_KeyDown;

            Button btnSearch = CreateSearchButton("查询", btnSearch_Click);
            Button btnReset = CreateSearchButton("全部", btnReset_Click);

            row.Controls.Add(searchBox);
            row.Controls.Add(btnSearch);
            row.Controls.Add(btnReset);
            panel.Controls.Add(row);
            return panel;
        }

        private static Button CreateSearchButton(string text, EventHandler handler)
        {
            Button button = new Button
            {
                Width = 74,
                Height = 32,
                Margin = new Padding(0, 4, 8, 0),
                Text = text,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.White,
                ForeColor = Color.Black,
                Font = new Font("Microsoft YaHei UI", 10F),
                UseVisualStyleBackColor = false
            };
            button.Click += handler;
            return button;
        }

        private static Button CreateActionButton(string text, EventHandler handler)
        {
            Button button = new Button
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(4),
                Text = text,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.White,
                ForeColor = Color.Black,
                Font = new Font("Microsoft YaHei UI", 10F),
                UseVisualStyleBackColor = false
            };
            button.Click += handler;
            return button;
        }

        private void ConfigureGrid()
        {
            grid.Dock = DockStyle.Fill;
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.MultiSelect = false;
            grid.ReadOnly = true;
            grid.RowHeadersVisible = false;
            grid.BackgroundColor = Color.White;
            grid.BorderStyle = BorderStyle.FixedSingle;
            grid.EnableHeadersVisualStyles = false;
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold);
            grid.ColumnHeadersHeight = 36;
            grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(230, 230, 230);
            grid.DefaultCellStyle.SelectionForeColor = Color.Black;
            grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 248, 248);
            grid.RowTemplate.Height = 32;
            grid.CellClick += grid_CellClick;
        }

        private void BuildInputs()
        {
            foreach (string field in module.Fields)
            {
                Control input = CreateInput(field);
                ModuleFieldDefinition? definition = module.GetField(field);
                if (definition?.ReadOnly == true)
                {
                    SetReadOnly(input);
                }

                Panel panel = new Panel
                {
                    Width = 320,
                    Height = IsLongTextField(field) ? 104 : 68,
                    Margin = new Padding(0, 0, 0, 10),
                    BackColor = Color.White
                };
                panel.Controls.Add(input);
                panel.Controls.Add(new Label
                {
                    Text = field,
                    Dock = DockStyle.Top,
                    Height = 24,
                    TextAlign = ContentAlignment.MiddleLeft
                });

                input.Dock = DockStyle.Top;
                fieldsPanel.Controls.Add(panel);
                inputs.Add(field, input);
            }
        }

        private static Control CreateInput(string field)
        {
            if (field.Contains("日期") || field.Contains("时间"))
            {
                return new DateTimePicker
                {
                    Height = 32,
                    Format = DateTimePickerFormat.Custom,
                    CustomFormat = "yyyy-MM-dd HH:mm"
                };
            }

            string[] options = GetOptions(field);
            if (options.Length > 0)
            {
                ComboBox comboBox = new ComboBox
                {
                    Height = 32,
                    DropDownStyle = ComboBoxStyle.DropDownList
                };
                comboBox.Items.AddRange(options);
                comboBox.SelectedIndex = 0;
                return comboBox;
            }

            return new TextBox
            {
                Height = IsLongTextField(field) ? 72 : 32,
                Multiline = IsLongTextField(field),
                ScrollBars = IsLongTextField(field) ? ScrollBars.Vertical : ScrollBars.None,
                BorderStyle = BorderStyle.FixedSingle
            };
        }

        private static string[] GetOptions(string field)
        {
            return field switch
            {
                "分类" => new[] { "计算机", "文学", "教育", "历史", "经济管理", "少儿读物", "艺术设计", "外语学习", "生活健康", "科技科普" },
                "销售状态" => new[] { "正常销售", "暂不上架", "已下架" },
                "入库状态" => new[] { "待入库", "部分入库", "已入库" },
                "出库类型" => new[] { "销售出库", "损坏出库", "盘点调整", "其他出库" },
                "会员等级" => new[] { "普通会员", "银卡会员", "金卡会员" },
                "订单状态" => new[] { "待支付", "待发货", "已发货", "已完成", "已取消", "已退货" },
                "支付方式" => new[] { "微信支付", "支付宝", "银行卡", "现金" },
                "支付状态" => new[] { "未支付", "已支付", "已退款", "已取消" },
                "发货状态" => new[] { "未发货", "待发货", "已发货", "配送中", "已签收" },
                "审核状态" => new[] { "待审核", "已通过", "已完成", "已拒绝" },
                "类型" => new[] { "作者", "出版社" },
                _ => Array.Empty<string>()
            };
        }

        private static bool IsLongTextField(string field)
        {
            return field.Contains("简介") || field.Contains("说明") ||
                field.Contains("原因") || field.Contains("地址") || field.Contains("备注");
        }

        private DataTable LoadTable()
        {
            if (!string.IsNullOrWhiteSpace(module.QuerySql))
            {
                try
                {
                    return DbHelper.Query(module.QuerySql);
                }
                catch
                {
                }
            }

            DataTable fallback = new DataTable();
            foreach (string field in module.Fields)
            {
                fallback.Columns.Add(field);
            }
            return fallback;
        }

        private void btnAdd_Click(object? sender, EventArgs e)
        {
            if (module.CanWrite)
            {
                ExecuteInsert();
                return;
            }

            DataRow row = table.NewRow();
            FillRow(row);
            table.Rows.Add(row);
            ClearInputs();
        }

        private void btnUpdate_Click(object? sender, EventArgs e)
        {
            DataRowView? view = grid.CurrentRow?.DataBoundItem as DataRowView;
            if (view == null)
            {
                MessageBox.Show("请先选择一条记录");
                return;
            }

            if (module.CanWrite)
            {
                ExecuteUpdate(view);
                return;
            }

            FillRow(view.Row);
            ClearInputs();
        }

        private void btnDelete_Click(object? sender, EventArgs e)
        {
            DataRowView? view = grid.CurrentRow?.DataBoundItem as DataRowView;
            if (view == null)
            {
                MessageBox.Show("请先选择一条记录");
                return;
            }

            if (module.CanWrite)
            {
                ExecuteDelete(view);
                return;
            }

            view.Row.Delete();
            ClearInputs();
        }

        private void btnClear_Click(object? sender, EventArgs e)
        {
            ClearInputs();
        }

        private void btnSearch_Click(object? sender, EventArgs e)
        {
            ApplySearchFilter();
        }

        private void btnReset_Click(object? sender, EventArgs e)
        {
            searchBox.Text = string.Empty;
            ApplySearchFilter();
        }

        private void searchBox_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ApplySearchFilter();
                e.SuppressKeyPress = true;
            }
        }

        private void grid_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            DataRowView? view = grid.CurrentRow?.DataBoundItem as DataRowView;
            if (e.RowIndex < 0 || view == null)
            {
                return;
            }

            foreach (string field in module.Fields)
            {
                if (!inputs.ContainsKey(field))
                {
                    continue;
                }

                SetInputValue(inputs[field], view.Row[field]?.ToString() ?? string.Empty);
            }
        }

        private void ExecuteInsert()
        {
            ModuleFieldDefinition[] fields = GetWritableFields();
            if (fields.Length == 0 || string.IsNullOrWhiteSpace(module.TableName))
            {
                return;
            }

            string columns = string.Join(", ", fields.Select(field => $"[{field.ColumnName}]"));
            string parameters = string.Join(", ", fields.Select((_, index) => $"@p{index}"));
            string sql = $"INSERT INTO [{module.TableName}] ({columns}) VALUES ({parameters})";
            ExecuteWrite(sql, fields.Select((field, index) =>
                new SqlParameter($"@p{index}", GetDbValue(inputs[field.DisplayName]))).ToArray());
        }

        private void ExecuteUpdate(DataRowView view)
        {
            object? keyValue = GetKeyValue(view);
            ModuleFieldDefinition[] fields = GetWritableFields();
            if (keyValue == null || fields.Length == 0 || string.IsNullOrWhiteSpace(module.TableName))
            {
                return;
            }

            string setClause = string.Join(", ", fields.Select((field, index) => $"[{field.ColumnName}]=@p{index}"));
            string sql = $"UPDATE [{module.TableName}] SET {setClause} WHERE [{module.KeyColumn}]=@id";
            List<SqlParameter> parameters = fields.Select((field, index) =>
                new SqlParameter($"@p{index}", GetDbValue(inputs[field.DisplayName]))).ToList();
            parameters.Add(new SqlParameter("@id", keyValue));
            ExecuteWrite(sql, parameters.ToArray());
        }

        private void ExecuteDelete(DataRowView view)
        {
            object? keyValue = GetKeyValue(view);
            if (keyValue == null || string.IsNullOrWhiteSpace(module.TableName))
            {
                return;
            }

            ExecuteWrite($"DELETE FROM [{module.TableName}] WHERE [{module.KeyColumn}]=@id",
                new SqlParameter("@id", keyValue));
        }

        private void ExecuteWrite(string sql, params SqlParameter[] parameters)
        {
            try
            {
                DbHelper.Execute(sql, parameters);
                Reload();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存失败：" + ex.Message);
            }
        }

        private ModuleFieldDefinition[] GetWritableFields()
        {
            return module.FieldDefinitions
                .Where(field => !field.ReadOnly && inputs.ContainsKey(field.DisplayName))
                .ToArray();
        }

        private object? GetKeyValue(DataRowView view)
        {
            if (view.Row.Table.Columns.Contains(module.KeyColumn))
            {
                return view.Row[module.KeyColumn];
            }

            return view.Row.Table.Columns.Contains("Id") ? view.Row["Id"] : null;
        }

        private void FillRow(DataRow row)
        {
            foreach (string field in module.Fields)
            {
                row[field] = GetInputValue(inputs[field]);
            }
        }

        private static string GetInputValue(Control input)
        {
            if (input is DateTimePicker dateTimePicker)
            {
                return dateTimePicker.Value.ToString("yyyy-MM-dd HH:mm");
            }

            return input.Text.Trim();
        }

        private static object GetDbValue(Control input)
        {
            string value = GetInputValue(input);
            return string.IsNullOrWhiteSpace(value) ? DBNull.Value : value;
        }

        private static void SetInputValue(Control input, string value)
        {
            if (input is DateTimePicker dateTimePicker)
            {
                if (DateTime.TryParse(value, out DateTime dateTime))
                {
                    dateTimePicker.Value = dateTime;
                }
                return;
            }

            if (input is ComboBox comboBox)
            {
                int index = comboBox.FindStringExact(value);
                comboBox.SelectedIndex = index >= 0 ? index : 0;
                return;
            }

            input.Text = value;
        }

        private static void SetReadOnly(Control input)
        {
            if (input is TextBox textBox)
            {
                textBox.ReadOnly = true;
            }
            else
            {
                input.Enabled = false;
            }
        }

        private void HideSystemColumns()
        {
            if (grid.Columns.Contains(module.KeyColumn))
            {
                grid.Columns[module.KeyColumn].Visible = false;
            }

            if (module.KeyColumn != "Id" && grid.Columns.Contains("Id"))
            {
                grid.Columns["Id"].Visible = false;
            }
        }

        private void ApplySearchFilter()
        {
            if (table.Columns.Count == 0)
            {
                return;
            }

            string keyword = EscapeFilterValue(searchBox.Text.Trim());
            if (string.IsNullOrWhiteSpace(keyword))
            {
                table.DefaultView.RowFilter = string.Empty;
                return;
            }

            string[] filters = table.Columns
                .Cast<DataColumn>()
                .Where(column => !string.Equals(column.ColumnName, module.KeyColumn, StringComparison.OrdinalIgnoreCase)
                    && !string.Equals(column.ColumnName, "Id", StringComparison.OrdinalIgnoreCase))
                .Select(column => $"CONVERT([{column.ColumnName}], 'System.String') LIKE '%{keyword}%'")
                .ToArray();
            table.DefaultView.RowFilter = string.Join(" OR ", filters);
        }

        private static string EscapeFilterValue(string value)
        {
            return value.Replace("'", "''").Replace("[", "[[]").Replace("%", "[%]").Replace("*", "[*]");
        }

        private void ClearInputs()
        {
            foreach (Control input in inputs.Values)
            {
                if (input is DateTimePicker dateTimePicker)
                {
                    dateTimePicker.Value = DateTime.Now;
                }
                else if (input is ComboBox comboBox)
                {
                    comboBox.SelectedIndex = comboBox.Items.Count > 0 ? 0 : -1;
                }
                else
                {
                    input.Text = string.Empty;
                }
            }
        }
    }
}
