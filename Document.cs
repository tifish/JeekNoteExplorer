using System.Text.RegularExpressions;

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

    public bool MatchFilter(List<Regex> filters)
    {
        if (filters.Count == 0)
            return true;

        return filters.All(filter => filter.IsMatch(Name));
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
