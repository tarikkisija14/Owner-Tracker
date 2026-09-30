namespace OwnerTrack.App.Helpers
{
    public static class FormHelper
    {
        public static void ApplyEditModeTitle(Form form, Button saveButton, bool isEditMode, string editTitle, string addTitle)
        {
            form.Text = isEditMode ? editTitle : addTitle;
            saveButton.Text = isEditMode
                ? Constants.UiMessages.KlijentSaveChangesButton
                : Constants.UiMessages.KlijentSaveNewButton;
        }

        public static void PopulateEnumCombo<TEnum>(ComboBox cb) where TEnum : struct, Enum
        {
            cb.Items.Clear();
            foreach (TEnum v in Enum.GetValues(typeof(TEnum)))
                cb.Items.Add(v.ToString());
        }

        public static void PopulateEnumComboWithEmpty<TEnum>(ComboBox cb) where TEnum : struct, Enum
        {
            cb.Items.Clear();
            cb.Items.Add(string.Empty);
            foreach (TEnum v in Enum.GetValues(typeof(TEnum)))
                cb.Items.Add(v.ToString());
        }

        public static void SetCombo(ComboBox cb, string? value)
        {
            if (string.IsNullOrEmpty(value)) { cb.SelectedIndex = 0; return; }
            int idx = cb.FindStringExact(value);
            cb.SelectedIndex = idx >= 0 ? idx : 0;
        }

        // Za DateTimePicker sa ShowCheckBox = true: neoznačen checkbox znači "datum nije unesen"
        // (null), a ne "danas". Isti obrazac kao dtDatumValjanosti u FrmDodajVlasnika/Direktora.
        public static void SetNullableDate(DateTimePicker dt, DateTime? value)
        {
            if (value.HasValue)
            {
                dt.Value = value.Value;
                dt.Checked = true;
            }
            else
            {
                dt.Checked = false;
            }
        }

        public static DateTime? GetNullableDate(DateTimePicker dt)
            => dt.Checked ? dt.Value : null;

        public static string? NullIfEmpty(string? s)
            => string.IsNullOrWhiteSpace(s) ? null : s.Trim();

        /// <summary>
        /// Wires up the given "mark dirty" callback on every input-like
        /// control under <paramref name="root"/> (TextBox, ComboBox,
        /// DateTimePicker, CheckBox), recursing into containers (GroupBox,
        /// Panel, etc). Used to detect unsaved changes on edit forms without
        /// hand-wiring every single field.
        /// </summary>
        public static void AttachDirtyTracking(Control root, Action onChanged)
        {
            foreach (Control control in root.Controls)
            {
                switch (control)
                {
                    case TextBox tb:
                        tb.TextChanged += (_, _) => onChanged();
                        break;
                    case ComboBox cb:
                        cb.SelectedIndexChanged += (_, _) => onChanged();
                        break;
                    case DateTimePicker dt:
                        dt.ValueChanged += (_, _) => onChanged();
                        break;
                    case CheckBox chk:
                        chk.CheckedChanged += (_, _) => onChanged();
                        break;
                }

                if (control.HasChildren)
                    AttachDirtyTracking(control, onChanged);
            }
        }
    }
}