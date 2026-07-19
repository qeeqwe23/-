using System.Data;
using System.Data.SqlClient;

namespace WinFormsApp2
{
    public partial class SaleForm : Form
    {
        public SaleForm()
        {
            InitializeComponent();
        }

        private void SaleForm_Load(object sender, EventArgs e)
        {
            LoadBooks();
            txtQuantity.Text = "1";
        }

        private void LoadBooks()
        {
            DataTable table = DbHelper.Query("SELECT Id, BookName, Price, Stock FROM Books WHERE Stock > 0 ORDER BY BookName");
            cmbBook.DataSource = table;
            cmbBook.DisplayMember = "BookName";
            cmbBook.ValueMember = "Id";
            ShowSelectedBookInfo();
        }

        private void cmbBook_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowSelectedBookInfo();
        }

        private void ShowSelectedBookInfo()
        {
            if (cmbBook.SelectedItem is not DataRowView row)
            {
                lblPrice.Text = "0";
                lblStock.Text = "0";
                return;
            }

            lblPrice.Text = Convert.ToDecimal(row["Price"]).ToString("0.00");
            lblStock.Text = row["Stock"].ToString();
        }

        private void btnSale_Click(object sender, EventArgs e)
        {
            if (cmbBook.SelectedValue == null)
            {
                MessageBox.Show("请先选择图书");
                return;
            }

            if (!int.TryParse(txtQuantity.Text.Trim(), out int quantity) || quantity <= 0)
            {
                MessageBox.Show("销售数量必须是大于 0 的整数");
                return;
            }

            int bookId = Convert.ToInt32(cmbBook.SelectedValue);

            DataTable bookTable = DbHelper.Query(
                "SELECT Price, Stock FROM Books WHERE Id=@Id",
                new SqlParameter("@Id", bookId));

            if (bookTable.Rows.Count == 0)
            {
                MessageBox.Show("图书不存在");
                return;
            }

            decimal price = Convert.ToDecimal(bookTable.Rows[0]["Price"]);
            int stock = Convert.ToInt32(bookTable.Rows[0]["Stock"]);

            if (quantity > stock)
            {
                MessageBox.Show("库存不足");
                return;
            }

            string sql = @"
INSERT INTO Sales(BookId, Quantity, UnitPrice)
VALUES(@BookId, @Quantity, @UnitPrice);

UPDATE Books
SET Stock = Stock - @Quantity
WHERE Id=@BookId;";

            DbHelper.Execute(sql,
                new SqlParameter("@BookId", bookId),
                new SqlParameter("@Quantity", quantity),
                new SqlParameter("@UnitPrice", price));

            MessageBox.Show("销售成功");
            LoadBooks();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadBooks();
        }
    }
}
