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
            Font = UiTheme.Regular(10F);
            BackColor = UiTheme.Page;

            Panel header = new Panel
            {
                Dock = DockStyle.Top,
                Height = 82,
                BackColor = UiTheme.Page,
                Padding = new Padding(22, 12, 22, 10)
            };

            Label title = new Label
            {
                Dock = DockStyle.Top,
                Height = 36,
                Text = group.Title,
                Font = UiTheme.Bold(18F),
                ForeColor = UiTheme.Text,
                TextAlign = ContentAlignment.MiddleLeft
            };

            Label summary = new Label
            {
                Dock = DockStyle.Fill,
                Text = group.Summary,
                ForeColor = UiTheme.MutedText,
                TextAlign = ContentAlignment.MiddleLeft
            };

            header.Controls.Add(summary);
            header.Controls.Add(title);

            tabControl.Dock = DockStyle.Fill;
            tabControl.Padding = new Point(18, 8);
            tabControl.Font = UiTheme.Regular(10F);
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
                if (ReferenceEquals(module, ModuleCatalog.SaleOrder))
                {
                    page.Controls.Add(new SalesOrderPageControl());
                }
                else
                {
                    page.Controls.Add(new ModulePageControl(module));
                }
                tabControl.TabPages.Add(page);
            }
        }

        private void tabControl_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (tabControl.SelectedTab?.Controls.OfType<ModulePageControl>().FirstOrDefault() is ModulePageControl page)
            {
                page.Reload();
            }
            else if (tabControl.SelectedTab?.Controls.OfType<SalesOrderPageControl>().FirstOrDefault() is SalesOrderPageControl orderPage)
            {
                orderPage.Reload();
            }
        }
    }
}
