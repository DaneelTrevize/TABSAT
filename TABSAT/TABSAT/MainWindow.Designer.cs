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
            this.reflectorGroupBox = new System.Windows.Forms.GroupBox();
            this.reflectorExtractRadioButton = new System.Windows.Forms.RadioButton();
            this.reflectorRepackRadioButton = new System.Windows.Forms.RadioButton();
            this.reflectorExitRadioButton = new System.Windows.Forms.RadioButton();
            this.manualGroupBox = new System.Windows.Forms.GroupBox();
            this.skipRepackButton = new System.Windows.Forms.Button();
            this.repackSaveButton = new System.Windows.Forms.Button();
            this.extractSaveButton = new System.Windows.Forms.Button();
            this.extractGroupBox = new System.Windows.Forms.GroupBox();
            this.extractManualRadioButton = new System.Windows.Forms.RadioButton();
            this.extractLeaveRadioButton = new System.Windows.Forms.RadioButton();
            this.extractTidyRadioButton = new System.Windows.Forms.RadioButton();
            this.terrainGroupBox = new System.Windows.Forms.GroupBox();
            this.themeComboBox = new System.Windows.Forms.ComboBox();
            this.themeCheckBox = new System.Windows.Forms.CheckBox();
            this.showFullCheckBox = new System.Windows.Forms.CheckBox();
            this.mutantGroupBox = new System.Windows.Forms.GroupBox();
            this.mutantMoveGlobalComboBox = new System.Windows.Forms.ComboBox();
            this.mutantMoveCCLabel = new System.Windows.Forms.Label();
            this.mutantMoveWhatComboBox = new System.Windows.Forms.ComboBox();
            this.mutantsMoveRadio = new System.Windows.Forms.RadioButton();
            this.mutantsRemoveRadio = new System.Windows.Forms.RadioButton();
            this.mutantsNothingRadio = new System.Windows.Forms.RadioButton();
            this.reflectorOutputLabel = new System.Windows.Forms.Label();
            this.reflectorTextBox = new System.Windows.Forms.TextBox();
            this.corruptionGroupBox = new System.Windows.Forms.GroupBox();
            this.saveFileGroupBox = new System.Windows.Forms.GroupBox();
            this.saveFileTextBox = new System.Windows.Forms.TextBox();
            this.saveFileChooseButton = new System.Windows.Forms.Button();
            this.backupCheckBox = new System.Windows.Forms.CheckBox();
            this.modifyGroupBox = new System.Windows.Forms.GroupBox();
            this.modifySaveButton = new System.Windows.Forms.Button();
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
            this.extractGroupBox.SuspendLayout();
            this.terrainGroupBox.SuspendLayout();
            this.mutantGroupBox.SuspendLayout();
            this.corruptionGroupBox.SuspendLayout();
            this.saveFileGroupBox.SuspendLayout();
            this.modifyGroupBox.SuspendLayout();
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
            this.cloudDisableLabel.Location = new System.Drawing.Point(73, 423);
            this.cloudDisableLabel.MaximumSize = new System.Drawing.Size(220, 0);
            this.cloudDisableLabel.Name = "cloudDisableLabel";
            this.cloudDisableLabel.Size = new System.Drawing.Size(216, 26);
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
            this.beforeUsingTabPage.Text = "Before using TABSAT";
            this.beforeUsingTabPage.UseVisualStyleBackColor = true;
            // 
            // saveEditorTabPage
            // 
            this.saveEditorTabPage.Controls.Add(this.reflectorGroupBox);
            this.saveEditorTabPage.Controls.Add(this.manualGroupBox);
            this.saveEditorTabPage.Controls.Add(this.extractGroupBox);
            this.saveEditorTabPage.Controls.Add(this.terrainGroupBox);
            this.saveEditorTabPage.Controls.Add(this.mutantGroupBox);
            this.saveEditorTabPage.Controls.Add(this.reflectorOutputLabel);
            this.saveEditorTabPage.Controls.Add(this.reflectorTextBox);
            this.saveEditorTabPage.Controls.Add(this.corruptionGroupBox);
            this.saveEditorTabPage.Controls.Add(this.saveFileGroupBox);
            this.saveEditorTabPage.Controls.Add(this.modifyGroupBox);
            this.saveEditorTabPage.Location = new System.Drawing.Point(4, 22);
            this.saveEditorTabPage.Name = "saveEditorTabPage";
            this.saveEditorTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.saveEditorTabPage.Size = new System.Drawing.Size(820, 571);
            this.saveEditorTabPage.TabIndex = 1;
            this.saveEditorTabPage.Text = "Modify Save Files";
            this.saveEditorTabPage.UseVisualStyleBackColor = true;
            // 
            // reflectorGroupBox
            // 
            this.reflectorGroupBox.Controls.Add(this.reflectorExtractRadioButton);
            this.reflectorGroupBox.Controls.Add(this.reflectorRepackRadioButton);
            this.reflectorGroupBox.Controls.Add(this.reflectorExitRadioButton);
            this.reflectorGroupBox.Location = new System.Drawing.Point(10, 403);
            this.reflectorGroupBox.Name = "reflectorGroupBox";
            this.reflectorGroupBox.Size = new System.Drawing.Size(210, 91);
            this.reflectorGroupBox.TabIndex = 27;
            this.reflectorGroupBox.TabStop = false;
            this.reflectorGroupBox.Text = "Reflector stop options:";
            // 
            // reflectorExtractRadioButton
            // 
            this.reflectorExtractRadioButton.AutoSize = true;
            this.reflectorExtractRadioButton.Enabled = false;
            this.reflectorExtractRadioButton.Location = new System.Drawing.Point(17, 65);
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
            this.reflectorRepackRadioButton.Location = new System.Drawing.Point(17, 42);
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
            this.reflectorExitRadioButton.Enabled = false;
            this.reflectorExitRadioButton.Location = new System.Drawing.Point(17, 19);
            this.reflectorExitRadioButton.Name = "reflectorExitRadioButton";
            this.reflectorExitRadioButton.Size = new System.Drawing.Size(124, 17);
            this.reflectorExitRadioButton.TabIndex = 15;
            this.reflectorExitRadioButton.Text = "Upon application exit";
            this.reflectorExitRadioButton.UseVisualStyleBackColor = true;
            // 
            // manualGroupBox
            // 
            this.manualGroupBox.Controls.Add(this.skipRepackButton);
            this.manualGroupBox.Controls.Add(this.repackSaveButton);
            this.manualGroupBox.Controls.Add(this.extractSaveButton);
            this.manualGroupBox.Enabled = false;
            this.manualGroupBox.Location = new System.Drawing.Point(241, 371);
            this.manualGroupBox.Name = "manualGroupBox";
            this.manualGroupBox.Size = new System.Drawing.Size(154, 123);
            this.manualGroupBox.TabIndex = 26;
            this.manualGroupBox.TabStop = false;
            this.manualGroupBox.Text = "Manual Editing";
            // 
            // skipRepackButton
            // 
            this.skipRepackButton.Enabled = false;
            this.skipRepackButton.Location = new System.Drawing.Point(20, 91);
            this.skipRepackButton.Name = "skipRepackButton";
            this.skipRepackButton.Size = new System.Drawing.Size(118, 23);
            this.skipRepackButton.TabIndex = 21;
            this.skipRepackButton.Text = "Skip Repacking";
            this.skipRepackButton.UseVisualStyleBackColor = true;
            this.skipRepackButton.Click += new System.EventHandler(this.skipRepackButton_Click);
            // 
            // repackSaveButton
            // 
            this.repackSaveButton.Enabled = false;
            this.repackSaveButton.Location = new System.Drawing.Point(20, 62);
            this.repackSaveButton.Name = "repackSaveButton";
            this.repackSaveButton.Size = new System.Drawing.Size(118, 23);
            this.repackSaveButton.TabIndex = 20;
            this.repackSaveButton.Text = "Repack the Save File";
            this.repackSaveButton.UseVisualStyleBackColor = true;
            this.repackSaveButton.Click += new System.EventHandler(this.repackSaveButton_Click);
            // 
            // extractSaveButton
            // 
            this.extractSaveButton.Enabled = false;
            this.extractSaveButton.Location = new System.Drawing.Point(20, 19);
            this.extractSaveButton.Name = "extractSaveButton";
            this.extractSaveButton.Size = new System.Drawing.Size(118, 23);
            this.extractSaveButton.TabIndex = 19;
            this.extractSaveButton.Text = "Extract the Save File";
            this.extractSaveButton.UseVisualStyleBackColor = true;
            this.extractSaveButton.Click += new System.EventHandler(this.extractSaveButton_Click);
            // 
            // extractGroupBox
            // 
            this.extractGroupBox.Controls.Add(this.extractManualRadioButton);
            this.extractGroupBox.Controls.Add(this.extractLeaveRadioButton);
            this.extractGroupBox.Controls.Add(this.extractTidyRadioButton);
            this.extractGroupBox.Location = new System.Drawing.Point(10, 306);
            this.extractGroupBox.Name = "extractGroupBox";
            this.extractGroupBox.Size = new System.Drawing.Size(210, 91);
            this.extractGroupBox.TabIndex = 24;
            this.extractGroupBox.TabStop = false;
            this.extractGroupBox.Text = "Extracted Files options:";
            // 
            // extractManualRadioButton
            // 
            this.extractManualRadioButton.AutoSize = true;
            this.extractManualRadioButton.Location = new System.Drawing.Point(17, 65);
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
            this.extractLeaveRadioButton.Checked = true;
            this.extractLeaveRadioButton.Location = new System.Drawing.Point(17, 42);
            this.extractLeaveRadioButton.Name = "extractLeaveRadioButton";
            this.extractLeaveRadioButton.Size = new System.Drawing.Size(55, 17);
            this.extractLeaveRadioButton.TabIndex = 13;
            this.extractLeaveRadioButton.TabStop = true;
            this.extractLeaveRadioButton.Text = "Leave";
            this.extractLeaveRadioButton.UseVisualStyleBackColor = true;
            this.extractLeaveRadioButton.CheckedChanged += new System.EventHandler(this.extractRadioButtons_CheckedChanged);
            // 
            // extractTidyRadioButton
            // 
            this.extractTidyRadioButton.AutoSize = true;
            this.extractTidyRadioButton.Enabled = false;
            this.extractTidyRadioButton.Location = new System.Drawing.Point(17, 19);
            this.extractTidyRadioButton.Name = "extractTidyRadioButton";
            this.extractTidyRadioButton.Size = new System.Drawing.Size(65, 17);
            this.extractTidyRadioButton.TabIndex = 12;
            this.extractTidyRadioButton.Text = "Remove";
            this.extractTidyRadioButton.UseVisualStyleBackColor = true;
            this.extractTidyRadioButton.CheckedChanged += new System.EventHandler(this.extractRadioButtons_CheckedChanged);
            // 
            // terrainGroupBox
            // 
            this.terrainGroupBox.Controls.Add(this.themeComboBox);
            this.terrainGroupBox.Controls.Add(this.themeCheckBox);
            this.terrainGroupBox.Controls.Add(this.showFullCheckBox);
            this.terrainGroupBox.Location = new System.Drawing.Point(10, 121);
            this.terrainGroupBox.Name = "terrainGroupBox";
            this.terrainGroupBox.Size = new System.Drawing.Size(385, 71);
            this.terrainGroupBox.TabIndex = 22;
            this.terrainGroupBox.TabStop = false;
            this.terrainGroupBox.Text = "Terrain options:";
            // 
            // themeComboBox
            // 
            this.themeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.themeComboBox.FormattingEnabled = true;
            this.themeComboBox.Location = new System.Drawing.Point(133, 40);
            this.themeComboBox.Name = "themeComboBox";
            this.themeComboBox.Size = new System.Drawing.Size(118, 21);
            this.themeComboBox.TabIndex = 8;
            // 
            // themeCheckBox
            // 
            this.themeCheckBox.AutoSize = true;
            this.themeCheckBox.Location = new System.Drawing.Point(17, 42);
            this.themeCheckBox.Name = "themeCheckBox";
            this.themeCheckBox.Size = new System.Drawing.Size(110, 17);
            this.themeCheckBox.TabIndex = 7;
            this.themeCheckBox.Text = "Change theme to:";
            this.themeCheckBox.UseVisualStyleBackColor = true;
            // 
            // showFullCheckBox
            // 
            this.showFullCheckBox.AutoSize = true;
            this.showFullCheckBox.Location = new System.Drawing.Point(17, 19);
            this.showFullCheckBox.Name = "showFullCheckBox";
            this.showFullCheckBox.Size = new System.Drawing.Size(126, 17);
            this.showFullCheckBox.TabIndex = 6;
            this.showFullCheckBox.Text = "Reveal all of the map";
            this.showFullCheckBox.UseVisualStyleBackColor = true;
            // 
            // mutantGroupBox
            // 
            this.mutantGroupBox.Controls.Add(this.mutantMoveGlobalComboBox);
            this.mutantGroupBox.Controls.Add(this.mutantMoveCCLabel);
            this.mutantGroupBox.Controls.Add(this.mutantMoveWhatComboBox);
            this.mutantGroupBox.Controls.Add(this.mutantsMoveRadio);
            this.mutantGroupBox.Controls.Add(this.mutantsRemoveRadio);
            this.mutantGroupBox.Controls.Add(this.mutantsNothingRadio);
            this.mutantGroupBox.Location = new System.Drawing.Point(10, 19);
            this.mutantGroupBox.Name = "mutantGroupBox";
            this.mutantGroupBox.Size = new System.Drawing.Size(385, 96);
            this.mutantGroupBox.TabIndex = 21;
            this.mutantGroupBox.TabStop = false;
            this.mutantGroupBox.Text = "Mutant options:";
            // 
            // mutantMoveGlobalComboBox
            // 
            this.mutantMoveGlobalComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.mutantMoveGlobalComboBox.Enabled = false;
            this.mutantMoveGlobalComboBox.FormattingEnabled = true;
            this.mutantMoveGlobalComboBox.Items.AddRange(new object[] {
            "anywhere on the map",
            "per compass quadrant"});
            this.mutantMoveGlobalComboBox.Location = new System.Drawing.Point(238, 64);
            this.mutantMoveGlobalComboBox.Name = "mutantMoveGlobalComboBox";
            this.mutantMoveGlobalComboBox.Size = new System.Drawing.Size(131, 21);
            this.mutantMoveGlobalComboBox.TabIndex = 5;
            // 
            // mutantMoveCCLabel
            // 
            this.mutantMoveCCLabel.AutoSize = true;
            this.mutantMoveCCLabel.Location = new System.Drawing.Point(188, 67);
            this.mutantMoveCCLabel.Name = "mutantMoveCCLabel";
            this.mutantMoveCCLabel.Size = new System.Drawing.Size(44, 13);
            this.mutantMoveCCLabel.TabIndex = 5;
            this.mutantMoveCCLabel.Text = "from CC";
            // 
            // mutantMoveWhatComboBox
            // 
            this.mutantMoveWhatComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.mutantMoveWhatComboBox.Enabled = false;
            this.mutantMoveWhatComboBox.FormattingEnabled = true;
            this.mutantMoveWhatComboBox.Items.AddRange(new object[] {
            "Giant",
            "Mutant"});
            this.mutantMoveWhatComboBox.Location = new System.Drawing.Point(119, 64);
            this.mutantMoveWhatComboBox.Name = "mutantMoveWhatComboBox";
            this.mutantMoveWhatComboBox.Size = new System.Drawing.Size(63, 21);
            this.mutantMoveWhatComboBox.TabIndex = 4;
            // 
            // mutantsMoveRadio
            // 
            this.mutantsMoveRadio.AutoSize = true;
            this.mutantsMoveRadio.Location = new System.Drawing.Point(17, 65);
            this.mutantsMoveRadio.Name = "mutantsMoveRadio";
            this.mutantsMoveRadio.Size = new System.Drawing.Size(102, 17);
            this.mutantsMoveRadio.TabIndex = 3;
            this.mutantsMoveRadio.Text = "Move to farthest";
            this.mutantsMoveRadio.UseVisualStyleBackColor = true;
            // 
            // mutantsRemoveRadio
            // 
            this.mutantsRemoveRadio.AutoSize = true;
            this.mutantsRemoveRadio.Enabled = false;
            this.mutantsRemoveRadio.Location = new System.Drawing.Point(17, 42);
            this.mutantsRemoveRadio.Name = "mutantsRemoveRadio";
            this.mutantsRemoveRadio.Size = new System.Drawing.Size(65, 17);
            this.mutantsRemoveRadio.TabIndex = 2;
            this.mutantsRemoveRadio.Text = "Remove";
            this.mutantsRemoveRadio.UseVisualStyleBackColor = true;
            // 
            // mutantsNothingRadio
            // 
            this.mutantsNothingRadio.AutoSize = true;
            this.mutantsNothingRadio.Checked = true;
            this.mutantsNothingRadio.Location = new System.Drawing.Point(17, 19);
            this.mutantsNothingRadio.Name = "mutantsNothingRadio";
            this.mutantsNothingRadio.Size = new System.Drawing.Size(77, 17);
            this.mutantsNothingRadio.TabIndex = 1;
            this.mutantsNothingRadio.TabStop = true;
            this.mutantsNothingRadio.Text = "Do nothing";
            this.mutantsNothingRadio.UseVisualStyleBackColor = true;
            // 
            // reflectorOutputLabel
            // 
            this.reflectorOutputLabel.AutoSize = true;
            this.reflectorOutputLabel.Location = new System.Drawing.Point(413, 296);
            this.reflectorOutputLabel.Name = "reflectorOutputLabel";
            this.reflectorOutputLabel.Size = new System.Drawing.Size(93, 13);
            this.reflectorOutputLabel.TabIndex = 21;
            this.reflectorOutputLabel.Text = "Reflector process:";
            // 
            // reflectorTextBox
            // 
            this.reflectorTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.reflectorTextBox.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.reflectorTextBox.Location = new System.Drawing.Point(416, 312);
            this.reflectorTextBox.Multiline = true;
            this.reflectorTextBox.Name = "reflectorTextBox";
            this.reflectorTextBox.ReadOnly = true;
            this.reflectorTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.reflectorTextBox.Size = new System.Drawing.Size(403, 253);
            this.reflectorTextBox.TabIndex = 22;
            // 
            // corruptionGroupBox
            // 
            this.corruptionGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.corruptionGroupBox.Controls.Add(this.popupLabel);
            this.corruptionGroupBox.Controls.Add(this.popupPictureBox);
            this.corruptionGroupBox.Location = new System.Drawing.Point(416, 19);
            this.corruptionGroupBox.Name = "corruptionGroupBox";
            this.corruptionGroupBox.Size = new System.Drawing.Size(403, 204);
            this.corruptionGroupBox.TabIndex = 6;
            this.corruptionGroupBox.TabStop = false;
            this.corruptionGroupBox.Text = "Notice:";
            // 
            // saveFileGroupBox
            // 
            this.saveFileGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.saveFileGroupBox.Controls.Add(this.saveFileTextBox);
            this.saveFileGroupBox.Controls.Add(this.saveFileChooseButton);
            this.saveFileGroupBox.Controls.Add(this.backupCheckBox);
            this.saveFileGroupBox.Location = new System.Drawing.Point(10, 229);
            this.saveFileGroupBox.Name = "saveFileGroupBox";
            this.saveFileGroupBox.Size = new System.Drawing.Size(808, 55);
            this.saveFileGroupBox.TabIndex = 23;
            this.saveFileGroupBox.TabStop = false;
            this.saveFileGroupBox.Text = "Save File";
            // 
            // saveFileTextBox
            // 
            this.saveFileTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.saveFileTextBox.Location = new System.Drawing.Point(143, 21);
            this.saveFileTextBox.Name = "saveFileTextBox";
            this.saveFileTextBox.ReadOnly = true;
            this.saveFileTextBox.Size = new System.Drawing.Size(507, 20);
            this.saveFileTextBox.TabIndex = 10;
            this.saveFileTextBox.WordWrap = false;
            // 
            // saveFileChooseButton
            // 
            this.saveFileChooseButton.Location = new System.Drawing.Point(6, 19);
            this.saveFileChooseButton.Name = "saveFileChooseButton";
            this.saveFileChooseButton.Size = new System.Drawing.Size(131, 23);
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
            this.backupCheckBox.Enabled = false;
            this.backupCheckBox.Location = new System.Drawing.Point(656, 23);
            this.backupCheckBox.Name = "backupCheckBox";
            this.backupCheckBox.Size = new System.Drawing.Size(146, 17);
            this.backupCheckBox.TabIndex = 11;
            this.backupCheckBox.Text = "Backup original Save File";
            this.backupCheckBox.UseVisualStyleBackColor = true;
            // 
            // modifyGroupBox
            // 
            this.modifyGroupBox.Controls.Add(this.modifySaveButton);
            this.modifyGroupBox.Location = new System.Drawing.Point(241, 306);
            this.modifyGroupBox.Name = "modifyGroupBox";
            this.modifyGroupBox.Size = new System.Drawing.Size(153, 59);
            this.modifyGroupBox.TabIndex = 28;
            this.modifyGroupBox.TabStop = false;
            this.modifyGroupBox.Text = "Quick Mode";
            // 
            // modifySaveButton
            // 
            this.modifySaveButton.Location = new System.Drawing.Point(20, 19);
            this.modifySaveButton.Name = "modifySaveButton";
            this.modifySaveButton.Size = new System.Drawing.Size(118, 30);
            this.modifySaveButton.TabIndex = 18;
            this.modifySaveButton.Text = "Modify the Save File";
            this.modifySaveButton.UseVisualStyleBackColor = true;
            this.modifySaveButton.Click += new System.EventHandler(this.modifySaveButton_Click);
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
            this.statusTextBox.Size = new System.Drawing.Size(824, 59);
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
            this.saveEditorTabPage.PerformLayout();
            this.reflectorGroupBox.ResumeLayout(false);
            this.reflectorGroupBox.PerformLayout();
            this.manualGroupBox.ResumeLayout(false);
            this.extractGroupBox.ResumeLayout(false);
            this.extractGroupBox.PerformLayout();
            this.terrainGroupBox.ResumeLayout(false);
            this.terrainGroupBox.PerformLayout();
            this.mutantGroupBox.ResumeLayout(false);
            this.mutantGroupBox.PerformLayout();
            this.corruptionGroupBox.ResumeLayout(false);
            this.corruptionGroupBox.PerformLayout();
            this.saveFileGroupBox.ResumeLayout(false);
            this.saveFileGroupBox.PerformLayout();
            this.modifyGroupBox.ResumeLayout(false);
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
        private System.Windows.Forms.Label reflectorOutputLabel;
        private System.Windows.Forms.TextBox reflectorTextBox;
        private System.Windows.Forms.CheckBox backupCheckBox;
        private System.Windows.Forms.GroupBox mutantGroupBox;
        private System.Windows.Forms.RadioButton mutantsMoveRadio;
        private System.Windows.Forms.RadioButton mutantsRemoveRadio;
        private System.Windows.Forms.RadioButton mutantsNothingRadio;
        private System.Windows.Forms.Label mutantMoveCCLabel;
        private System.Windows.Forms.ComboBox mutantMoveWhatComboBox;
        private System.Windows.Forms.ComboBox mutantMoveGlobalComboBox;
        private System.Windows.Forms.GroupBox terrainGroupBox;
        private System.Windows.Forms.CheckBox themeCheckBox;
        private System.Windows.Forms.CheckBox showFullCheckBox;
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
    }
}

