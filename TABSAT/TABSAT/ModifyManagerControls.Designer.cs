namespace TABSAT
{
    partial class ModifyManagerControls
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
            this.extractLeaveCheckBox = new System.Windows.Forms.CheckBox();
            this.extractRepackSaveButton = new System.Windows.Forms.Button();
            this.saveFileGroupBox = new System.Windows.Forms.GroupBox();
            this.saveFileTextBox = new System.Windows.Forms.TextBox();
            this.saveFileChooseButton = new System.Windows.Forms.Button();
            this.backupCheckBox = new System.Windows.Forms.CheckBox();
            this.quickSkipSaveButton = new System.Windows.Forms.Button();
            this.reflectorStopExtractCheckBox = new System.Windows.Forms.CheckBox();
            this.reflectorStopRepackCheckBox = new System.Windows.Forms.CheckBox();
            this.reflectorStopButton = new System.Windows.Forms.Button();
            this.reflectorOutputGroupBox = new System.Windows.Forms.GroupBox();
            this.reflectorTextBox = new System.Windows.Forms.TextBox();
            this.verticalDividerLabel = new System.Windows.Forms.Label();
            this.modifyHintLabel = new System.Windows.Forms.Label();
            this.saveOpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.modifyGroupBox = new System.Windows.Forms.GroupBox();
            this.reflectorSplitContainer = new System.Windows.Forms.SplitContainer();
            this.processFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.reflectorGroupBox = new System.Windows.Forms.GroupBox();
            this.modifySaveBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.optionsSplitContainer = new System.Windows.Forms.SplitContainer();
            this.horizontalDividerLabel = new System.Windows.Forms.Label();
            this.resetGroupBox = new System.Windows.Forms.GroupBox();
            this.resetButton = new System.Windows.Forms.Button();
            this.saveFileGroupBox.SuspendLayout();
            this.reflectorOutputGroupBox.SuspendLayout();
            this.modifyGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.reflectorSplitContainer)).BeginInit();
            this.reflectorSplitContainer.Panel1.SuspendLayout();
            this.reflectorSplitContainer.Panel2.SuspendLayout();
            this.reflectorSplitContainer.SuspendLayout();
            this.processFlowLayoutPanel.SuspendLayout();
            this.reflectorGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.optionsSplitContainer)).BeginInit();
            this.optionsSplitContainer.Panel2.SuspendLayout();
            this.optionsSplitContainer.SuspendLayout();
            this.resetGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // extractLeaveCheckBox
            // 
            this.extractLeaveCheckBox.Checked = true;
            this.extractLeaveCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.extractLeaveCheckBox.Location = new System.Drawing.Point(6, 20);
            this.extractLeaveCheckBox.Name = "extractLeaveCheckBox";
            this.extractLeaveCheckBox.Size = new System.Drawing.Size(130, 30);
            this.extractLeaveCheckBox.TabIndex = 2;
            this.extractLeaveCheckBox.Text = "Leave Extracted Files";
            this.extractLeaveCheckBox.UseVisualStyleBackColor = true;
            // 
            // extractRepackSaveButton
            // 
            this.extractRepackSaveButton.Enabled = false;
            this.extractRepackSaveButton.Location = new System.Drawing.Point(143, 19);
            this.extractRepackSaveButton.Name = "extractRepackSaveButton";
            this.extractRepackSaveButton.Size = new System.Drawing.Size(120, 30);
            this.extractRepackSaveButton.TabIndex = 1;
            this.extractRepackSaveButton.Text = "Manual Modify";
            this.extractRepackSaveButton.UseVisualStyleBackColor = true;
            this.extractRepackSaveButton.Click += new System.EventHandler(this.extractRepackSaveButton_Click);
            // 
            // saveFileGroupBox
            // 
            this.saveFileGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.saveFileGroupBox.Controls.Add(this.saveFileTextBox);
            this.saveFileGroupBox.Controls.Add(this.saveFileChooseButton);
            this.saveFileGroupBox.Controls.Add(this.backupCheckBox);
            this.saveFileGroupBox.Location = new System.Drawing.Point(126, 43);
            this.saveFileGroupBox.Margin = new System.Windows.Forms.Padding(6);
            this.saveFileGroupBox.Name = "saveFileGroupBox";
            this.saveFileGroupBox.Size = new System.Drawing.Size(448, 60);
            this.saveFileGroupBox.TabIndex = 0;
            this.saveFileGroupBox.TabStop = false;
            this.saveFileGroupBox.Text = "Save File";
            // 
            // saveFileTextBox
            // 
            this.saveFileTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.saveFileTextBox.Location = new System.Drawing.Point(105, 25);
            this.saveFileTextBox.Name = "saveFileTextBox";
            this.saveFileTextBox.ReadOnly = true;
            this.saveFileTextBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.saveFileTextBox.Size = new System.Drawing.Size(211, 20);
            this.saveFileTextBox.TabIndex = 0;
            this.saveFileTextBox.TabStop = false;
            this.saveFileTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.saveFileTextBox.WordWrap = false;
            // 
            // saveFileChooseButton
            // 
            this.saveFileChooseButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.saveFileChooseButton.Location = new System.Drawing.Point(322, 19);
            this.saveFileChooseButton.Name = "saveFileChooseButton";
            this.saveFileChooseButton.Size = new System.Drawing.Size(120, 30);
            this.saveFileChooseButton.TabIndex = 0;
            this.saveFileChooseButton.Text = "Choose a Save File...";
            this.saveFileChooseButton.UseVisualStyleBackColor = true;
            this.saveFileChooseButton.Click += new System.EventHandler(this.saveFileChooseButton_Click);
            // 
            // backupCheckBox
            // 
            this.backupCheckBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.backupCheckBox.AutoSize = true;
            this.backupCheckBox.Checked = true;
            this.backupCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.backupCheckBox.Location = new System.Drawing.Point(6, 27);
            this.backupCheckBox.Name = "backupCheckBox";
            this.backupCheckBox.Size = new System.Drawing.Size(101, 17);
            this.backupCheckBox.TabIndex = 1;
            this.backupCheckBox.Text = "Backup Original";
            this.backupCheckBox.UseVisualStyleBackColor = true;
            // 
            // quickSkipSaveButton
            // 
            this.quickSkipSaveButton.Enabled = false;
            this.quickSkipSaveButton.Location = new System.Drawing.Point(269, 19);
            this.quickSkipSaveButton.Name = "quickSkipSaveButton";
            this.quickSkipSaveButton.Size = new System.Drawing.Size(120, 30);
            this.quickSkipSaveButton.TabIndex = 0;
            this.quickSkipSaveButton.Text = "Quick Modify";
            this.quickSkipSaveButton.UseVisualStyleBackColor = true;
            this.quickSkipSaveButton.Click += new System.EventHandler(this.quickSkipSaveButton_Click);
            // 
            // reflectorStopExtractCheckBox
            // 
            this.reflectorStopExtractCheckBox.Location = new System.Drawing.Point(143, 19);
            this.reflectorStopExtractCheckBox.Name = "reflectorStopExtractCheckBox";
            this.reflectorStopExtractCheckBox.Size = new System.Drawing.Size(125, 30);
            this.reflectorStopExtractCheckBox.TabIndex = 1;
            this.reflectorStopExtractCheckBox.Text = "Stop After Extracting";
            this.reflectorStopExtractCheckBox.UseVisualStyleBackColor = true;
            // 
            // reflectorStopRepackCheckBox
            // 
            this.reflectorStopRepackCheckBox.Checked = true;
            this.reflectorStopRepackCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.reflectorStopRepackCheckBox.Location = new System.Drawing.Point(6, 20);
            this.reflectorStopRepackCheckBox.Name = "reflectorStopRepackCheckBox";
            this.reflectorStopRepackCheckBox.Size = new System.Drawing.Size(128, 30);
            this.reflectorStopRepackCheckBox.TabIndex = 0;
            this.reflectorStopRepackCheckBox.Text = "Stop After Repacking";
            this.reflectorStopRepackCheckBox.UseVisualStyleBackColor = true;
            this.reflectorStopRepackCheckBox.CheckedChanged += new System.EventHandler(this.reflectorStopRepackCheckBox_CheckedChanged);
            // 
            // reflectorStopButton
            // 
            this.reflectorStopButton.Enabled = false;
            this.reflectorStopButton.Location = new System.Drawing.Point(269, 19);
            this.reflectorStopButton.Name = "reflectorStopButton";
            this.reflectorStopButton.Size = new System.Drawing.Size(120, 30);
            this.reflectorStopButton.TabIndex = 2;
            this.reflectorStopButton.Text = "Stop Reflector";
            this.reflectorStopButton.UseVisualStyleBackColor = true;
            this.reflectorStopButton.Click += new System.EventHandler(this.reflectorStopButton_Click);
            // 
            // reflectorOutputGroupBox
            // 
            this.reflectorOutputGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.reflectorOutputGroupBox.Controls.Add(this.reflectorTextBox);
            this.reflectorOutputGroupBox.Location = new System.Drawing.Point(3, 3);
            this.reflectorOutputGroupBox.MinimumSize = new System.Drawing.Size(150, 30);
            this.reflectorOutputGroupBox.Name = "reflectorOutputGroupBox";
            this.reflectorOutputGroupBox.Size = new System.Drawing.Size(159, 128);
            this.reflectorOutputGroupBox.TabIndex = 2;
            this.reflectorOutputGroupBox.TabStop = false;
            this.reflectorOutputGroupBox.Text = "Reflector Output";
            // 
            // reflectorTextBox
            // 
            this.reflectorTextBox.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.reflectorTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.reflectorTextBox.Location = new System.Drawing.Point(3, 16);
            this.reflectorTextBox.Multiline = true;
            this.reflectorTextBox.Name = "reflectorTextBox";
            this.reflectorTextBox.ReadOnly = true;
            this.reflectorTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.reflectorTextBox.Size = new System.Drawing.Size(153, 109);
            this.reflectorTextBox.TabIndex = 0;
            this.reflectorTextBox.Text = "Reflector not yet deployed.";
            this.reflectorTextBox.WordWrap = false;
            // 
            // verticalDividerLabel
            // 
            this.verticalDividerLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.verticalDividerLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.verticalDividerLabel.Location = new System.Drawing.Point(164, 0);
            this.verticalDividerLabel.Name = "verticalDividerLabel";
            this.verticalDividerLabel.Size = new System.Drawing.Size(2, 134);
            this.verticalDividerLabel.TabIndex = 2;
            // 
            // modifyHintLabel
            // 
            this.modifyHintLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.modifyHintLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.modifyHintLabel.Location = new System.Drawing.Point(40, 7);
            this.modifyHintLabel.MinimumSize = new System.Drawing.Size(2, 30);
            this.modifyHintLabel.Name = "modifyHintLabel";
            this.modifyHintLabel.Size = new System.Drawing.Size(500, 30);
            this.modifyHintLabel.TabIndex = 0;
            this.modifyHintLabel.Text = "1) Choose which Save File to modify.   2) Choose which Modifications to make.   3" +
    ") Start the Process.";
            this.modifyHintLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // modifyGroupBox
            // 
            this.modifyGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.modifyGroupBox.Controls.Add(this.extractRepackSaveButton);
            this.modifyGroupBox.Controls.Add(this.quickSkipSaveButton);
            this.modifyGroupBox.Controls.Add(this.extractLeaveCheckBox);
            this.modifyGroupBox.Location = new System.Drawing.Point(7, 3);
            this.modifyGroupBox.MaximumSize = new System.Drawing.Size(395, 0);
            this.modifyGroupBox.MinimumSize = new System.Drawing.Size(130, 60);
            this.modifyGroupBox.Name = "modifyGroupBox";
            this.modifyGroupBox.Size = new System.Drawing.Size(395, 60);
            this.modifyGroupBox.TabIndex = 0;
            this.modifyGroupBox.TabStop = false;
            this.modifyGroupBox.Text = "Modify Save";
            // 
            // reflectorSplitContainer
            // 
            this.reflectorSplitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.reflectorSplitContainer.Location = new System.Drawing.Point(3, 5);
            this.reflectorSplitContainer.Name = "reflectorSplitContainer";
            // 
            // reflectorSplitContainer.Panel1
            // 
            this.reflectorSplitContainer.Panel1.Controls.Add(this.verticalDividerLabel);
            this.reflectorSplitContainer.Panel1.Controls.Add(this.reflectorOutputGroupBox);
            this.reflectorSplitContainer.Panel1MinSize = 165;
            // 
            // reflectorSplitContainer.Panel2
            // 
            this.reflectorSplitContainer.Panel2.Controls.Add(this.processFlowLayoutPanel);
            this.reflectorSplitContainer.Panel2MinSize = 405;
            this.reflectorSplitContainer.Size = new System.Drawing.Size(574, 134);
            this.reflectorSplitContainer.SplitterDistance = 165;
            this.reflectorSplitContainer.TabIndex = 0;
            // 
            // processFlowLayoutPanel
            // 
            this.processFlowLayoutPanel.AutoScroll = true;
            this.processFlowLayoutPanel.Controls.Add(this.modifyGroupBox);
            this.processFlowLayoutPanel.Controls.Add(this.reflectorGroupBox);
            this.processFlowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.processFlowLayoutPanel.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.processFlowLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.processFlowLayoutPanel.Name = "processFlowLayoutPanel";
            this.processFlowLayoutPanel.Size = new System.Drawing.Size(405, 134);
            this.processFlowLayoutPanel.TabIndex = 0;
            // 
            // reflectorGroupBox
            // 
            this.reflectorGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.reflectorGroupBox.Controls.Add(this.reflectorStopButton);
            this.reflectorGroupBox.Controls.Add(this.reflectorStopExtractCheckBox);
            this.reflectorGroupBox.Controls.Add(this.reflectorStopRepackCheckBox);
            this.reflectorGroupBox.Location = new System.Drawing.Point(7, 69);
            this.reflectorGroupBox.MaximumSize = new System.Drawing.Size(525, 0);
            this.reflectorGroupBox.MinimumSize = new System.Drawing.Size(130, 60);
            this.reflectorGroupBox.Name = "reflectorGroupBox";
            this.reflectorGroupBox.Size = new System.Drawing.Size(395, 60);
            this.reflectorGroupBox.TabIndex = 1;
            this.reflectorGroupBox.TabStop = false;
            this.reflectorGroupBox.Text = "Reflector Process";
            // 
            // optionsSplitContainer
            // 
            this.optionsSplitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.optionsSplitContainer.Location = new System.Drawing.Point(0, 105);
            this.optionsSplitContainer.Name = "optionsSplitContainer";
            this.optionsSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.optionsSplitContainer.Panel1MinSize = 170;
            // 
            // optionsSplitContainer.Panel2
            // 
            this.optionsSplitContainer.Panel2.Controls.Add(this.horizontalDividerLabel);
            this.optionsSplitContainer.Panel2.Controls.Add(this.reflectorSplitContainer);
            this.optionsSplitContainer.Panel2MinSize = 140;
            this.optionsSplitContainer.Size = new System.Drawing.Size(580, 655);
            this.optionsSplitContainer.SplitterDistance = 509;
            this.optionsSplitContainer.TabIndex = 0;
            // 
            // horizontalDividerLabel
            // 
            this.horizontalDividerLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.horizontalDividerLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.horizontalDividerLabel.Location = new System.Drawing.Point(0, 0);
            this.horizontalDividerLabel.Name = "horizontalDividerLabel";
            this.horizontalDividerLabel.Size = new System.Drawing.Size(580, 2);
            this.horizontalDividerLabel.TabIndex = 2;
            // 
            // resetGroupBox
            // 
            this.resetGroupBox.Controls.Add(this.resetButton);
            this.resetGroupBox.Location = new System.Drawing.Point(6, 43);
            this.resetGroupBox.Name = "resetGroupBox";
            this.resetGroupBox.Size = new System.Drawing.Size(111, 60);
            this.resetGroupBox.TabIndex = 1;
            this.resetGroupBox.TabStop = false;
            this.resetGroupBox.Text = "Reset";
            // 
            // resetButton
            // 
            this.resetButton.Location = new System.Drawing.Point(6, 19);
            this.resetButton.Name = "resetButton";
            this.resetButton.Size = new System.Drawing.Size(99, 30);
            this.resetButton.TabIndex = 0;
            this.resetButton.Text = "Reset Choices";
            this.resetButton.UseVisualStyleBackColor = true;
            this.resetButton.Click += new System.EventHandler(this.resetButton_Click);
            // 
            // ModifyManagerControls
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.resetGroupBox);
            this.Controls.Add(this.optionsSplitContainer);
            this.Controls.Add(this.modifyHintLabel);
            this.Controls.Add(this.saveFileGroupBox);
            this.MinimumSize = new System.Drawing.Size(580, 290);
            this.Name = "ModifyManagerControls";
            this.Size = new System.Drawing.Size(580, 760);
            this.saveFileGroupBox.ResumeLayout(false);
            this.saveFileGroupBox.PerformLayout();
            this.reflectorOutputGroupBox.ResumeLayout(false);
            this.reflectorOutputGroupBox.PerformLayout();
            this.modifyGroupBox.ResumeLayout(false);
            this.reflectorSplitContainer.Panel1.ResumeLayout(false);
            this.reflectorSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.reflectorSplitContainer)).EndInit();
            this.reflectorSplitContainer.ResumeLayout(false);
            this.processFlowLayoutPanel.ResumeLayout(false);
            this.reflectorGroupBox.ResumeLayout(false);
            this.optionsSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.optionsSplitContainer)).EndInit();
            this.optionsSplitContainer.ResumeLayout(false);
            this.resetGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox saveFileGroupBox;
        private System.Windows.Forms.TextBox saveFileTextBox;
        private System.Windows.Forms.Button saveFileChooseButton;
        private System.Windows.Forms.CheckBox backupCheckBox;
        private System.Windows.Forms.GroupBox reflectorOutputGroupBox;
        private System.Windows.Forms.TextBox reflectorTextBox;
        private System.Windows.Forms.Button extractRepackSaveButton;
        private System.Windows.Forms.Button quickSkipSaveButton;
        private System.Windows.Forms.Label modifyHintLabel;
        private System.Windows.Forms.OpenFileDialog saveOpenFileDialog;
        private System.ComponentModel.BackgroundWorker modifySaveBackgroundWorker;
        private System.Windows.Forms.Button reflectorStopButton;
        private System.Windows.Forms.CheckBox extractLeaveCheckBox;
        private System.Windows.Forms.CheckBox reflectorStopRepackCheckBox;
        private System.Windows.Forms.CheckBox reflectorStopExtractCheckBox;
        private System.Windows.Forms.GroupBox modifyGroupBox;
        private System.Windows.Forms.GroupBox reflectorGroupBox;
        private System.Windows.Forms.FlowLayoutPanel processFlowLayoutPanel;
        private System.Windows.Forms.SplitContainer reflectorSplitContainer;
        private System.Windows.Forms.SplitContainer optionsSplitContainer;
        private System.Windows.Forms.Label horizontalDividerLabel;
        private System.Windows.Forms.Label verticalDividerLabel;
        private System.Windows.Forms.GroupBox resetGroupBox;
        private System.Windows.Forms.Button resetButton;
    }
}
