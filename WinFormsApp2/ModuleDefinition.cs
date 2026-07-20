namespace WinFormsApp2
{
    internal sealed class ModuleDefinition
    {
        public ModuleDefinition(string name, string description, params string[] fields)
        {
            Name = name;
            Description = description;
            Fields = fields;
        }

        public string Name { get; }

        public string Description { get; }

        public string[] Fields { get; }

        public string EditTitle { get; private set; } = "信息填写";

        public string ListTitle { get; private set; } = "查询结果";

        public string? QuerySql { get; set; }

        public string? TableName { get; private set; }

        public string KeyColumn { get; private set; } = "Id";

        public ModuleFieldDefinition[] FieldDefinitions { get; private set; } = Array.Empty<ModuleFieldDefinition>();

        public bool IsReadOnly { get; private set; }

        public bool CanWrite => !IsReadOnly && !string.IsNullOrWhiteSpace(TableName) && FieldDefinitions.Length > 0;

        public ModuleDefinition BindTable(string tableName, params ModuleFieldDefinition[] fieldDefinitions)
        {
            TableName = tableName;
            FieldDefinitions = fieldDefinitions;
            return this;
        }

        public ModuleDefinition AsReadOnly()
        {
            IsReadOnly = true;
            return this;
        }

        public ModuleDefinition WithTitles(string editTitle, string listTitle)
        {
            EditTitle = editTitle;
            ListTitle = listTitle;
            return this;
        }

        public ModuleFieldDefinition? GetField(string displayName)
        {
            return FieldDefinitions.FirstOrDefault(field => field.DisplayName == displayName);
        }
    }
}
