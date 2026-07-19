namespace WinFormsApp2
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnBooks_Click(object sender, EventArgs e)
        {
            new BookForm().ShowDialog();
        }

        private void btnSale_Click(object sender, EventArgs e)
        {
            new SaleForm().ShowDialog();
        }

        private void btnSaleQuery_Click(object sender, EventArgs e)
        {
            new SaleQueryForm().ShowDialog();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
