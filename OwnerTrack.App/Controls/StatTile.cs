using OwnerTrack.App.Constants;

namespace OwnerTrack.App.Controls
{
    /// <summary>
    /// Reusable "stat tile" card — a dark (Navy) tile with a large bold
    /// number and a caption underneath. Extracted from the inline stat-tile
    /// pattern already used in FrmUpozorenja's summary panel (ConfigureStatTile),
    /// so both that form and the Compliance Dashboard share one visual
    /// definition instead of two hand-copied ones. Optionally clickable via
    /// <see cref="TileClick"/>, e.g. to navigate to a filtered screen.
    /// </summary>
    public class StatTile : UserControl
    {
        private readonly Label _lblValue;
        private readonly Label _lblTitle;
        private Color _accentColor = UiTheme.Navy;

        public event EventHandler? TileClick;

        public string Title
        {
            get => _lblTitle.Text;
            set => _lblTitle.Text = value;
        }

        public string Value
        {
            get => _lblValue.Text;
            set => _lblValue.Text = value;
        }

        public Color AccentColor
        {
            get => _accentColor;
            set
            {
                _accentColor = value;
                _lblValue.ForeColor = value;
            }
        }

        /// <summary>
        /// Za "akcijske" kartice (npr. Dodaj klijenta, Izvoz PDF) koje nemaju
        /// brojčanu statistiku — prikazuje veliki glyph ikonu umjesto broja.
        /// IconGlyph mora biti Segoe MDL2 Assets kod (isti font kao IconButton).
        /// </summary>
        public string? IconGlyph
        {
            get => _lblValue.Text;
            set
            {
                _lblValue.Text = value ?? string.Empty;
                _lblValue.Font = new Font("Segoe MDL2 Assets", 36f);
            }
        }

        public StatTile()
        {
            BackColor = UiTheme.Navy;
            Dock = DockStyle.Fill;
            Margin = new Padding(10);
            Cursor = Cursors.Default;
            Padding = new Padding(4);

            _lblValue = new Label
            {
                Dock = DockStyle.Top,
                Height = 92,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = UiTheme.Base(46f, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = UiTheme.Navy,
                Text = "0",
            };
            _lblTitle = new Label
            {
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = UiTheme.Base(15f, FontStyle.Bold),
                ForeColor = UiTheme.HeaderSubText,
                BackColor = UiTheme.Navy,
                Padding = new Padding(6, 0, 6, 0),
                Text = string.Empty,
            };

            Controls.Add(_lblTitle);
            Controls.Add(_lblValue);

            WireHoverAndClick(this);
            WireHoverAndClick(_lblValue);
            WireHoverAndClick(_lblTitle);
        }

        // UserControl.OnMouseEnter/Leave se ne okida kad je miš iznad
        // djece (_lblValue/_lblTitle prekrivaju cijelu površinu kartice),
        // pa hover mora biti ožičen na sva tri elementa da pokriva cijelu
        // karticu, ne samo uski rub oko labela.
        private void WireHoverAndClick(Control c)
        {
            c.Click += (_, _) => TileClick?.Invoke(this, EventArgs.Empty);
            c.Cursor = Cursors.Hand;
            c.MouseEnter += (_, _) => ApplyHoverState(true);
            c.MouseLeave += (_, _) => ApplyHoverState(false);
        }

        private void ApplyHoverState(bool hovering)
        {
            Color bg = hovering ? ControlPaint.Light(UiTheme.Navy, 0.25f) : UiTheme.Navy;
            BackColor = bg;
            _lblValue.BackColor = bg;
            _lblTitle.BackColor = bg;
        }
    }
}
