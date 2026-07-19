namespace WinFormsApp2
{
    internal sealed class ManagementWorkspaceForm : Form
    {
        private readonly ModuleGroupDefinition group;
        private readonly TabControl tabControl = new TabControl();

        public ManagementWorkspaceForm(ModuleGroupDefinition group)
        {
            this.group = group;
            InitializeComponent();
            BuildTabs();
        }

        private void InitializeComponent()
        {
            Text = group.Title;
            StartPosition = FormStartPosition.CenterParent;
            MinimumSize = new Size(1120, 720);
            Size = new Size(1240, 760);
            Font = new Font("Microsoft YaHei UI", 10F);
            BackColor = Color.White;

            Panel header = new Panel
            {
                Dock = DockStyle.Top,
                Height = 96,
                BackColor = Color.White,
                Padding = new Padding(28, 16, 28, 14)
            };

            Label title = new Label
            {
                Dock = DockStyle.Top,
                Height = 42,
                Text = group.Title,
                Font = new Font("Microsoft YaHei UI", 20F, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft
            };

            Label summary = new Label
            {
                Dock = DockStyle.Fill,
                Text = group.Summary,
                ForeColor = Color.FromArgb(80, 80, 80),
                TextAlign = ContentAlignment.MiddleLeft
            };

            header.Controls.Add(summary);
            header.Controls.Add(title);

            tabControl.Dock = DockStyle.Fill;
            tabControl.Padding = new Point(18, 8);
            tabControl.Font = new Font("Microsoft YaHei UI", 10F);
            tabControl.SelectedIndexChanged += tabControl_SelectedIndexChanged;

            Controls.Add(tabControl);
            Controls.Add(header);
        }

        private void BuildTabs()
        {
            tabControl.TabPages.Clear();

            foreach (ModuleDefinition module in group.Modules)
            {
                TabPage page = new TabPage(module.Name)
                {
                    BackColor = Color.White,
                    Padding = new Padding(0)
                };
                page.Controls.Add(new ModulePageControl(module));
                tabControl.TabPages.Add(page);
            }
        }

        private void tabControl_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (tabControl.SelectedTab?.Controls.OfType<ModulePageControl>().FirstOrDefault() is ModulePageControl page)
            {
                page.Reload();
            }
        }
    }
}
