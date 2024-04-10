namespace JeekNoteExplorer;

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

    public void Delete()
    {
        if (Parent == null)
            return;

        if (IsFile)
            Parent.Files.Remove(this);
        else
            Parent.SubFolders.Remove(ToFolder());

        Parent = null;
    }
}
