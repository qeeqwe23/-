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
            lblTitle = new Label();
            btnBooks = new Button();
            btnSale = new Button();
            btnSaleQuery = new Button();
            btnExit = new Button();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Microsoft YaHei UI", 20F, FontStyle.Bold);
            lblTitle.Location = new Point(231, 70);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(326, 45);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "图书销售管理系统";
            // 
            // btnBooks
            // 
            btnBooks.Location = new Point(260, 165);
            btnBooks.Name = "btnBooks";
            btnBooks.Size = new Size(260, 42);
            btnBooks.TabIndex = 1;
            btnBooks.Text = "图书管理";
            btnBooks.UseVisualStyleBackColor = true;
            btnBooks.Click += btnBooks_Click;
            // 
            // btnSale
            // 
            btnSale.Location = new Point(260, 225);
            btnSale.Name = "btnSale";
            btnSale.Size = new Size(260, 42);
            btnSale.TabIndex = 2;
            btnSale.Text = "销售出库";
            btnSale.UseVisualStyleBackColor = true;
            btnSale.Click += btnSale_Click;
            // 
            // btnSaleQuery
            // 
            btnSaleQuery.Location = new Point(260, 285);
            btnSaleQuery.Name = "btnSaleQuery";
            btnSaleQuery.Size = new Size(260, 42);
            btnSaleQuery.TabIndex = 3;
            btnSaleQuery.Text = "销售查询";
            btnSaleQuery.UseVisualStyleBackColor = true;
            btnSaleQuery.Click += btnSaleQuery_Click;
            // 
            // btnExit
            // 
            btnExit.Location = new Point(260, 345);
            btnExit.Name = "btnExit";
            btnExit.Size = new Size(260, 42);
            btnExit.TabIndex = 4;
            btnExit.Text = "退出系统";
            btnExit.UseVisualStyleBackColor = true;
            btnExit.Click += btnExit_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnExit);
            Controls.Add(btnSaleQuery);
            Controls.Add(btnSale);
            Controls.Add(btnBooks);
            Controls.Add(lblTitle);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "主界面";
            ResumeLayout(false);
            PerformLayout();
        }

        private Label lblTitle;
        private Button btnBooks;
        private Button btnSale;
        private Button btnSaleQuery;
        private Button btnExit;
    }
}
