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
                {
                    grid.Columns[ime].DefaultCellStyle.Format = format;
                    grid.Columns[ime].DefaultCellStyle.NullValue = "—";
                }
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

        /// <summary>
        /// Klik na već selektovan red poništava selekciju (inače u DataGridView-u
        /// nema načina da se selekcija ukloni klikom, npr. kad postoji samo 1 red).
        /// </summary>
        public static void EnableClickToDeselect(DataGridView grid)
        {
            int lastRow = -1;

            grid.CellClick += (_, e) =>
            {
                if (e.RowIndex < 0) return;

                if (e.RowIndex == lastRow)
                {
                    grid.ClearSelection();
                    lastRow = -1;
                }
                else
                {
                    lastRow = e.RowIndex;
                }
            };
        }

        public static void FreezeColumns(DataGridView grid, params string[] names)
        {
            foreach (var name in names)
                if (grid.Columns.Contains(name))
                    grid.Columns[name].Frozen = true;
        }

        public static void AlignRight(DataGridView grid, params string[] names)
        {
            foreach (var name in names)
            {
                if (!grid.Columns.Contains(name)) continue;
                grid.Columns[name].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                grid.Columns[name].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
        }

        public static void AlignCenter(DataGridView grid, params string[] names)
        {
            foreach (var name in names)
            {
                if (!grid.Columns.Contains(name)) continue;
                grid.Columns[name].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                grid.Columns[name].DefaultCellStyle.Padding = new Padding(0);
                grid.Columns[name].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                grid.Columns[name].HeaderCell.Style.Padding = new Padding(0);
            }
        }

        /// <summary>
        /// Reads exactly what's currently shown in the grid (visible columns,
        /// in display order, with each cell's formatted text) — used for PDF
        /// export of the generic evidencija views, so the export always
        /// matches what's on screen, nothing more.
        /// </summary>
        public static (string[] Headers, float[] Weights, List<string[]> Rows) ExtractVisibleData(
            DataGridView grid, bool selectedOnly)
        {
            var cols = grid.Columns.Cast<DataGridViewColumn>()
                .Where(c => c.Visible)
                .OrderBy(c => c.DisplayIndex)
                .ToList();

            string[] headers = cols.Select(c => c.HeaderText).ToArray();
            float[] weights = cols.Select(c => (float)Math.Max(c.Width, 40)).ToArray();

            IEnumerable<DataGridViewRow> source = selectedOnly
                ? grid.SelectedRows.Cast<DataGridViewRow>()
                : grid.Rows.Cast<DataGridViewRow>().Where(r => r.DataBoundItem is not null);

            var rows = source
                .Select(r => cols
                    .Select(c => r.Cells[c.Index].FormattedValue?.ToString() ?? string.Empty)
                    .ToArray())
                .ToList();

            return (headers, weights, rows);
        }

        public static void Emphasize(DataGridView grid, string name, Font font)
        {
            if (grid.Columns.Contains(name))
                grid.Columns[name].DefaultCellStyle.Font = font;
        }

        /// <summary>
        /// Klik na header kolone sortira grid po toj koloni (rastuće/opadajuće
        /// naizmjenično). Radi direktno nad trenutnim DataSource (List&lt;T&gt;)
        /// bez potrebe za BindingSource. Pošto se DataSource ponovo postavlja,
        /// stilizacija (širine, freeze, poravnanje...) se gubi — <paramref name="afterRebind"/>
        /// se poziva odmah nakon da je ponovo primijeni.
        /// </summary>
        public static void EnableColumnSort(DataGridView grid, Action? afterRebind = null)
        {
            string? sortedProperty = null;
            bool ascending = true;

            grid.ColumnHeaderMouseClick += (_, e) =>
            {
                if (grid.DataSource is not System.Collections.IEnumerable source) return;
                if (e.ColumnIndex < 0 || e.ColumnIndex >= grid.Columns.Count) return;

                var column = grid.Columns[e.ColumnIndex];
                string propName = string.IsNullOrEmpty(column.DataPropertyName) ? column.Name : column.DataPropertyName;

                ascending = sortedProperty != propName || !ascending;
                sortedProperty = propName;

                var listType = source.GetType();
                if (!listType.IsGenericType) return;
                var elementType = listType.GetGenericArguments()[0];
                var prop = elementType.GetProperty(propName);
                if (prop == null) return;

                var items = source.Cast<object>().ToList();
                items = ascending
                    ? items.OrderBy(x => prop.GetValue(x), ValueComparer.Instance).ToList()
                    : items.OrderByDescending(x => prop.GetValue(x), ValueComparer.Instance).ToList();

                var typedList = (System.Collections.IList)Activator.CreateInstance(listType)!;
                foreach (var item in items) typedList.Add(item);

                grid.DataSource = typedList;
                afterRebind?.Invoke();

                if (grid.Columns.Contains(propName))
                {
                    foreach (DataGridViewColumn c in grid.Columns)
                        c.HeaderCell.SortGlyphDirection = SortOrder.None;
                    grid.Columns[propName].HeaderCell.SortGlyphDirection =
                        ascending ? SortOrder.Ascending : SortOrder.Descending;
                }
            };
        }

        private sealed class ValueComparer : IComparer<object?>
        {
            public static readonly ValueComparer Instance = new();

            public int Compare(object? a, object? b)
            {
                if (a is null && b is null) return 0;
                if (a is null) return -1;
                if (b is null) return 1;
                if (a is IComparable ca) return ca.CompareTo(b);
                return string.Compare(a.ToString(), b.ToString(), StringComparison.OrdinalIgnoreCase);
            }
        }
    }
}