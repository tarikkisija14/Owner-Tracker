namespace OwnerTrack.App.Helpers
{
  
    public static class FormHelper
    {
        
        public static void PopuniEnumCombo<TEnum>(ComboBox cb) where TEnum : struct, Enum
        {
            cb.Items.Clear();
            foreach (TEnum v in Enum.GetValues(typeof(TEnum)))
                cb.Items.Add(v.ToString());
        }

      
        public static void PopuniEnumComboSPraznim<TEnum>(ComboBox cb) where TEnum : struct, Enum
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

        
        public static string? NullAkoJePrazno(string? s)
            => string.IsNullOrWhiteSpace(s) ? null : s.Trim();
    }
}