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
            this.saveEditorTabPage = new System.Windows.Forms.TabPage();
            this.filesLineLabel = new System.Windows.Forms.Label();
            this.reflectorLineLabel = new System.Windows.Forms.Label();
            this.hintLabel = new System.Windows.Forms.Label();
            this.reflectorGroupBox = new System.Windows.Forms.GroupBox();
            this.reflectorExtractRadioButton = new System.Windows.Forms.RadioButton();
            this.reflectorRepackRadioButton = new System.Windows.Forms.RadioButton();
            this.reflectorExitRadioButton = new System.Windows.Forms.RadioButton();
            this.manualGroupBox = new System.Windows.Forms.GroupBox();
            this.skipRepackButton = new System.Windows.Forms.Button();
            this.repackSaveButton = new System.Windows.Forms.Button();
            this.extractSaveButton = new System.Windows.Forms.Button();
            this.mutantGroupBox = new System.Windows.Forms.GroupBox();
            this.mutantMoveGlobalComboBox = new System.Windows.Forms.ComboBox();
            this.mutantMoveCCLabel = new System.Windows.Forms.Label();
            this.mutantMoveWhatComboBox = new System.Windows.Forms.ComboBox();
            this.mutantsMoveRadio = new System.Windows.Forms.RadioButton();
            this.mutantsRemoveRadio = new System.Windows.Forms.RadioButton();
            this.mutantsNothingRadio = new System.Windows.Forms.RadioButton();
            this.modifyGroupBox = new System.Windows.Forms.GroupBox();
            this.modifySaveButton = new System.Windows.Forms.Button();
            this.extractGroupBox = new System.Windows.Forms.GroupBox();
            this.extractManualRadioButton = new System.Windows.Forms.RadioButton();
            this.extractLeaveRadioButton = new System.Windows.Forms.RadioButton();
            this.extractTidyRadioButton = new System.Windows.Forms.RadioButton();
            this.fogGroupBox = new System.Windows.Forms.GroupBox();
            this.fogNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.reduceFogRadioButton = new System.Windows.Forms.RadioButton();
            this.showFullRadioButton = new System.Windows.Forms.RadioButton();
            this.removeFogRadioButton = new System.Windows.Forms.RadioButton();
            this.leaveFogRadioButton = new System.Windows.Forms.RadioButton();
            this.themeGroupBox = new System.Windows.Forms.GroupBox();
            this.themeCheckBox = new System.Windows.Forms.CheckBox();
            this.themeComboBox = new System.Windows.Forms.ComboBox();
            this.corruptionGroupBox = new System.Windows.Forms.GroupBox();
            this.saveFileGroupBox = new System.Windows.Forms.GroupBox();
            this.saveFileTextBox = new System.Windows.Forms.TextBox();
            this.saveFileChooseButton = new System.Windows.Forms.Button();
            this.backupCheckBox = new System.Windows.Forms.CheckBox();
            this.processGroupBox = new System.Windows.Forms.GroupBox();
            this.reflectorTextBox = new System.Windows.Forms.TextBox();
            this.openSaveFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.statusTextBox = new System.Windows.Forms.TextBox();
            this.cloudGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.propertiesPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.updatesPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupPictureBox)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.beforeUsingTabPage.SuspendLayout();
            this.saveEditorTabPage.SuspendLayout();
            this.reflectorGroupBox.SuspendLayout();
            this.manualGroupBox.SuspendLayout();
            this.mutantGroupBox.SuspendLayout();
            this.modifyGroupBox.SuspendLayout();
            this.extractGroupBox.SuspendLayout();
            this.fogGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fogNumericUpDown)).BeginInit();
            this.themeGroupBox.SuspendLayout();
            this.corruptionGroupBox.SuspendLayout();
            this.saveFileGroupBox.SuspendLayout();
            this.processGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // popupLabel
            // 
            this.popupLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.popupLabel.Location = new System.Drawing.Point(9, 13);
            this.popupLabel.Name = "popupLabel";
            this.popupLabel.Size = new System.Drawing.Size(386, 30);
            this.popupLabel.TabIndex = 3;
            this.popupLabel.Text = "During normal operation, a \'Data Files Corrupted\' popup will temporarily appear.\r" +
    "\nPlease do not interact with it.";
            this.popupLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            this.cloudGroupBox.Text = "Please ensure Steam Cloud sync for TAB is NOT enabled";
            // 
            // cloudDisableLabel
            // 
            this.cloudDisableLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cloudDisableLabel.Location = new System.Drawing.Point(73, 415);
            this.cloudDisableLabel.Name = "cloudDisableLabel";
            this.cloudDisableLabel.Size = new System.Drawing.Size(216, 42);
            this.cloudDisableLabel.TabIndex = 4;
            this.cloudDisableLabel.Text = "Steam Cloud being disabled for TAB cannot be automatically verified at this time." +
    "";
            this.cloudDisableLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // updatesLabel
            // 
            this.updatesLabel.AutoSize = true;
            this.updatesLabel.Location = new System.Drawing.Point(316, 22);
            this.updatesLabel.Name = "updatesLabel";
            this.updatesLabel.Size = new System.Drawing.Size(124, 13);
            this.updatesLabel.TabIndex = 3;
            this.updatesLabel.Text = "2) View the Updates tab:";
            // 
            // propertiesLabel
            // 
            this.propertiesLabel.AutoSize = true;
            this.propertiesLabel.Location = new System.Drawing.Point(31, 22);
            this.propertiesLabel.Name = "propertiesLabel";
            this.propertiesLabel.Size = new System.Drawing.Size(197, 13);
            this.propertiesLabel.TabIndex = 2;
            this.propertiesLabel.Text = "1) Right-click TAB in your Steam Library:";
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
            this.popupPictureBox.Location = new System.Drawing.Point(9, 47);
            this.popupPictureBox.Name = "popupPictureBox";
            this.popupPictureBox.Size = new System.Drawing.Size(386, 152);
            this.popupPictureBox.TabIndex = 2;
            this.popupPictureBox.TabStop = false;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.beforeUsingTabPage);
            this.tabControl1.Controls.Add(this.saveEditorTabPage);
            this.tabControl1.Location = new System.Drawing.Point(8, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(828, 597);
            this.tabControl1.TabIndex = 5;
            // 
            // beforeUsingTabPage
            // 
            this.beforeUsingTabPage.Controls.Add(this.cloudGroupBox);
            this.beforeUsingTabPage.Location = new System.Drawing.Point(4, 22);
            this.beforeUsingTabPage.Name = "beforeUsingTabPage";
            this.beforeUsingTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.beforeUsingTabPage.Size = new System.Drawing.Size(820, 571);
            this.beforeUsingTabPage.TabIndex = 0;
            this.beforeUsingTabPage.Text = " Before using TABSAT ";
            this.beforeUsingTabPage.UseVisualStyleBackColor = true;
            // 
            // saveEditorTabPage
            // 
            this.saveEditorTabPage.Controls.Add(this.filesLineLabel);
            this.saveEditorTabPage.Controls.Add(this.reflectorLineLabel);
            this.saveEditorTabPage.Controls.Add(this.hintLabel);
            this.saveEditorTabPage.Controls.Add(this.reflectorGroupBox);
            this.saveEditorTabPage.Controls.Add(this.manualGroupBox);
            this.saveEditorTabPage.Controls.Add(this.mutantGroupBox);
            this.saveEditorTabPage.Controls.Add(this.modifyGroupBox);
            this.saveEditorTabPage.Controls.Add(this.extractGroupBox);
            this.saveEditorTabPage.Controls.Add(this.fogGroupBox);
            this.saveEditorTabPage.Controls.Add(this.themeGroupBox);
            this.saveEditorTabPage.Controls.Add(this.corruptionGroupBox);
            this.saveEditorTabPage.Controls.Add(this.saveFileGroupBox);
            this.saveEditorTabPage.Controls.Add(this.processGroupBox);
            this.saveEditorTabPage.Location = new System.Drawing.Point(4, 22);
            this.saveEditorTabPage.Name = "saveEditorTabPage";
            this.saveEditorTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.saveEditorTabPage.Size = new System.Drawing.Size(820, 571);
            this.saveEditorTabPage.TabIndex = 1;
            this.saveEditorTabPage.Text = " Modify Save Files ";
            this.saveEditorTabPage.UseVisualStyleBackColor = true;
            // 
            // filesLineLabel
            // 
            this.filesLineLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.filesLineLabel.Location = new System.Drawing.Point(9, 168);
            this.filesLineLabel.Name = "filesLineLabel";
            this.filesLineLabel.Size = new System.Drawing.Size(805, 2);
            this.filesLineLabel.TabIndex = 33;
            // 
            // reflectorLineLabel
            // 
            this.reflectorLineLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.reflectorLineLabel.Location = new System.Drawing.Point(9, 349);
            this.reflectorLineLabel.Name = "reflectorLineLabel";
            this.reflectorLineLabel.Size = new System.Drawing.Size(805, 2);
            this.reflectorLineLabel.TabIndex = 32;
            // 
            // hintLabel
            // 
            this.hintLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.hintLabel.Location = new System.Drawing.Point(20, 14);
            this.hintLabel.Name = "hintLabel";
            this.hintLabel.Size = new System.Drawing.Size(529, 35);
            this.hintLabel.TabIndex = 31;
            this.hintLabel.Text = "1) Choose which Modifications to make.    2) Choose which Save File to modify.   " +
    " 3) Start the Process.";
            this.hintLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // reflectorGroupBox
            // 
            this.reflectorGroupBox.Controls.Add(this.reflectorExtractRadioButton);
            this.reflectorGroupBox.Controls.Add(this.reflectorRepackRadioButton);
            this.reflectorGroupBox.Controls.Add(this.reflectorExitRadioButton);
            this.reflectorGroupBox.Location = new System.Drawing.Point(9, 360);
            this.reflectorGroupBox.Name = "reflectorGroupBox";
            this.reflectorGroupBox.Size = new System.Drawing.Size(398, 51);
            this.reflectorGroupBox.TabIndex = 27;
            this.reflectorGroupBox.TabStop = false;
            this.reflectorGroupBox.Text = "Reflector Stops:";
            // 
            // reflectorExtractRadioButton
            // 
            this.reflectorExtractRadioButton.AutoSize = true;
            this.reflectorExtractRadioButton.Enabled = false;
            this.reflectorExtractRadioButton.Location = new System.Drawing.Point(249, 19);
            this.reflectorExtractRadioButton.Name = "reflectorExtractRadioButton";
            this.reflectorExtractRadioButton.Size = new System.Drawing.Size(143, 17);
            this.reflectorExtractRadioButton.TabIndex = 17;
            this.reflectorExtractRadioButton.Text = "After extracting Save File";
            this.reflectorExtractRadioButton.UseVisualStyleBackColor = true;
            // 
            // reflectorRepackRadioButton
            // 
            this.reflectorRepackRadioButton.AutoSize = true;
            this.reflectorRepackRadioButton.Checked = true;
            this.reflectorRepackRadioButton.Location = new System.Drawing.Point(88, 19);
            this.reflectorRepackRadioButton.Name = "reflectorRepackRadioButton";
            this.reflectorRepackRadioButton.Size = new System.Drawing.Size(144, 17);
            this.reflectorRepackRadioButton.TabIndex = 16;
            this.reflectorRepackRadioButton.TabStop = true;
            this.reflectorRepackRadioButton.Text = "After repacking Save File";
            this.reflectorRepackRadioButton.UseVisualStyleBackColor = true;
            // 
            // reflectorExitRadioButton
            // 
            this.reflectorExitRadioButton.AutoSize = true;
            this.reflectorExitRadioButton.Location = new System.Drawing.Point(11, 19);
            this.reflectorExitRadioButton.Name = "reflectorExitRadioButton";
            this.reflectorExitRadioButton.Size = new System.Drawing.Size(59, 17);
            this.reflectorExitRadioButton.TabIndex = 15;
            this.reflectorExitRadioButton.Text = "On Exit";
            this.reflectorExitRadioButton.UseVisualStyleBackColor = true;
            // 
            // manualGroupBox
            // 
            this.manualGroupBox.Controls.Add(this.skipRepackButton);
            this.manualGroupBox.Controls.Add(this.repackSaveButton);
            this.manualGroupBox.Controls.Add(this.extractSaveButton);
            this.manualGroupBox.Enabled = false;
            this.manualGroupBox.Location = new System.Drawing.Point(206, 243);
            this.manualGroupBox.Name = "manualGroupBox";
            this.manualGroupBox.Size = new System.Drawing.Size(397, 91);
            this.manualGroupBox.TabIndex = 26;
            this.manualGroupBox.TabStop = false;
            this.manualGroupBox.Text = "Manual Editing";
            // 
            // skipRepackButton
            // 
            this.skipRepackButton.Enabled = false;
            this.skipRepackButton.Location = new System.Drawing.Point(262, 35);
            this.skipRepackButton.Name = "skipRepackButton";
            this.skipRepackButton.Size = new System.Drawing.Size(118, 30);
            this.skipRepackButton.TabIndex = 21;
            this.skipRepackButton.Text = "Skip Repacking";
            this.skipRepackButton.UseVisualStyleBackColor = true;
            this.skipRepackButton.Click += new System.EventHandler(this.skipRepackButton_Click);
            // 
            // repackSaveButton
            // 
            this.repackSaveButton.Enabled = false;
            this.repackSaveButton.Location = new System.Drawing.Point(138, 35);
            this.repackSaveButton.Name = "repackSaveButton";
            this.repackSaveButton.Size = new System.Drawing.Size(118, 30);
            this.repackSaveButton.TabIndex = 20;
            this.repackSaveButton.Text = "Repack the Save File";
            this.repackSaveButton.UseVisualStyleBackColor = true;
            this.repackSaveButton.Click += new System.EventHandler(this.repackSaveButton_Click);
            // 
            // extractSaveButton
            // 
            this.extractSaveButton.Enabled = false;
            this.extractSaveButton.Location = new System.Drawing.Point(14, 35);
            this.extractSaveButton.Name = "extractSaveButton";
            this.extractSaveButton.Size = new System.Drawing.Size(118, 30);
            this.extractSaveButton.TabIndex = 19;
            this.extractSaveButton.Text = "Extract the Save File";
            this.extractSaveButton.UseVisualStyleBackColor = true;
            this.extractSaveButton.Click += new System.EventHandler(this.extractSaveButton_Click);
            // 
            // mutantGroupBox
            // 
            this.mutantGroupBox.Controls.Add(this.mutantMoveGlobalComboBox);
            this.mutantGroupBox.Controls.Add(this.mutantMoveCCLabel);
            this.mutantGroupBox.Controls.Add(this.mutantMoveWhatComboBox);
            this.mutantGroupBox.Controls.Add(this.mutantsMoveRadio);
            this.mutantGroupBox.Controls.Add(this.mutantsRemoveRadio);
            this.mutantGroupBox.Controls.Add(this.mutantsNothingRadio);
            this.mutantGroupBox.Location = new System.Drawing.Point(9, 60);
            this.mutantGroupBox.Name = "mutantGroupBox";
            this.mutantGroupBox.Size = new System.Drawing.Size(398, 94);
            this.mutantGroupBox.TabIndex = 21;
            this.mutantGroupBox.TabStop = false;
            this.mutantGroupBox.Text = "Mutants";
            // 
            // mutantMoveGlobalComboBox
            // 
            this.mutantMoveGlobalComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.mutantMoveGlobalComboBox.Enabled = false;
            this.mutantMoveGlobalComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.mutantMoveGlobalComboBox.FormattingEnabled = true;
            this.mutantMoveGlobalComboBox.Items.AddRange(new object[] {
            "anywhere on the map",
            "per compass quadrant"});
            this.mutantMoveGlobalComboBox.Location = new System.Drawing.Point(217, 63);
            this.mutantMoveGlobalComboBox.Name = "mutantMoveGlobalComboBox";
            this.mutantMoveGlobalComboBox.Size = new System.Drawing.Size(131, 21);
            this.mutantMoveGlobalComboBox.TabIndex = 5;
            // 
            // mutantMoveCCLabel
            // 
            this.mutantMoveCCLabel.AutoSize = true;
            this.mutantMoveCCLabel.Location = new System.Drawing.Point(173, 67);
            this.mutantMoveCCLabel.Name = "mutantMoveCCLabel";
            this.mutantMoveCCLabel.Size = new System.Drawing.Size(44, 13);
            this.mutantMoveCCLabel.TabIndex = 5;
            this.mutantMoveCCLabel.Text = "from CC";
            // 
            // mutantMoveWhatComboBox
            // 
            this.mutantMoveWhatComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.mutantMoveWhatComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.mutantMoveWhatComboBox.FormattingEnabled = true;
            this.mutantMoveWhatComboBox.Items.AddRange(new object[] {
            "Giant",
            "Mutant"});
            this.mutantMoveWhatComboBox.Location = new System.Drawing.Point(110, 63);
            this.mutantMoveWhatComboBox.Name = "mutantMoveWhatComboBox";
            this.mutantMoveWhatComboBox.Size = new System.Drawing.Size(57, 21);
            this.mutantMoveWhatComboBox.TabIndex = 4;
            // 
            // mutantsMoveRadio
            // 
            this.mutantsMoveRadio.AutoSize = true;
            this.mutantsMoveRadio.Location = new System.Drawing.Point(11, 65);
            this.mutantsMoveRadio.Name = "mutantsMoveRadio";
            this.mutantsMoveRadio.Size = new System.Drawing.Size(102, 17);
            this.mutantsMoveRadio.TabIndex = 3;
            this.mutantsMoveRadio.Text = "Move to farthest";
            this.mutantsMoveRadio.UseVisualStyleBackColor = true;
            // 
            // mutantsRemoveRadio
            // 
            this.mutantsRemoveRadio.AutoSize = true;
            this.mutantsRemoveRadio.Location = new System.Drawing.Point(11, 42);
            this.mutantsRemoveRadio.Name = "mutantsRemoveRadio";
            this.mutantsRemoveRadio.Size = new System.Drawing.Size(79, 17);
            this.mutantsRemoveRadio.TabIndex = 2;
            this.mutantsRemoveRadio.Text = "Remove All";
            this.mutantsRemoveRadio.UseVisualStyleBackColor = true;
            // 
            // mutantsNothingRadio
            // 
            this.mutantsNothingRadio.AutoSize = true;
            this.mutantsNothingRadio.Checked = true;
            this.mutantsNothingRadio.Location = new System.Drawing.Point(11, 19);
            this.mutantsNothingRadio.Name = "mutantsNothingRadio";
            this.mutantsNothingRadio.Size = new System.Drawing.Size(90, 17);
            this.mutantsNothingRadio.TabIndex = 1;
            this.mutantsNothingRadio.TabStop = true;
            this.mutantsNothingRadio.Text = "Do not modify";
            this.mutantsNothingRadio.UseVisualStyleBackColor = true;
            // 
            // modifyGroupBox
            // 
            this.modifyGroupBox.Controls.Add(this.modifySaveButton);
            this.modifyGroupBox.Location = new System.Drawing.Point(652, 243);
            this.modifyGroupBox.Name = "modifyGroupBox";
            this.modifyGroupBox.Size = new System.Drawing.Size(162, 91);
            this.modifyGroupBox.TabIndex = 28;
            this.modifyGroupBox.TabStop = false;
            this.modifyGroupBox.Text = "Quick Mode";
            // 
            // modifySaveButton
            // 
            this.modifySaveButton.Location = new System.Drawing.Point(16, 30);
            this.modifySaveButton.Name = "modifySaveButton";
            this.modifySaveButton.Size = new System.Drawing.Size(131, 41);
            this.modifySaveButton.TabIndex = 18;
            this.modifySaveButton.Text = "Modify the Save File";
            this.modifySaveButton.UseVisualStyleBackColor = true;
            this.modifySaveButton.Click += new System.EventHandler(this.modifySaveButton_Click);
            // 
            // extractGroupBox
            // 
            this.extractGroupBox.Controls.Add(this.extractManualRadioButton);
            this.extractGroupBox.Controls.Add(this.extractLeaveRadioButton);
            this.extractGroupBox.Controls.Add(this.extractTidyRadioButton);
            this.extractGroupBox.Location = new System.Drawing.Point(9, 243);
            this.extractGroupBox.Name = "extractGroupBox";
            this.extractGroupBox.Size = new System.Drawing.Size(143, 91);
            this.extractGroupBox.TabIndex = 24;
            this.extractGroupBox.TabStop = false;
            this.extractGroupBox.Text = "Extracted Files";
            // 
            // extractManualRadioButton
            // 
            this.extractManualRadioButton.AutoSize = true;
            this.extractManualRadioButton.Location = new System.Drawing.Point(11, 65);
            this.extractManualRadioButton.Name = "extractManualRadioButton";
            this.extractManualRadioButton.Size = new System.Drawing.Size(125, 17);
            this.extractManualRadioButton.TabIndex = 14;
            this.extractManualRadioButton.Text = "Manual Editing Mode";
            this.extractManualRadioButton.UseVisualStyleBackColor = true;
            this.extractManualRadioButton.CheckedChanged += new System.EventHandler(this.extractRadioButtons_CheckedChanged);
            // 
            // extractLeaveRadioButton
            // 
            this.extractLeaveRadioButton.AutoSize = true;
            this.extractLeaveRadioButton.Location = new System.Drawing.Point(11, 42);
            this.extractLeaveRadioButton.Name = "extractLeaveRadioButton";
            this.extractLeaveRadioButton.Size = new System.Drawing.Size(55, 17);
            this.extractLeaveRadioButton.TabIndex = 13;
            this.extractLeaveRadioButton.Text = "Leave";
            this.extractLeaveRadioButton.UseVisualStyleBackColor = true;
            this.extractLeaveRadioButton.CheckedChanged += new System.EventHandler(this.extractRadioButtons_CheckedChanged);
            // 
            // extractTidyRadioButton
            // 
            this.extractTidyRadioButton.AutoSize = true;
            this.extractTidyRadioButton.Checked = true;
            this.extractTidyRadioButton.Location = new System.Drawing.Point(11, 19);
            this.extractTidyRadioButton.Name = "extractTidyRadioButton";
            this.extractTidyRadioButton.Size = new System.Drawing.Size(74, 17);
            this.extractTidyRadioButton.TabIndex = 12;
            this.extractTidyRadioButton.TabStop = true;
            this.extractTidyRadioButton.Text = "Tidy Away";
            this.extractTidyRadioButton.UseVisualStyleBackColor = true;
            this.extractTidyRadioButton.CheckedChanged += new System.EventHandler(this.extractRadioButtons_CheckedChanged);
            // 
            // fogGroupBox
            // 
            this.fogGroupBox.Controls.Add(this.fogNumericUpDown);
            this.fogGroupBox.Controls.Add(this.reduceFogRadioButton);
            this.fogGroupBox.Controls.Add(this.showFullRadioButton);
            this.fogGroupBox.Controls.Add(this.removeFogRadioButton);
            this.fogGroupBox.Controls.Add(this.leaveFogRadioButton);
            this.fogGroupBox.Location = new System.Drawing.Point(413, 60);
            this.fogGroupBox.Name = "fogGroupBox";
            this.fogGroupBox.Size = new System.Drawing.Size(401, 94);
            this.fogGroupBox.TabIndex = 22;
            this.fogGroupBox.TabStop = false;
            this.fogGroupBox.Text = "Fog of War";
            // 
            // fogNumericUpDown
            // 
            this.fogNumericUpDown.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.fogNumericUpDown.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.fogNumericUpDown.Location = new System.Drawing.Point(199, 65);
            this.fogNumericUpDown.Maximum = new decimal(new int[] {
            90,
            0,
            0,
            0});
            this.fogNumericUpDown.Minimum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.fogNumericUpDown.Name = "fogNumericUpDown";
            this.fogNumericUpDown.Size = new System.Drawing.Size(35, 20);
            this.fogNumericUpDown.TabIndex = 13;
            this.fogNumericUpDown.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // reduceFogRadioButton
            // 
            this.reduceFogRadioButton.AutoSize = true;
            this.reduceFogRadioButton.Enabled = false;
            this.reduceFogRadioButton.Location = new System.Drawing.Point(11, 65);
            this.reduceFogRadioButton.Name = "reduceFogRadioButton";
            this.reduceFogRadioButton.Size = new System.Drawing.Size(194, 17);
            this.reduceFogRadioButton.TabIndex = 12;
            this.reduceFogRadioButton.TabStop = true;
            this.reduceFogRadioButton.Text = "Clear a circle around the CC, radius:";
            this.reduceFogRadioButton.UseVisualStyleBackColor = true;
            // 
            // showFullRadioButton
            // 
            this.showFullRadioButton.AutoSize = true;
            this.showFullRadioButton.Location = new System.Drawing.Point(239, 19);
            this.showFullRadioButton.Name = "showFullRadioButton";
            this.showFullRadioButton.Size = new System.Drawing.Size(125, 17);
            this.showFullRadioButton.TabIndex = 11;
            this.showFullRadioButton.Text = "Grant Full Map Vision";
            this.showFullRadioButton.UseVisualStyleBackColor = true;
            // 
            // removeFogRadioButton
            // 
            this.removeFogRadioButton.AutoSize = true;
            this.removeFogRadioButton.Location = new System.Drawing.Point(11, 42);
            this.removeFogRadioButton.Name = "removeFogRadioButton";
            this.removeFogRadioButton.Size = new System.Drawing.Size(79, 17);
            this.removeFogRadioButton.TabIndex = 10;
            this.removeFogRadioButton.Text = "Remove All";
            this.removeFogRadioButton.UseVisualStyleBackColor = true;
            // 
            // leaveFogRadioButton
            // 
            this.leaveFogRadioButton.AutoSize = true;
            this.leaveFogRadioButton.Checked = true;
            this.leaveFogRadioButton.Location = new System.Drawing.Point(11, 19);
            this.leaveFogRadioButton.Name = "leaveFogRadioButton";
            this.leaveFogRadioButton.Size = new System.Drawing.Size(90, 17);
            this.leaveFogRadioButton.TabIndex = 9;
            this.leaveFogRadioButton.TabStop = true;
            this.leaveFogRadioButton.Text = "Do not modify";
            this.leaveFogRadioButton.UseVisualStyleBackColor = true;
            // 
            // themeGroupBox
            // 
            this.themeGroupBox.Controls.Add(this.themeCheckBox);
            this.themeGroupBox.Controls.Add(this.themeComboBox);
            this.themeGroupBox.Location = new System.Drawing.Point(596, 6);
            this.themeGroupBox.Name = "themeGroupBox";
            this.themeGroupBox.Size = new System.Drawing.Size(218, 48);
            this.themeGroupBox.TabIndex = 30;
            this.themeGroupBox.TabStop = false;
            this.themeGroupBox.Text = "Theme";
            // 
            // themeCheckBox
            // 
            this.themeCheckBox.AutoSize = true;
            this.themeCheckBox.Location = new System.Drawing.Point(11, 19);
            this.themeCheckBox.Name = "themeCheckBox";
            this.themeCheckBox.Size = new System.Drawing.Size(78, 17);
            this.themeCheckBox.TabIndex = 7;
            this.themeCheckBox.Text = "Change to:";
            this.themeCheckBox.UseVisualStyleBackColor = true;
            // 
            // themeComboBox
            // 
            this.themeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.themeComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.themeComboBox.FormattingEnabled = true;
            this.themeComboBox.Location = new System.Drawing.Point(86, 16);
            this.themeComboBox.Name = "themeComboBox";
            this.themeComboBox.Size = new System.Drawing.Size(117, 21);
            this.themeComboBox.TabIndex = 8;
            // 
            // corruptionGroupBox
            // 
            this.corruptionGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.corruptionGroupBox.Controls.Add(this.popupLabel);
            this.corruptionGroupBox.Controls.Add(this.popupPictureBox);
            this.corruptionGroupBox.Location = new System.Drawing.Point(413, 360);
            this.corruptionGroupBox.Name = "corruptionGroupBox";
            this.corruptionGroupBox.Size = new System.Drawing.Size(401, 205);
            this.corruptionGroupBox.TabIndex = 6;
            this.corruptionGroupBox.TabStop = false;
            // 
            // saveFileGroupBox
            // 
            this.saveFileGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.saveFileGroupBox.Controls.Add(this.saveFileTextBox);
            this.saveFileGroupBox.Controls.Add(this.saveFileChooseButton);
            this.saveFileGroupBox.Controls.Add(this.backupCheckBox);
            this.saveFileGroupBox.Location = new System.Drawing.Point(9, 179);
            this.saveFileGroupBox.Name = "saveFileGroupBox";
            this.saveFileGroupBox.Size = new System.Drawing.Size(805, 58);
            this.saveFileGroupBox.TabIndex = 23;
            this.saveFileGroupBox.TabStop = false;
            this.saveFileGroupBox.Text = "Save File";
            // 
            // saveFileTextBox
            // 
            this.saveFileTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.saveFileTextBox.Location = new System.Drawing.Point(143, 24);
            this.saveFileTextBox.Name = "saveFileTextBox";
            this.saveFileTextBox.ReadOnly = true;
            this.saveFileTextBox.Size = new System.Drawing.Size(475, 20);
            this.saveFileTextBox.TabIndex = 10;
            this.saveFileTextBox.WordWrap = false;
            // 
            // saveFileChooseButton
            // 
            this.saveFileChooseButton.Location = new System.Drawing.Point(659, 18);
            this.saveFileChooseButton.Name = "saveFileChooseButton";
            this.saveFileChooseButton.Size = new System.Drawing.Size(131, 30);
            this.saveFileChooseButton.TabIndex = 9;
            this.saveFileChooseButton.Text = "Choose a Save File...";
            this.saveFileChooseButton.UseVisualStyleBackColor = true;
            this.saveFileChooseButton.Click += new System.EventHandler(this.saveFileChooseButton_Click);
            // 
            // backupCheckBox
            // 
            this.backupCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.backupCheckBox.AutoSize = true;
            this.backupCheckBox.Checked = true;
            this.backupCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.backupCheckBox.Location = new System.Drawing.Point(11, 27);
            this.backupCheckBox.Name = "backupCheckBox";
            this.backupCheckBox.Size = new System.Drawing.Size(99, 17);
            this.backupCheckBox.TabIndex = 11;
            this.backupCheckBox.Text = "Backup original";
            this.backupCheckBox.UseVisualStyleBackColor = true;
            // 
            // processGroupBox
            // 
            this.processGroupBox.Controls.Add(this.reflectorTextBox);
            this.processGroupBox.Location = new System.Drawing.Point(9, 417);
            this.processGroupBox.Name = "processGroupBox";
            this.processGroupBox.Size = new System.Drawing.Size(398, 148);
            this.processGroupBox.TabIndex = 29;
            this.processGroupBox.TabStop = false;
            this.processGroupBox.Text = "Reflector process output";
            // 
            // reflectorTextBox
            // 
            this.reflectorTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.reflectorTextBox.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.reflectorTextBox.Location = new System.Drawing.Point(6, 19);
            this.reflectorTextBox.Multiline = true;
            this.reflectorTextBox.Name = "reflectorTextBox";
            this.reflectorTextBox.ReadOnly = true;
            this.reflectorTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.reflectorTextBox.Size = new System.Drawing.Size(386, 123);
            this.reflectorTextBox.TabIndex = 22;
            // 
            // openSaveFileDialog
            // 
            this.openSaveFileDialog.Filter = "TAB Save Files|*.zxsav";
            // 
            // statusTextBox
            // 
            this.statusTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.statusTextBox.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.statusTextBox.Location = new System.Drawing.Point(8, 615);
            this.statusTextBox.Multiline = true;
            this.statusTextBox.Name = "statusTextBox";
            this.statusTextBox.ReadOnly = true;
            this.statusTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.statusTextBox.Size = new System.Drawing.Size(826, 64);
            this.statusTextBox.TabIndex = 6;
            this.statusTextBox.Text = "TABSAT Log:\r\n";
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(844, 686);
            this.Controls.Add(this.statusTextBox);
            this.Controls.Add(this.tabControl1);
            this.Name = "MainWindow";
            this.Text = "TABSAT - They Are Billions Save Automation Tool";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.cloudGroupBox.ResumeLayout(false);
            this.cloudGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.propertiesPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.updatesPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupPictureBox)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.beforeUsingTabPage.ResumeLayout(false);
            this.saveEditorTabPage.ResumeLayout(false);
            this.reflectorGroupBox.ResumeLayout(false);
            this.reflectorGroupBox.PerformLayout();
            this.manualGroupBox.ResumeLayout(false);
            this.mutantGroupBox.ResumeLayout(false);
            this.mutantGroupBox.PerformLayout();
            this.modifyGroupBox.ResumeLayout(false);
            this.extractGroupBox.ResumeLayout(false);
            this.extractGroupBox.PerformLayout();
            this.fogGroupBox.ResumeLayout(false);
            this.fogGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fogNumericUpDown)).EndInit();
            this.themeGroupBox.ResumeLayout(false);
            this.themeGroupBox.PerformLayout();
            this.corruptionGroupBox.ResumeLayout(false);
            this.saveFileGroupBox.ResumeLayout(false);
            this.saveFileGroupBox.PerformLayout();
            this.processGroupBox.ResumeLayout(false);
            this.processGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

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
        private System.Windows.Forms.TabPage saveEditorTabPage;
        private System.Windows.Forms.Button saveFileChooseButton;
        private System.Windows.Forms.TextBox saveFileTextBox;
        private System.Windows.Forms.OpenFileDialog openSaveFileDialog;
        private System.Windows.Forms.GroupBox corruptionGroupBox;
        private System.Windows.Forms.Button extractSaveButton;
        private System.Windows.Forms.Label cloudDisableLabel;
        private System.Windows.Forms.Button repackSaveButton;
        private System.Windows.Forms.TextBox reflectorTextBox;
        private System.Windows.Forms.CheckBox backupCheckBox;
        private System.Windows.Forms.GroupBox mutantGroupBox;
        private System.Windows.Forms.RadioButton mutantsMoveRadio;
        private System.Windows.Forms.RadioButton mutantsRemoveRadio;
        private System.Windows.Forms.RadioButton mutantsNothingRadio;
        private System.Windows.Forms.Label mutantMoveCCLabel;
        private System.Windows.Forms.ComboBox mutantMoveWhatComboBox;
        private System.Windows.Forms.ComboBox mutantMoveGlobalComboBox;
        private System.Windows.Forms.GroupBox fogGroupBox;
        private System.Windows.Forms.CheckBox themeCheckBox;
        private System.Windows.Forms.TextBox statusTextBox;
        private System.Windows.Forms.ComboBox themeComboBox;
        private System.Windows.Forms.GroupBox extractGroupBox;
        private System.Windows.Forms.RadioButton extractLeaveRadioButton;
        private System.Windows.Forms.RadioButton extractTidyRadioButton;
        private System.Windows.Forms.RadioButton extractManualRadioButton;
        private System.Windows.Forms.GroupBox manualGroupBox;
        private System.Windows.Forms.GroupBox saveFileGroupBox;
        private System.Windows.Forms.Button modifySaveButton;
        private System.Windows.Forms.Button skipRepackButton;
        private System.Windows.Forms.GroupBox reflectorGroupBox;
        private System.Windows.Forms.RadioButton reflectorExtractRadioButton;
        private System.Windows.Forms.RadioButton reflectorRepackRadioButton;
        private System.Windows.Forms.RadioButton reflectorExitRadioButton;
        private System.Windows.Forms.GroupBox modifyGroupBox;
        private System.Windows.Forms.RadioButton showFullRadioButton;
        private System.Windows.Forms.RadioButton removeFogRadioButton;
        private System.Windows.Forms.RadioButton leaveFogRadioButton;
        private System.Windows.Forms.NumericUpDown fogNumericUpDown;
        private System.Windows.Forms.RadioButton reduceFogRadioButton;
        private System.Windows.Forms.GroupBox processGroupBox;
        private System.Windows.Forms.GroupBox themeGroupBox;
        private System.Windows.Forms.Label hintLabel;
        private System.Windows.Forms.Label reflectorLineLabel;
        private System.Windows.Forms.Label filesLineLabel;
    }
}

