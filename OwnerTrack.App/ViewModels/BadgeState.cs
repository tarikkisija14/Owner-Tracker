using OwnerTrack.App.Constants;

namespace OwnerTrack.App.ViewModels
{
    /// <summary>
    /// Immutable value object that describes the visual state of the warnings badge button.
    /// Encapsulates all badge display decisions so Form1 only needs to apply them.
    /// </summary>
    public sealed class BadgeState
    {
        public string Label { get; }
        public Color BackColor { get; }
        public Color ForeColor { get; }
        public FontStyle FontStyle { get; }

        private BadgeState(string label, Color backColor, Color foreColor, FontStyle fontStyle)
        {
            Label = label;
            BackColor = backColor;
            ForeColor = foreColor;
            FontStyle = fontStyle;
        }

        public static BadgeState FromStats(int count, bool imaIsteklih)
        {
            if (count == 0)
                return new BadgeState(
                    UiConstants.BadgeLabelDefault,
                    SystemColors.Control,
                    SystemColors.ControlText,
                    FontStyle.Regular);

            Color backColor = imaIsteklih
                ? Color.Firebrick
                : Color.FromArgb(220, 120, 20);

            return new BadgeState(
                string.Format(UiConstants.BadgeLabelFormat, count),
                backColor,
                Color.White,
                FontStyle.Bold);
        }
    }
}