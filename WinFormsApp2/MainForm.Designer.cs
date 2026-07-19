namespace WinFormsApp2
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            shell = new TableLayoutPanel();
            headerPanel = new Panel();
            btnExit = new Button();
            lblTitle = new Label();
            lblSubtitle = new Label();
            entryLayout = new TableLayoutPanel();
            shell.SuspendLayout();
            headerPanel.SuspendLayout();
            SuspendLayout();
            // 
            // shell
            // 
            shell.BackColor = Color.White;
            shell.ColumnCount = 1;
            shell.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            shell.Controls.Add(headerPanel, 0, 0);
            shell.Controls.Add(entryLayout, 0, 1);
            shell.Dock = DockStyle.Fill;
            shell.Location = new Point(0, 0);
            shell.Name = "shell";
            shell.RowCount = 2;
            shell.RowStyles.Add(new RowStyle(SizeType.Absolute, 150F));
            shell.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            shell.Size = new Size(1180, 720);
            shell.TabIndex = 0;
            // 
            // headerPanel
            // 
            headerPanel.BackColor = Color.White;
            headerPanel.Controls.Add(btnExit);
            headerPanel.Controls.Add(lblTitle);
            headerPanel.Controls.Add(lblSubtitle);
            headerPanel.Dock = DockStyle.Fill;
            headerPanel.Location = new Point(0, 0);
            headerPanel.Margin = new Padding(0);
            headerPanel.Name = "headerPanel";
            headerPanel.Padding = new Padding(56, 34, 56, 18);
            headerPanel.Size = new Size(1180, 150);
            headerPanel.TabIndex = 0;
            // 
            // btnExit
            // 
            btnExit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnExit.BackColor = Color.White;
            btnExit.FlatStyle = FlatStyle.Flat;
            btnExit.Font = new Font("Microsoft YaHei UI", 10F);
            btnExit.Location = new Point(1046, 48);
            btnExit.Name = "btnExit";
            btnExit.Size = new Size(78, 34);
            btnExit.TabIndex = 2;
            btnExit.Text = "退出";
            btnExit.UseVisualStyleBackColor = false;
            btnExit.Click += btnExit_Click;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Microsoft YaHei UI", 24F, FontStyle.Bold);
            lblTitle.Location = new Point(56, 34);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(424, 52);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "图书销售管理系统";
            // 
            // lblSubtitle
            // 
            lblSubtitle.AutoSize = true;
            lblSubtitle.Font = new Font("Microsoft YaHei UI", 10.5F);
            lblSubtitle.ForeColor = Color.FromArgb(80, 80, 80);
            lblSubtitle.Location = new Point(60, 94);
            lblSubtitle.Name = "lblSubtitle";
            lblSubtitle.Size = new Size(408, 24);
            lblSubtitle.TabIndex = 1;
            lblSubtitle.Text = "把资料、库存、客户、订单和统计放回业务流程里";
            // 
            // entryLayout
            // 
            entryLayout.BackColor = Color.White;
            entryLayout.ColumnCount = 2;
            entryLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            entryLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            entryLayout.Dock = DockStyle.Fill;
            entryLayout.Location = new Point(40, 150);
            entryLayout.Margin = new Padding(40, 0, 40, 40);
            entryLayout.Name = "entryLayout";
            entryLayout.RowCount = 2;
            entryLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            entryLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            entryLayout.Size = new Size(1100, 530);
            entryLayout.TabIndex = 1;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1180, 720);
            Controls.Add(shell);
            Font = new Font("Microsoft YaHei UI", 10F);
            MinimumSize = new Size(1040, 680);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "图书销售管理系统";
            Load += MainForm_Load;
            shell.ResumeLayout(false);
            headerPanel.ResumeLayout(false);
            headerPanel.PerformLayout();
            ResumeLayout(false);
        }

        private TableLayoutPanel shell;
        private Panel headerPanel;
        private Button btnExit;
        private Label lblTitle;
        private Label lblSubtitle;
        private TableLayoutPanel entryLayout;
    }
}
