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

        private Image? _avatarImage;

        /// <summary>
        /// Okrugla slika umjesto glyph ikone (npr. profilna slika u sidebaru). Ima prednost
        /// nad IconGlyph; vlasnik slike je pozivalac (on je i uništava).
        /// </summary>
        public Image? AvatarImage
        {
            get => _avatarImage;
            set
            {
                _avatarImage = value;
                RebuildAvatarBitmap();
                UpdateIconPadding();
                Invalidate();
            }
        }

        public int AvatarSize { get; set; } = 26;

        // Slika unaprijed smanjena na tačnu veličinu avatara: glatke ivice kruga dolaze iz
        // FillEllipse + TextureBrush (antialiasing radi), a ne iz SetClip (ima nazubljen rub).
        private Bitmap? _avatarBitmap;

        private void RebuildAvatarBitmap()
        {
            _avatarBitmap?.Dispose();
            _avatarBitmap = null;
            if (_avatarImage is null) return;

            // Krug se crta 4× većim, sa prozirnom pozadinom i Clamp wrap modom (inače
            // TextureBrush na rubu uzorkuje suprotnu stranu slike i pravi tamni/svijetli obrub),
            // a pri crtanju se smanjuje — rub je tako potpuno gladak.
            const int Scale = 4;
            int big = AvatarSize * Scale;

            using var scaled = new Bitmap(big, big);
            using (var gs = Graphics.FromImage(scaled))
            {
                gs.InterpolationMode = InterpolationMode.HighQualityBicubic;
                gs.PixelOffsetMode = PixelOffsetMode.HighQuality;
                gs.DrawImage(_avatarImage, new Rectangle(0, 0, big, big));
            }

            _avatarBitmap = new Bitmap(big, big, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
            using var g = Graphics.FromImage(_avatarBitmap);
            g.Clear(Color.Transparent);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            using var brush = new TextureBrush(scaled, WrapMode.Clamp);
            g.FillEllipse(brush, 0, 0, big - 1, big - 1);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) _avatarBitmap?.Dispose();
            base.Dispose(disposing);
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
            int left = LeftInset + (_avatarImage is not null
                ? AvatarSize + IconTextGap
                : string.IsNullOrEmpty(_iconGlyph) ? 0 : IconZoneWidth);
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

            if (_avatarBitmap is not null)
            {
                bool avatarHasText = !string.IsNullOrEmpty(Text);
                int ax = avatarHasText ? LeftInset : (Width - AvatarSize) / 2;
                int ay = (Height - AvatarSize) / 2;

                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.DrawImage(_avatarBitmap, new Rectangle(ax, ay, AvatarSize, AvatarSize));
                return;
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
