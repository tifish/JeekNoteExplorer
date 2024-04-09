namespace JeekNoteExplorer
{
    class Folder : Document
    {
        public override bool IsFile => false;
        public List<Folder> SubFolders { get; } = [];
        public List<Document> Files { get; } = [];
    }
}
