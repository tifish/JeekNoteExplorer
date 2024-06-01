﻿namespace JeekNoteExplorer
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            noteTreeView = new TreeView();
            noteTreeViewContextMenuStrip = new ContextMenuStrip(components);
            openToolStripMenuItem = new ToolStripMenuItem();
            newFileToolStripMenuItem = new ToolStripMenuItem();
            newFolderToolStripMenuItem = new ToolStripMenuItem();
            renameToolStripMenuItem = new ToolStripMenuItem();
            deleteToolStripMenuItem = new ToolStripMenuItem();
            toolsFlowLayoutPanel = new FlowLayoutPanel();
            filterTextBox = new TextBox();
            filterAllCheckBox = new CheckBox();
            renameButton = new Button();
            deleteButton = new Button();
            settingsButton = new Button();
            tableLayoutPanel1 = new TableLayoutPanel();
            notifyIcon = new NotifyIcon(components);
            notifyIconContextMenuStrip = new ContextMenuStrip(components);
            exitToolStripMenuItem = new ToolStripMenuItem();
            noteTreeViewContextMenuStrip.SuspendLayout();
            toolsFlowLayoutPanel.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            notifyIconContextMenuStrip.SuspendLayout();
            SuspendLayout();
            // 
            // noteTreeView
            // 
            noteTreeView.ContextMenuStrip = noteTreeViewContextMenuStrip;
            noteTreeView.Dock = DockStyle.Fill;
            noteTreeView.LabelEdit = true;
            noteTreeView.Location = new Point(4, 51);
            noteTreeView.Margin = new Padding(4);
            noteTreeView.Name = "noteTreeView";
            noteTreeView.Size = new Size(1121, 631);
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
            noteTreeViewContextMenuStrip.ImageScalingSize = new Size(24, 24);
            noteTreeViewContextMenuStrip.Items.AddRange(new ToolStripItem[] { openToolStripMenuItem, newFileToolStripMenuItem, newFolderToolStripMenuItem, renameToolStripMenuItem, deleteToolStripMenuItem });
            noteTreeViewContextMenuStrip.Name = "notifyIconContextMenuStrip";
            noteTreeViewContextMenuStrip.Size = new Size(163, 114);
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.Size = new Size(162, 22);
            openToolStripMenuItem.Text = "&Open";
            openToolStripMenuItem.Click += openToolStripMenuItem_Click;
            // 
            // newFileToolStripMenuItem
            // 
            newFileToolStripMenuItem.Name = "newFileToolStripMenuItem";
            newFileToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.N;
            newFileToolStripMenuItem.Size = new Size(162, 22);
            newFileToolStripMenuItem.Text = "&New File";
            newFileToolStripMenuItem.Click += newFileToolStripMenuItem_Click;
            // 
            // newFolderToolStripMenuItem
            // 
            newFolderToolStripMenuItem.Name = "newFolderToolStripMenuItem";
            newFolderToolStripMenuItem.ShortcutKeys = Keys.F7;
            newFolderToolStripMenuItem.Size = new Size(162, 22);
            newFolderToolStripMenuItem.Text = "New &Folder";
            newFolderToolStripMenuItem.Click += newfolderToolStripMenuItem_Click;
            // 
            // renameToolStripMenuItem
            // 
            renameToolStripMenuItem.Name = "renameToolStripMenuItem";
            renameToolStripMenuItem.ShortcutKeys = Keys.F2;
            renameToolStripMenuItem.Size = new Size(162, 22);
            renameToolStripMenuItem.Text = "&Rename";
            renameToolStripMenuItem.Click += renameToolStripMenuItem_Click;
            // 
            // deleteToolStripMenuItem
            // 
            deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            deleteToolStripMenuItem.ShortcutKeys = Keys.Delete;
            deleteToolStripMenuItem.Size = new Size(162, 22);
            deleteToolStripMenuItem.Text = "&Delete";
            deleteToolStripMenuItem.Click += deleteToolStripMenuItem_Click;
            // 
            // toolsFlowLayoutPanel
            // 
            toolsFlowLayoutPanel.AutoSize = true;
            toolsFlowLayoutPanel.Controls.Add(filterTextBox);
            toolsFlowLayoutPanel.Controls.Add(filterAllCheckBox);
            toolsFlowLayoutPanel.Controls.Add(renameButton);
            toolsFlowLayoutPanel.Controls.Add(deleteButton);
            toolsFlowLayoutPanel.Controls.Add(settingsButton);
            toolsFlowLayoutPanel.Location = new Point(4, 4);
            toolsFlowLayoutPanel.Margin = new Padding(4);
            toolsFlowLayoutPanel.Name = "toolsFlowLayoutPanel";
            toolsFlowLayoutPanel.Size = new Size(712, 39);
            toolsFlowLayoutPanel.TabIndex = 3;
            // 
            // filterTextBox
            // 
            filterTextBox.Anchor = AnchorStyles.Left;
            filterTextBox.Location = new Point(4, 5);
            filterTextBox.Margin = new Padding(4);
            filterTextBox.Name = "filterTextBox";
            filterTextBox.Size = new Size(353, 29);
            filterTextBox.TabIndex = 2;
            filterTextBox.Visible = false;
            filterTextBox.TextChanged += filterTextBox_TextChanged;
            // 
            // filterAllCheckBox
            // 
            filterAllCheckBox.Anchor = AnchorStyles.Left;
            filterAllCheckBox.AutoSize = true;
            filterAllCheckBox.Checked = true;
            filterAllCheckBox.CheckState = CheckState.Checked;
            filterAllCheckBox.Location = new Point(364, 7);
            filterAllCheckBox.Name = "filterAllCheckBox";
            filterAllCheckBox.Size = new Size(89, 25);
            filterAllCheckBox.TabIndex = 6;
            filterAllCheckBox.Text = "&Filter all";
            filterAllCheckBox.UseVisualStyleBackColor = true;
            filterAllCheckBox.CheckedChanged += filterAllCheckBox_CheckedChanged;
            // 
            // renameButton
            // 
            renameButton.Anchor = AnchorStyles.Left;
            renameButton.AutoSize = true;
            renameButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            renameButton.Location = new Point(460, 4);
            renameButton.Margin = new Padding(4);
            renameButton.Name = "renameButton";
            renameButton.Size = new Size(82, 31);
            renameButton.TabIndex = 4;
            renameButton.Text = "&Rename";
            renameButton.UseVisualStyleBackColor = true;
            renameButton.Click += renameButton_Click;
            // 
            // deleteButton
            // 
            deleteButton.Anchor = AnchorStyles.Left;
            deleteButton.AutoSize = true;
            deleteButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            deleteButton.Location = new Point(550, 4);
            deleteButton.Margin = new Padding(4);
            deleteButton.Name = "deleteButton";
            deleteButton.Size = new Size(69, 31);
            deleteButton.TabIndex = 5;
            deleteButton.Text = "&Delete";
            deleteButton.UseVisualStyleBackColor = true;
            deleteButton.Click += deleteButton_Click;
            // 
            // settingsButton
            // 
            settingsButton.Anchor = AnchorStyles.Left;
            settingsButton.AutoSize = true;
            settingsButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            settingsButton.Location = new Point(627, 4);
            settingsButton.Margin = new Padding(4);
            settingsButton.Name = "settingsButton";
            settingsButton.Size = new Size(81, 31);
            settingsButton.TabIndex = 3;
            settingsButton.Text = "&Settings";
            settingsButton.UseVisualStyleBackColor = true;
            settingsButton.Click += settingsButton_Click;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 29F));
            tableLayoutPanel1.Controls.Add(noteTreeView, 0, 1);
            tableLayoutPanel1.Controls.Add(toolsFlowLayoutPanel, 0, 0);
            tableLayoutPanel1.Location = new Point(17, 15);
            tableLayoutPanel1.Margin = new Padding(4);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(1129, 686);
            tableLayoutPanel1.TabIndex = 2;
            // 
            // notifyIcon
            // 
            notifyIcon.ContextMenuStrip = notifyIconContextMenuStrip;
            notifyIcon.Icon = (Icon)resources.GetObject("notifyIcon.Icon");
            notifyIcon.Text = "Jeek Note";
            notifyIcon.Visible = true;
            notifyIcon.MouseClick += notifyIcon_MouseClick;
            // 
            // notifyIconContextMenuStrip
            // 
            notifyIconContextMenuStrip.ImageScalingSize = new Size(24, 24);
            notifyIconContextMenuStrip.Items.AddRange(new ToolStripItem[] { exitToolStripMenuItem });
            notifyIconContextMenuStrip.Name = "notifyIconContextMenuStrip";
            notifyIconContextMenuStrip.Size = new Size(94, 26);
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(93, 22);
            exitToolStripMenuItem.Text = "E&xit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(10F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1163, 715);
            Controls.Add(tableLayoutPanel1);
            DoubleBuffered = true;
            Font = new Font("Microsoft YaHei", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            Margin = new Padding(4);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
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
    }
}
