using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using OwnerTrack.App.Constants;

namespace OwnerTrack.App.Helpers
{
    public static class ProfileImageHelper
    {
        private const int StoredSize = 256;

        /// <summary>
        /// Učitava odabranu sliku, centrirano je izrezuje u kvadrat i smanjuje na
        /// 256×256 PNG, da baza (i backup) ne raste zbog velikih fotografija.
        /// </summary>
        public static byte[] LoadAndNormalize(string path)
        {
            using var source = Image.FromStream(new MemoryStream(File.ReadAllBytes(path)));
            int side = Math.Min(source.Width, source.Height);
            var crop = new Rectangle((source.Width - side) / 2, (source.Height - side) / 2, side, side);

            using var result = new Bitmap(StoredSize, StoredSize);
            using (var g = Graphics.FromImage(result))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.DrawImage(source, new Rectangle(0, 0, StoredSize, StoredSize), crop, GraphicsUnit.Pixel);
            }

            using var ms = new MemoryStream();
            result.Save(ms, ImageFormat.Png);
            return ms.ToArray();
        }

        public static Image FromBytes(byte[] data)
        {
            using var ms = new MemoryStream(data);
            using var img = Image.FromStream(ms);
            return new Bitmap(img);
        }

        /// <summary>Osnovni placeholder: siluet osobe (Segoe MDL2 glyph) na svijetloj pozadini.</summary>
        public static Image CreatePlaceholder(int size = 160)
        {
            var bmp = new Bitmap(size, size);
            using var g = Graphics.FromImage(bmp);
            g.Clear(UiTheme.PanelLight);
            g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

            using var font = new Font("Segoe MDL2 Assets", size * 0.45f);
            using var brush = new SolidBrush(UiTheme.GridBorder);
            using var format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            g.DrawString("", font, brush, new RectangleF(0, 0, size, size), format);
            return bmp;
        }
    }
}
