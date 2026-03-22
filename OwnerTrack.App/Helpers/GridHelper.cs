namespace OwnerTrack.App.Helpers
{
    
    public static class GridHelper
    {
       
        public static void PostaviKolone(
            DataGridView grid,
            (string Ime, int Sirina, string Zaglavlje, string? Format)[] kolone)
        {
            if (grid.Columns.Count == 0) return;

            foreach (var (ime, sirina, zaglavlje, format) in kolone)
            {
                if (!grid.Columns.Contains(ime)) continue;
                grid.Columns[ime].Width = sirina;
                grid.Columns[ime].HeaderText = zaglavlje;
                if (format != null)
                    grid.Columns[ime].DefaultCellStyle.Format = format;
            }
        }

     
        public static void KonfigurirajKolonu(
            DataGridView grid,
            string ime,
            string zaglavlje,
            float fillWeight,
            string? format = null)
        {
            if (!grid.Columns.Contains(ime)) return;
            grid.Columns[ime].HeaderText = zaglavlje;
            grid.Columns[ime].FillWeight = fillWeight;
            if (format != null)
                grid.Columns[ime].DefaultCellStyle.Format = format;
        }

        
        public static bool PokupiOdabraniId(
            DataGridView grid,
            out int id,
            string? porukaAkoNema = null)
        {
            id = 0;

            if (grid.SelectedRows.Count == 0)
            {
                if (porukaAkoNema != null)
                    MessageBox.Show(porukaAkoNema);
                return false;
            }

            if (grid.SelectedRows[0].Cells["Id"].Value is not int parsed)
                return false;

            id = parsed;
            return true;
        }

       
        public static void BindBezEventa<T>(
            DataGridView grid,
            EventHandler selectionChangedHandler,
            List<T> podaci)
        {
            grid.SelectionChanged -= selectionChangedHandler;
            grid.DataSource = podaci;
            grid.ClearSelection();
            grid.SelectionChanged += selectionChangedHandler;
        }
    }
}