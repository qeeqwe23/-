using System.Drawing.Drawing2D;

namespace WinFormsApp2
{
    internal sealed class ModuleCardButton : Button
    {
        private bool hovered;

        public string TitleText { get; set; } = string.Empty;

        public string HintText { get; set; } = string.Empty;

        public Color AccentColor { get; set; } = UiTheme.Accent;

        public ModuleCardButton()
        {
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            BackColor = Color.Transparent;
            Cursor = Cursors.Hand;
            Text = string.Empty;
            UseVisualStyleBackColor = false;
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            hovered = true;
            Invalidate();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            hovered = false;
            Invalidate();
            base.OnMouseLeave(e);
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            Graphics g = pevent.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle bounds = ClientRectangle;
            bounds.Inflate(-1, -1);

            using GraphicsPath path = UiTheme.CreateRoundRect(bounds, 8);
            using SolidBrush fill = new SolidBrush(hovered ? Color.White : UiTheme.Surface);
            using Pen border = new Pen(hovered ? AccentColor : UiTheme.Border);
            g.FillPath(fill, path);
            g.DrawPath(border, path);

            Rectangle accent = new Rectangle(bounds.Left + 22, bounds.Top + 24, 5, bounds.Height - 48);
            using GraphicsPath accentPath = UiTheme.CreateRoundRect(accent, 3);
            using SolidBrush accentBrush = new SolidBrush(AccentColor);
            g.FillPath(accentBrush, accentPath);

            Rectangle textArea = new Rectangle(bounds.Left + 44, bounds.Top + 28, bounds.Width - 88, bounds.Height - 56);
            TextRenderer.DrawText(g, TitleText, UiTheme.Bold(18F), textArea, UiTheme.Text,
                TextFormatFlags.Left | TextFormatFlags.Top | TextFormatFlags.EndEllipsis);

            Rectangle hintArea = new Rectangle(textArea.Left, textArea.Top + 56, textArea.Width, 32);
            TextRenderer.DrawText(g, HintText, UiTheme.Regular(10.5F), hintArea, UiTheme.MutedText,
                TextFormatFlags.Left | TextFormatFlags.Top | TextFormatFlags.EndEllipsis);

            Rectangle actionArea = new Rectangle(textArea.Left, bounds.Bottom - 48, textArea.Width, 24);
            TextRenderer.DrawText(g, "进入管理", UiTheme.Bold(10F), actionArea, AccentColor,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);

            Rectangle arrowArea = new Rectangle(bounds.Right - 58, bounds.Bottom - 53, 28, 28);
            TextRenderer.DrawText(g, ">", UiTheme.Bold(13F), arrowArea, AccentColor,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }
    }
}
