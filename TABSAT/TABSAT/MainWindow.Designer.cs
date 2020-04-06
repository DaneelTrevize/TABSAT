using System;

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
            this.cloudGroupBox = new System.Windows.Forms.GroupBox();
            this.updatesPictureBox = new System.Windows.Forms.PictureBox();
            this.propertiesPictureBox = new System.Windows.Forms.PictureBox();
            this.cloudDisableLabel = new System.Windows.Forms.Label();
            this.updatesLabel = new System.Windows.Forms.Label();
            this.propertiesLabel = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.beforeUsingTabPage = new System.Windows.Forms.TabPage();
            this.corruptionGroupBox = new System.Windows.Forms.GroupBox();
            this.popupLabel = new System.Windows.Forms.Label();
            this.popupPictureBox = new System.Windows.Forms.PictureBox();
            this.logResizeLabel = new System.Windows.Forms.Label();
            this.saveEditorTabPage = new System.Windows.Forms.TabPage();
            this.autoBackupTabPage = new System.Windows.Forms.TabPage();
            this.statusTextBox = new System.Windows.Forms.TextBox();
            this.tabsLogSplitContainer = new System.Windows.Forms.SplitContainer();
            this.cloudGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.updatesPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.propertiesPictureBox)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.beforeUsingTabPage.SuspendLayout();
            this.corruptionGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.popupPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabsLogSplitContainer)).BeginInit();
            this.tabsLogSplitContainer.Panel1.SuspendLayout();
            this.tabsLogSplitContainer.Panel2.SuspendLayout();
            this.tabsLogSplitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // cloudGroupBox
            // 
            this.cloudGroupBox.BackColor = System.Drawing.Color.Transparent;
            this.cloudGroupBox.Controls.Add(this.updatesPictureBox);
            this.cloudGroupBox.Controls.Add(this.propertiesPictureBox);
            this.cloudGroupBox.Controls.Add(this.cloudDisableLabel);
            this.cloudGroupBox.Controls.Add(this.updatesLabel);
            this.cloudGroupBox.Controls.Add(this.propertiesLabel);
            this.cloudGroupBox.Location = new System.Drawing.Point(6, 6);
            this.cloudGroupBox.MinimumSize = new System.Drawing.Size(740, 570);
            this.cloudGroupBox.Name = "cloudGroupBox";
            this.cloudGroupBox.Size = new System.Drawing.Size(746, 570);
            this.cloudGroupBox.TabIndex = 0;
            this.cloudGroupBox.TabStop = false;
            this.cloudGroupBox.Text = "Disable Steam Cloud sync";
            // 
            // updatesPictureBox
            // 
            this.updatesPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.updatesPictureBox.Image = global::TABSAT.Properties.Resources.TAB_updates_menu;
            this.updatesPictureBox.Location = new System.Drawing.Point(277, 42);
            this.updatesPictureBox.Name = "updatesPictureBox";
            this.updatesPictureBox.Size = new System.Drawing.Size(457, 520);
            this.updatesPictureBox.TabIndex = 1;
            this.updatesPictureBox.TabStop = false;
            // 
            // propertiesPictureBox
            // 
            this.propertiesPictureBox.Image = global::TABSAT.Properties.Resources.TAB_properties_menu;
            this.propertiesPictureBox.Location = new System.Drawing.Point(13, 42);
            this.propertiesPictureBox.Name = "propertiesPictureBox";
            this.propertiesPictureBox.Size = new System.Drawing.Size(255, 257);
            this.propertiesPictureBox.TabIndex = 0;
            this.propertiesPictureBox.TabStop = false;
            // 
            // cloudDisableLabel
            // 
            this.cloudDisableLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cloudDisableLabel.Location = new System.Drawing.Point(6, 419);
            this.cloudDisableLabel.Name = "cloudDisableLabel";
            this.cloudDisableLabel.Size = new System.Drawing.Size(265, 49);
            this.cloudDisableLabel.TabIndex = 2;
            this.cloudDisableLabel.Text = "Please ensure Steam Cloud sync for TAB is disabled.\r\nThis cannot be automatically" +
    " verified at this time.";
            this.cloudDisableLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // updatesLabel
            // 
            this.updatesLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.updatesLabel.AutoSize = true;
            this.updatesLabel.Location = new System.Drawing.Point(274, 26);
            this.updatesLabel.Name = "updatesLabel";
            this.updatesLabel.Size = new System.Drawing.Size(124, 13);
            this.updatesLabel.TabIndex = 1;
            this.updatesLabel.Text = "2) View the Updates tab:";
            // 
            // propertiesLabel
            // 
            this.propertiesLabel.AutoSize = true;
            this.propertiesLabel.Location = new System.Drawing.Point(13, 26);
            this.propertiesLabel.Name = "propertiesLabel";
            this.propertiesLabel.Size = new System.Drawing.Size(197, 13);
            this.propertiesLabel.TabIndex = 0;
            this.propertiesLabel.Text = "1) Right-click TAB in your Steam Library:";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.beforeUsingTabPage);
            this.tabControl1.Controls.Add(this.saveEditorTabPage);
            this.tabControl1.Controls.Add(this.autoBackupTabPage);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(766, 819);
            this.tabControl1.TabIndex = 0;
            // 
            // beforeUsingTabPage
            // 
            this.beforeUsingTabPage.AutoScroll = true;
            this.beforeUsingTabPage.Controls.Add(this.corruptionGroupBox);
            this.beforeUsingTabPage.Controls.Add(this.logResizeLabel);
            this.beforeUsingTabPage.Controls.Add(this.cloudGroupBox);
            this.beforeUsingTabPage.Location = new System.Drawing.Point(4, 22);
            this.beforeUsingTabPage.Name = "beforeUsingTabPage";
            this.beforeUsingTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.beforeUsingTabPage.Size = new System.Drawing.Size(758, 793);
            this.beforeUsingTabPage.TabIndex = 0;
            this.beforeUsingTabPage.Text = " Before using TABSAT ";
            this.beforeUsingTabPage.UseVisualStyleBackColor = true;
            // 
            // corruptionGroupBox
            // 
            this.corruptionGroupBox.Controls.Add(this.popupLabel);
            this.corruptionGroupBox.Controls.Add(this.popupPictureBox);
            this.corruptionGroupBox.Location = new System.Drawing.Point(6, 582);
            this.corruptionGroupBox.MinimumSize = new System.Drawing.Size(700, 180);
            this.corruptionGroupBox.Name = "corruptionGroupBox";
            this.corruptionGroupBox.Size = new System.Drawing.Size(746, 180);
            this.corruptionGroupBox.TabIndex = 1;
            this.corruptionGroupBox.TabStop = false;
            this.corruptionGroupBox.Text = "Save Modification Popup";
            // 
            // popupLabel
            // 
            this.popupLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.popupLabel.Location = new System.Drawing.Point(6, 61);
            this.popupLabel.Name = "popupLabel";
            this.popupLabel.Size = new System.Drawing.Size(265, 55);
            this.popupLabel.TabIndex = 0;
            this.popupLabel.Text = "During normal Save modification, a\r\n\'Data Files Corrupted\' popup will appear.\r\nPl" +
    "ease do not interact with it.";
            this.popupLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // popupPictureBox
            // 
            this.popupPictureBox.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.popupPictureBox.Image = global::TABSAT.Properties.Resources.TAB_ignore_popup;
            this.popupPictureBox.Location = new System.Drawing.Point(317, 19);
            this.popupPictureBox.Name = "popupPictureBox";
            this.popupPictureBox.Size = new System.Drawing.Size(386, 152);
            this.popupPictureBox.TabIndex = 2;
            this.popupPictureBox.TabStop = false;
            // 
            // logResizeLabel
            // 
            this.logResizeLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.logResizeLabel.Location = new System.Drawing.Point(6, 766);
            this.logResizeLabel.MinimumSize = new System.Drawing.Size(210, 23);
            this.logResizeLabel.Name = "logResizeLabel";
            this.logResizeLabel.Size = new System.Drawing.Size(746, 23);
            this.logResizeLabel.TabIndex = 2;
            this.logResizeLabel.Text = "The footer text areas, such as the Log area below, can be resized vertically.";
            this.logResizeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // saveEditorTabPage
            // 
            this.saveEditorTabPage.Location = new System.Drawing.Point(4, 22);
            this.saveEditorTabPage.Name = "saveEditorTabPage";
            this.saveEditorTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.saveEditorTabPage.Size = new System.Drawing.Size(758, 793);
            this.saveEditorTabPage.TabIndex = 1;
            this.saveEditorTabPage.Text = " Modify Save Files ";
            this.saveEditorTabPage.UseVisualStyleBackColor = true;
            // 
            // autoBackupTabPage
            // 
            this.autoBackupTabPage.Location = new System.Drawing.Point(4, 22);
            this.autoBackupTabPage.Name = "autoBackupTabPage";
            this.autoBackupTabPage.Size = new System.Drawing.Size(758, 793);
            this.autoBackupTabPage.TabIndex = 2;
            this.autoBackupTabPage.Text = "Automatically Backup Save Files";
            this.autoBackupTabPage.UseVisualStyleBackColor = true;
            // 
            // statusTextBox
            // 
            this.statusTextBox.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.statusTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statusTextBox.Location = new System.Drawing.Point(0, 0);
            this.statusTextBox.Multiline = true;
            this.statusTextBox.Name = "statusTextBox";
            this.statusTextBox.ReadOnly = true;
            this.statusTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.statusTextBox.Size = new System.Drawing.Size(766, 68);
            this.statusTextBox.TabIndex = 0;
            this.statusTextBox.Text = "Latest version available at:\thttps://github.com/DaneelTrevize/TABSAT/releases\r\nTA" +
    "BSAT Log:\r\n";
            // 
            // tabsLogSplitContainer
            // 
            this.tabsLogSplitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabsLogSplitContainer.Location = new System.Drawing.Point(12, 12);
            this.tabsLogSplitContainer.Name = "tabsLogSplitContainer";
            this.tabsLogSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // tabsLogSplitContainer.Panel1
            // 
            this.tabsLogSplitContainer.Panel1.AutoScroll = true;
            this.tabsLogSplitContainer.Panel1.Controls.Add(this.tabControl1);
            this.tabsLogSplitContainer.Panel1MinSize = 395;
            // 
            // tabsLogSplitContainer.Panel2
            // 
            this.tabsLogSplitContainer.Panel2.Controls.Add(this.statusTextBox);
            this.tabsLogSplitContainer.Panel2MinSize = 40;
            this.tabsLogSplitContainer.Size = new System.Drawing.Size(766, 891);
            this.tabsLogSplitContainer.SplitterDistance = 819;
            this.tabsLogSplitContainer.TabIndex = 2;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(790, 915);
            this.Controls.Add(this.tabsLogSplitContainer);
            this.MinimumSize = new System.Drawing.Size(450, 500);
            this.Name = "MainWindow";
            this.Text = "TABSAT - They Are Billions Save Automation Tool";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.cloudGroupBox.ResumeLayout(false);
            this.cloudGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.updatesPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.propertiesPictureBox)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.beforeUsingTabPage.ResumeLayout(false);
            this.corruptionGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.popupPictureBox)).EndInit();
            this.tabsLogSplitContainer.Panel1.ResumeLayout(false);
            this.tabsLogSplitContainer.Panel2.ResumeLayout(false);
            this.tabsLogSplitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabsLogSplitContainer)).EndInit();
            this.tabsLogSplitContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox propertiesPictureBox;
        private System.Windows.Forms.PictureBox updatesPictureBox;
        private System.Windows.Forms.GroupBox cloudGroupBox;
        private System.Windows.Forms.Label propertiesLabel;
        private System.Windows.Forms.Label updatesLabel;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage beforeUsingTabPage;
        private System.Windows.Forms.TabPage saveEditorTabPage;
        private System.Windows.Forms.Label cloudDisableLabel;
        private System.Windows.Forms.TextBox statusTextBox;
        private System.Windows.Forms.TabPage autoBackupTabPage;
        private System.Windows.Forms.SplitContainer tabsLogSplitContainer;
        private System.Windows.Forms.Label logResizeLabel;
        private System.Windows.Forms.GroupBox corruptionGroupBox;
        private System.Windows.Forms.Label popupLabel;
        private System.Windows.Forms.PictureBox popupPictureBox;
    }
}

