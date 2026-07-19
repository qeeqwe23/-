namespace WinFormsApp2
{
    internal sealed class ModuleGroupDefinition
    {
        public ModuleGroupDefinition(string title, string summary, string actionText, params ModuleDefinition[] modules)
        {
            Title = title;
            Summary = summary;
            ActionText = actionText;
            Modules = modules;
        }

        public string Title { get; }

        public string Summary { get; }

        public string ActionText { get; }

        public ModuleDefinition[] Modules { get; }
    }
}
