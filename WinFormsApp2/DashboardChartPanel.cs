using System.Drawing.Drawing2D;

namespace WinFormsApp2
{
    internal enum DashboardChartKind
    {
        Line,
        Bar
    }

    internal sealed class DashboardChartPanel : Panel
    {
        public DashboardChartKind ChartKind { get; set; }

        public decimal[] Values { get; set; } = Array.Empty<decimal>();

        public string[] Labels { get; set; } = Array.Empty<string>();

        public Color AccentColor { get; set; } = UiTheme.Accent;

        public DashboardChartPanel()
        {
            BackColor = UiTheme.Surface;
            DoubleBuffered = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle area = ClientRectangle;
            area.Inflate(-22, -18);
            area.Y += 18;
            area.Height -= 18;
            area.Height -= 34;
            if (area.Width <= 20 || area.Height <= 20)
            {
                return;
            }

            if (Values.Length == 0)
            {
                TextRenderer.DrawText(g, "暂无数据", UiTheme.Regular(10F), area, UiTheme.MutedText,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                return;
            }

            decimal max = Math.Max(1, Values.Max());
            DrawGrid(g, area);
            if (ChartKind == DashboardChartKind.Bar)
            {
                DrawBars(g, area, max);
            }
            else
            {
                DrawLine(g, area, max);
            }
        }

        private static void DrawGrid(Graphics g, Rectangle area)
        {
            using Pen gridPen = new Pen(Color.FromArgb(229, 235, 244));
            for (int i = 0; i <= 4; i++)
            {
                int y = area.Bottom - (area.Height * i / 4);
                g.DrawLine(gridPen, area.Left, y, area.Right, y);
            }
        }

        private void DrawLine(Graphics g, Rectangle area, decimal max)
        {
            int count = Values.Length;
            PointF[] points = new PointF[count];
            for (int i = 0; i < count; i++)
            {
                float x = count == 1 ? area.Left + area.Width / 2F : area.Left + i * area.Width / (float)(count - 1);
                float y = area.Bottom - (float)(Values[i] / max) * (area.Height - 24);
                points[i] = new PointF(x, y);
            }

            using Pen linePen = new Pen(AccentColor, 3F);
            if (points.Length > 1)
            {
                g.DrawLines(linePen, points);
            }

            using SolidBrush dotBrush = new SolidBrush(Color.White);
            using Pen dotPen = new Pen(AccentColor, 2F);
            for (int i = 0; i < points.Length; i++)
            {
                RectangleF dot = new RectangleF(points[i].X - 4, points[i].Y - 4, 8, 8);
                g.FillEllipse(dotBrush, dot);
                g.DrawEllipse(dotPen, dot);
                DrawValueLabel(g, Values[i], points[i].X, points[i].Y - 26, area);
                DrawLabel(g, Labels.ElementAtOrDefault(i), points[i].X, area.Bottom + 10, 7);
            }
        }

        private void DrawBars(Graphics g, Rectangle area, decimal max)
        {
            int count = Values.Length;
            float gap = 12F;
            float barWidth = Math.Max(12F, (area.Width - gap * (count + 1)) / count);
            using SolidBrush brush = new SolidBrush(AccentColor);

            for (int i = 0; i < count; i++)
            {
                float height = (float)(Values[i] / max) * (area.Height - 30);
                float x = area.Left + gap + i * (barWidth + gap);
                RectangleF bar = new RectangleF(x, area.Bottom - height, barWidth, height);
                using GraphicsPath path = UiTheme.CreateRoundRect(Rectangle.Round(bar), 4);
                g.FillPath(brush, path);
                DrawValueLabel(g, Values[i], x + barWidth / 2F, bar.Top - 24, area);
                DrawLabel(g, Labels.ElementAtOrDefault(i), x + barWidth / 2F, area.Bottom + 10, 4);
            }
        }

        private void DrawValueLabel(Graphics g, decimal value, float centerX, float y, Rectangle area)
        {
            string label = FormatValue(value);
            Font font = UiTheme.Bold(8.5F);
            Size size = TextRenderer.MeasureText(label, font);
            int x = (int)(centerX - size.Width / 2F);
            x = Math.Max(area.Left, Math.Min(x, area.Right - size.Width));
            int top = Math.Max(area.Top - 16, (int)y);
            Rectangle rect = new Rectangle(x, top, size.Width, 22);

            TextRenderer.DrawText(g, label, font, rect, AccentColor,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
        }

        private string FormatValue(decimal value)
        {
            if (ChartKind == DashboardChartKind.Line)
            {
                return value >= 1000 ? "¥" + value.ToString("#,0") : "¥" + value.ToString("0.##");
            }

            return value.ToString("#,0");
        }

        private static void DrawLabel(Graphics g, string? text, float centerX, float y, int maxLength)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return;
            }

            string label = text.Length > maxLength ? text.Substring(0, maxLength) : text;
            Size size = TextRenderer.MeasureText(label, UiTheme.Regular(8F));
            Rectangle rect = new Rectangle((int)(centerX - size.Width / 2F), (int)y, size.Width, 20);
            TextRenderer.DrawText(g, label, UiTheme.Regular(8F), rect, UiTheme.MutedText,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.EndEllipsis);
        }
    }
}
