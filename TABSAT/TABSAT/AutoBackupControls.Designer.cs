namespace TABSAT
{
    partial class AutoBackupControls
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing )
        {
            if( disposing && ( components != null ) )
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.restoreButton = new System.Windows.Forms.Button();
            this.backupButton = new System.Windows.Forms.Button();
            this.savesGroupBox = new System.Windows.Forms.GroupBox();
            this.savesCheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.watcherButton = new System.Windows.Forms.Button();
            this.backupsGroupBox = new System.Windows.Forms.GroupBox();
            this.backupsTreeView = new System.Windows.Forms.TreeView();
            this.backupsHintLabel = new System.Windows.Forms.Label();
            this.backupFolderGroupBox = new System.Windows.Forms.GroupBox();
            this.backupFolderChooseButton = new System.Windows.Forms.Button();
            this.backupsDirectoryTextBox = new System.Windows.Forms.TextBox();
            this.autoBackupsBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.keyGroupBox = new System.Windows.Forms.GroupBox();
            this.keyCheckBox2 = new System.Windows.Forms.CheckBox();
            this.keyCheckBox3 = new System.Windows.Forms.CheckBox();
            this.keyCheckBox1 = new System.Windows.Forms.CheckBox();
            this.progressGroupBox = new System.Windows.Forms.GroupBox();
            this.backupsFolderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.copySaveGroupBox = new System.Windows.Forms.GroupBox();
            this.savesGroupBox.SuspendLayout();
            this.backupsGroupBox.SuspendLayout();
            this.backupFolderGroupBox.SuspendLayout();
            this.keyGroupBox.SuspendLayout();
            this.progressGroupBox.SuspendLayout();
            this.copySaveGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(6, 25);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(426, 17);
            this.progressBar.Step = 1;
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar.TabIndex = 0;
            // 
            // restoreButton
            // 
            this.restoreButton.Enabled = false;
            this.restoreButton.Location = new System.Drawing.Point(94, 19);
            this.restoreButton.Name = "restoreButton";
            this.restoreButton.Size = new System.Drawing.Size(80, 23);
            this.restoreButton.TabIndex = 1;
            this.restoreButton.Text = "< Restore <";
            this.restoreButton.UseVisualStyleBackColor = true;
            this.restoreButton.Click += new System.EventHandler(this.restoreButton_Click);
            // 
            // backupButton
            // 
            this.backupButton.Enabled = false;
            this.backupButton.Location = new System.Drawing.Point(8, 19);
            this.backupButton.Name = "backupButton";
            this.backupButton.Size = new System.Drawing.Size(80, 23);
            this.backupButton.TabIndex = 0;
            this.backupButton.Text = "> Backup >";
            this.backupButton.UseVisualStyleBackColor = true;
            this.backupButton.Click += new System.EventHandler(this.backupButton_Click);
            // 
            // savesGroupBox
            // 
            this.savesGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.savesGroupBox.Controls.Add(this.savesCheckedListBox);
            this.savesGroupBox.Location = new System.Drawing.Point(3, 175);
            this.savesGroupBox.Name = "savesGroupBox";
            this.savesGroupBox.Size = new System.Drawing.Size(268, 406);
            this.savesGroupBox.TabIndex = 4;
            this.savesGroupBox.TabStop = false;
            this.savesGroupBox.Text = "Active Save Files";
            // 
            // savesCheckedListBox
            // 
            this.savesCheckedListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.savesCheckedListBox.FormattingEnabled = true;
            this.savesCheckedListBox.Location = new System.Drawing.Point(6, 19);
            this.savesCheckedListBox.Name = "savesCheckedListBox";
            this.savesCheckedListBox.Size = new System.Drawing.Size(256, 379);
            this.savesCheckedListBox.TabIndex = 0;
            this.savesCheckedListBox.SelectedIndexChanged += new System.EventHandler(this.savesCheckedListBox_SelectedIndexChanged);
            // 
            // watcherButton
            // 
            this.watcherButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.watcherButton.Enabled = false;
            this.watcherButton.Location = new System.Drawing.Point(650, 14);
            this.watcherButton.Name = "watcherButton";
            this.watcherButton.Size = new System.Drawing.Size(153, 30);
            this.watcherButton.TabIndex = 1;
            this.watcherButton.Text = "Start Auto-Backup";
            this.watcherButton.UseVisualStyleBackColor = true;
            // 
            // backupsGroupBox
            // 
            this.backupsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.backupsGroupBox.Controls.Add(this.backupsTreeView);
            this.backupsGroupBox.Location = new System.Drawing.Point(277, 175);
            this.backupsGroupBox.Name = "backupsGroupBox";
            this.backupsGroupBox.Size = new System.Drawing.Size(532, 406);
            this.backupsGroupBox.TabIndex = 5;
            this.backupsGroupBox.TabStop = false;
            this.backupsGroupBox.Text = "Backups";
            // 
            // backupsTreeView
            // 
            this.backupsTreeView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.backupsTreeView.Location = new System.Drawing.Point(6, 19);
            this.backupsTreeView.Name = "backupsTreeView";
            this.backupsTreeView.Size = new System.Drawing.Size(520, 381);
            this.backupsTreeView.TabIndex = 0;
            this.backupsTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.backupsTreeView_AfterSelect);
            // 
            // backupsHintLabel
            // 
            this.backupsHintLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.backupsHintLabel.Location = new System.Drawing.Point(9, 10);
            this.backupsHintLabel.Name = "backupsHintLabel";
            this.backupsHintLabel.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.backupsHintLabel.Size = new System.Drawing.Size(498, 38);
            this.backupsHintLabel.TabIndex = 0;
            this.backupsHintLabel.Text = "1) Choose where copies of your Save Files are backed up.   2) Select a save to Ba" +
    "ckup or Restore.\r\n3) Click the appropriate Backup or Restore button.";
            this.backupsHintLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // backupFolderGroupBox
            // 
            this.backupFolderGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.backupFolderGroupBox.Controls.Add(this.backupFolderChooseButton);
            this.backupFolderGroupBox.Controls.Add(this.backupsDirectoryTextBox);
            this.backupFolderGroupBox.Location = new System.Drawing.Point(3, 63);
            this.backupFolderGroupBox.Name = "backupFolderGroupBox";
            this.backupFolderGroupBox.Size = new System.Drawing.Size(806, 52);
            this.backupFolderGroupBox.TabIndex = 2;
            this.backupFolderGroupBox.TabStop = false;
            this.backupFolderGroupBox.Text = "Folder for Backups";
            // 
            // backupFolderChooseButton
            // 
            this.backupFolderChooseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.backupFolderChooseButton.Location = new System.Drawing.Point(647, 13);
            this.backupFolderChooseButton.Name = "backupFolderChooseButton";
            this.backupFolderChooseButton.Size = new System.Drawing.Size(153, 30);
            this.backupFolderChooseButton.TabIndex = 0;
            this.backupFolderChooseButton.Text = "Choose Folder...";
            this.backupFolderChooseButton.UseVisualStyleBackColor = true;
            this.backupFolderChooseButton.Click += new System.EventHandler(this.backupFolderChooseButton_Click);
            // 
            // backupsDirectoryTextBox
            // 
            this.backupsDirectoryTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.backupsDirectoryTextBox.Location = new System.Drawing.Point(6, 19);
            this.backupsDirectoryTextBox.Name = "backupsDirectoryTextBox";
            this.backupsDirectoryTextBox.ReadOnly = true;
            this.backupsDirectoryTextBox.Size = new System.Drawing.Size(596, 20);
            this.backupsDirectoryTextBox.TabIndex = 1;
            // 
            // autoBackupsBackgroundWorker
            // 
            this.autoBackupsBackgroundWorker.WorkerReportsProgress = true;
            // 
            // keyGroupBox
            // 
            this.keyGroupBox.Controls.Add(this.keyCheckBox2);
            this.keyGroupBox.Controls.Add(this.keyCheckBox3);
            this.keyGroupBox.Controls.Add(this.keyCheckBox1);
            this.keyGroupBox.Location = new System.Drawing.Point(3, 121);
            this.keyGroupBox.Name = "keyGroupBox";
            this.keyGroupBox.Size = new System.Drawing.Size(174, 48);
            this.keyGroupBox.TabIndex = 6;
            this.keyGroupBox.TabStop = false;
            this.keyGroupBox.Text = "Save Backup Exists Key";
            // 
            // keyCheckBox2
            // 
            this.keyCheckBox2.AutoCheck = false;
            this.keyCheckBox2.AutoSize = true;
            this.keyCheckBox2.Checked = true;
            this.keyCheckBox2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.keyCheckBox2.Location = new System.Drawing.Point(84, 23);
            this.keyCheckBox2.Name = "keyCheckBox2";
            this.keyCheckBox2.Size = new System.Drawing.Size(44, 17);
            this.keyCheckBox2.TabIndex = 1;
            this.keyCheckBox2.Text = "Yes";
            this.keyCheckBox2.UseVisualStyleBackColor = true;
            // 
            // keyCheckBox3
            // 
            this.keyCheckBox3.AutoCheck = false;
            this.keyCheckBox3.AutoSize = true;
            this.keyCheckBox3.Location = new System.Drawing.Point(134, 23);
            this.keyCheckBox3.Name = "keyCheckBox3";
            this.keyCheckBox3.Size = new System.Drawing.Size(40, 17);
            this.keyCheckBox3.TabIndex = 2;
            this.keyCheckBox3.Text = "No";
            this.keyCheckBox3.UseVisualStyleBackColor = true;
            // 
            // keyCheckBox1
            // 
            this.keyCheckBox1.AutoCheck = false;
            this.keyCheckBox1.AutoSize = true;
            this.keyCheckBox1.Checked = true;
            this.keyCheckBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.keyCheckBox1.Enabled = false;
            this.keyCheckBox1.Location = new System.Drawing.Point(9, 23);
            this.keyCheckBox1.Name = "keyCheckBox1";
            this.keyCheckBox1.Size = new System.Drawing.Size(72, 17);
            this.keyCheckBox1.TabIndex = 0;
            this.keyCheckBox1.Text = "Unknown";
            this.keyCheckBox1.UseVisualStyleBackColor = true;
            // 
            // progressGroupBox
            // 
            this.progressGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressGroupBox.Controls.Add(this.progressBar);
            this.progressGroupBox.Location = new System.Drawing.Point(371, 121);
            this.progressGroupBox.Name = "progressGroupBox";
            this.progressGroupBox.Size = new System.Drawing.Size(438, 48);
            this.progressGroupBox.TabIndex = 7;
            this.progressGroupBox.TabStop = false;
            this.progressGroupBox.Text = "Loading Progress";
            // 
            // backupsFolderBrowserDialog
            // 
            this.backupsFolderBrowserDialog.RootFolder = System.Environment.SpecialFolder.MyComputer;
            // 
            // copySaveGroupBox
            // 
            this.copySaveGroupBox.Controls.Add(this.restoreButton);
            this.copySaveGroupBox.Controls.Add(this.backupButton);
            this.copySaveGroupBox.Location = new System.Drawing.Point(183, 121);
            this.copySaveGroupBox.Name = "copySaveGroupBox";
            this.copySaveGroupBox.Size = new System.Drawing.Size(182, 48);
            this.copySaveGroupBox.TabIndex = 3;
            this.copySaveGroupBox.TabStop = false;
            this.copySaveGroupBox.Text = "Copy Save File";
            // 
            // AutoBackupControls
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.copySaveGroupBox);
            this.Controls.Add(this.progressGroupBox);
            this.Controls.Add(this.keyGroupBox);
            this.Controls.Add(this.savesGroupBox);
            this.Controls.Add(this.watcherButton);
            this.Controls.Add(this.backupsGroupBox);
            this.Controls.Add(this.backupsHintLabel);
            this.Controls.Add(this.backupFolderGroupBox);
            this.Name = "AutoBackupControls";
            this.Size = new System.Drawing.Size(812, 584);
            this.Load += new System.EventHandler(this.AutoBackupControls_Load);
            this.savesGroupBox.ResumeLayout(false);
            this.backupsGroupBox.ResumeLayout(false);
            this.backupFolderGroupBox.ResumeLayout(false);
            this.backupFolderGroupBox.PerformLayout();
            this.keyGroupBox.ResumeLayout(false);
            this.keyGroupBox.PerformLayout();
            this.progressGroupBox.ResumeLayout(false);
            this.copySaveGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Button restoreButton;
        private System.Windows.Forms.Button backupButton;
        private System.Windows.Forms.GroupBox savesGroupBox;
        private System.Windows.Forms.CheckedListBox savesCheckedListBox;
        private System.Windows.Forms.Button watcherButton;
        private System.Windows.Forms.GroupBox backupsGroupBox;
        private System.Windows.Forms.TreeView backupsTreeView;
        private System.Windows.Forms.Label backupsHintLabel;
        private System.Windows.Forms.GroupBox backupFolderGroupBox;
        private System.Windows.Forms.Button backupFolderChooseButton;
        private System.Windows.Forms.TextBox backupsDirectoryTextBox;
        private System.ComponentModel.BackgroundWorker autoBackupsBackgroundWorker;
        private System.Windows.Forms.GroupBox keyGroupBox;
        private System.Windows.Forms.CheckBox keyCheckBox2;
        private System.Windows.Forms.CheckBox keyCheckBox3;
        private System.Windows.Forms.CheckBox keyCheckBox1;
        private System.Windows.Forms.GroupBox progressGroupBox;
        private System.Windows.Forms.FolderBrowserDialog backupsFolderBrowserDialog;
        private System.Windows.Forms.GroupBox copySaveGroupBox;
    }
}
