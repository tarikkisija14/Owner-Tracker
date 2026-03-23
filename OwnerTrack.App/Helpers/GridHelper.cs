namespace OwnerTrack.App.Helpers
{
    public static class GridHelper
    {
        public static void ApplyColumns(
            DataGridView grid,
            (string Ime, int Sirina, string Zaglavlje, string? Format)[] columns)
        {
            if (grid.Columns.Count == 0) return;

            foreach (var (ime, sirina, zaglavlje, format) in columns)
            {
                if (!grid.Columns.Contains(ime)) continue;
                grid.Columns[ime].Width = sirina;
                grid.Columns[ime].HeaderText = zaglavlje;
                if (format != null)
                    grid.Columns[ime].DefaultCellStyle.Format = format;
            }
        }

        public static void ConfigureColumn(
            DataGridView grid,
            string name,
            string header,
            float fillWeight,
            string? format = null)
        {
            if (!grid.Columns.Contains(name)) return;
            grid.Columns[name].HeaderText = header;
            grid.Columns[name].FillWeight = fillWeight;
            if (format != null)
                grid.Columns[name].DefaultCellStyle.Format = format;
        }

        public static bool TryGetSelectedId(
            DataGridView grid,
            out int id,
            string? messageIfNone = null)
        {
            id = 0;

            if (grid.SelectedRows.Count == 0)
            {
                if (messageIfNone != null)
                    MessageBox.Show(messageIfNone);
                return false;
            }

            if (grid.SelectedRows[0].Cells["Id"].Value is not int parsed)
                return false;

            id = parsed;
            return true;
        }

        public static void BindWithoutEvent<T>(
            DataGridView grid,
            EventHandler selectionChangedHandler,
            List<T> data)
        {
            grid.SelectionChanged -= selectionChangedHandler;
            grid.DataSource = data;
            grid.ClearSelection();
            grid.SelectionChanged += selectionChangedHandler;
        }
    }
}