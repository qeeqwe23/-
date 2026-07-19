namespace WinFormsApp2
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            BuildEntryButtons();
        }

        private void OpenWorkspace_Click(object? sender, EventArgs e)
        {
            if (sender is Button { Tag: ModuleGroupDefinition group })
            {
                using ManagementWorkspaceForm form = new ManagementWorkspaceForm(group);
                form.ShowDialog();
            }
        }

        private void BuildEntryButtons()
        {
            entryLayout.Controls.Clear();

            for (int i = 0; i < ModuleCatalog.MainGroups.Length; i++)
            {
                Button entry = CreateEntryButton(ModuleCatalog.MainGroups[i]);
                entryLayout.Controls.Add(entry, i % 2, i / 2);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private Button CreateEntryButton(ModuleGroupDefinition group)
        {
            Button button = new Button
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(16),
                Padding = new Padding(28, 18, 28, 18),
                BackColor = Color.White,
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Microsoft YaHei UI", 12F),
                TextAlign = ContentAlignment.MiddleLeft,
                Tag = group,
                Text = $"{group.Title}\r\n\r\n{group.Summary}\r\n\r\n{group.ActionText}",
                UseVisualStyleBackColor = false
            };

            button.FlatAppearance.BorderColor = Color.Black;
            button.FlatAppearance.BorderSize = 1;
            button.MouseEnter += EntryButton_MouseEnter;
            button.MouseLeave += EntryButton_MouseLeave;
            button.Click += OpenWorkspace_Click;
            return button;
        }

        private static void EntryButton_MouseEnter(object? sender, EventArgs e)
        {
            if (sender is Button button)
            {
                button.BackColor = Color.FromArgb(245, 245, 245);
            }
        }

        private static void EntryButton_MouseLeave(object? sender, EventArgs e)
        {
            if (sender is Button button)
            {
                button.BackColor = Color.White;
            }
        }
    }
}
