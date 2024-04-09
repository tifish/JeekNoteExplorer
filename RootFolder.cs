namespace JeekNoteExplorer;

static class RootFolder
{
    public static Folder Folder { get; } = new();

    public static void Refresh()
    {
        RefreshFolder(Folder);
    }

    private static void RefreshFolder(Folder folder)
    {
        folder.SubFolders.Clear();
        foreach (var subDir in Directory.GetDirectories(folder.FullPath))
        {
            var dirName = Path.GetFileName(subDir);
            if (dirName.StartsWith('.')
                || dirName.EndsWith(".assets")
                || dirName.EndsWith("_files"))
                continue;

            var subFolder = new Folder
            {
                Name = dirName,
                FullPath = subDir,
                Parent = Folder,
            };
            folder.SubFolders.Add(subFolder);

            RefreshFolder(subFolder);
        }

        folder.Files.Clear();
        foreach (var file in Directory.GetFiles(folder.FullPath))
        {
            var document = new Document
            {
                Name = Path.GetFileName(file),
                FullPath = file,
                Parent = Folder,
            };
            folder.Files.Add(document);
        }
    }
}
