using System.Globalization;
using JeekTools;
using Microsoft.Extensions.Logging;
using NHotkey;
using NHotkey.WindowsForms;
using ZLogger;

namespace JeekNoteExplorer;

partial class MainForm : Form
{
    private static readonly ILogger Log = LogManager.CreateLogger(nameof(MainForm));

    public static MainForm? Instance { get; private set; }

    public MainForm()
    {
        Instance = this;

        InitializeComponent();
    }

    private void MainForm_Load(object sender, EventArgs e)
    {
        RegisterWakeUpHotkey(Hotkey.Parse(Settings.WakeUpKey));

        RootFolder.Changed += RootFolderOnChanged;

        AppSettings.Load();
    }

    public void RegisterWakeUpHotkey(Hotkey hotkey)
    {
        try
        {
            HotkeyManager.Current.AddOrReplace("OpenJeekNote", hotkey.ToKeys(), OpenJeekNote);
        }
        catch (HotkeyAlreadyRegisteredException ex)
        {
            Log.ZLogError(ex, $"Hotkey {hotkey} is already registered");
            MessageBox.Show($"Hotkey {hotkey} is already registered");
        }
        catch (Exception ex)
        {
            Log.ZLogError(ex, $"Failed to register hotkey {hotkey}");
            MessageBox.Show($"Failed to register hotkey {hotkey}");
        }
    }

    private void RootFolderOnChanged(object? sender, EventArgs e)
    {
        RefreshTree();
    }

    private void OpenJeekNote(object? sender, HotkeyEventArgs e)
    {
        Show();
        if (WindowState == FormWindowState.Minimized)
            WindowState = FormWindowState.Normal;
        Activate();
        e.Handled = true;
    }

    private string _selectedPathAfterRefresh = "";
    private string _selectedPathBeforeFilter = "";
    private List<Document> _filteredDocuments = [];
    private Dictionary<int, int> _filteredDocumentsIdIndexMap = [];

    /// <summary>
    ///     There are 3 modes of refreshing the tree:
    ///     - No filter: filter text is empty, show selected file/folder and collapse all the other nodes.
    ///     - Filter all: search filter text in all files and expand all nodes.
    ///     - Filter current: search filter text in the same folder of selected file/folder, and collapse all the other nodes.
    /// </summary>
    private void RefreshTree()
    {
        var selectedPathPassed = false;
        var filteredNodeIdBeforeSelectedNode = -1;
        var filteredNodeIdAfterSelectedNode = -1;

        // Only save expanded paths when no filter => has filter
        var hasFilter = filterTextBox.Text != "";
        var filterAll = filterAllCheckBox.Checked;

        noteTreeView.BeginUpdate();

        try
        {
            // Save selected path before filter, which will be used in filter current mode
            if (filterTextBox.Text.Length == 1 && _selectedPathBeforeFilter == "")
            {
                _selectedPathBeforeFilter = noteTreeView.SelectedNode?.GetDocument().FullPath ?? "";
            }
            else if (filterTextBox.Text.Length == 0)
            {
                _selectedPathBeforeFilter = "";
                _filteredDocuments.Clear();
                _filteredDocumentsIdIndexMap.Clear();
            }

            // _selectedPathAfterRefresh can be specified to select the nearest node after refresh.
            // If it's not specified, use the current selected node.
            if (_selectedPathAfterRefresh == "" && noteTreeView.SelectedNode != null)
                _selectedPathAfterRefresh = noteTreeView.SelectedNode.GetDocument().FullPath;

            _filteredDocuments.Clear();
            _filteredDocumentsIdIndexMap.Clear();

            // No filter or filter all
            if (!hasFilter || filterAll)
            {
                if (AddFolderDocuments(RootFolder.Root, _filteredDocuments, true))
                {
                    for (var i = 0; i < _filteredDocuments.Count; i++)
                        _filteredDocumentsIdIndexMap[_filteredDocuments[i].Id] = i;
                }
            }
            else // filter current
                FilterCurrentFolder(_filteredDocuments);

            // Load documents into noteTreeView
            int getId(Document x) => x.Id;
            int? getParentId(Document x) => x.Parent?.Id > 0 ? x.Parent.Id : null;
            string getDisplayName(Document x) => x.Name;
            noteTreeView.LoadItems(_filteredDocuments, getId, getParentId, getDisplayName);
        }
        finally
        {
            noteTreeView.EndUpdate();

            // Select node after refresh
            if (filteredNodeIdAfterSelectedNode != -1)
                noteTreeView.SelectedNode = noteTreeView.GetNode(filteredNodeIdAfterSelectedNode);
            else if (filteredNodeIdBeforeSelectedNode != -1)
                noteTreeView.SelectedNode = noteTreeView.GetNode(filteredNodeIdBeforeSelectedNode);
            else if (noteTreeView.Nodes.Count > 0)
                noteTreeView.SelectedNode = noteTreeView.Nodes[0];

            noteTreeView.SelectedNode?.EnsureVisible();
            _selectedPathAfterRefresh = "";
        }

        return;

        bool AddFolderDocuments(Folder folder, List<Document> documents, bool recursive)
        {
            var hasAddedDoc = false;

            foreach (var doc in folder.SubFolders.Concat(folder.Files))
            {
                if (
                    !selectedPathPassed
                    && _selectedPathAfterRefresh != ""
                    && _selectedPathAfterRefresh == doc.FullPath
                )
                    selectedPathPassed = true;

                var filterMatched = false;

                if (doc.MatchFilter(_filters))
                {
                    documents.Add(doc);
                    filterMatched = true;
                    hasAddedDoc = true;

                    // Select next node if current selected node doesn't match the filter
                    if (selectedPathPassed)
                    {
                        if (filteredNodeIdAfterSelectedNode == -1)
                            filteredNodeIdAfterSelectedNode = doc.Id;
                    }
                    else
                    {
                        filteredNodeIdBeforeSelectedNode = doc.Id;
                    }
                }

                if (doc.IsFolder && recursive)
                {
                    if (!filterMatched)
                        documents.Add(doc);

                    if (AddFolderDocuments(doc.ToFolder(), documents, true))
                        hasAddedDoc = true;
                    else if (!filterMatched)
                        documents.RemoveAt(documents.Count - 1);
                }
            }

            return hasAddedDoc;
        }

        void FilterCurrentFolder(List<Document> documents)
        {
            var doc = RootFolder.FindPath(_selectedPathBeforeFilter);

            // If the selected path is not found, select the first folder/file in the root folder
            if (doc == null)
            {
                if (RootFolder.Root.SubFolders.Count > 0)
                    doc = RootFolder.Root.SubFolders[0];
                else if (RootFolder.Root.Files.Count > 0)
                    doc = RootFolder.Root.Files[0];
                else
                    return;

                _selectedPathBeforeFilter = doc.FullPath;
            }

            // Get all parent folders from top to bottom
            var parentFolders = new List<Folder>();
            var parent = doc.Parent;
            while (parent != null && parent != RootFolder.Root)
            {
                parentFolders.Add(parent);
                parent = parent.Parent;
            }

            parentFolders.Reverse();

            // Add all parent folders to the documents list
            documents.AddRange(parentFolders);

            // Add all documents in the current folder with filter
            AddFolderDocuments(doc.Parent!, documents, false);
        }
    }

    private void noteTreeView_DoubleClick(object sender, EventArgs e)
    {
        OpenFileNode();
    }

    private bool OpenFileNode()
    {
        if (noteTreeView.SelectedNode == null)
            return false;

        var doc = noteTreeView.SelectedNode.GetDocument();
        if (!doc.IsFile)
            return false;

        doc.OpenFile();

        return true;
    }

    private bool OpenFolderNode()
    {
        if (noteTreeView.SelectedNode == null)
            return false;

        if (!noteTreeView.SelectedNode.IsFolder())
            return false;

        _selectedPathAfterRefresh = noteTreeView.SelectedNode.GetDocument().FullPath;

        filterTextBox.Text = "";
        if (noteTreeView.SelectedNode == null)
            return true;

        noteTreeView.SelectedNode.EnsureVisible();
        noteTreeView.SelectedNode.Expand();

        // Select the first node after expanding. Filter current mode requires this.
        if (noteTreeView.SelectedNode.Nodes.Count > 0)
            noteTreeView.SelectedNode = noteTreeView.SelectedNode.Nodes[0];

        return true;
    }

    private void noteTreeView_KeyDown(object sender, KeyEventArgs e)
    {
        switch (e)
        {
            case { KeyCode: Keys.Enter, Control: false, Alt: false, Shift: false }:
                e.Handled = true;
                OpenSelectedNode();
                break;

            // Backspace to delete the last filter text character
            case { KeyCode: Keys.Back, Control: false, Alt: false, Shift: false }:
                if (filterTextBox.Text.Length > 0)
                {
                    filterTextBox.Text = filterTextBox.Text[..^1];
                    e.Handled = true;
                }

                break;

            // Esc to clear the filter text or hide the form
            case { KeyCode: Keys.Escape, Control: false, Alt: false, Shift: false }:
                if (filterTextBox.Text != "")
                    filterTextBox.Text = "";
                else
                    Hide();
                e.Handled = true;

                break;

            // Up and Down to navigate matching nodes
            case { KeyCode: Keys.Up, Control: false, Alt: false, Shift: false }:
                if (NoteTreeViewUp())
                    e.Handled = true;
                break;

            // Up and Down to navigate matching nodes
            case { KeyCode: Keys.Down, Control: false, Alt: false, Shift: false }:
                if (NoteTreeViewDown())
                    e.Handled = true;
                break;

            // PageUp and PageDown to navigate matching nodes
            case { KeyCode: Keys.PageUp, Control: false, Alt: false, Shift: false }:
                if (NoteTreeViewPageUp())
                    e.Handled = true;
                break;

            // PageUp and PageDown to navigate matching nodes
            case { KeyCode: Keys.PageDown, Control: false, Alt: false, Shift: false }:
                if (NoteTreeViewPageDown())
                    e.Handled = true;
                break;

            // Ctrl+R or F5 to refresh the tree
            case { KeyCode: Keys.R, Control: true, Alt: false, Shift: false }:
            case { KeyCode: Keys.F5, Control: false, Alt: false, Shift: false }:
                RootFolder.Refresh();
                e.Handled = true;
                break;

            // Ctrl+Alt+S to open settings
            case { KeyCode: Keys.S, Control: true, Alt: true, Shift: false }:
                settingsButton_Click(sender, e);
                e.Handled = true;
                break;

            // Alt+F to switch filter mode
            case { KeyCode: Keys.F, Control: false, Alt: true, Shift: false }:
                filterAllCheckBox.Checked = !filterAllCheckBox.Checked;
                e.Handled = true;
                break;

            // Delete to delete the selected node
            case { KeyCode: Keys.Delete, Control: false, Alt: false, Shift: false }:
                if (noteTreeView.SelectedNode == null)
                    break;
                if (noteTreeView.SelectedNode.IsEditing)
                    break;

                DeleteSelectedNode();
                e.Handled = true;
                break;

            // Ctrl+C to copy the selected file or folder
            case { KeyCode: Keys.C, Control: true, Alt: false, Shift: false }:
                e.Handled = CopySelectedNode();
                break;

            // Ctrl+X to cut the selected file or folder
            case { KeyCode: Keys.X, Control: true, Alt: false, Shift: false }:
                e.Handled = CutSelectedNode();
                break;

            // Ctrl+V to paste the copied file or folder
            case { KeyCode: Keys.V, Control: true, Alt: false, Shift: false }:
                e.Handled = PasteSelectedNode();
                break;
        }
    }

    private bool NoteTreeViewUp()
    {
        if (filterTextBox.Text == "")
            return false;
        if (noteTreeView.SelectedNode == null)
            return false;
        if (!filterAllCheckBox.Checked)
            return false;

        var selectedDoc = noteTreeView.SelectedNode.GetDocument();
        var index = _filteredDocumentsIdIndexMap[selectedDoc.Id] - 1;

        while (index >= 0)
        {
            var doc = _filteredDocuments[index];
            if (doc.MatchFilter(_filters))
            {
                noteTreeView.SelectedNode = noteTreeView.GetNode(doc.Id);
                noteTreeView.SelectedNode.EnsureVisible();
                break;
            }

            index--;
        }

        return true;
    }

    private bool NoteTreeViewDown()
    {
        if (filterTextBox.Text == "")
            return false;
        if (noteTreeView.SelectedNode == null)
            return false;
        if (!filterAllCheckBox.Checked)
            return false;

        var selectedDoc = noteTreeView.SelectedNode.GetDocument();
        var index = _filteredDocumentsIdIndexMap[selectedDoc.Id] + 1;

        while (index < _filteredDocuments.Count)
        {
            var doc = _filteredDocuments[index];
            if (doc.MatchFilter(_filters))
            {
                noteTreeView.SelectedNode = noteTreeView.GetNode(doc.Id);
                noteTreeView.SelectedNode.EnsureVisible();
                break;
            }

            index++;
        }

        return true;
    }

    private bool NoteTreeViewPageUp()
    {
        if (filterTextBox.Text == "")
            return false;
        if (noteTreeView.SelectedNode == null)
            return false;
        if (!filterAllCheckBox.Checked)
            return false;

        var selectedDoc = noteTreeView.SelectedNode.GetDocument();
        // Get how many notes show in the current page
        var pageSize = noteTreeView.ClientSize.Height / noteTreeView.ItemHeight;
        var index = _filteredDocumentsIdIndexMap[selectedDoc.Id] - 1;
        if (index < 0)
            return true;

        TreeNode? node = null;
        for (int i = 0; i < pageSize; i++)
        {
            var doc = _filteredDocuments[index];
            node = noteTreeView.GetNode(doc.Id);
            node.EnsureVisible();

            index--;
            if (index < 0)
                break;
        }

        if (node != null)
            noteTreeView.SelectedNode = node;

        return true;
    }

    private bool NoteTreeViewPageDown()
    {
        if (filterTextBox.Text == "")
            return false;
        if (noteTreeView.SelectedNode == null)
            return false;
        if (!filterAllCheckBox.Checked)
            return false;

        var selectedDoc = noteTreeView.SelectedNode.GetDocument();
        // Get how many notes show in the current page
        var pageSize = noteTreeView.ClientSize.Height / noteTreeView.ItemHeight;
        var index = _filteredDocumentsIdIndexMap[selectedDoc.Id] + 1;
        if (index >= _filteredDocuments.Count)
            return true;

        TreeNode? node = null;
        for (int i = 0; i < pageSize; i++)
        {
            var doc = _filteredDocuments[index];
            node = noteTreeView.GetNode(doc.Id);
            node.EnsureVisible();

            index++;
            if (index >= _filteredDocuments.Count)
                break;
        }

        if (node != null)
            noteTreeView.SelectedNode = node;

        return true;
    }

    private void OpenSelectedNode()
    {
        // Open file
        if (!OpenFileNode())
            // Expand folder
            OpenFolderNode();
    }

    private bool _isCreating;

    private void CreateNewFileOrDirectory(bool isFile)
    {
        if (noteTreeView.SelectedNode == null)
            return;
        if (noteTreeView.SelectedNode.IsEditing)
            return;

        var selectedNode = noteTreeView.SelectedNode;
        var parentNode = selectedNode.IsFolder() ? selectedNode : selectedNode.Parent;
        var parentPath = parentNode?.GetDocument().FullPath ?? RootFolder.Root.FullPath;
        var parentNodes = parentNode == null ? noteTreeView.Nodes : parentNode.Nodes;
        var newNode = new TreeNode();
        parentNodes.Add(newNode);
        selectedNode.Expand();
        noteTreeView.SelectedNode = newNode;

        _isCreating = true;
        noteTreeView.AfterLabelEdit += OnNoteTreeViewOnAfterLabelEdit;
        newNode.BeginEdit();

        return;

        void OnNoteTreeViewOnAfterLabelEdit(object? sender, NodeLabelEditEventArgs e)
        {
            noteTreeView.AfterLabelEdit -= OnNoteTreeViewOnAfterLabelEdit;

            try
            {
                if (e.Label == null || e.Node == null)
                    return;

                var newPath = Path.Combine(parentPath, e.Label);
                if (File.Exists(newPath) || Directory.Exists(newPath))
                    return;

                try
                {
                    if (isFile)
                    {
                        var ext = Path.GetExtension(newPath);
                        var newFileTemplate = Path.Combine(AppSettings.NewFilesDirectory, ext);
                        if (File.Exists(newFileTemplate))
                            File.Copy(newFileTemplate, newPath);
                        else
                            File.WriteAllText(newPath, "");
                    }
                    else
                    {
                        Directory.CreateDirectory(newPath);
                    }
                }
                catch (Exception ex)
                {
                    Log.ZLogError(
                        ex,
                        $"Failed to create {(isFile ? "file" : "directory")} {newPath}"
                    );
                    return;
                }

                _selectedPathAfterRefresh = newPath;
            }
            finally
            {
                parentNodes.Remove(newNode);

                _isCreating = false;

                noteTreeView.SelectedNode?.EnsureVisible();
            }
        }
    }

    private void RenameSelectedNode()
    {
        if (noteTreeView.SelectedNode == null)
            return;
        if (noteTreeView.SelectedNode.IsEditing)
            return;

        noteTreeView.SelectedNode.BeginEdit();
    }

    private void DeleteSelectedNode()
    {
        if (noteTreeView.SelectedNode == null)
            return;
        if (noteTreeView.SelectedNode.IsEditing)
            return;

        var nextSelectedNode =
            noteTreeView.SelectedNode.NextVisibleNode ?? noteTreeView.SelectedNode.PrevVisibleNode;
        _selectedPathAfterRefresh = nextSelectedNode?.GetDocument().FullPath ?? "";

        noteTreeView.SelectedNode.GetDocument().DeleteInFileSystem();
    }

    private bool CopySelectedNode()
    {
        if (noteTreeView.SelectedNode == null)
            return false;
        if (noteTreeView.SelectedNode.IsEditing)
            return false;

        noteTreeView.SelectedNode.GetDocument().CopyToClipboard();
        return true;
    }

    private bool CutSelectedNode()
    {
        if (noteTreeView.SelectedNode == null)
            return false;
        if (noteTreeView.SelectedNode.IsEditing)
            return false;

        noteTreeView.SelectedNode.GetDocument().CutToClipboard();
        return true;
    }

    private bool PasteSelectedNode()
    {
        if (noteTreeView.SelectedNode == null)
            return false;
        if (noteTreeView.SelectedNode.IsEditing)
            return false;

        noteTreeView.SelectedNode.GetDocument().PasteFromClipboard();
        return true;
    }

    // Type to filter
    private void noteTreeView_KeyPress(object sender, KeyPressEventArgs e)
    {
        if ((ModifierKeys & Keys.Control) != 0)
            return;
        if ((ModifierKeys & Keys.Alt) != 0)
            return;

        if (
            char.GetUnicodeCategory(e.KeyChar)
            is UnicodeCategory.Control
                or UnicodeCategory.OtherNotAssigned
        )
            return;

        filterTextBox.Text += e.KeyChar;
        e.Handled = true;
    }

    private List<string> _filters = [];

    // Filter tree nodes
    private void filterTextBox_TextChanged(object sender, EventArgs e)
    {
        var filterText = filterTextBox.Text.Trim();
        if (filterText == "")
            _filters.Clear();
        else
            _filters = filterText.Split(' ').ToList();

        RefreshTree();

        filterTextBox.Visible = filterText != "";
    }

    private void settingsButton_Click(object sender, EventArgs e)
    {
        var settingsForm = new SettingsForm();
        settingsForm.ShowDialog();
    }

    private bool _isExiting;

    private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        if (_isExiting)
            return;

        e.Cancel = true;
        Hide();
    }

    private void MainForm_Activated(object sender, EventArgs e)
    {
        noteTreeView.Focus();
    }

    private void noteTreeView_Leave(object sender, EventArgs e)
    {
        noteTreeView.Focus();
    }

    private void exitToolStripMenuItem_Click(object sender, EventArgs e)
    {
        _isExiting = true;
        Application.Exit();
    }

    private void notifyIcon_MouseClick(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
        {
            if (Visible)
                Hide();
            else
                Show();
        }
    }

    private void noteTreeView_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
    {
        if (_isCreating)
            return;

        // Is editing
        e.CancelEdit = true;

        try
        {
            if (e.Label == null || e.Node == null)
                return;

            var doc = e.Node.GetDocument();
            if (doc.Name == e.Label)
                return;

            if (doc.RenameInFileSystem(e.Label))
                e.CancelEdit = false;
        }
        finally
        {
            noteTreeView.SelectedNode?.EnsureVisible();
        }
    }

    private void openToolStripMenuItem_Click(object sender, EventArgs e)
    {
        OpenSelectedNode();
    }

    private void newFileToolStripMenuItem_Click(object sender, EventArgs e)
    {
        CreateNewFileOrDirectory(true);
    }

    private void newFolderToolStripMenuItem_Click(object sender, EventArgs e)
    {
        CreateNewFileOrDirectory(false);
    }

    private void renameToolStripMenuItem_Click(object sender, EventArgs e)
    {
        RenameSelectedNode();
    }

    private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
    {
        DeleteSelectedNode();
    }

    private void noteTreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
    {
        if (e.Button == MouseButtons.Right)
            noteTreeView.SelectedNode = e.Node;
    }

    private void renameButton_Click(object sender, EventArgs e)
    {
        RenameSelectedNode();
    }

    private void deleteButton_Click(object sender, EventArgs e)
    {
        DeleteSelectedNode();
    }

    private void filterAllCheckBox_CheckedChanged(object sender, EventArgs e)
    {
        if (filterTextBox.Text != "")
            RefreshTree();
    }

    private void copyToolStripMenuItem_Click(object sender, EventArgs e)
    {
        CopySelectedNode();
    }

    private void cutToolStripMenuItem_Click(object sender, EventArgs e)
    {
        CutSelectedNode();
    }

    private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
    {
        PasteSelectedNode();
    }

    private void exploreToToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (noteTreeView.SelectedNode == null)
            return;

        var doc = noteTreeView.SelectedNode.GetDocument();
        ShellFile.Explore(doc.FullPath);
    }
}
