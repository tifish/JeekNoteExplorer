namespace JeekNoteExplorer
{
    class Document
    {
        public string Name { get; set; } = "";
        public string FullPath { get; set; } = "";
        public Folder? Parent { get; set; }
        public virtual bool IsFile => true;
        public bool IsFolder => !IsFile;

        public Folder ToFolder()
        {
            return (Folder)this;
        }

        public bool MatchFilter(string filter)
        {
            return filter == "" || Name.Contains(filter, StringComparison.OrdinalIgnoreCase);
        }
    }
}
