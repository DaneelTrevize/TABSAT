namespace TABSAT
{
    partial class MapSelectorControl
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
            this.mapSelectorGroupBox = new System.Windows.Forms.GroupBox();
            this.mapSelectorGroupBox.SuspendLayout();
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
            this.extractedSaveTextBox.Size = new System.Drawing.Size(374, 20);
            this.extractedSaveTextBox.TabIndex = 1;
            this.extractedSaveTextBox.TabStop = false;
            this.extractedSaveTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.extractedSaveTextBox.WordWrap = false;
            // 
            // extractedSaveChooseButton
            // 
            this.extractedSaveChooseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.extractedSaveChooseButton.AutoSize = true;
            this.extractedSaveChooseButton.Location = new System.Drawing.Point(386, 17);
            this.extractedSaveChooseButton.Name = "extractedSaveChooseButton";
            this.extractedSaveChooseButton.Size = new System.Drawing.Size(152, 23);
            this.extractedSaveChooseButton.TabIndex = 2;
            this.extractedSaveChooseButton.Text = "Choose an extracted Save...";
            this.extractedSaveChooseButton.UseVisualStyleBackColor = true;
            this.extractedSaveChooseButton.Click += new System.EventHandler(this.extractedSaveChooseButton_Click);
            // 
            // mapFolderBrowserDialog
            // 
            this.mapFolderBrowserDialog.Description = "Select an extracter Save folder.";
            this.mapFolderBrowserDialog.ShowNewFolderButton = false;
            // 
            // mapSelectorGroupBox
            // 
            this.mapSelectorGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mapSelectorGroupBox.Controls.Add(this.extractedSaveTextBox);
            this.mapSelectorGroupBox.Controls.Add(this.extractedSaveChooseButton);
            this.mapSelectorGroupBox.Location = new System.Drawing.Point(3, 3);
            this.mapSelectorGroupBox.Name = "mapSelectorGroupBox";
            this.mapSelectorGroupBox.Size = new System.Drawing.Size(544, 50);
            this.mapSelectorGroupBox.TabIndex = 3;
            this.mapSelectorGroupBox.TabStop = false;
            this.mapSelectorGroupBox.Text = "Selected Save to View";
            // 
            // MapSelectorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mapSelectorGroupBox);
            this.Name = "MapSelectorControl";
            this.Size = new System.Drawing.Size(550, 65);
            this.mapSelectorGroupBox.ResumeLayout(false);
            this.mapSelectorGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox extractedSaveTextBox;
        private System.Windows.Forms.Button extractedSaveChooseButton;
        private System.Windows.Forms.FolderBrowserDialog mapFolderBrowserDialog;
        private System.Windows.Forms.GroupBox mapSelectorGroupBox;
    }
}
