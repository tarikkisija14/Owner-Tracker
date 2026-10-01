namespace OwnerTrack.App.Constants
{
    internal static class UiTheme
    {
        public const string FontFamily = "Segoe UI";

        public static readonly Color Navy = Color.FromArgb(28, 40, 65);
        public static readonly Color Green = Color.FromArgb(39, 174, 96);
        public static readonly Color Blue = Color.FromArgb(52, 120, 200);
        public static readonly Color Red = Color.FromArgb(192, 57, 43);

        public static readonly Color FormBackgroundDialog = Color.FromArgb(245, 248, 252);
        public static readonly Color FormBackgroundMain = Color.FromArgb(248, 250, 253);
        public static readonly Color PanelLight = Color.FromArgb(240, 244, 250);
        public static readonly Color InputBackground = Color.FromArgb(250, 252, 255);
        public static readonly Color LabelText = Color.FromArgb(45, 55, 75);
        public static readonly Color MutedText = Color.FromArgb(50, 60, 80);

        public static readonly Color GridBorder = Color.FromArgb(210, 218, 230);
        public static readonly Color GridAltRow = Color.FromArgb(246, 249, 253);
        public static readonly Color GridHoverRow = Color.FromArgb(225, 236, 248);

        // Sekundarni tekst na tamnoj (Navy) pozadini — MutedText je predviđen za bijelu pozadinu.
        public static readonly Color HeaderSubText = Color.FromArgb(176, 196, 222);
        public static readonly Color AlertRedOnDark = Color.FromArgb(255, 140, 130);
        public static readonly Color AlertAmberOnDark = Color.FromArgb(255, 198, 120);

        public static readonly Color SidebarBackground = Color.FromArgb(22, 32, 53);
        public static readonly Color SidebarActive = Color.FromArgb(41, 58, 94);
        // Subtle hairline tint for sidebar separators — a lighter shade of
        // SidebarBackground, not a new accent colour.
        public static readonly Color SidebarDivider = Color.FromArgb(42, 54, 80);

        // Named accents for specific Form1 toolbar actions — previously
        // inline Color.FromArgb literals repeated at several call sites.
        // Values unchanged, only named for a single source of truth.
        public static readonly Color ImportAccent = Color.FromArgb(22, 141, 84);
        public static readonly Color ResetAccent = Color.FromArgb(150, 40, 40);
        public static readonly Color PdfSaveAccent = Color.FromArgb(70, 100, 160);
        public static readonly Color PdfExportAccent = Color.FromArgb(41, 98, 155);
        public static readonly Color WarningsAccent = Color.FromArgb(200, 155, 10);

        public const int GridRowHeight = 28;
        public const int GridHeaderHeight = 34;

        public static Font Base(float size = 9f, FontStyle style = FontStyle.Regular) =>
            new Font(FontFamily, size, style);

        // ── DataGridView ──────────────────────────────────────────────

        // DataGridView.DoubleBuffered is protected — reflection is the
        // standard, well-known way to enable it from outside the control.
        // Without this, redraws (especially with Frozen columns, which need
        // a two-pane repaint) can visibly flicker.
        private static void EnableDoubleBuffering(DataGridView g)
        {
            typeof(DataGridView).InvokeMember(
                "DoubleBuffered",
                System.Reflection.BindingFlags.SetProperty
                    | System.Reflection.BindingFlags.Instance
                    | System.Reflection.BindingFlags.NonPublic,
                null, g, new object[] { true });
        }

        public static void StyleGrid(DataGridView g, bool enableRowHover = true)
        {
            EnableDoubleBuffering(g);

            g.BackgroundColor = Color.White;
            g.GridColor = GridBorder;
            g.BorderStyle = BorderStyle.None;
            g.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            g.ColumnHeadersDefaultCellStyle.BackColor = Navy;
            g.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            // Bez ovoga, header kolone trenutne (kliknute) ćelije mijenja boju
            // (sistemska "selected" boja) — neutrališemo da izgleda identično.
            g.ColumnHeadersDefaultCellStyle.SelectionBackColor = Navy;
            g.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
            g.ColumnHeadersDefaultCellStyle.Font = Base(9f, FontStyle.Bold);
            g.ColumnHeadersDefaultCellStyle.Padding = new Padding(6, 0, 0, 0);
            g.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            g.ColumnHeadersHeight = GridHeaderHeight;
            g.EnableHeadersVisualStyles = false;
            g.DefaultCellStyle.Font = Base(9f);
            g.DefaultCellStyle.SelectionBackColor = Blue;
            g.DefaultCellStyle.SelectionForeColor = Color.White;
            g.DefaultCellStyle.Padding = new Padding(5, 0, 0, 0);
            g.RowTemplate.Height = GridRowHeight;
            g.AlternatingRowsDefaultCellStyle.BackColor = GridAltRow;

            if (enableRowHover)
                AttachRowHover(g);

            AttachCellTooltips(g);
        }

        // Shows the full cell value as a tooltip on hover — helps when a
        // column is narrower than its content (long addresses, notes, ...).
        private static void AttachCellTooltips(DataGridView g)
        {
            g.ShowCellToolTips = true;
            g.CellToolTipTextNeeded += (_, e) =>
            {
                if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
                e.ToolTipText = g.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString() ?? string.Empty;
            };
        }

        private static void AttachRowHover(DataGridView g)
        {
            g.CellMouseEnter += (_, e) =>
            {
                if (e.RowIndex < 0 || e.RowIndex >= g.Rows.Count) return;
                if (!g.Rows[e.RowIndex].Selected)
                    g.Rows[e.RowIndex].DefaultCellStyle.BackColor = GridHoverRow;
            };
            g.CellMouseLeave += (_, e) =>
            {
                if (e.RowIndex < 0 || e.RowIndex >= g.Rows.Count) return;
                if (!g.Rows[e.RowIndex].Selected)
                    g.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Empty;
            };
        }

        // ── Buttons ───────────────────────────────────────────────────

        public static void StyleAccentButton(Button b, Color accent, float fontSize = 9f)
        {
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.BackColor = accent;
            b.ForeColor = Color.White;
            b.Font = Base(fontSize, FontStyle.Bold);
            b.Cursor = Cursors.Hand;
            b.UseVisualStyleBackColor = false;

            AttachHoverEffect(b, accent);
        }

        public static void StyleFlatButton(Button b, Color backColor, float fontSize = 10f)
        {
            b.BackColor = backColor;
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.ForeColor = Color.White;
            b.Font = Base(fontSize, FontStyle.Bold);
            b.Cursor = Cursors.Hand;
            b.UseVisualStyleBackColor = false;
            b.TextAlign = ContentAlignment.MiddleCenter;

            AttachHoverEffect(b, backColor);
        }

        /// <summary>
        /// Lijevo poravnat item za sidebar (ikonica + labela), umjesto
        /// centriranog izgleda toolbar dugmadi. Aktivna stavka dobija
        /// suptilnu accent traku sa lijeve strane (postojeća Blue boja) —
        /// bez mijenjanja postojeće palete.
        /// </summary>
        public static void StyleSidebarButton(Button b, bool active = false)
        {
            Color bg = active ? SidebarActive : SidebarBackground;
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.BackColor = bg;
            b.ForeColor = active ? Color.White : HeaderSubText;
            b.Font = Base(active ? 10f : 9.5f, active ? FontStyle.Bold : FontStyle.Regular);
            b.TextAlign = ContentAlignment.MiddleLeft;
            b.Cursor = Cursors.Hand;
            b.UseVisualStyleBackColor = false;

            if (b is OwnerTrack.App.Controls.IconButton icon)
            {
                icon.LeftInset = 14;
                icon.IconSize = 15f;
                icon.IconTextGap = 10;
                icon.AccentBarColor = active ? Blue : Color.Empty;
                icon.AccentBarWidth = 3;
            }

            AttachHoverEffect(b, bg);
        }

        private static void AttachHoverEffect(Button b, Color baseColor)
        {
            Color hover = ControlPaint.Light(baseColor, 0.15f);
            Color pressed = ControlPaint.Dark(baseColor, 0.1f);

            b.MouseEnter += (_, _) => b.BackColor = hover;
            b.MouseLeave += (_, _) => b.BackColor = baseColor;
            b.MouseDown += (_, _) => b.BackColor = pressed;
            b.MouseUp += (_, _) =>
            {
                // Click handler može zatvoriti (i uništiti) formu prije MouseUp-a
                // (npr. Odjava) — dodir uništenog dugmeta bi bacio ObjectDisposedException.
                if (b.IsDisposed) return;
                b.BackColor = b.ClientRectangle.Contains(b.PointToClient(Cursor.Position)) ? hover : baseColor;
            };
        }

        // ── Form input controls ──────────────────────────────────────

        public static void StyleLabel(Label l, float fontSize = 9.5f)
        {
            l.AutoSize = true;
            l.Font = Base(fontSize);
            l.ForeColor = LabelText;
        }

        public static void StyleTextBox(TextBox t, float fontSize = 9.5f)
        {
            t.Font = Base(fontSize);
            t.BorderStyle = BorderStyle.None;
            t.BackColor = Color.White;
        }

        /// <summary>
        /// Wraps a borderless TextBox in a Panel that paints a thin 1px
        /// border around it. Caller adds the returned Panel to its parent instead of
        /// the TextBox directly; the TextBox itself keeps working exactly as
        /// before (code-behind still reads/writes txt.Text unchanged).
        /// </summary>
        public static Panel WrapWithFocusBorder(TextBox input, Point location, Size size)
        {
            var wrapper = new Panel
            {
                Location = location,
                Size = size,
                BackColor = Color.White,
            };

            // Border is painted (not the panel background), so any height
            // difference between the wrapper and the TextBox never shows up
            // as a thick line; the gap just matches the input background.
            wrapper.Paint += (_, e) =>
            {
                using var pen = new Pen(SystemColors.ControlDark);
                e.Graphics.DrawRectangle(pen, 0, 0, wrapper.ClientSize.Width - 1, wrapper.ClientSize.Height - 1);
            };

            void LayoutInput()
            {
                int w = Math.Max(0, wrapper.ClientSize.Width - 2);
                int h = Math.Max(0, wrapper.ClientSize.Height - 2);
                if (input.Multiline)
                {
                    input.SetBounds(1, 1, w, h);
                }
                else
                {
                    // Borderless single-line TextBox draws text at the top of
                    // its box; shrink it to the text height and centre it.
                    int textH = Math.Min(h, input.PreferredHeight);
                    input.SetBounds(1, 1 + (h - textH) / 2, w, textH);
                }
                wrapper.Invalidate();
            }

            input.Dock = DockStyle.None;
            wrapper.Controls.Add(input);
            LayoutInput();
            wrapper.Resize += (_, _) => LayoutInput();
            // StyleTextBox / DPI scaling change the font after wrapping, so
            // the centring must be recomputed once the final font is known.
            input.FontChanged += (_, _) => LayoutInput();
            wrapper.HandleCreated += (_, _) => LayoutInput();

            return wrapper;
        }

        public static void StyleEmptyState(Label l, string text)
        {
            l.Text = text;
            l.Dock = DockStyle.Fill;
            l.TextAlign = ContentAlignment.MiddleCenter;
            l.BackColor = Color.White;
            l.ForeColor = MutedText;
            l.Font = Base(10f, FontStyle.Italic);
            l.Visible = false;
        }

        public static void StyleComboBox(ComboBox c, float fontSize = 9.5f)
        {
            c.Font = Base(fontSize);
            c.FlatStyle = FlatStyle.System;
        }

        public static void StyleDateTimePicker(DateTimePicker dtp, float fontSize = 9.5f)
        {
            dtp.Font = Base(fontSize);
            dtp.CalendarForeColor = LabelText;
            dtp.CalendarMonthBackground = Color.White;
        }

        public static void StyleGroupBox(GroupBox gb, string title, float fontSize = 9.5f)
        {
            gb.Text = title;
            gb.Font = Base(fontSize, FontStyle.Bold);
            gb.ForeColor = Navy;
            gb.BackColor = Color.White;
            gb.Padding = new Padding(6);
        }
    }
}
