using System.Data.SqlClient;

namespace WinFormsApp2
{
    public partial class BookForm : Form
    {
        private int selectedId;

        public BookForm()
        {
            InitializeComponent();
        }

        private void BookForm_Load(object sender, EventArgs e)
        {
            LoadBooks();
        }

        private void LoadBooks()
        {
            dgvBooks.DataSource = DbHelper.Query("SELECT Id, BookName, Author, Publisher, Price, Stock, CreatedAt FROM Books ORDER BY Id DESC");
        }

        private bool ValidateInput(out decimal price, out int stock)
        {
            price = 0;
            stock = 0;

            if (string.IsNullOrWhiteSpace(txtBookName.Text) || string.IsNullOrWhiteSpace(txtAuthor.Text))
            {
                MessageBox.Show("书名和作者不能为空");
                return false;
            }

            if (!decimal.TryParse(txtPrice.Text.Trim(), out price) || price < 0)
            {
                MessageBox.Show("价格必须是大于等于 0 的数字");
                return false;
            }

            if (!int.TryParse(txtStock.Text.Trim(), out stock) || stock < 0)
            {
                MessageBox.Show("库存必须是大于等于 0 的整数");
                return false;
            }

            return true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateInput(out decimal price, out int stock))
            {
                return;
            }

            DbHelper.Execute(
                "INSERT INTO Books(BookName, Author, Publisher, Price, Stock) VALUES(@BookName, @Author, @Publisher, @Price, @Stock)",
                new SqlParameter("@BookName", txtBookName.Text.Trim()),
                new SqlParameter("@Author", txtAuthor.Text.Trim()),
                new SqlParameter("@Publisher", txtPublisher.Text.Trim()),
                new SqlParameter("@Price", price),
                new SqlParameter("@Stock", stock));

            MessageBox.Show("新增成功");
            ClearInput();
            LoadBooks();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedId == 0)
            {
                MessageBox.Show("请先选择一本图书");
                return;
            }

            if (!ValidateInput(out decimal price, out int stock))
            {
                return;
            }

            DbHelper.Execute(
                "UPDATE Books SET BookName=@BookName, Author=@Author, Publisher=@Publisher, Price=@Price, Stock=@Stock WHERE Id=@Id",
                new SqlParameter("@BookName", txtBookName.Text.Trim()),
                new SqlParameter("@Author", txtAuthor.Text.Trim()),
                new SqlParameter("@Publisher", txtPublisher.Text.Trim()),
                new SqlParameter("@Price", price),
                new SqlParameter("@Stock", stock),
                new SqlParameter("@Id", selectedId));

            MessageBox.Show("修改成功");
            ClearInput();
            LoadBooks();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedId == 0)
            {
                MessageBox.Show("请先选择一本图书");
                return;
            }

            DialogResult result = MessageBox.Show("确定删除选中的图书吗？", "提示", MessageBoxButtons.YesNo);
            if (result != DialogResult.Yes)
            {
                return;
            }

            try
            {
                DbHelper.Execute("DELETE FROM Books WHERE Id=@Id", new SqlParameter("@Id", selectedId));
                MessageBox.Show("删除成功");
                ClearInput();
                LoadBooks();
            }
            catch (SqlException)
            {
                MessageBox.Show("这本书已有销售记录，不能直接删除");
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = "%" + txtBookName.Text.Trim() + "%";
            dgvBooks.DataSource = DbHelper.Query(
                "SELECT Id, BookName, Author, Publisher, Price, Stock, CreatedAt FROM Books WHERE BookName LIKE @Keyword OR Author LIKE @Keyword ORDER BY Id DESC",
                new SqlParameter("@Keyword", keyword));
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearInput();
            LoadBooks();
        }

        private void dgvBooks_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            DataGridViewRow row = dgvBooks.Rows[e.RowIndex];
            selectedId = Convert.ToInt32(row.Cells["Id"].Value);
            txtBookName.Text = row.Cells["BookName"].Value?.ToString();
            txtAuthor.Text = row.Cells["Author"].Value?.ToString();
            txtPublisher.Text = row.Cells["Publisher"].Value?.ToString();
            txtPrice.Text = row.Cells["Price"].Value?.ToString();
            txtStock.Text = row.Cells["Stock"].Value?.ToString();
        }

        private void ClearInput()
        {
            selectedId = 0;
            txtBookName.Clear();
            txtAuthor.Clear();
            txtPublisher.Clear();
            txtPrice.Clear();
            txtStock.Clear();
        }
    }
}
