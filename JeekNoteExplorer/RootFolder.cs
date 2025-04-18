namespace JeekNoteExplorer;

static class RootFolder
{
    public static Folder Root { get; } = new();

    private static FileSystemWatcher? _watcher;

    private static void InitOnce()
    {
        if (_watcher != null)
            return;

        _watcher = new FileSystemWatcher();
        _watcher.NotifyFilter = NotifyFilters.Attributes
                                | NotifyFilters.FileName
                                | NotifyFilters.DirectoryName;
        _watcher.IncludeSubdirectories = true;
        _watcher.Created += OnFileCreated;
        _watcher.Deleted += OnFileDeleted;
        _watcher.Renamed += OnFileRenamed;

        Application.Idle += ApplicationOnIdle;
    }

    private static readonly List<Action> _pendingActions = [];

    private static void ApplicationOnIdle(object? sender, EventArgs e)
    {
        if (!Monitor.TryEnter(_pendingActions))
            return;

        try
        {
            if (_pendingActions.Count == 0)
                return;
            _pendingActions.ForEach(action => action());
            _pendingActions.Clear();
            Changed?.Invoke(null, EventArgs.Empty);
        }
        finally
        {
            Monitor.Exit(_pendingActions);
        }
    }

    public static void Refresh()
    {
        if (Directory.Exists(Root.FullPath))
        {
            RefreshFolder(Root);

            InitOnce();
            if (_watcher!.Path != Root.FullPath)
            {
                _watcher.Path = Root.FullPath;
                _watcher.EnableRaisingEvents = true;
            }
        }
        else
        {
            Root.SubFolders.Clear();
            Root.Files.Clear();
        }

        Changed?.Invoke(null, EventArgs.Empty);
    }

    public static event EventHandler? Changed;

    private static void OnFileCreated(object sender, FileSystemEventArgs e)
    {
        var docName = Path.GetFileName(e.FullPath);
        if (IsIgnored(docName))
            return;

        var isFile = File.Exists(e.FullPath);

        Monitor.Enter(_pendingActions);
        try
        {
            _pendingActions.Add(() =>
            {
                Log.Debug("File creating: {FullPath}", e.FullPath);

                var parentPath = Path.GetDirectoryName(e.FullPath);
                if (parentPath == null)
                    return;
                var parentDoc = FindPath(parentPath);
                if (parentDoc == null || parentDoc.IsFile)
                    return;
                var parentFolder = parentDoc.ToFolder();

                if (isFile)
                {
                    var document = new Document
                    {
                        Name = docName,
                        FullPath = e.FullPath,
                        Parent = parentFolder,
                    };
                    parentFolder.Files.Add(document);
                    parentFolder.SortFiles();
                }
                else
                {
                    var subFolder = new Folder
                    {
                        Name = docName,
                        FullPath = e.FullPath,
                        Parent = parentFolder,
                    };

                    parentFolder.SubFolders.Add(subFolder);
                    parentFolder.SortSubFolders();
                }
            });
        }
        finally
        {
            Monitor.Exit(_pendingActions);
        }
    }

    private static void OnFileDeleted(object sender, FileSystemEventArgs e)
    {
        var docName = Path.GetFileName(e.FullPath);
        if (IsIgnored(docName))
            return;

        Monitor.Enter(_pendingActions);
        try
        {
            _pendingActions.Add(() =>
            {
                Log.Debug("File deleting: {FullPath}", e.FullPath);

                var doc = FindPath(e.FullPath);
                doc?.Delete();
            });
        }
        finally
        {
            Monitor.Exit(_pendingActions);
        }
    }

    private static void OnFileRenamed(object sender, RenamedEventArgs e)
    {
        var docName = Path.GetFileName(e.FullPath);
        if (IsIgnored(docName))
            return;

        Monitor.Enter(_pendingActions);
        try
        {
            _pendingActions.Add(() =>
            {
                Log.Debug("File renaming: {OldFullPath} -> {FullPath}", e.OldFullPath, e.FullPath);

                var doc = FindPath(e.OldFullPath);
                if (doc == null)
                    return;

                doc.Name = docName;
                doc.FullPath = e.FullPath;
                if (doc.IsFile)
                    doc.Parent?.SortFiles();
                else
                    doc.Parent?.SortSubFolders();
            });
        }
        finally
        {
            Monitor.Exit(_pendingActions);
        }
    }

    public static Document? FindPath(string fullPath)
    {
        if (fullPath == "")
            return null;

        var relativePath = Path.GetRelativePath(Root.FullPath, fullPath);
        if (relativePath == ".")
            return Root;
        var pathItems = relativePath.Split(Path.DirectorySeparatorChar);

        var currentFolder = Root;
        for (var i = 0; i < pathItems.Length - 1; i++)
        {
            var pathItem = pathItems[i];
            var subFolder = currentFolder.SubFolders.Find(subFolder => subFolder.Name == pathItem);
            if (subFolder == null)
                return null;

            currentFolder = subFolder;
        }

        return currentFolder.Files.Find(file => file.Name == pathItems.Last())
               ?? currentFolder.SubFolders.Find(subFolder => subFolder.Name == pathItems.Last());
    }

    private static void RefreshFolder(Folder folder)
    {
        folder.SubFolders.Clear();
        var subDirs = Directory.GetDirectories(folder.FullPath);
        foreach (var subDir in subDirs)
        {
            var dirName = Path.GetFileName(subDir);
            if (IsIgnored(dirName))
                continue;

            var subFolder = new Folder
            {
                Name = dirName,
                FullPath = subDir,
                Parent = folder,
            };
            folder.SubFolders.Add(subFolder);

            RefreshFolder(subFolder);
        }

        folder.SortSubFolders();

        folder.Files.Clear();
        var files = Directory.GetFiles(folder.FullPath);
        foreach (var file in files)
        {
            var fileName = Path.GetFileName(file);
            if (IsIgnored(fileName))
                continue;

            var document = new Document
            {
                Name = fileName,
                FullPath = file,
                Parent = folder,
            };
            folder.Files.Add(document);
        }

        folder.SortFiles();
    }

    private static bool IsIgnored(string dirName)
    {
        return dirName.StartsWith('.')
               || dirName.EndsWith(".assets")
               || dirName.EndsWith("_files");
    }
}
