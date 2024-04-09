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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            noteTreeView = new TreeView();
            refreshButton = new Button();
            toolsFlowLayoutPanel = new FlowLayoutPanel();
            filterTextBox = new TextBox();
            tableLayoutPanel1 = new TableLayoutPanel();
            toolsFlowLayoutPanel.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            //
            // noteTreeView
            //
            noteTreeView.Dock = DockStyle.Fill;
            noteTreeView.Location = new Point(4, 53);
            noteTreeView.Margin = new Padding(4);
            noteTreeView.Name = "noteTreeView";
            noteTreeView.Size = new Size(1121, 629);
            noteTreeView.TabIndex = 0;
            noteTreeView.DoubleClick += noteTreeView_DoubleClick;
            noteTreeView.KeyDown += noteTreeView_KeyDown;
            noteTreeView.KeyPress += noteTreeView_KeyPress;
            //
            // refreshButton
            //
            refreshButton.Anchor = AnchorStyles.Left;
            refreshButton.Location = new Point(4, 4);
            refreshButton.Margin = new Padding(4);
            refreshButton.Name = "refreshButton";
            refreshButton.Size = new Size(107, 33);
            refreshButton.TabIndex = 1;
            refreshButton.Text = "Refresh(F5)";
            refreshButton.UseVisualStyleBackColor = true;
            refreshButton.Click += refreshButton_Click;
            //
            // toolsFlowLayoutPanel
            //
            toolsFlowLayoutPanel.AutoSize = true;
            toolsFlowLayoutPanel.Controls.Add(refreshButton);
            toolsFlowLayoutPanel.Controls.Add(filterTextBox);
            toolsFlowLayoutPanel.Location = new Point(4, 4);
            toolsFlowLayoutPanel.Margin = new Padding(4);
            toolsFlowLayoutPanel.Name = "toolsFlowLayoutPanel";
            toolsFlowLayoutPanel.Size = new Size(476, 41);
            toolsFlowLayoutPanel.TabIndex = 3;
            //
            // filterTextBox
            //
            filterTextBox.Anchor = AnchorStyles.Left;
            filterTextBox.Location = new Point(119, 6);
            filterTextBox.Margin = new Padding(4);
            filterTextBox.Name = "filterTextBox";
            filterTextBox.Size = new Size(353, 29);
            filterTextBox.TabIndex = 2;
            filterTextBox.Visible = false;
            filterTextBox.WordWrap = false;
            filterTextBox.TextChanged += filterTextBox_TextChanged;
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
            Load += MainForm_Load;
            toolsFlowLayoutPanel.ResumeLayout(false);
            toolsFlowLayoutPanel.PerformLayout();
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TreeView noteTreeView;
        private Button refreshButton;
        private FlowLayoutPanel toolsFlowLayoutPanel;
        private TableLayoutPanel tableLayoutPanel1;
        private TextBox filterTextBox;
    }
}
