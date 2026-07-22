namespace WinFormsApp2
{
    internal static class UiTheme
    {
        public static readonly Color Page = Color.FromArgb(243, 246, 250);
        public static readonly Color Surface = Color.White;
        public static readonly Color SurfaceAlt = Color.FromArgb(239, 243, 248);
        public static readonly Color Border = Color.FromArgb(215, 222, 232);
        public static readonly Color Text = Color.FromArgb(31, 41, 55);
        public static readonly Color MutedText = Color.FromArgb(100, 116, 139);
        public static readonly Color Accent = Color.FromArgb(20, 99, 183);
        public static readonly Color AccentHover = Color.FromArgb(13, 82, 155);
        public static readonly Color Danger = Color.FromArgb(185, 28, 28);

        public static Font Regular(float size) => new Font("Microsoft YaHei UI", size, FontStyle.Regular);

        public static Font Bold(float size) => new Font("Microsoft YaHei UI", size, FontStyle.Bold);

        public static void StylePrimaryButton(Button button)
        {
            button.FlatStyle = FlatStyle.Flat;
            button.BackColor = Accent;
            button.ForeColor = Color.White;
            button.Font = Bold(10F);
            button.Cursor = Cursors.Hand;
            button.UseVisualStyleBackColor = false;
            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseOverBackColor = AccentHover;
            button.FlatAppearance.MouseDownBackColor = Color.FromArgb(9, 64, 124);
        }

        public static void StyleSecondaryButton(Button button)
        {
            button.FlatStyle = FlatStyle.Flat;
            button.BackColor = Surface;
            button.ForeColor = Text;
            button.Font = Regular(10F);
            button.Cursor = Cursors.Hand;
            button.UseVisualStyleBackColor = false;
            button.FlatAppearance.BorderColor = Border;
            button.FlatAppearance.BorderSize = 1;
            button.FlatAppearance.MouseOverBackColor = SurfaceAlt;
            button.FlatAppearance.MouseDownBackColor = Color.FromArgb(226, 232, 240);
        }

        public static void StyleInput(TextBox textBox)
        {
            textBox.BorderStyle = BorderStyle.FixedSingle;
            textBox.BackColor = Surface;
            textBox.ForeColor = Text;
            textBox.Font = Regular(10.5F);
        }

        public static System.Drawing.Drawing2D.GraphicsPath CreateRoundRect(Rectangle bounds, int radius)
        {
            int diameter = radius * 2;
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddArc(bounds.Left, bounds.Top, diameter, diameter, 180, 90);
            path.AddArc(bounds.Right - diameter, bounds.Top, diameter, diameter, 270, 90);
            path.AddArc(bounds.Right - diameter, bounds.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(bounds.Left, bounds.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}
