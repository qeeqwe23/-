namespace WinFormsApp2
{
    partial class LoginForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            bindingSource1 = new BindingSource(components);
            shell = new TableLayoutPanel();
            brandPanel = new Panel();
            lblBrandTitle = new Label();
            lblBrandSubtitle = new Label();
            loginPanel = new Panel();
            formLayout = new TableLayoutPanel();
            lblLoginTitle = new Label();
            lblUserName = new Label();
            txtUserName = new TextBox();
            lblPassword = new Label();
            txtPassword = new TextBox();
            btnLogin = new Button();
            ((System.ComponentModel.ISupportInitialize)bindingSource1).BeginInit();
            shell.SuspendLayout();
            brandPanel.SuspendLayout();
            loginPanel.SuspendLayout();
            formLayout.SuspendLayout();
            SuspendLayout();
            // 
            // shell
            // 
            shell.BackColor = UiTheme.Page;
            shell.ColumnCount = 2;
            shell.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 48F));
            shell.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 52F));
            shell.Controls.Add(brandPanel, 0, 0);
            shell.Controls.Add(loginPanel, 1, 0);
            shell.Dock = DockStyle.Fill;
            shell.Location = new Point(0, 0);
            shell.Name = "shell";
            shell.RowCount = 1;
            shell.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            shell.Size = new Size(900, 520);
            shell.TabIndex = 0;
            // 
            // brandPanel
            // 
            brandPanel.BackColor = UiTheme.Accent;
            brandPanel.Controls.Add(lblBrandSubtitle);
            brandPanel.Controls.Add(lblBrandTitle);
            brandPanel.Dock = DockStyle.Fill;
            brandPanel.Location = new Point(0, 0);
            brandPanel.Margin = new Padding(0);
            brandPanel.Name = "brandPanel";
            brandPanel.Padding = new Padding(48);
            brandPanel.Size = new Size(432, 520);
            brandPanel.TabIndex = 0;
            // 
            // lblBrandTitle
            // 
            lblBrandTitle.Dock = DockStyle.Top;
            lblBrandTitle.Font = UiTheme.Bold(25F);
            lblBrandTitle.ForeColor = Color.White;
            lblBrandTitle.Location = new Point(48, 48);
            lblBrandTitle.Name = "lblBrandTitle";
            lblBrandTitle.Size = new Size(336, 130);
            lblBrandTitle.TabIndex = 0;
            lblBrandTitle.Text = "图书销售\r\n管理系统";
            lblBrandTitle.TextAlign = ContentAlignment.BottomLeft;
            // 
            // lblBrandSubtitle
            // 
            lblBrandSubtitle.Dock = DockStyle.Top;
            lblBrandSubtitle.Font = UiTheme.Regular(11F);
            lblBrandSubtitle.ForeColor = Color.FromArgb(225, 239, 255);
            lblBrandSubtitle.Location = new Point(48, 178);
            lblBrandSubtitle.Name = "lblBrandSubtitle";
            lblBrandSubtitle.Size = new Size(336, 92);
            lblBrandSubtitle.TabIndex = 1;
            lblBrandSubtitle.Text = "管理图书档案、库存流转、客户订单与经营统计";
            lblBrandSubtitle.TextAlign = ContentAlignment.TopLeft;
            // 
            // loginPanel
            // 
            loginPanel.BackColor = UiTheme.Page;
            loginPanel.Controls.Add(formLayout);
            loginPanel.Dock = DockStyle.Fill;
            loginPanel.Location = new Point(432, 0);
            loginPanel.Margin = new Padding(0);
            loginPanel.Name = "loginPanel";
            loginPanel.Padding = new Padding(72, 86, 72, 86);
            loginPanel.Size = new Size(468, 520);
            loginPanel.TabIndex = 1;
            // 
            // formLayout
            // 
            formLayout.BackColor = UiTheme.Surface;
            formLayout.ColumnCount = 1;
            formLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            formLayout.Controls.Add(lblLoginTitle, 0, 0);
            formLayout.Controls.Add(lblUserName, 0, 1);
            formLayout.Controls.Add(txtUserName, 0, 2);
            formLayout.Controls.Add(lblPassword, 0, 3);
            formLayout.Controls.Add(txtPassword, 0, 4);
            formLayout.Controls.Add(btnLogin, 0, 5);
            formLayout.Dock = DockStyle.Fill;
            formLayout.Location = new Point(72, 86);
            formLayout.Name = "formLayout";
            formLayout.Padding = new Padding(32, 28, 32, 28);
            formLayout.RowCount = 6;
            formLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 62F));
            formLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            formLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
            formLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            formLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
            formLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 58F));
            formLayout.Size = new Size(324, 348);
            formLayout.TabIndex = 0;
            // 
            // lblLoginTitle
            // 
            lblLoginTitle.Dock = DockStyle.Fill;
            lblLoginTitle.Font = UiTheme.Bold(18F);
            lblLoginTitle.ForeColor = UiTheme.Text;
            lblLoginTitle.Location = new Point(35, 28);
            lblLoginTitle.Name = "lblLoginTitle";
            lblLoginTitle.Size = new Size(254, 62);
            lblLoginTitle.TabIndex = 0;
            lblLoginTitle.Text = "系统登录";
            lblLoginTitle.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblUserName
            // 
            lblUserName.Dock = DockStyle.Fill;
            lblUserName.Font = UiTheme.Regular(10F);
            lblUserName.ForeColor = UiTheme.MutedText;
            lblUserName.Location = new Point(35, 90);
            lblUserName.Name = "lblUserName";
            lblUserName.Size = new Size(254, 30);
            lblUserName.TabIndex = 1;
            lblUserName.Text = "用户名";
            lblUserName.TextAlign = ContentAlignment.BottomLeft;
            // 
            // txtUserName
            // 
            txtUserName.Dock = DockStyle.Fill;
            txtUserName.Location = new Point(35, 123);
            txtUserName.Name = "txtUserName";
            txtUserName.Size = new Size(254, 31);
            txtUserName.TabIndex = 0;
            // 
            // lblPassword
            // 
            lblPassword.Dock = DockStyle.Fill;
            lblPassword.Font = UiTheme.Regular(10F);
            lblPassword.ForeColor = UiTheme.MutedText;
            lblPassword.Location = new Point(35, 164);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(254, 30);
            lblPassword.TabIndex = 3;
            lblPassword.Text = "密码";
            lblPassword.TextAlign = ContentAlignment.BottomLeft;
            // 
            // txtPassword
            // 
            txtPassword.Dock = DockStyle.Fill;
            txtPassword.Location = new Point(35, 197);
            txtPassword.Name = "txtPassword";
            txtPassword.PasswordChar = '*';
            txtPassword.Size = new Size(254, 31);
            txtPassword.TabIndex = 1;
            // 
            // btnLogin
            // 
            btnLogin.Dock = DockStyle.Bottom;
            btnLogin.Location = new Point(35, 266);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(254, 48);
            btnLogin.TabIndex = 2;
            btnLogin.Text = "登录";
            btnLogin.Click += btnLogin_Click;
            // 
            // LoginForm
            // 
            AcceptButton = btnLogin;
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = UiTheme.Page;
            ClientSize = new Size(900, 520);
            Controls.Add(shell);
            Font = UiTheme.Regular(10F);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "LoginForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "图书销售管理系统登录";
            Load += LoginForm_Load;
            ((System.ComponentModel.ISupportInitialize)bindingSource1).EndInit();
            shell.ResumeLayout(false);
            brandPanel.ResumeLayout(false);
            loginPanel.ResumeLayout(false);
            formLayout.ResumeLayout(false);
            formLayout.PerformLayout();
            ResumeLayout(false);
        }

        private BindingSource bindingSource1;
        private Button btnLogin;
        private TextBox txtUserName;
        private TextBox txtPassword;
        private Label lblUserName;
        private Label lblPassword;
        private TableLayoutPanel shell;
        private Panel brandPanel;
        private Label lblBrandTitle;
        private Label lblBrandSubtitle;
        private Panel loginPanel;
        private TableLayoutPanel formLayout;
        private Label lblLoginTitle;
    }
}
