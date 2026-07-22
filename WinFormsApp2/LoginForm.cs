using System.Data.SqlClient;

namespace WinFormsApp2
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            UiTheme.StyleInput(txtUserName);
            UiTheme.StyleInput(txtPassword);
            UiTheme.StylePrimaryButton(btnLogin);
            txtUserName.Focus();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string userName = txtUserName.Text.Trim();
            string password = txtPassword.Text.Trim();

            string sql = "SELECT COUNT(*) FROM Users WHERE UserName=@UserName AND Password=@Password";
            int count = Convert.ToInt32(DbHelper.Query(sql,
                new SqlParameter("@UserName", userName),
                new SqlParameter("@Password", password)).Rows[0][0]);

            if (count > 0)
            {
                MainForm mainForm = new MainForm();
                mainForm.Show();
                Hide();
            }
            else
            {
                MessageBox.Show("用户名或密码错误");
            }
        }
    }
}
