namespace TABSAT
{
    partial class SaveSelectorControl
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
            this.extractedSaveTextBox = new System.Windows.Forms.TextBox();
            this.extractedSaveChooseButton = new System.Windows.Forms.Button();
            this.mapFolderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.saveSelectorGroupBox = new System.Windows.Forms.GroupBox();
            this.viewMapButton = new System.Windows.Forms.Button();
            this.inspectionOptionsGroupBox = new System.Windows.Forms.GroupBox();
            this.saveSelectorGroupBox.SuspendLayout();
            this.inspectionOptionsGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // extractedSaveTextBox
            // 
            this.extractedSaveTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.extractedSaveTextBox.Location = new System.Drawing.Point(6, 19);
            this.extractedSaveTextBox.Name = "extractedSaveTextBox";
            this.extractedSaveTextBox.ReadOnly = true;
            this.extractedSaveTextBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.extractedSaveTextBox.Size = new System.Drawing.Size(373, 20);
            this.extractedSaveTextBox.TabIndex = 1;
            this.extractedSaveTextBox.TabStop = false;
            this.extractedSaveTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.extractedSaveTextBox.WordWrap = false;
            // 
            // extractedSaveChooseButton
            // 
            this.extractedSaveChooseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.extractedSaveChooseButton.AutoSize = true;
            this.extractedSaveChooseButton.Location = new System.Drawing.Point(385, 13);
            this.extractedSaveChooseButton.Name = "extractedSaveChooseButton";
            this.extractedSaveChooseButton.Size = new System.Drawing.Size(153, 30);
            this.extractedSaveChooseButton.TabIndex = 2;
            this.extractedSaveChooseButton.Text = "Choose an Extracted Save...";
            this.extractedSaveChooseButton.UseVisualStyleBackColor = true;
            this.extractedSaveChooseButton.Click += new System.EventHandler(this.extractedSaveChooseButton_Click);
            // 
            // mapFolderBrowserDialog
            // 
            this.mapFolderBrowserDialog.Description = "Select an extracter Save folder.";
            this.mapFolderBrowserDialog.ShowNewFolderButton = false;
            // 
            // saveSelectorGroupBox
            // 
            this.saveSelectorGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.saveSelectorGroupBox.Controls.Add(this.extractedSaveTextBox);
            this.saveSelectorGroupBox.Controls.Add(this.extractedSaveChooseButton);
            this.saveSelectorGroupBox.Location = new System.Drawing.Point(3, 3);
            this.saveSelectorGroupBox.Name = "saveSelectorGroupBox";
            this.saveSelectorGroupBox.Size = new System.Drawing.Size(544, 52);
            this.saveSelectorGroupBox.TabIndex = 3;
            this.saveSelectorGroupBox.TabStop = false;
            this.saveSelectorGroupBox.Text = "Extracted Save";
            // 
            // viewMapButton
            // 
            this.viewMapButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.viewMapButton.Enabled = false;
            this.viewMapButton.Location = new System.Drawing.Point(385, 18);
            this.viewMapButton.Name = "viewMapButton";
            this.viewMapButton.Size = new System.Drawing.Size(153, 30);
            this.viewMapButton.TabIndex = 4;
            this.viewMapButton.Text = "View Map";
            this.viewMapButton.UseVisualStyleBackColor = true;
            this.viewMapButton.Click += new System.EventHandler(this.viewMapButton_Click);
            // 
            // inspectionOptionsGroupBox
            // 
            this.inspectionOptionsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.inspectionOptionsGroupBox.Controls.Add(this.viewMapButton);
            this.inspectionOptionsGroupBox.Location = new System.Drawing.Point(3, 58);
            this.inspectionOptionsGroupBox.Name = "inspectionOptionsGroupBox";
            this.inspectionOptionsGroupBox.Size = new System.Drawing.Size(544, 58);
            this.inspectionOptionsGroupBox.TabIndex = 5;
            this.inspectionOptionsGroupBox.TabStop = false;
            this.inspectionOptionsGroupBox.Text = "Save Inspection Options";
            // 
            // SaveSelectorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.inspectionOptionsGroupBox);
            this.Controls.Add(this.saveSelectorGroupBox);
            this.Name = "SaveSelectorControl";
            this.Size = new System.Drawing.Size(550, 126);
            this.saveSelectorGroupBox.ResumeLayout(false);
            this.saveSelectorGroupBox.PerformLayout();
            this.inspectionOptionsGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox extractedSaveTextBox;
        private System.Windows.Forms.Button extractedSaveChooseButton;
        private System.Windows.Forms.FolderBrowserDialog mapFolderBrowserDialog;
        private System.Windows.Forms.GroupBox saveSelectorGroupBox;
        private System.Windows.Forms.Button viewMapButton;
        private System.Windows.Forms.GroupBox inspectionOptionsGroupBox;
    }
}
