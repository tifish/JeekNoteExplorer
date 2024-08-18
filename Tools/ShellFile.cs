public static class ShellFile
{
    public static void Copy(params string[] paths)
    {
        IDataObject data = new DataObject(DataFormats.FileDrop, paths);
        data.SetData("Preferred DropEffect", new MemoryStream(BitConverter.GetBytes(5)));
        Clipboard.SetDataObject(data);
    }

    public static void Cut(params string[] paths)
    {
        IDataObject data = new DataObject(DataFormats.FileDrop, paths);
        data.SetData("Preferred DropEffect", new MemoryStream(BitConverter.GetBytes(2)));
        Clipboard.SetDataObject(data);
    }

    public static void Paste(string destinationPath)
    {
        var data = Clipboard.GetDataObject();
        if (data == null)
            return;
        if (!data.GetDataPresent(DataFormats.FileDrop))
            return;

        var files = (string[]?)data.GetData(DataFormats.FileDrop);
        if (files == null)
            return;

        var stream = (MemoryStream?)data.GetData("Preferred DropEffect", true);
        if (stream == null)
            return;

        var flag = stream.ReadByte();
        if (flag != 2 && flag != 5)
            return;

        var cut = flag == 2;
        foreach (var file in files)
        {
            var dest = Path.Combine(destinationPath, Path.GetFileName(file));
            try
            {
                if (cut)
                    File.Move(file, dest);
                else
                    File.Copy(file, dest, false);
            }
            catch (IOException ex)
            {
                MessageBox.Show(@"Failed to perform the specified operation:\n\n" + ex.Message,
                    @"File operation failed",
                    MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }
    }
}
