using OwnerTrack.App.Constants;
using OwnerTrack.Data.Enums;
using OwnerTrack.Infrastructure.ViewModels;

namespace OwnerTrack.App.Controls
{
    /// <summary>
    /// Read-only vertical timeline view of audit history entries
    /// (AuditEntryViewModel) — a second, chronological way to look at exactly
    /// the same data the "Historija" grid in FrmKlijentProfil already shows.
    /// No new data source: SetEntries() takes the same
    /// KlijentProfilViewModel.Historija list.
    /// </summary>
    public class ActivityTimeline : UserControl
    {
        private readonly FlowLayoutPanel _flow;
        private readonly Label _lblEmpty;

        public ActivityTimeline()
        {
            BackColor = Color.White;
            AutoScroll = true;

            _flow = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Padding = new Padding(10, 8, 10, 8),
            };

            _lblEmpty = new Label();
            UiTheme.StyleEmptyState(_lblEmpty, "Nema evidentiranih promjena.");
            _lblEmpty.Dock = DockStyle.Fill;

            Controls.Add(_flow);
            Controls.Add(_lblEmpty);
        }

        public void SetEntries(IEnumerable<AuditEntryViewModel> entries)
        {
            _flow.SuspendLayout();
            _flow.Controls.Clear();

            var list = entries.ToList();
            _lblEmpty.Visible = list.Count == 0;
            _flow.Visible = list.Count > 0;

            foreach (var entry in list)
                _flow.Controls.Add(BuildRow(entry));

            _flow.ResumeLayout();
        }

        private static Panel BuildRow(AuditEntryViewModel entry)
        {
            var row = new Panel
            {
                Width = 780,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Padding = new Padding(0, 0, 0, 10),
            };

            var lblVrijeme = new Label
            {
                Text = entry.Vrijeme.ToString("dd.MM.yyyy HH:mm"),
                AutoSize = true,
                Location = new Point(0, 2),
                Font = UiTheme.Base(9f, FontStyle.Bold),
                ForeColor = UiTheme.Navy,
            };

            var lblBadge = new Label
            {
                Text = $" {entry.Akcija} — {entry.Tabela} ",
                AutoSize = true,
                Location = new Point(150, 0),
                Font = UiTheme.Base(8f, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = AkcijaColor(entry.Akcija),
                Padding = new Padding(6, 3, 6, 3),
            };

            var lblOpis = new Label
            {
                Text = entry.Opis ?? string.Empty,
                AutoSize = false,
                Location = new Point(0, 26),
                Size = new Size(780, 34),
                Font = UiTheme.Base(9f),
                ForeColor = UiTheme.LabelText,
            };

            row.Controls.Add(lblVrijeme);
            row.Controls.Add(lblBadge);
            row.Controls.Add(lblOpis);
            row.Height = 62;

            return row;
        }

        private static Color AkcijaColor(string? akcija) => akcija switch
        {
            AuditConstants.Dodano => UiTheme.Green,
            AuditConstants.Izmijenjeno => UiTheme.Blue,
            AuditConstants.Obrisano => UiTheme.Red,
            AuditConstants.Pregledano => UiTheme.Blue,
            AuditConstants.Deaktivirano => UiTheme.Red,
            AuditConstants.Aktivirano => UiTheme.Green,
            _ => UiTheme.MutedText,
        };
    }
}
