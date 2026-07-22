namespace WinFormsApp2
{
    partial class MainForm
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
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = UiTheme.Page;
            ClientSize = new Size(1360, 820);
            Font = UiTheme.Regular(10F);
            MinimumSize = new Size(1180, 760);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "图书销售管理系统";
            Load += MainForm_Load;
        }
    }
}
