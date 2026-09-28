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

        private int _leftInset;

        /// <summary>
        /// Extra left margin reserved before the icon zone (e.g. a sidebar
        /// inset so items don't sit flush against the panel edge). Zero by
        /// default — existing (toolbar) buttons are unaffected.
        /// </summary>
        public int LeftInset
        {
            get => _leftInset;
            set
            {
                _leftInset = value;
                UpdateIconPadding();
            }
        }

        /// <summary>
        /// When set (non-Empty), a solid vertical bar of this colour is
        /// painted along the left edge — used for a sidebar "active item"
        /// indicator. A plain filled rectangle, not a Region/rounded shape,
        /// to stay clear of the clipping/seam issues that ruled those out.
        /// </summary>
        public Color AccentBarColor { get; set; } = Color.Empty;
        public int AccentBarWidth { get; set; } = 3;

        private void UpdateIconPadding()
        {
            int left = LeftInset + (string.IsNullOrEmpty(_iconGlyph) ? 0 : IconZoneWidth);
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

            var g = pevent.Graphics;

            if (AccentBarColor != Color.Empty)
            {
                using var barBrush = new SolidBrush(AccentBarColor);
                g.FillRectangle(barBrush, 0, 0, AccentBarWidth, Height);
            }

            if (string.IsNullOrEmpty(IconGlyph)) return;

            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            using var iconFont = new Font(IconFontFamily, IconSize);
            SizeF iconSize = g.MeasureString(IconGlyph, iconFont);

            // With no text (e.g. a collapsed sidebar item), centre the icon
            // across the whole button instead of the padding-reserved zone.
            bool hasText = !string.IsNullOrEmpty(Text);
            float x = hasText
                ? LeftInset + (Padding.Left - LeftInset - iconSize.Width) / 2f
                : (Width - iconSize.Width) / 2f;
            float y = (Height - iconSize.Height) / 2f;

            using var brush = new SolidBrush(ForeColor);
            g.DrawString(IconGlyph, iconFont, brush, x, y);
        }
    }
}
