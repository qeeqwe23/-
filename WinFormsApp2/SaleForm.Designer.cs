namespace WinFormsApp2
{
    partial class SaleForm
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
            lblBook = new Label();
            cmbBook = new ComboBox();
            lblQuantity = new Label();
            txtQuantity = new TextBox();
            lblPriceTitle = new Label();
            lblPrice = new Label();
            lblStockTitle = new Label();
            lblStock = new Label();
            btnSale = new Button();
            btnRefresh = new Button();
            SuspendLayout();
            // 
            // lblBook
            // 
            lblBook.AutoSize = true;
            lblBook.Location = new Point(210, 92);
            lblBook.Name = "lblBook";
            lblBook.Size = new Size(39, 20);
            lblBook.Text = "图书";
            // 
            // cmbBook
            // 
            cmbBook.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbBook.FormattingEnabled = true;
            cmbBook.Location = new Point(280, 89);
            cmbBook.Name = "cmbBook";
            cmbBook.Size = new Size(260, 28);
            cmbBook.TabIndex = 0;
            cmbBook.SelectedIndexChanged += cmbBook_SelectedIndexChanged;
            // 
            // lblQuantity
            // 
            lblQuantity.AutoSize = true;
            lblQuantity.Location = new Point(210, 145);
            lblQuantity.Name = "lblQuantity";
            lblQuantity.Size = new Size(39, 20);
            lblQuantity.Text = "数量";
            // 
            // txtQuantity
            // 
            txtQuantity.Location = new Point(280, 142);
            txtQuantity.Name = "txtQuantity";
            txtQuantity.Size = new Size(260, 27);
            txtQuantity.TabIndex = 1;
            // 
            // lblPriceTitle
            // 
            lblPriceTitle.AutoSize = true;
            lblPriceTitle.Location = new Point(210, 198);
            lblPriceTitle.Name = "lblPriceTitle";
            lblPriceTitle.Size = new Size(39, 20);
            lblPriceTitle.Text = "单价";
            // 
            // lblPrice
            // 
            lblPrice.AutoSize = true;
            lblPrice.Location = new Point(280, 198);
            lblPrice.Name = "lblPrice";
            lblPrice.Size = new Size(18, 20);
            lblPrice.Text = "0";
            // 
            // lblStockTitle
            // 
            lblStockTitle.AutoSize = true;
            lblStockTitle.Location = new Point(210, 245);
            lblStockTitle.Name = "lblStockTitle";
            lblStockTitle.Size = new Size(39, 20);
            lblStockTitle.Text = "库存";
            // 
            // lblStock
            // 
            lblStock.AutoSize = true;
            lblStock.Location = new Point(280, 245);
            lblStock.Name = "lblStock";
            lblStock.Size = new Size(18, 20);
            lblStock.Text = "0";
            // 
            // btnSale
            // 
            btnSale.Location = new Point(280, 305);
            btnSale.Name = "btnSale";
            btnSale.Size = new Size(120, 36);
            btnSale.TabIndex = 2;
            btnSale.Text = "确认销售";
            btnSale.UseVisualStyleBackColor = true;
            btnSale.Click += btnSale_Click;
            // 
            // btnRefresh
            // 
            btnRefresh.Location = new Point(420, 305);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(120, 36);
            btnRefresh.TabIndex = 3;
            btnRefresh.Text = "刷新图书";
            btnRefresh.UseVisualStyleBackColor = true;
            btnRefresh.Click += btnRefresh_Click;
            // 
            // SaleForm
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(760, 430);
            Controls.Add(btnRefresh);
            Controls.Add(btnSale);
            Controls.Add(lblStock);
            Controls.Add(lblStockTitle);
            Controls.Add(lblPrice);
            Controls.Add(lblPriceTitle);
            Controls.Add(txtQuantity);
            Controls.Add(lblQuantity);
            Controls.Add(cmbBook);
            Controls.Add(lblBook);
            Name = "SaleForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "销售出库";
            Load += SaleForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        private Label lblBook;
        private ComboBox cmbBook;
        private Label lblQuantity;
        private TextBox txtQuantity;
        private Label lblPriceTitle;
        private Label lblPrice;
        private Label lblStockTitle;
        private Label lblStock;
        private Button btnSale;
        private Button btnRefresh;
    }
}
