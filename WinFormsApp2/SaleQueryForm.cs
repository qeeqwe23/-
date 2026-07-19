using System.Data;

namespace WinFormsApp2
{
    public partial class SaleQueryForm : Form
    {
        public SaleQueryForm()
        {
            InitializeComponent();
        }

        private void SaleQueryForm_Load(object sender, EventArgs e)
        {
            LoadSales();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadSales();
        }

        private void LoadSales()
        {
            string sql = @"
SELECT s.Id, b.BookName, s.Quantity, s.UnitPrice, s.TotalAmount, s.SaleTime
FROM Sales s
INNER JOIN Books b ON s.BookId = b.Id
ORDER BY s.SaleTime DESC";

            DataTable table = DbHelper.Query(sql);
            dgvSales.DataSource = table;

            decimal total = 0;
            foreach (DataRow row in table.Rows)
            {
                total += Convert.ToDecimal(row["TotalAmount"]);
            }

            lblTotal.Text = total.ToString("0.00");
        }
    }
}
