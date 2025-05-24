namespace JeekNoteExplorer
{
    partial class SettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
            tableLayoutPanel1 = new TableLayoutPanel();
            label1 = new Label();
            noteFolderTextBox = new TextBox();
            closeButton = new Button();
            startWithSystemCheckBox = new CheckBox();
            label2 = new Label();
            wakeUpHotkeyInputBox = new JeekNoteExplorer.Controls.HotkeyInputBox();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.AutoSize = true;
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel1.Controls.Add(label1, 0, 0);
            tableLayoutPanel1.Controls.Add(noteFolderTextBox, 1, 0);
            tableLayoutPanel1.Controls.Add(closeButton, 1, 3);
            tableLayoutPanel1.Controls.Add(startWithSystemCheckBox, 0, 1);
            tableLayoutPanel1.Controls.Add(label2, 0, 2);
            tableLayoutPanel1.Controls.Add(wakeUpHotkeyInputBox, 1, 2);
            tableLayoutPanel1.Location = new Point(17, 17);
            tableLayoutPanel1.Margin = new Padding(4);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 4;
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.Size = new Size(721, 150);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Location = new Point(60, 8);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(102, 21);
            label1.TabIndex = 0;
            label1.Text = "Note folder:";
            // 
            // noteFolderTextBox
            // 
            noteFolderTextBox.Anchor = AnchorStyles.Left;
            noteFolderTextBox.Location = new Point(170, 4);
            noteFolderTextBox.Margin = new Padding(4);
            noteFolderTextBox.Name = "noteFolderTextBox";
            noteFolderTextBox.Size = new Size(427, 29);
            noteFolderTextBox.TabIndex = 2;
            // 
            // closeButton
            // 
            closeButton.Anchor = AnchorStyles.Right;
            closeButton.AutoSize = true;
            closeButton.Location = new Point(610, 107);
            closeButton.Margin = new Padding(4);
            closeButton.Name = "closeButton";
            closeButton.Size = new Size(107, 39);
            closeButton.TabIndex = 3;
            closeButton.Text = "Close";
            closeButton.UseVisualStyleBackColor = true;
            closeButton.Click += closeButton_Click;
            // 
            // startWithSystemCheckBox
            // 
            startWithSystemCheckBox.Anchor = AnchorStyles.Left;
            startWithSystemCheckBox.AutoSize = true;
            startWithSystemCheckBox.CheckAlign = ContentAlignment.MiddleRight;
            startWithSystemCheckBox.Location = new Point(3, 40);
            startWithSystemCheckBox.Name = "startWithSystemCheckBox";
            startWithSystemCheckBox.Size = new Size(160, 25);
            startWithSystemCheckBox.TabIndex = 4;
            startWithSystemCheckBox.Text = "&Start with system";
            startWithSystemCheckBox.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Right;
            label2.AutoSize = true;
            label2.Location = new Point(49, 75);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(113, 21);
            label2.TabIndex = 5;
            label2.Text = "Wake up key:";
            // 
            // wakeUpHotkeyInputBox
            // 
            wakeUpHotkeyInputBox.Alt = false;
            wakeUpHotkeyInputBox.Control = false;
            wakeUpHotkeyInputBox.KeyCode = Keys.None;
            wakeUpHotkeyInputBox.Location = new Point(169, 71);
            wakeUpHotkeyInputBox.Name = "wakeUpHotkeyInputBox";
            wakeUpHotkeyInputBox.Shift = false;
            wakeUpHotkeyInputBox.ShortcutsEnabled = false;
            wakeUpHotkeyInputBox.Size = new Size(428, 29);
            wakeUpHotkeyInputBox.TabIndex = 6;
            wakeUpHotkeyInputBox.Windows = false;
            wakeUpHotkeyInputBox.WordWrap = false;
            // 
            // SettingsForm
            // 
            AutoScaleDimensions = new SizeF(10F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            CancelButton = closeButton;
            ClientSize = new Size(1143, 630);
            Controls.Add(tableLayoutPanel1);
            Font = new Font("Microsoft YaHei", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Margin = new Padding(4);
            Name = "SettingsForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Settings";
            FormClosing += SettingsForm_FormClosing;
            Load += SettingsForm_Load;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private Label label1;
        private TextBox noteFolderTextBox;
        private Button closeButton;
        private CheckBox startWithSystemCheckBox;
        private Label label2;
        private Controls.HotkeyInputBox wakeUpHotkeyInputBox;
    }
}