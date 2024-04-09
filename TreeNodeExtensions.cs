namespace JeekNoteExplorer;

static class TreeNodeExtensions
{
    public static bool IsInExpandedNode(this TreeNode node)
    {
        var parent = node.Parent;
        while (parent != null)
        {
            if (!parent.IsExpanded)
                return false;
            parent = parent.Parent;
        }

        return true;
    }

    public static void SetDocument(this TreeNode node, Document document)
    {
        node.Tag = document;
        node.Text = document.Name;
    }

    public static bool IsFile(this TreeNode node)
    {
        return ((Document)node.Tag).IsFile;
    }

    public static bool IsFolder(this TreeNode node)
    {
        return ((Document)node.Tag).IsFolder;
    }

    public static Document GetDocument(this TreeNode node)
    {
        return (Document)node.Tag;
    }

    public static Folder GetFolder(this TreeNode node)
    {
        return (Folder)node.Tag;
    }
}
