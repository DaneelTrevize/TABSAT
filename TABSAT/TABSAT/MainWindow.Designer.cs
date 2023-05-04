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
            this.generalPictureBox = new System.Windows.Forms.PictureBox();
            this.propertiesPictureBox = new System.Windows.Forms.PictureBox();
            this.cloudDisableLabel = new System.Windows.Forms.Label();
            this.updatesLabel = new System.Windows.Forms.Label();
            this.propertiesLabel = new System.Windows.Forms.Label();
            this.mainTabControl = new System.Windows.Forms.TabControl();
            this.beforeUsingTabPage = new System.Windows.Forms.TabPage();
            this.noticesFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.corruptionGroupBox = new System.Windows.Forms.GroupBox();
            this.popupLabel = new System.Windows.Forms.Label();
            this.popupPictureBox = new System.Windows.Forms.PictureBox();
            this.logResizeLabel = new System.Windows.Forms.Label();
            this.saveEditorTabPage = new System.Windows.Forms.TabPage();
            this.autoBackupTabPage = new System.Windows.Forms.TabPage();
            this.saveSelectorTabPage = new System.Windows.Forms.TabPage();
            this.statusTextBox = new System.Windows.Forms.TextBox();
            this.tabsLogSplitContainer = new System.Windows.Forms.SplitContainer();
            this.horizontalDividerLabel = new System.Windows.Forms.Label();
            this.cloudGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.generalPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.propertiesPictureBox)).BeginInit();
            this.mainTabControl.SuspendLayout();
            this.beforeUsingTabPage.SuspendLayout();
            this.noticesFlowLayoutPanel.SuspendLayout();
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
            this.cloudGroupBox.Controls.Add(this.generalPictureBox);
            this.cloudGroupBox.Controls.Add(this.propertiesPictureBox);
            this.cloudGroupBox.Controls.Add(this.cloudDisableLabel);
            this.cloudGroupBox.Controls.Add(this.updatesLabel);
            this.cloudGroupBox.Controls.Add(this.propertiesLabel);
            this.noticesFlowLayoutPanel.SetFlowBreak(this.cloudGroupBox, true);
            this.cloudGroupBox.Location = new System.Drawing.Point(3, 229);
            this.cloudGroupBox.Name = "cloudGroupBox";
            this.cloudGroupBox.Size = new System.Drawing.Size(576, 435);
            this.cloudGroupBox.TabIndex = 0;
            this.cloudGroupBox.TabStop = false;
            this.cloudGroupBox.Text = "Disable Steam Cloud Synchonization";
            // 
            // generalPictureBox
            // 
            this.generalPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.generalPictureBox.Image = global::TABSAT.Properties.Resources.TAB_Steam_Cloud;
            this.generalPictureBox.Location = new System.Drawing.Point(47, 332);
            this.generalPictureBox.Name = "generalPictureBox";
            this.generalPictureBox.Size = new System.Drawing.Size(488, 95);
            this.generalPictureBox.TabIndex = 3;
            this.generalPictureBox.TabStop = false;
            // 
            // propertiesPictureBox
            // 
            this.propertiesPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.propertiesPictureBox.Image = global::TABSAT.Properties.Resources.TAB_properties_menu;
            this.propertiesPictureBox.Location = new System.Drawing.Point(164, 96);
            this.propertiesPictureBox.Name = "propertiesPictureBox";
            this.propertiesPictureBox.Size = new System.Drawing.Size(254, 201);
            this.propertiesPictureBox.TabIndex = 0;
            this.propertiesPictureBox.TabStop = false;
            // 
            // cloudDisableLabel
            // 
            this.cloudDisableLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cloudDisableLabel.Location = new System.Drawing.Point(159, 16);
            this.cloudDisableLabel.Name = "cloudDisableLabel";
            this.cloudDisableLabel.Size = new System.Drawing.Size(265, 49);
            this.cloudDisableLabel.TabIndex = 2;
            this.cloudDisableLabel.Text = "Please ensure Steam Cloud sync for TAB is disabled.\r\nThis cannot be automatically" +
    " verified at this time.";
            this.cloudDisableLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // updatesLabel
            // 
            this.updatesLabel.AutoSize = true;
            this.updatesLabel.Location = new System.Drawing.Point(6, 316);
            this.updatesLabel.Name = "updatesLabel";
            this.updatesLabel.Size = new System.Drawing.Size(335, 13);
            this.updatesLabel.TabIndex = 1;
            this.updatesLabel.Text = "2) On the General tab, uncheck Steam Cloud synchonization for TAB:";
            // 
            // propertiesLabel
            // 
            this.propertiesLabel.AutoSize = true;
            this.propertiesLabel.Location = new System.Drawing.Point(6, 80);
            this.propertiesLabel.Name = "propertiesLabel";
            this.propertiesLabel.Size = new System.Drawing.Size(288, 13);
            this.propertiesLabel.TabIndex = 0;
            this.propertiesLabel.Text = "1) Right-click TAB in your Steam Library, choose Properties:";
            // 
            // mainTabControl
            // 
            this.mainTabControl.Controls.Add(this.beforeUsingTabPage);
            this.mainTabControl.Controls.Add(this.saveEditorTabPage);
            this.mainTabControl.Controls.Add(this.autoBackupTabPage);
            this.mainTabControl.Controls.Add(this.saveSelectorTabPage);
            this.mainTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTabControl.Location = new System.Drawing.Point(0, 0);
            this.mainTabControl.Margin = new System.Windows.Forms.Padding(0);
            this.mainTabControl.Name = "mainTabControl";
            this.mainTabControl.Padding = new System.Drawing.Point(0, 0);
            this.mainTabControl.SelectedIndex = 0;
            this.mainTabControl.Size = new System.Drawing.Size(590, 753);
            this.mainTabControl.TabIndex = 0;
            // 
            // beforeUsingTabPage
            // 
            this.beforeUsingTabPage.AutoScroll = true;
            this.beforeUsingTabPage.Controls.Add(this.noticesFlowLayoutPanel);
            this.beforeUsingTabPage.Location = new System.Drawing.Point(4, 22);
            this.beforeUsingTabPage.Name = "beforeUsingTabPage";
            this.beforeUsingTabPage.Size = new System.Drawing.Size(582, 727);
            this.beforeUsingTabPage.TabIndex = 0;
            this.beforeUsingTabPage.Text = " Before using TABSAT ";
            this.beforeUsingTabPage.UseVisualStyleBackColor = true;
            // 
            // noticesFlowLayoutPanel
            // 
            this.noticesFlowLayoutPanel.AutoScroll = true;
            this.noticesFlowLayoutPanel.Controls.Add(this.corruptionGroupBox);
            this.noticesFlowLayoutPanel.Controls.Add(this.cloudGroupBox);
            this.noticesFlowLayoutPanel.Controls.Add(this.logResizeLabel);
            this.noticesFlowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.noticesFlowLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.noticesFlowLayoutPanel.Name = "noticesFlowLayoutPanel";
            this.noticesFlowLayoutPanel.Size = new System.Drawing.Size(582, 727);
            this.noticesFlowLayoutPanel.TabIndex = 3;
            // 
            // corruptionGroupBox
            // 
            this.corruptionGroupBox.Controls.Add(this.popupLabel);
            this.corruptionGroupBox.Controls.Add(this.popupPictureBox);
            this.corruptionGroupBox.Location = new System.Drawing.Point(3, 3);
            this.corruptionGroupBox.Name = "corruptionGroupBox";
            this.corruptionGroupBox.Size = new System.Drawing.Size(576, 220);
            this.corruptionGroupBox.TabIndex = 1;
            this.corruptionGroupBox.TabStop = false;
            this.corruptionGroupBox.Text = "Save Modification Popup Notice";
            // 
            // popupLabel
            // 
            this.popupLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.popupLabel.Location = new System.Drawing.Point(107, 16);
            this.popupLabel.Name = "popupLabel";
            this.popupLabel.Size = new System.Drawing.Size(369, 40);
            this.popupLabel.TabIndex = 0;
            this.popupLabel.Text = "During normal Save modification, a \'Data Files Corrupted\' popup will appear.\r\nPle" +
    "ase do not interact with it.";
            this.popupLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // popupPictureBox
            // 
            this.popupPictureBox.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.popupPictureBox.Image = global::TABSAT.Properties.Resources.TAB_ignore_popup;
            this.popupPictureBox.Location = new System.Drawing.Point(98, 63);
            this.popupPictureBox.Name = "popupPictureBox";
            this.popupPictureBox.Size = new System.Drawing.Size(386, 147);
            this.popupPictureBox.TabIndex = 2;
            this.popupPictureBox.TabStop = false;
            // 
            // logResizeLabel
            // 
            this.logResizeLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.logResizeLabel.Location = new System.Drawing.Point(3, 667);
            this.logResizeLabel.MinimumSize = new System.Drawing.Size(210, 23);
            this.logResizeLabel.Name = "logResizeLabel";
            this.logResizeLabel.Size = new System.Drawing.Size(576, 54);
            this.logResizeLabel.TabIndex = 2;
            this.logResizeLabel.Text = "In TABSAT, you can drag a thin handle to resize certain areas, such as the Log be" +
    "low.\r\nThe Save Modification options area can be resized vertically.\r\nThe Reflect" +
    "or Output can be resized horizontally.";
            this.logResizeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // saveEditorTabPage
            // 
            this.saveEditorTabPage.Location = new System.Drawing.Point(4, 22);
            this.saveEditorTabPage.Name = "saveEditorTabPage";
            this.saveEditorTabPage.Size = new System.Drawing.Size(582, 732);
            this.saveEditorTabPage.TabIndex = 1;
            this.saveEditorTabPage.Text = " Modify Save Files ";
            this.saveEditorTabPage.UseVisualStyleBackColor = true;
            // 
            // autoBackupTabPage
            // 
            this.autoBackupTabPage.Location = new System.Drawing.Point(4, 22);
            this.autoBackupTabPage.Name = "autoBackupTabPage";
            this.autoBackupTabPage.Size = new System.Drawing.Size(582, 732);
            this.autoBackupTabPage.TabIndex = 2;
            this.autoBackupTabPage.Text = "Backup Save Files";
            this.autoBackupTabPage.UseVisualStyleBackColor = true;
            // 
            // saveSelectorTabPage
            // 
            this.saveSelectorTabPage.Location = new System.Drawing.Point(4, 22);
            this.saveSelectorTabPage.Name = "saveSelectorTabPage";
            this.saveSelectorTabPage.Size = new System.Drawing.Size(582, 732);
            this.saveSelectorTabPage.TabIndex = 3;
            this.saveSelectorTabPage.Text = "Map Viewer";
            this.saveSelectorTabPage.UseVisualStyleBackColor = true;
            // 
            // statusTextBox
            // 
            this.statusTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.statusTextBox.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.statusTextBox.Location = new System.Drawing.Point(0, 0);
            this.statusTextBox.Multiline = true;
            this.statusTextBox.Name = "statusTextBox";
            this.statusTextBox.ReadOnly = true;
            this.statusTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.statusTextBox.Size = new System.Drawing.Size(587, 34);
            this.statusTextBox.TabIndex = 0;
            this.statusTextBox.Text = "Latest version available at:\thttps://github.com/DaneelTrevize/TABSAT/releases\r\nTA" +
    "BSAT Log:";
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
            this.tabsLogSplitContainer.Panel1.Controls.Add(this.mainTabControl);
            this.tabsLogSplitContainer.Panel1MinSize = 370;
            // 
            // tabsLogSplitContainer.Panel2
            // 
            this.tabsLogSplitContainer.Panel2.Controls.Add(this.horizontalDividerLabel);
            this.tabsLogSplitContainer.Panel2.Controls.Add(this.statusTextBox);
            this.tabsLogSplitContainer.Panel2MinSize = 34;
            this.tabsLogSplitContainer.Size = new System.Drawing.Size(590, 791);
            this.tabsLogSplitContainer.SplitterDistance = 753;
            this.tabsLogSplitContainer.TabIndex = 2;
            // 
            // horizontalDividerLabel
            // 
            this.horizontalDividerLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.horizontalDividerLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.horizontalDividerLabel.Location = new System.Drawing.Point(0, 0);
            this.horizontalDividerLabel.Name = "horizontalDividerLabel";
            this.horizontalDividerLabel.Size = new System.Drawing.Size(590, 2);
            this.horizontalDividerLabel.TabIndex = 3;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(614, 806);
            this.Controls.Add(this.tabsLogSplitContainer);
            this.MinimumSize = new System.Drawing.Size(630, 525);
            this.Name = "MainWindow";
            this.Text = "TABSAT - They Are Billions Save Automation Tool";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.cloudGroupBox.ResumeLayout(false);
            this.cloudGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.generalPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.propertiesPictureBox)).EndInit();
            this.mainTabControl.ResumeLayout(false);
            this.beforeUsingTabPage.ResumeLayout(false);
            this.noticesFlowLayoutPanel.ResumeLayout(false);
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
        private System.Windows.Forms.GroupBox cloudGroupBox;
        private System.Windows.Forms.Label propertiesLabel;
        private System.Windows.Forms.Label updatesLabel;
        private System.Windows.Forms.TabControl mainTabControl;
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
        private System.Windows.Forms.FlowLayoutPanel noticesFlowLayoutPanel;
        private System.Windows.Forms.PictureBox generalPictureBox;
        private System.Windows.Forms.Label horizontalDividerLabel;
        private System.Windows.Forms.TabPage saveSelectorTabPage;
    }
}

