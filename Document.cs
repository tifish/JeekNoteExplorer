using System.Diagnostics;
using System.Text;
using Microsoft.VisualBasic.FileIO;
using NPinyin;

namespace JeekNoteExplorer;

class Document
{
    public string Name { get; set; } = "";

    private string _fullPath = "";

    public string FullPath
    {
        get => _fullPath;
        set
        {
            if (_fullPath == value)
                return;

            _fullPath = value;
            AssetsPath = "";
        }
    }

    private string _assetsPath = "";

    public string AssetsPath
    {
        get
        {
            if (_assetsPath == "")
                _assetsPath = Path.ChangeExtension(FullPath, ".assets");

            return _assetsPath;
        }
        private set => _assetsPath = value;
    }

    public Folder? Parent { get; set; }
    public virtual bool IsFile => true;
    public bool IsFolder => !IsFile;

    public Folder ToFolder()
    {
        return (Folder)this;
    }

    public bool MatchFilter(List<string> filters)
    {
        if (filters.Count == 0)
            return true;

        if (filters.All(filter => Name.Contains(filter, StringComparison.InvariantCultureIgnoreCase)))
            return true;

        var pinyinName = Pinyin.GetInitials(Name);
        return filters.All(filter => pinyinName.Contains(filter, StringComparison.InvariantCultureIgnoreCase));
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

    public bool OpenFile()
    {
        if (IsFile && File.Exists(FullPath))
        {
            Process.Start(new ProcessStartInfo(FullPath)
            {
                UseShellExecute = true,
            });
            return true;
        }

        return false;
    }

    public bool OpenFolder()
    {
        if (IsFolder && Directory.Exists(FullPath))
        {
            Process.Start(new ProcessStartInfo(FullPath)
            {
                UseShellExecute = true,
            });
            return true;
        }

        return false;
    }

    public bool DeleteInFileSystem()
    {
        try
        {
            if (IsFile)
            {
                FileSystem.DeleteFile(FullPath, UIOption.AllDialogs, RecycleOption.SendToRecycleBin);
                if (Directory.Exists(AssetsPath))
                    FileSystem.DeleteDirectory(AssetsPath, UIOption.AllDialogs, RecycleOption.SendToRecycleBin);
            }
            else
            {
                FileSystem.DeleteDirectory(FullPath, UIOption.AllDialogs, RecycleOption.SendToRecycleBin);
            }

            return true;
        }
        catch (Exception e)
        {
            Log.Error(e, "Delete file failed: {FullPath}", FullPath);
            return false;
        }
    }

    private static readonly Encoding UTF8WithBOM = new UTF8Encoding(true);

    public bool RenameInFileSystem(string newName)
    {
        try
        {
            if (IsFile)
            {
                if (!File.Exists(FullPath))
                    return false;

                if (Directory.Exists(AssetsPath))
                {
                    var newAssetsPath = Path.ChangeExtension(newName, ".assets");
                    FileSystem.RenameDirectory(AssetsPath, newAssetsPath);

                    var ext = Path.GetExtension(FullPath);
                    if (ext == ".md")
                    {
                        var oldAssetsName = Path.GetFileName(AssetsPath);
                        var newAssetsName = Path.GetFileName(newAssetsPath);

                        var content = File.ReadAllText(FullPath);
                        content = content.Replace(oldAssetsName, newAssetsName);
                        File.WriteAllText(FullPath, content, UTF8WithBOM);
                    }
                }

                FileSystem.RenameFile(FullPath, newName);
            }
            else
            {
                FileSystem.RenameDirectory(FullPath, newName);
            }

            return true;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to rename {Type} {OldName} to {NewName}",
                IsFile ? "file" : "directory", FullPath, newName);
            return false;
        }
    }
}
