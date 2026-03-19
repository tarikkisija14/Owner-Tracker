using System;
using System.Windows.Forms;

namespace OwnerTrack.App.Helpers
{
    public static class FormHelper
    {
        /// <summary>Clears and populates <paramref name="cb"/> with every value of <typeparamref name="TEnum"/>.</summary>
        public static void PopuniEnumCombo<TEnum>(ComboBox cb) where TEnum : struct, Enum
        {
            cb.Items.Clear();
            foreach (TEnum v in Enum.GetValues(typeof(TEnum)))
                cb.Items.Add(v.ToString());
        }

        /// <summary>Same as <see cref="PopuniEnumCombo{TEnum}"/> but inserts an empty option first.</summary>
        public static void PopuniEnumComboSPraznim<TEnum>(ComboBox cb) where TEnum : struct, Enum
        {
            cb.Items.Clear();
            cb.Items.Add(string.Empty);
            foreach (TEnum v in Enum.GetValues(typeof(TEnum)))
                cb.Items.Add(v.ToString());
        }

        /// <summary>Selects the item matching <paramref name="value"/>, or index 0 if not found.</summary>
        public static void SetCombo(ComboBox cb, string? value)
        {
            if (string.IsNullOrEmpty(value)) { cb.SelectedIndex = 0; return; }
            int idx = cb.FindStringExact(value);
            cb.SelectedIndex = idx >= 0 ? idx : 0;
        }

        /// <summary>Returns null for blank/whitespace strings; otherwise returns the trimmed value.</summary>
        public static string? NullAkoJePrazno(string? s)
            => string.IsNullOrWhiteSpace(s) ? null : s.Trim();
    }
}