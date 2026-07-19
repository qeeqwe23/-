namespace WinFormsApp2
{
    partial class BookForm
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
            dgvBooks = new DataGridView();
            lblBookName = new Label();
            lblAuthor = new Label();
            lblPublisher = new Label();
            lblPrice = new Label();
            lblStock = new Label();
            txtBookName = new TextBox();
            txtAuthor = new TextBox();
            txtPublisher = new TextBox();
            txtPrice = new TextBox();
            txtStock = new TextBox();
            btnAdd = new Button();
            btnUpdate = new Button();
            btnDelete = new Button();
            btnSearch = new Button();
            btnClear = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvBooks).BeginInit();
            SuspendLayout();
            // 
            // dgvBooks
            // 
            dgvBooks.AllowUserToAddRows = false;
            dgvBooks.AllowUserToDeleteRows = false;
            dgvBooks.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvBooks.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvBooks.Location = new Point(20, 175);
            dgvBooks.MultiSelect = false;
            dgvBooks.Name = "dgvBooks";
            dgvBooks.ReadOnly = true;
            dgvBooks.RowHeadersWidth = 51;
            dgvBooks.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvBooks.Size = new Size(860, 320);
            dgvBooks.TabIndex = 14;
            dgvBooks.CellClick += dgvBooks_CellClick;
            // 
            // labels and inputs
            // 
            lblBookName.AutoSize = true;
            lblBookName.Location = new Point(28, 28);
            lblBookName.Name = "lblBookName";
            lblBookName.Size = new Size(39, 20);
            lblBookName.Text = "书名";
            txtBookName.Location = new Point(85, 25);
            txtBookName.Name = "txtBookName";
            txtBookName.Size = new Size(180, 27);
            txtBookName.TabIndex = 0;
            lblAuthor.AutoSize = true;
            lblAuthor.Location = new Point(294, 28);
            lblAuthor.Name = "lblAuthor";
            lblAuthor.Size = new Size(39, 20);
            lblAuthor.Text = "作者";
            txtAuthor.Location = new Point(351, 25);
            txtAuthor.Name = "txtAuthor";
            txtAuthor.Size = new Size(150, 27);
            txtAuthor.TabIndex = 1;
            lblPublisher.AutoSize = true;
            lblPublisher.Location = new Point(530, 28);
            lblPublisher.Name = "lblPublisher";
            lblPublisher.Size = new Size(54, 20);
            lblPublisher.Text = "出版社";
            txtPublisher.Location = new Point(602, 25);
            txtPublisher.Name = "txtPublisher";
            txtPublisher.Size = new Size(180, 27);
            txtPublisher.TabIndex = 2;
            lblPrice.AutoSize = true;
            lblPrice.Location = new Point(28, 75);
            lblPrice.Name = "lblPrice";
            lblPrice.Size = new Size(39, 20);
            lblPrice.Text = "价格";
            txtPrice.Location = new Point(85, 72);
            txtPrice.Name = "txtPrice";
            txtPrice.Size = new Size(180, 27);
            txtPrice.TabIndex = 3;
            lblStock.AutoSize = true;
            lblStock.Location = new Point(294, 75);
            lblStock.Name = "lblStock";
            lblStock.Size = new Size(39, 20);
            lblStock.Text = "库存";
            txtStock.Location = new Point(351, 72);
            txtStock.Name = "txtStock";
            txtStock.Size = new Size(150, 27);
            txtStock.TabIndex = 4;
            // 
            // buttons
            // 
            btnAdd.Location = new Point(85, 122);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(95, 32);
            btnAdd.TabIndex = 5;
            btnAdd.Text = "新增";
            btnAdd.Click += btnAdd_Click;
            btnUpdate.Location = new Point(200, 122);
            btnUpdate.Name = "btnUpdate";
            btnUpdate.Size = new Size(95, 32);
            btnUpdate.TabIndex = 6;
            btnUpdate.Text = "修改";
            btnUpdate.Click += btnUpdate_Click;
            btnDelete.Location = new Point(315, 122);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(95, 32);
            btnDelete.TabIndex = 7;
            btnDelete.Text = "删除";
            btnDelete.Click += btnDelete_Click;
            btnSearch.Location = new Point(430, 122);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(95, 32);
            btnSearch.TabIndex = 8;
            btnSearch.Text = "查询";
            btnSearch.Click += btnSearch_Click;
            btnClear.Location = new Point(545, 122);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(95, 32);
            btnClear.TabIndex = 9;
            btnClear.Text = "清空";
            btnClear.Click += btnClear_Click;
            // 
            // BookForm
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(900, 520);
            Controls.Add(btnClear);
            Controls.Add(btnSearch);
            Controls.Add(btnDelete);
            Controls.Add(btnUpdate);
            Controls.Add(btnAdd);
            Controls.Add(txtStock);
            Controls.Add(txtPrice);
            Controls.Add(txtPublisher);
            Controls.Add(txtAuthor);
            Controls.Add(txtBookName);
            Controls.Add(lblStock);
            Controls.Add(lblPrice);
            Controls.Add(lblPublisher);
            Controls.Add(lblAuthor);
            Controls.Add(lblBookName);
            Controls.Add(dgvBooks);
            Name = "BookForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "图书管理";
            Load += BookForm_Load;
            ((System.ComponentModel.ISupportInitialize)dgvBooks).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private DataGridView dgvBooks;
        private Label lblBookName;
        private Label lblAuthor;
        private Label lblPublisher;
        private Label lblPrice;
        private Label lblStock;
        private TextBox txtBookName;
        private TextBox txtAuthor;
        private TextBox txtPublisher;
        private TextBox txtPrice;
        private TextBox txtStock;
        private Button btnAdd;
        private Button btnUpdate;
        private Button btnDelete;
        private Button btnSearch;
        private Button btnClear;
    }
}
