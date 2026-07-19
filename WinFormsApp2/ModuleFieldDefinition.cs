namespace WinFormsApp2
{
    internal sealed class ModuleFieldDefinition
    {
        public ModuleFieldDefinition(string displayName, string columnName, bool readOnly = false)
        {
            DisplayName = displayName;
            ColumnName = columnName;
            ReadOnly = readOnly;
        }

        public string DisplayName { get; }

        public string ColumnName { get; }

        public bool ReadOnly { get; }
    }
}
