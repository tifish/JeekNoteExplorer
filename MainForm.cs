using System.Globalization;
using System.Text.RegularExpressions;
using BlueMystic;
using NHotkey;
using NHotkey.WindowsForms;

namespace JeekNoteExplorer;

public partial class MainForm : Form
{
    private readonly DarkModeCS _darkMode;

    public MainForm()
    {
        InitializeComponent();

        _darkMode = new DarkModeCS(this);
    }

    private void MainForm_Load(object sender, EventArgs e)
    {
        // Register global hotkey to open JeekNoteExplorer
        HotkeyManager.Current.AddOrReplace("OpenJeekNote", Keys.Alt | Keys.Oemtilde, OpenJeekNote);

        RootFolder.Changed += RootFolderOnChanged;

        AppSettings.Load();
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
    private readonly HashSet<string> _expandedPaths = [];
    private bool _isExpandingAll;

    private void RefreshTree()
    {
        var selectedPathPassed = false;
        TreeNode? selectedNode = null;

        // Only save expanded paths when no filter => filtered
        if (!_isExpandingAll)
        {
            _expandedPaths.Clear();
            SaveExpandedPaths(noteTreeView.Nodes, _expandedPaths);
        }

        noteTreeView.BeginUpdate();

        try
        {
            // _selectedPathAfterRefresh can be specified to select a nearest node after refresh.
            // If it's not specified, use the current selected node.
            if (_selectedPathAfterRefresh == "" && noteTreeView.SelectedNode != null)
                _selectedPathAfterRefresh = noteTreeView.SelectedNode.GetDocument().FullPath;

            noteTreeView.Nodes.Clear();
            AddFolderToNode(RootFolder.Root, noteTreeView.Nodes);
        }
        finally
        {
            _isExpandingAll = filterTextBox.Text != "";
            if (_isExpandingAll)
                noteTreeView.ExpandAll();
            else // filtered => no filter
                RestoreExpandedPaths(noteTreeView.Nodes, _expandedPaths);

            noteTreeView.EndUpdate();

            // The nearest node to select after refresh
            if (selectedNode != null)
            {
                noteTreeView.SelectedNode = selectedNode;
                noteTreeView.SelectedNode.EnsureVisible();
                _selectedPathAfterRefresh = "";
            }
        }

        return;

        bool AddFolderToNode(Folder folder, TreeNodeCollection nodes)
        {
            var hasAddedNode = false;

            foreach (var doc in folder.SubFolders.Concat(folder.Files))
            {
                if (!selectedPathPassed && _selectedPathAfterRefresh != "" && _selectedPathAfterRefresh == doc.FullPath)
                    selectedPathPassed = true;

                TreeNode? subNode = null;
                var shouldAdd = false;

                if (doc.MatchFilter(_filters))
                {
                    subNode = new TreeNode();
                    subNode.SetDocument(doc);
                    shouldAdd = true;

                    // Select next visible node if current selected node is not visible
                    if (selectedPathPassed && selectedNode == null)
                        selectedNode = subNode;
                }

                if (doc.IsFolder)
                {
                    if (subNode == null)
                    {
                        subNode = new TreeNode();
                        subNode.SetDocument(doc);
                    }

                    if (AddFolderToNode(doc.ToFolder(), subNode.Nodes))
                        shouldAdd = true;
                }

                if (shouldAdd)
                {
                    nodes.Add(subNode!);
                    hasAddedNode = true;
                }
            }

            return hasAddedNode;
        }

        void SaveExpandedPaths(TreeNodeCollection nodes, HashSet<string> paths)
        {
            foreach (TreeNode node in nodes)
            {
                if (node.IsExpanded)
                    paths.Add(node.GetDocument().FullPath);
                if (node.Nodes.Count > 0)
                    SaveExpandedPaths(node.Nodes, paths);
            }
        }

        void RestoreExpandedPaths(TreeNodeCollection nodes, HashSet<string> paths)
        {
            foreach (TreeNode node in nodes)
            {
                if (node.Nodes.Count == 0)
                    continue;
                if (paths.Contains(node.GetDocument().FullPath))
                    node.Expand();
                RestoreExpandedPaths(node.Nodes, paths);
            }
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

        filterTextBox.Text = "";
        if (noteTreeView.SelectedNode == null)
            return true;

        noteTreeView.SelectedNode.EnsureVisible();
        noteTreeView.SelectedNode.Expand();

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
                if (filterTextBox.Text == "")
                    break;
                if (noteTreeView.SelectedNode == null)
                    break;

                e.Handled = true;

                var prevNode = noteTreeView.SelectedNode.PrevVisibleNode;
                while (prevNode != null)
                {
                    if (prevNode.GetDocument().MatchFilter(_filters))
                    {
                        noteTreeView.SelectedNode = prevNode;
                        break;
                    }

                    prevNode = prevNode.PrevVisibleNode;
                }

                break;

            // Up and Down to navigate matching nodes
            case { KeyCode: Keys.Down, Control: false, Alt: false, Shift: false }:
                if (filterTextBox.Text == "")
                    break;
                if (noteTreeView.SelectedNode == null)
                    break;

                e.Handled = true;

                var nextNode = noteTreeView.SelectedNode.NextVisibleNode;
                while (nextNode != null)
                {
                    if (nextNode.GetDocument().MatchFilter(_filters))
                    {
                        noteTreeView.SelectedNode = nextNode;
                        break;
                    }

                    nextNode = nextNode.NextVisibleNode;
                }

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
        }
    }

    private void OpenSelectedNode()
    {
        // Open file
        if (!OpenFileNode())
            // Expand folder
            OpenFolderNode();
    }

    private void CreateNewFile()
    {
        CreateNewFileOrDirectory(true);
    }

    private void CreateNewDirectory()
    {
        CreateNewFileOrDirectory(false);
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
                    Log.Error("Failed to create {Type} {File}, exception: {Exception}",
                        isFile ? "file" : "directory", newPath, ex.Message);
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

        var nextSelectedNode = noteTreeView.SelectedNode.NextVisibleNode ?? noteTreeView.SelectedNode.PrevVisibleNode;
        _selectedPathAfterRefresh = nextSelectedNode?.GetDocument().FullPath ?? "";

        noteTreeView.SelectedNode.GetDocument().DeleteInFileSystem();
    }

    // Type to filter
    private void noteTreeView_KeyPress(object sender, KeyPressEventArgs e)
    {
        if ((ModifierKeys & Keys.Control) != 0)
            return;
        if ((ModifierKeys & Keys.Alt) != 0)
            return;

        if (char.GetUnicodeCategory(e.KeyChar) is UnicodeCategory.Control or UnicodeCategory.OtherNotAssigned)
            return;

        filterTextBox.Text += e.KeyChar;
        e.Handled = true;
    }

    private List<Regex> _filters = new();

    // Filter tree nodes
    private void filterTextBox_TextChanged(object sender, EventArgs e)
    {
        var filterText = filterTextBox.Text.Trim();
        if (filterText == "")
            _filters.Clear();
        else
            _filters = filterText.Split(' ')
                .Where(filter => filter != "")
                .Select(filter => new Regex(filter, RegexOptions.IgnoreCase))
                .ToList();

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

            var newPath = Path.Combine(Path.GetDirectoryName(doc.FullPath)!, e.Label);
            if (File.Exists(newPath) || Directory.Exists(newPath))
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
        CreateNewFile();
    }

    private void newfolderToolStripMenuItem_Click(object sender, EventArgs e)
    {
        CreateNewDirectory();
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
}
