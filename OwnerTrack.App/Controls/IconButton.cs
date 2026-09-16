using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace OwnerTrack.App.Controls
{
    /// <summary>
    /// Flat button that renders an optional Segoe MDL2 Assets glyph before
    /// its text. Background, hover/pressed chrome, focus handling and text
    /// layout are all still done by the normal Button/ButtonBase renderer
    /// (via base.OnPaint) — proven, artifact-free — we only reserve a left
    /// margin (via Padding) for the icon and draw the glyph into it
    /// afterwards. No custom background painting or clipping Region: those
    /// approaches produced visible seams/borders that a plain base-class
    /// render doesn't have.
    /// </summary>
    public class IconButton : Button
    {
        private const string IconFontFamily = "Segoe MDL2 Assets";
        private string? _iconGlyph;

        public string? IconGlyph
        {
            get => _iconGlyph;
            set
            {
                _iconGlyph = value;
                UpdateIconPadding();
            }
        }

        public float IconSize { get; set; } = 13f;
        public int IconTextGap { get; set; } = 7;

        private void UpdateIconPadding()
        {
            int left = string.IsNullOrEmpty(_iconGlyph) ? 0 : IconZoneWidth;
            if (Padding.Left != left)
                Padding = new Padding(left, Padding.Top, Padding.Right, Padding.Bottom);
        }

        private int IconZoneWidth
        {
            get
            {
                using var iconFont = new Font(IconFontFamily, IconSize);
                using var g = CreateGraphics();
                float w = g.MeasureString(_iconGlyph, iconFont).Width;
                return (int)Math.Ceiling(w) + IconTextGap;
            }
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);

            if (string.IsNullOrEmpty(IconGlyph)) return;

            var g = pevent.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            using var iconFont = new Font(IconFontFamily, IconSize);
            SizeF iconSize = g.MeasureString(IconGlyph, iconFont);

            float x = (Padding.Left - iconSize.Width) / 2f;
            float y = (Height - iconSize.Height) / 2f;

            using var brush = new SolidBrush(ForeColor);
            g.DrawString(IconGlyph, iconFont, brush, x, y);
        }
    }
}
