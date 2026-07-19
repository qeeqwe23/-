namespace WinFormsApp2
{
    partial class SaleQueryForm
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
            dgvSales = new DataGridView();
            btnRefresh = new Button();
            lblTotalTitle = new Label();
            lblTotal = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvSales).BeginInit();
            SuspendLayout();
            // 
            // dgvSales
            // 
            dgvSales.AllowUserToAddRows = false;
            dgvSales.AllowUserToDeleteRows = false;
            dgvSales.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvSales.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvSales.Location = new Point(20, 70);
            dgvSales.Name = "dgvSales";
            dgvSales.ReadOnly = true;
            dgvSales.RowHeadersWidth = 51;
            dgvSales.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSales.Size = new Size(840, 390);
            dgvSales.TabIndex = 0;
            // 
            // btnRefresh
            // 
            btnRefresh.Location = new Point(20, 22);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(110, 32);
            btnRefresh.TabIndex = 1;
            btnRefresh.Text = "刷新";
            btnRefresh.UseVisualStyleBackColor = true;
            btnRefresh.Click += btnRefresh_Click;
            // 
            // lblTotalTitle
            // 
            lblTotalTitle.AutoSize = true;
            lblTotalTitle.Location = new Point(170, 28);
            lblTotalTitle.Name = "lblTotalTitle";
            lblTotalTitle.Size = new Size(69, 20);
            lblTotalTitle.Text = "销售总额";
            // 
            // lblTotal
            // 
            lblTotal.AutoSize = true;
            lblTotal.Location = new Point(250, 28);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new Size(18, 20);
            lblTotal.Text = "0";
            // 
            // SaleQueryForm
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(880, 485);
            Controls.Add(lblTotal);
            Controls.Add(lblTotalTitle);
            Controls.Add(btnRefresh);
            Controls.Add(dgvSales);
            Name = "SaleQueryForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "销售查询";
            Load += SaleQueryForm_Load;
            ((System.ComponentModel.ISupportInitialize)dgvSales).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private DataGridView dgvSales;
        private Button btnRefresh;
        private Label lblTotalTitle;
        private Label lblTotal;
    }
}
