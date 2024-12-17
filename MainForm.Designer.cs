namespace JeekNoteExplorer
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            noteTreeView = new System.Windows.Forms.TreeView();
            noteTreeViewContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(components);
            openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            newFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            newFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            renameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolsFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            renameButton = new System.Windows.Forms.Button();
            deleteButton = new System.Windows.Forms.Button();
            settingsButton = new System.Windows.Forms.Button();
            filterAllCheckBox = new System.Windows.Forms.CheckBox();
            filterTextBox = new System.Windows.Forms.TextBox();
            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            notifyIcon = new System.Windows.Forms.NotifyIcon(components);
            notifyIconContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(components);
            exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            noteTreeViewContextMenuStrip.SuspendLayout();
            toolsFlowLayoutPanel.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            notifyIconContextMenuStrip.SuspendLayout();
            SuspendLayout();
            //
            // noteTreeView
            //
            noteTreeView.ContextMenuStrip = noteTreeViewContextMenuStrip;
            noteTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            noteTreeView.LabelEdit = true;
            noteTreeView.Location = new System.Drawing.Point(6, 61);
            noteTreeView.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            noteTreeView.Name = "noteTreeView";
            noteTreeView.Size = new System.Drawing.Size(1601, 893);
            noteTreeView.TabIndex = 0;
            noteTreeView.AfterLabelEdit += noteTreeView_AfterLabelEdit;
            noteTreeView.NodeMouseClick += noteTreeView_NodeMouseClick;
            noteTreeView.DoubleClick += noteTreeView_DoubleClick;
            noteTreeView.KeyDown += noteTreeView_KeyDown;
            noteTreeView.KeyPress += noteTreeView_KeyPress;
            noteTreeView.Leave += noteTreeView_Leave;
            //
            // noteTreeViewContextMenuStrip
            //
            noteTreeViewContextMenuStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            noteTreeViewContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { openToolStripMenuItem, newFileToolStripMenuItem, newFolderToolStripMenuItem, renameToolStripMenuItem, deleteToolStripMenuItem, copyToolStripMenuItem, cutToolStripMenuItem, pasteToolStripMenuItem });
            noteTreeViewContextMenuStrip.Name = "notifyIconContextMenuStrip";
            noteTreeViewContextMenuStrip.Size = new System.Drawing.Size(163, 180);
            //
            // openToolStripMenuItem
            //
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            openToolStripMenuItem.Text = "&Open";
            openToolStripMenuItem.Click += openToolStripMenuItem_Click;
            //
            // newFileToolStripMenuItem
            //
            newFileToolStripMenuItem.Name = "newFileToolStripMenuItem";
            newFileToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N));
            newFileToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            newFileToolStripMenuItem.Text = "&New File";
            newFileToolStripMenuItem.Click += newFileToolStripMenuItem_Click;
            //
            // newFolderToolStripMenuItem
            //
            newFolderToolStripMenuItem.Name = "newFolderToolStripMenuItem";
            newFolderToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F7;
            newFolderToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            newFolderToolStripMenuItem.Text = "New &Folder";
            newFolderToolStripMenuItem.Click += newfolderToolStripMenuItem_Click;
            //
            // renameToolStripMenuItem
            //
            renameToolStripMenuItem.Name = "renameToolStripMenuItem";
            renameToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F2;
            renameToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            renameToolStripMenuItem.Text = "&Rename";
            renameToolStripMenuItem.Click += renameToolStripMenuItem_Click;
            //
            // deleteToolStripMenuItem
            //
            deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            deleteToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            deleteToolStripMenuItem.Text = "&Delete";
            deleteToolStripMenuItem.Click += deleteToolStripMenuItem_Click;
            //
            // copyToolStripMenuItem
            //
            copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            copyToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            copyToolStripMenuItem.Text = "Copy";
            copyToolStripMenuItem.Click += copyToolStripMenuItem_Click;
            //
            // cutToolStripMenuItem
            //
            cutToolStripMenuItem.Name = "cutToolStripMenuItem";
            cutToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            cutToolStripMenuItem.Text = "Cut";
            cutToolStripMenuItem.Click += cutToolStripMenuItem_Click;
            //
            // pasteToolStripMenuItem
            //
            pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            pasteToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            pasteToolStripMenuItem.Text = "Paste";
            pasteToolStripMenuItem.Click += pasteToolStripMenuItem_Click;
            //
            // toolsFlowLayoutPanel
            //
            toolsFlowLayoutPanel.AutoSize = true;
            toolsFlowLayoutPanel.Controls.Add(renameButton);
            toolsFlowLayoutPanel.Controls.Add(deleteButton);
            toolsFlowLayoutPanel.Controls.Add(settingsButton);
            toolsFlowLayoutPanel.Controls.Add(filterAllCheckBox);
            toolsFlowLayoutPanel.Controls.Add(filterTextBox);
            toolsFlowLayoutPanel.Location = new System.Drawing.Point(6, 6);
            toolsFlowLayoutPanel.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            toolsFlowLayoutPanel.Name = "toolsFlowLayoutPanel";
            toolsFlowLayoutPanel.Size = new System.Drawing.Size(880, 43);
            toolsFlowLayoutPanel.TabIndex = 3;
            //
            // renameButton
            //
            renameButton.Anchor = System.Windows.Forms.AnchorStyles.Left;
            renameButton.AutoSize = true;
            renameButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            renameButton.Location = new System.Drawing.Point(6, 6);
            renameButton.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            renameButton.Name = "renameButton";
            renameButton.Size = new System.Drawing.Size(82, 31);
            renameButton.TabIndex = 4;
            renameButton.Text = "&Rename";
            renameButton.UseVisualStyleBackColor = true;
            renameButton.Click += renameButton_Click;
            //
            // deleteButton
            //
            deleteButton.Anchor = System.Windows.Forms.AnchorStyles.Left;
            deleteButton.AutoSize = true;
            deleteButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            deleteButton.Location = new System.Drawing.Point(100, 6);
            deleteButton.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            deleteButton.Name = "deleteButton";
            deleteButton.Size = new System.Drawing.Size(69, 31);
            deleteButton.TabIndex = 5;
            deleteButton.Text = "&Delete";
            deleteButton.UseVisualStyleBackColor = true;
            deleteButton.Click += deleteButton_Click;
            //
            // settingsButton
            //
            settingsButton.Anchor = System.Windows.Forms.AnchorStyles.Left;
            settingsButton.AutoSize = true;
            settingsButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            settingsButton.Location = new System.Drawing.Point(181, 6);
            settingsButton.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            settingsButton.Name = "settingsButton";
            settingsButton.Size = new System.Drawing.Size(81, 31);
            settingsButton.TabIndex = 3;
            settingsButton.Text = "&Settings";
            settingsButton.UseVisualStyleBackColor = true;
            settingsButton.Click += settingsButton_Click;
            //
            // filterAllCheckBox
            //
            filterAllCheckBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            filterAllCheckBox.AutoSize = true;
            filterAllCheckBox.Checked = true;
            filterAllCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            filterAllCheckBox.Location = new System.Drawing.Point(272, 9);
            filterAllCheckBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            filterAllCheckBox.Name = "filterAllCheckBox";
            filterAllCheckBox.Size = new System.Drawing.Size(89, 25);
            filterAllCheckBox.TabIndex = 6;
            filterAllCheckBox.Text = "&Filter all";
            filterAllCheckBox.UseVisualStyleBackColor = true;
            filterAllCheckBox.CheckedChanged += filterAllCheckBox_CheckedChanged;
            //
            // filterTextBox
            //
            filterTextBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            filterTextBox.Location = new System.Drawing.Point(371, 7);
            filterTextBox.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            filterTextBox.Name = "filterTextBox";
            filterTextBox.Size = new System.Drawing.Size(503, 29);
            filterTextBox.TabIndex = 2;
            filterTextBox.Visible = false;
            filterTextBox.TextChanged += filterTextBox_TextChanged;
            //
            // tableLayoutPanel1
            //
            tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right));
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 41F));
            tableLayoutPanel1.Controls.Add(noteTreeView, 0, 1);
            tableLayoutPanel1.Controls.Add(toolsFlowLayoutPanel, 0, 0);
            tableLayoutPanel1.Location = new System.Drawing.Point(24, 21);
            tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new System.Drawing.Size(1613, 960);
            tableLayoutPanel1.TabIndex = 2;
            //
            // notifyIcon
            //
            notifyIcon.ContextMenuStrip = notifyIconContextMenuStrip;
            notifyIcon.Icon = ((System.Drawing.Icon)resources.GetObject("notifyIcon.Icon"));
            notifyIcon.Text = "Jeek Note";
            notifyIcon.Visible = true;
            notifyIcon.MouseClick += notifyIcon_MouseClick;
            //
            // notifyIconContextMenuStrip
            //
            notifyIconContextMenuStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            notifyIconContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { exitToolStripMenuItem });
            notifyIconContextMenuStrip.Name = "notifyIconContextMenuStrip";
            notifyIconContextMenuStrip.Size = new System.Drawing.Size(93, 26);
            //
            // exitToolStripMenuItem
            //
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
            exitToolStripMenuItem.Text = "E&xit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            //
            // MainForm
            //
            AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1661, 1001);
            Controls.Add(tableLayoutPanel1);
            DoubleBuffered = true;
            Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)134));
            Margin = new System.Windows.Forms.Padding(4);
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Jeek Note";
            Activated += MainForm_Activated;
            FormClosing += MainForm_FormClosing;
            Load += MainForm_Load;
            noteTreeViewContextMenuStrip.ResumeLayout(false);
            toolsFlowLayoutPanel.ResumeLayout(false);
            toolsFlowLayoutPanel.PerformLayout();
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            notifyIconContextMenuStrip.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TreeView noteTreeView;
        private FlowLayoutPanel toolsFlowLayoutPanel;
        private TableLayoutPanel tableLayoutPanel1;
        private TextBox filterTextBox;
        private Button settingsButton;
        private NotifyIcon notifyIcon;
        private ContextMenuStrip notifyIconContextMenuStrip;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ContextMenuStrip noteTreeViewContextMenuStrip;
        private ToolStripMenuItem newFileToolStripMenuItem;
        private ToolStripMenuItem newFolderToolStripMenuItem;
        private ToolStripMenuItem renameToolStripMenuItem;
        private ToolStripMenuItem deleteToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private Button renameButton;
        private Button deleteButton;
        private CheckBox filterAllCheckBox;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
    }
}
