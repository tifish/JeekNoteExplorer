using BlueMystic;
using NHotkey;
using NHotkey.WindowsForms;
using System.Diagnostics;
using System.Globalization;

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

        RefreshAll();

        RootFolder.Changed += RootFolderOnChanged;
    }

    private void RootFolderOnChanged(object? sender, EventArgs e)
    {
        if (filterTextBox.Text != "")
            return;

        RefreshTree();
    }

    private void OpenJeekNote(object? sender, HotkeyEventArgs e)
    {
        Show();
        BringToFront();
        Activate();
        e.Handled = true;
    }

    private void RefreshAll()
    {
        if (Settings.NoteFolder == "" || !Directory.Exists(Settings.NoteFolder))
            return;

        RootFolder.Root.FullPath = Settings.NoteFolder;
        RootFolder.Refresh();
        RefreshTree();
    }

    private void RefreshTree()
    {
        var selectedPath = "";
        var selectedPathPassed = false;
        TreeNode? selectedNode = null;

        var expandedPaths = new HashSet<string>();
        SaveExpandedPaths(noteTreeView.Nodes, expandedPaths);

        noteTreeView.BeginUpdate();

        try
        {
            if (noteTreeView.SelectedNode != null)
                selectedPath = noteTreeView.SelectedNode.GetDocument().FullPath;
            noteTreeView.Nodes.Clear();

            AddFolderToNode(RootFolder.Root, noteTreeView.Nodes);

            if (filterTextBox.Text != "")
                noteTreeView.ExpandAll();
        }
        finally
        {
            RestoreExpandedPaths(noteTreeView.Nodes, expandedPaths);

            noteTreeView.EndUpdate();

            if (selectedNode != null)
            {
                noteTreeView.SelectedNode = selectedNode;
                noteTreeView.SelectedNode.EnsureVisible();
            }
        }

        return;

        bool AddFolderToNode(Folder folder, TreeNodeCollection nodes)
        {
            var hasAddedNode = false;

            foreach (var doc in folder.SubFolders.Concat(folder.Files))
            {
                if (selectedPath != "" && selectedPath == doc.FullPath)
                {
                    selectedPath = "";
                    selectedPathPassed = true;
                }

                TreeNode? subNode = null;
                var shouldAdd = false;

                if (doc.MatchFilter(filterTextBox.Text))
                {
                    subNode = new TreeNode();
                    subNode.SetDocument(doc);
                    shouldAdd = true;

                    if (selectedPathPassed)
                    {
                        selectedPathPassed = false;
                        selectedNode = subNode;
                    }
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

        Process.Start(new ProcessStartInfo(doc.FullPath)
        {
            UseShellExecute = true,
        });

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
                // Open file
                if (!OpenFileNode())
                    // Expand folder
                    OpenFolderNode();
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
                    if (prevNode.GetDocument().MatchFilter(filterTextBox.Text))
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
                    if (nextNode.GetDocument().MatchFilter(filterTextBox.Text))
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
                RefreshAll();
                e.Handled = true;
                break;

            // Ctrl+Alt+S to open settings
            case { KeyCode: Keys.S, Control: true, Alt: true, Shift: false }:
                settingsButton_Click(sender, e);
                e.Handled = true;
                break;
        }
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

    // Filter tree nodes
    private void filterTextBox_TextChanged(object sender, EventArgs e)
    {
        RefreshTree();
        filterTextBox.Visible = filterTextBox.Text != "";
    }

    private void settingsButton_Click(object sender, EventArgs e)
    {
        var settingsForm = new SettingsForm();
        settingsForm.ShowDialog();
        noteTreeView.Focus();
    }

    private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        e.Cancel = true;
        Hide();
    }

    private void MainForm_Activated(object sender, EventArgs e)
    {
        noteTreeView.Focus();
    }

    private void exitToolStripMenuItem_Click(object sender, EventArgs e)
    {
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
}
