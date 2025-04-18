using System.Globalization;

namespace JeekNoteExplorer;

class Folder : Document
{
    public override bool IsFile => false;
    public List<Folder> SubFolders { get; } = [];
    public List<Document> Files { get; } = [];

    public void SortFiles()
    {
        Files.Sort(DocumentComparer);
    }

    public void SortSubFolders()
    {
        SubFolders.Sort(DocumentComparer);
    }

    // English first, then the current culture language
    private static readonly Comparer<string> StringComparer = Comparer<string>.Create((x, y) =>
    {
        if ((IsEnglish(x) && IsEnglish(y)) || (!IsEnglish(x) && !IsEnglish(y)))
            return string.Compare(x, y, CultureInfo.CurrentCulture, CompareOptions.StringSort);

        // x first
        if (IsEnglish(x) && !IsEnglish(y))
            return -1;

        // y first
        return 1;

        bool IsEnglish(string s)
        {
            return s.All(c => c <= 'z');
        }
    });

    private static readonly Comparer<Document> DocumentComparer = Comparer<Document>.Create((x, y) =>
        StringComparer.Compare(x.Name, y.Name));
}
