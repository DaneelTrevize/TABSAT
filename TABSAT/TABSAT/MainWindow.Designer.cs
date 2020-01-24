namespace TABSAT
{
    partial class MainWindow
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.popupLabel = new System.Windows.Forms.Label();
            this.cloudGroupBox = new System.Windows.Forms.GroupBox();
            this.cloudDisableLabel = new System.Windows.Forms.Label();
            this.updatesLabel = new System.Windows.Forms.Label();
            this.propertiesLabel = new System.Windows.Forms.Label();
            this.propertiesPictureBox = new System.Windows.Forms.PictureBox();
            this.updatesPictureBox = new System.Windows.Forms.PictureBox();
            this.popupPictureBox = new System.Windows.Forms.PictureBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.beforeUsingTabPage = new System.Windows.Forms.TabPage();
            this.mutantEditorTabPage = new System.Windows.Forms.TabPage();
            this.reflectorOutputLabel = new System.Windows.Forms.Label();
            this.reflectorTextBox = new System.Windows.Forms.TextBox();
            this.backupSaveLavel = new System.Windows.Forms.Label();
            this.backupSaveFileTextBox = new System.Windows.Forms.TextBox();
            this.repackedSaveLabel = new System.Windows.Forms.Label();
            this.repackedSaveFileTextBox = new System.Windows.Forms.TextBox();
            this.repackSaveButton = new System.Windows.Forms.Button();
            this.mutantModifyButton = new System.Windows.Forms.Button();
            this.extractedDirLlabel = new System.Windows.Forms.Label();
            this.extractedDirTextBox = new System.Windows.Forms.TextBox();
            this.tabDirTextBox = new System.Windows.Forms.TextBox();
            this.tabDirLabel = new System.Windows.Forms.Label();
            this.reflectorDirLabel = new System.Windows.Forms.Label();
            this.reflectorDirTextBox = new System.Windows.Forms.TextBox();
            this.extractSaveButton = new System.Windows.Forms.Button();
            this.corruptionGroupBox = new System.Windows.Forms.GroupBox();
            this.saveFileChooseButton = new System.Windows.Forms.Button();
            this.saveFileTextBox = new System.Windows.Forms.TextBox();
            this.openSaveFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.cloudGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.propertiesPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.updatesPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupPictureBox)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.beforeUsingTabPage.SuspendLayout();
            this.mutantEditorTabPage.SuspendLayout();
            this.corruptionGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // popupLabel
            // 
            this.popupLabel.AutoSize = true;
            this.popupLabel.Location = new System.Drawing.Point(6, 16);
            this.popupLabel.MaximumSize = new System.Drawing.Size(380, 0);
            this.popupLabel.Name = "popupLabel";
            this.popupLabel.Size = new System.Drawing.Size(340, 26);
            this.popupLabel.TabIndex = 3;
            this.popupLabel.Text = "During normal operation, the following \'Data Files Corrupted\' popup will temporar" +
    "ily appear. Please do not interact with it.";
            // 
            // cloudGroupBox
            // 
            this.cloudGroupBox.BackColor = System.Drawing.Color.Transparent;
            this.cloudGroupBox.Controls.Add(this.cloudDisableLabel);
            this.cloudGroupBox.Controls.Add(this.updatesLabel);
            this.cloudGroupBox.Controls.Add(this.propertiesLabel);
            this.cloudGroupBox.Controls.Add(this.propertiesPictureBox);
            this.cloudGroupBox.Controls.Add(this.updatesPictureBox);
            this.cloudGroupBox.Location = new System.Drawing.Point(6, 6);
            this.cloudGroupBox.Name = "cloudGroupBox";
            this.cloudGroupBox.Size = new System.Drawing.Size(810, 565);
            this.cloudGroupBox.TabIndex = 4;
            this.cloudGroupBox.TabStop = false;
            this.cloudGroupBox.Text = "Please ensure Steam Cloud sync for TAB is not enabled";
            // 
            // cloudDisableLabel
            // 
            this.cloudDisableLabel.AutoSize = true;
            this.cloudDisableLabel.Location = new System.Drawing.Point(18, 444);
            this.cloudDisableLabel.MaximumSize = new System.Drawing.Size(300, 0);
            this.cloudDisableLabel.Name = "cloudDisableLabel";
            this.cloudDisableLabel.Size = new System.Drawing.Size(295, 26);
            this.cloudDisableLabel.TabIndex = 4;
            this.cloudDisableLabel.Text = "Steam Cloud being disabled for TAB cannot be automatically verified at this time." +
    "";
            // 
            // updatesLabel
            // 
            this.updatesLabel.AutoSize = true;
            this.updatesLabel.Location = new System.Drawing.Point(316, 22);
            this.updatesLabel.Name = "updatesLabel";
            this.updatesLabel.Size = new System.Drawing.Size(124, 13);
            this.updatesLabel.TabIndex = 3;
            this.updatesLabel.Text = "2. View the Updates tab:";
            // 
            // propertiesLabel
            // 
            this.propertiesLabel.AutoSize = true;
            this.propertiesLabel.Location = new System.Drawing.Point(31, 22);
            this.propertiesLabel.Name = "propertiesLabel";
            this.propertiesLabel.Size = new System.Drawing.Size(197, 13);
            this.propertiesLabel.TabIndex = 2;
            this.propertiesLabel.Text = "1. Right-click TAB in your Steam Library:";
            // 
            // propertiesPictureBox
            // 
            this.propertiesPictureBox.Image = global::TABSAT.Properties.Resources.TAB_properties_menu;
            this.propertiesPictureBox.Location = new System.Drawing.Point(34, 38);
            this.propertiesPictureBox.Name = "propertiesPictureBox";
            this.propertiesPictureBox.Size = new System.Drawing.Size(255, 257);
            this.propertiesPictureBox.TabIndex = 0;
            this.propertiesPictureBox.TabStop = false;
            // 
            // updatesPictureBox
            // 
            this.updatesPictureBox.Image = global::TABSAT.Properties.Resources.TAB_updates_menu;
            this.updatesPictureBox.Location = new System.Drawing.Point(319, 38);
            this.updatesPictureBox.Name = "updatesPictureBox";
            this.updatesPictureBox.Size = new System.Drawing.Size(457, 520);
            this.updatesPictureBox.TabIndex = 1;
            this.updatesPictureBox.TabStop = false;
            // 
            // popupPictureBox
            // 
            this.popupPictureBox.Image = ((System.Drawing.Image)(resources.GetObject("popupPictureBox.Image")));
            this.popupPictureBox.Location = new System.Drawing.Point(9, 45);
            this.popupPictureBox.Name = "popupPictureBox";
            this.popupPictureBox.Size = new System.Drawing.Size(386, 152);
            this.popupPictureBox.TabIndex = 2;
            this.popupPictureBox.TabStop = false;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.beforeUsingTabPage);
            this.tabControl1.Controls.Add(this.mutantEditorTabPage);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(832, 597);
            this.tabControl1.TabIndex = 5;
            // 
            // beforeUsingTabPage
            // 
            this.beforeUsingTabPage.Controls.Add(this.cloudGroupBox);
            this.beforeUsingTabPage.Location = new System.Drawing.Point(4, 22);
            this.beforeUsingTabPage.Name = "beforeUsingTabPage";
            this.beforeUsingTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.beforeUsingTabPage.Size = new System.Drawing.Size(824, 571);
            this.beforeUsingTabPage.TabIndex = 0;
            this.beforeUsingTabPage.Text = "Before using TABSAT";
            this.beforeUsingTabPage.UseVisualStyleBackColor = true;
            // 
            // mutantEditorTabPage
            // 
            this.mutantEditorTabPage.Controls.Add(this.reflectorOutputLabel);
            this.mutantEditorTabPage.Controls.Add(this.reflectorTextBox);
            this.mutantEditorTabPage.Controls.Add(this.backupSaveLavel);
            this.mutantEditorTabPage.Controls.Add(this.backupSaveFileTextBox);
            this.mutantEditorTabPage.Controls.Add(this.repackedSaveLabel);
            this.mutantEditorTabPage.Controls.Add(this.repackedSaveFileTextBox);
            this.mutantEditorTabPage.Controls.Add(this.repackSaveButton);
            this.mutantEditorTabPage.Controls.Add(this.mutantModifyButton);
            this.mutantEditorTabPage.Controls.Add(this.extractedDirLlabel);
            this.mutantEditorTabPage.Controls.Add(this.extractedDirTextBox);
            this.mutantEditorTabPage.Controls.Add(this.tabDirTextBox);
            this.mutantEditorTabPage.Controls.Add(this.tabDirLabel);
            this.mutantEditorTabPage.Controls.Add(this.reflectorDirLabel);
            this.mutantEditorTabPage.Controls.Add(this.reflectorDirTextBox);
            this.mutantEditorTabPage.Controls.Add(this.extractSaveButton);
            this.mutantEditorTabPage.Controls.Add(this.corruptionGroupBox);
            this.mutantEditorTabPage.Controls.Add(this.saveFileChooseButton);
            this.mutantEditorTabPage.Controls.Add(this.saveFileTextBox);
            this.mutantEditorTabPage.Location = new System.Drawing.Point(4, 22);
            this.mutantEditorTabPage.Name = "mutantEditorTabPage";
            this.mutantEditorTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.mutantEditorTabPage.Size = new System.Drawing.Size(824, 571);
            this.mutantEditorTabPage.TabIndex = 1;
            this.mutantEditorTabPage.Text = "Modify Mutants in a Save File";
            this.mutantEditorTabPage.UseVisualStyleBackColor = true;
            // 
            // reflectorOutputLabel
            // 
            this.reflectorOutputLabel.AutoSize = true;
            this.reflectorOutputLabel.Location = new System.Drawing.Point(422, 226);
            this.reflectorOutputLabel.Name = "reflectorOutputLabel";
            this.reflectorOutputLabel.Size = new System.Drawing.Size(93, 13);
            this.reflectorOutputLabel.TabIndex = 21;
            this.reflectorOutputLabel.Text = "Reflector process:";
            // 
            // reflectorTextBox
            // 
            this.reflectorTextBox.Location = new System.Drawing.Point(425, 242);
            this.reflectorTextBox.Multiline = true;
            this.reflectorTextBox.Name = "reflectorTextBox";
            this.reflectorTextBox.ReadOnly = true;
            this.reflectorTextBox.Size = new System.Drawing.Size(386, 322);
            this.reflectorTextBox.TabIndex = 20;
            // 
            // backupSaveLavel
            // 
            this.backupSaveLavel.AutoSize = true;
            this.backupSaveLavel.Location = new System.Drawing.Point(6, 411);
            this.backupSaveLavel.Name = "backupSaveLavel";
            this.backupSaveLavel.Size = new System.Drawing.Size(134, 13);
            this.backupSaveLavel.TabIndex = 19;
            this.backupSaveLavel.Text = "Save File Backup location:";
            // 
            // backupSaveFileTextBox
            // 
            this.backupSaveFileTextBox.Location = new System.Drawing.Point(9, 427);
            this.backupSaveFileTextBox.Name = "backupSaveFileTextBox";
            this.backupSaveFileTextBox.ReadOnly = true;
            this.backupSaveFileTextBox.Size = new System.Drawing.Size(386, 20);
            this.backupSaveFileTextBox.TabIndex = 18;
            this.backupSaveFileTextBox.WordWrap = false;
            // 
            // repackedSaveLabel
            // 
            this.repackedSaveLabel.AutoSize = true;
            this.repackedSaveLabel.Location = new System.Drawing.Point(6, 451);
            this.repackedSaveLabel.Name = "repackedSaveLabel";
            this.repackedSaveLabel.Size = new System.Drawing.Size(147, 13);
            this.repackedSaveLabel.TabIndex = 17;
            this.repackedSaveLabel.Text = "Repacked Save File location:";
            // 
            // repackedSaveFileTextBox
            // 
            this.repackedSaveFileTextBox.Location = new System.Drawing.Point(9, 467);
            this.repackedSaveFileTextBox.Name = "repackedSaveFileTextBox";
            this.repackedSaveFileTextBox.ReadOnly = true;
            this.repackedSaveFileTextBox.Size = new System.Drawing.Size(386, 20);
            this.repackedSaveFileTextBox.TabIndex = 16;
            this.repackedSaveFileTextBox.WordWrap = false;
            // 
            // repackSaveButton
            // 
            this.repackSaveButton.Enabled = false;
            this.repackSaveButton.Location = new System.Drawing.Point(226, 386);
            this.repackSaveButton.Name = "repackSaveButton";
            this.repackSaveButton.Size = new System.Drawing.Size(169, 23);
            this.repackSaveButton.TabIndex = 15;
            this.repackSaveButton.Text = "Repack the extracted Save File";
            this.repackSaveButton.UseVisualStyleBackColor = true;
            this.repackSaveButton.Click += new System.EventHandler(this.repackSaveButton_Click);
            // 
            // mutantModifyButton
            // 
            this.mutantModifyButton.Enabled = false;
            this.mutantModifyButton.Location = new System.Drawing.Point(161, 329);
            this.mutantModifyButton.Name = "mutantModifyButton";
            this.mutantModifyButton.Size = new System.Drawing.Size(234, 23);
            this.mutantModifyButton.TabIndex = 14;
            this.mutantModifyButton.Text = "Modify the Mutants in the extracted Save File";
            this.mutantModifyButton.UseVisualStyleBackColor = true;
            this.mutantModifyButton.Click += new System.EventHandler(this.mutantModifyButton_Click);
            // 
            // extractedDirLlabel
            // 
            this.extractedDirLlabel.AutoSize = true;
            this.extractedDirLlabel.Location = new System.Drawing.Point(6, 255);
            this.extractedDirLlabel.Name = "extractedDirLlabel";
            this.extractedDirLlabel.Size = new System.Drawing.Size(142, 13);
            this.extractedDirLlabel.TabIndex = 13;
            this.extractedDirLlabel.Text = "Extracted Save File location:";
            // 
            // extractedDirTextBox
            // 
            this.extractedDirTextBox.Location = new System.Drawing.Point(9, 271);
            this.extractedDirTextBox.Name = "extractedDirTextBox";
            this.extractedDirTextBox.ReadOnly = true;
            this.extractedDirTextBox.Size = new System.Drawing.Size(386, 20);
            this.extractedDirTextBox.TabIndex = 12;
            this.extractedDirTextBox.WordWrap = false;
            // 
            // tabDirTextBox
            // 
            this.tabDirTextBox.Location = new System.Drawing.Point(9, 83);
            this.tabDirTextBox.Name = "tabDirTextBox";
            this.tabDirTextBox.ReadOnly = true;
            this.tabDirTextBox.Size = new System.Drawing.Size(386, 20);
            this.tabDirTextBox.TabIndex = 11;
            this.tabDirTextBox.WordWrap = false;
            // 
            // tabDirLabel
            // 
            this.tabDirLabel.AutoSize = true;
            this.tabDirLabel.Location = new System.Drawing.Point(6, 67);
            this.tabDirLabel.Name = "tabDirLabel";
            this.tabDirLabel.Size = new System.Drawing.Size(71, 13);
            this.tabDirLabel.TabIndex = 10;
            this.tabDirLabel.Text = "TAB location:";
            // 
            // reflectorDirLabel
            // 
            this.reflectorDirLabel.AutoSize = true;
            this.reflectorDirLabel.Location = new System.Drawing.Point(6, 19);
            this.reflectorDirLabel.Name = "reflectorDirLabel";
            this.reflectorDirLabel.Size = new System.Drawing.Size(93, 13);
            this.reflectorDirLabel.TabIndex = 9;
            this.reflectorDirLabel.Text = "Reflector location:";
            // 
            // reflectorDirTextBox
            // 
            this.reflectorDirTextBox.Location = new System.Drawing.Point(9, 35);
            this.reflectorDirTextBox.Name = "reflectorDirTextBox";
            this.reflectorDirTextBox.ReadOnly = true;
            this.reflectorDirTextBox.Size = new System.Drawing.Size(386, 20);
            this.reflectorDirTextBox.TabIndex = 8;
            this.reflectorDirTextBox.WordWrap = false;
            // 
            // extractSaveButton
            // 
            this.extractSaveButton.Enabled = false;
            this.extractSaveButton.Location = new System.Drawing.Point(275, 242);
            this.extractSaveButton.Name = "extractSaveButton";
            this.extractSaveButton.Size = new System.Drawing.Size(120, 23);
            this.extractSaveButton.TabIndex = 7;
            this.extractSaveButton.Text = "Extract the Save File";
            this.extractSaveButton.UseVisualStyleBackColor = true;
            this.extractSaveButton.Click += new System.EventHandler(this.extractSaveButton_Click);
            // 
            // corruptionGroupBox
            // 
            this.corruptionGroupBox.Controls.Add(this.popupLabel);
            this.corruptionGroupBox.Controls.Add(this.popupPictureBox);
            this.corruptionGroupBox.Location = new System.Drawing.Point(416, 19);
            this.corruptionGroupBox.Name = "corruptionGroupBox";
            this.corruptionGroupBox.Size = new System.Drawing.Size(403, 204);
            this.corruptionGroupBox.TabIndex = 6;
            this.corruptionGroupBox.TabStop = false;
            this.corruptionGroupBox.Text = "Notice:";
            // 
            // saveFileChooseButton
            // 
            this.saveFileChooseButton.Location = new System.Drawing.Point(9, 155);
            this.saveFileChooseButton.Name = "saveFileChooseButton";
            this.saveFileChooseButton.Size = new System.Drawing.Size(120, 23);
            this.saveFileChooseButton.TabIndex = 5;
            this.saveFileChooseButton.Text = "Choose a Save File...";
            this.saveFileChooseButton.UseVisualStyleBackColor = true;
            this.saveFileChooseButton.Click += new System.EventHandler(this.saveFileChooseButton_Click);
            // 
            // saveFileTextBox
            // 
            this.saveFileTextBox.Location = new System.Drawing.Point(135, 157);
            this.saveFileTextBox.Name = "saveFileTextBox";
            this.saveFileTextBox.ReadOnly = true;
            this.saveFileTextBox.Size = new System.Drawing.Size(260, 20);
            this.saveFileTextBox.TabIndex = 4;
            this.saveFileTextBox.WordWrap = false;
            // 
            // openSaveFileDialog
            // 
            this.openSaveFileDialog.Filter = "TAB Save Files|*.zxsav";
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(844, 610);
            this.Controls.Add(this.tabControl1);
            this.Name = "MainWindow";
            this.Text = "TABSAT: They Are Billions Save Automation Tool";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.cloudGroupBox.ResumeLayout(false);
            this.cloudGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.propertiesPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.updatesPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupPictureBox)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.beforeUsingTabPage.ResumeLayout(false);
            this.mutantEditorTabPage.ResumeLayout(false);
            this.mutantEditorTabPage.PerformLayout();
            this.corruptionGroupBox.ResumeLayout(false);
            this.corruptionGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox propertiesPictureBox;
        private System.Windows.Forms.PictureBox updatesPictureBox;
        private System.Windows.Forms.PictureBox popupPictureBox;
        private System.Windows.Forms.Label popupLabel;
        private System.Windows.Forms.GroupBox cloudGroupBox;
        private System.Windows.Forms.Label propertiesLabel;
        private System.Windows.Forms.Label updatesLabel;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage beforeUsingTabPage;
        private System.Windows.Forms.TabPage mutantEditorTabPage;
        private System.Windows.Forms.Button saveFileChooseButton;
        private System.Windows.Forms.TextBox saveFileTextBox;
        private System.Windows.Forms.OpenFileDialog openSaveFileDialog;
        private System.Windows.Forms.GroupBox corruptionGroupBox;
        private System.Windows.Forms.Button extractSaveButton;
        private System.Windows.Forms.TextBox reflectorDirTextBox;
        private System.Windows.Forms.Label reflectorDirLabel;
        private System.Windows.Forms.TextBox tabDirTextBox;
        private System.Windows.Forms.Label tabDirLabel;
        private System.Windows.Forms.Label extractedDirLlabel;
        private System.Windows.Forms.TextBox extractedDirTextBox;
        private System.Windows.Forms.Label cloudDisableLabel;
        private System.Windows.Forms.Label repackedSaveLabel;
        private System.Windows.Forms.TextBox repackedSaveFileTextBox;
        private System.Windows.Forms.Button repackSaveButton;
        private System.Windows.Forms.Button mutantModifyButton;
        private System.Windows.Forms.Label backupSaveLavel;
        private System.Windows.Forms.TextBox backupSaveFileTextBox;
        private System.Windows.Forms.Label reflectorOutputLabel;
        private System.Windows.Forms.TextBox reflectorTextBox;
    }
}

